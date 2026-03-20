using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPPMerits : UserControlCommonBase
    {
        #region Inner Classes

        private class MeritRow
        {
            public DateTime TimeUTC;
            public string Power;
            public int MeritsGained;
            public int TotalMerits;
            public string SystemName;
            public string CycleKey;
            public int SessionId;
            public HistoryEntry HE;
        }

        private class GroupingLevel
        {
            public Func<MeritRow, string> KeySelector;
            public Func<string, string> LabelFormatter;
        }

        private class GroupRow
        {
            public string Path;
            public bool Expanded;
            public bool HasChildren;
            public int Level;
        }

        #endregion

        #region Fields

        private readonly List<MeritRow> meritRows = new List<MeritRow>();
        private readonly Dictionary<string, bool> expandStates = new Dictionary<string, bool>();
        private int nextSessionId = 0;
        private MeritRow rightClickMeritRow = null;
        private GroupRow rightClickGroupRow = null;
        private System.Windows.Forms.Timer redrawDebounceTimer;
        private bool pendingRedraw;
        private DateTime? lastAddedMeritTime;

        // Duplicate detection state
        private double trackedMeritTotal = -1;  // -1 means not yet initialized
        private readonly List<PendingMerit> pendingMeritsQueue = new List<PendingMerit>();
        private static readonly TimeSpan QueueExpiryTime = TimeSpan.FromMinutes(10);
        private const double MeritTolerance = 0.01;

        private class PendingMerit
        {
            public HistoryEntry HE;
            public JournalPowerplayMerits PPM;
            public DateTime QueuedAtUTC;
            public int SessionId;
        }

        #endregion

        #region Initialization

        public UserControlPPMerits()
        {
            InitializeComponent();
            DBBaseName = "PPMerits";

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuGrid);
        }

        protected override void Init()
        {
            redrawDebounceTimer = new System.Windows.Forms.Timer();
            redrawDebounceTimer.Interval = 150; // ms
            redrawDebounceTimer.Tick += (s, e) =>
            {
                redrawDebounceTimer.Stop();
                if (pendingRedraw)
                {
                    pendingRedraw = false;
                    Redraw();
                }
            };

            checkCycle.CheckedChanged += (s, e) => { PutSetting("GroupCycle", checkCycle.Checked ? 1 : 0); Redraw(); };
            checkSession.CheckedChanged += (s, e) => { PutSetting("GroupSession", checkSession.Checked ? 1 : 0); Redraw(); };
            checkSystem.CheckedChanged += (s, e) => { PutSetting("GroupSystem", checkSystem.Checked ? 1 : 0); Redraw(); };

            grid.CellClick += Grid_CellClick;
            grid.MouseDown += grid_MouseDown;

            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange_Handler;
        }

        protected override void LoadLayout()
        {
            checkCycle.Checked = GetSetting("GroupCycle", 1) != 0;
            checkSession.Checked = GetSetting("GroupSession", 1) != 0;
            checkSystem.Checked = GetSetting("GroupSystem", 1) != 0;
        }

        protected override void InitialDisplay()
        {
            BuildFromHistory();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= DiscoveryForm_OnHistoryChange_Handler;
        }

        #endregion

        #region Event Handlers

        private void DiscoveryForm_OnHistoryChange_Handler()
        {
            BuildFromHistory();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            if (he == null || he.journalEntry == null)
                return;

            if (he.journalEntry is JournalPowerplay pp)
            {
                // Each journal file starts with a Powerplay status event containing the authoritative total.
                // Use this as a sync checkpoint - reconcile prior session tail and then reset tracked total.
                System.Diagnostics.Debug.WriteLine($"[PPMerits] Powerplay status received: AuthoritativeTotal={pp.Merits}, PreviousTracked={trackedMeritTotal}, QueueSize={pendingMeritsQueue.Count}");
                bool reconciled = ReconcileFromAuthoritativeTotal(pp.Merits);
                trackedMeritTotal = pp.Merits;
                pendingMeritsQueue.Clear();

                if (reconciled)
                {
                    pendingRedraw = true;
                    redrawDebounceTimer.Stop();
                    redrawDebounceTimer.Start();
                }
            }
            else if (he.journalEntry is JournalPowerplayMerits ppm)
            {
                if (TryProcessMerit(he, ppm))
                {
                    // Debounce redraws to avoid flicker on burst updates
                    pendingRedraw = true;
                    redrawDebounceTimer.Stop();
                    redrawDebounceTimer.Start();
                }
            }
            else if (he.journalEntry is JournalLoadGame)
            {
                nextSessionId++;
            }
            else if (he.journalEntry is JournalShutdown)
            {
                // Session ends; next LoadGame will increment session ID
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count)
                return;

            if (grid.Rows[e.RowIndex].Tag is GroupRow groupRow && groupRow.HasChildren)
            {
                expandStates[groupRow.Path] = !groupRow.Expanded;
                Redraw();
            }
        }

        private void grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var hitTest = grid.HitTest(e.X, e.Y);
            if (hitTest.RowIndex >= 0 && hitTest.RowIndex < grid.Rows.Count)
            {
                rightClickMeritRow = grid.Rows[hitTest.RowIndex].Tag as MeritRow;
                rightClickGroupRow = grid.Rows[hitTest.RowIndex].Tag as GroupRow;
            }
            else
            {
                rightClickMeritRow = null;
                rightClickGroupRow = null;
            }
        }

        #endregion

        #region Data Building

        private void BuildFromHistory()
        {
            System.Diagnostics.Debug.WriteLine($"[PPMerits] BuildFromHistory: Starting full rebuild");
            meritRows.Clear();
            pendingMeritsQueue.Clear();
            nextSessionId = 0;
            trackedMeritTotal = -1;

            foreach (var he in DiscoveryForm.History.EntryOrder())
            {
                if (he.journalEntry is JournalLoadGame)
                {
                    nextSessionId++;
                }
                else if (he.journalEntry is JournalPowerplay pp)
                {
                    // Each journal file starts with a Powerplay status event containing the authoritative total.
                    // Use this as a sync checkpoint - reconcile prior session tail and then reset tracked total.
                    System.Diagnostics.Debug.WriteLine($"[PPMerits] BuildFromHistory: Powerplay status at {he.EventTimeUTC:HH:mm:ss}, AuthoritativeTotal={pp.Merits}, PreviousTracked={trackedMeritTotal}");
                    ReconcileFromAuthoritativeTotal(pp.Merits);
                    trackedMeritTotal = pp.Merits;
                    pendingMeritsQueue.Clear();
                }
                else if (he.journalEntry is JournalPowerplayMerits ppm)
                {
                    TryProcessMerit(he, ppm);
                }
            }

            ResolvePendingQueue(true);

            System.Diagnostics.Debug.WriteLine($"[PPMerits] BuildFromHistory: Complete - MeritRows={meritRows.Count}, PendingQueue={pendingMeritsQueue.Count}, TrackedTotal={trackedMeritTotal}");
            Redraw();
        }

        private void AddMerit(HistoryEntry he, JournalPowerplayMerits ppm, int? sessionIdOverride = null)
        {
            var row = new MeritRow
            {
                TimeUTC = he.EventTimeUTC,
                Power = ppm.Power,
                MeritsGained = (int)ppm.MeritsGained,
                TotalMerits = (int)ppm.TotalMerits,
                SystemName = he.System != null ? he.System.Name : string.Empty,
                CycleKey = ComputeCycleKey(he.EventTimeUTC),
                SessionId = sessionIdOverride ?? nextSessionId,
                HE = he
            };
            meritRows.Add(row);
            lastAddedMeritTime = row.TimeUTC;
        }

        #endregion

        #region Rendering

        private void Redraw()
        {
            UpdateDuplicateTailWarning();

            grid.SuspendLayout();
            grid.Rows.Clear();

            var levels = new List<GroupingLevel>();

            if (checkCycle.Checked)
            {
                levels.Add(new GroupingLevel
                {
                    KeySelector = r => r.CycleKey,
                    LabelFormatter = key => "Cycle " + key,
                });
            }

            if (checkSession.Checked)
            {
                levels.Add(new GroupingLevel
                {
                    KeySelector = r => $"Session {r.SessionId}",
                    LabelFormatter = key => key,
                });
            }

            if (checkSystem.Checked)
            {
                levels.Add(new GroupingLevel
                {
                    KeySelector = r => string.IsNullOrEmpty(r.SystemName) ? "Unknown System" : r.SystemName,
                    LabelFormatter = key => key,
                });
            }

            if (levels.Count > 0)
            {
                // Calculate current and previous cycle keys for default expansion
                HashSet<string> defaultExpandCycles = new HashSet<string>();
                if (checkCycle.Checked && meritRows.Count > 0)
                {
                    var distinctCycles = meritRows.Select(r => new { r.CycleKey, r.TimeUTC })
                        .GroupBy(x => x.CycleKey)
                        .Select(g => new { CycleKey = g.Key, Latest = g.Max(x => x.TimeUTC) })
                        .OrderByDescending(x => x.Latest)
                        .Take(2)
                        .Select(x => x.CycleKey)
                        .ToList();
                    
                    foreach (var cycle in distinctCycles)
                        defaultExpandCycles.Add(cycle);
                }
                
                RenderGrouped(meritRows, levels, 0, 0, "", defaultExpandCycles);
            }
            else
            {
                foreach (var r in meritRows.OrderByDescending(x => x.TimeUTC))
                {
                    int idx = grid.Rows.Add();
                    var row = grid.Rows[idx];
                    row.Cells[0].Value = r.TimeUTC.ToString();
                    row.Cells[1].Value = r.Power;
                    row.Cells[2].Value = r.MeritsGained;
                    row.Cells[3].Value = r.TotalMerits;
                    row.Cells[4].Value = r.SystemName;
                    row.Tag = r;

                    // Highlight newly added row
                    if (lastAddedMeritTime.HasValue && r.TimeUTC == lastAddedMeritTime.Value)
                    {
                        row.DefaultCellStyle.BackColor = Theme.Current.GridHighlightBack;
                    }
                }
            }

            grid.ResumeLayout();

            // Auto-scroll to latest merit within expanded current cycle
            if (lastAddedMeritTime.HasValue && grid.Rows.Count > 0)
            {
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    var tag = grid.Rows[i].Tag as MeritRow;
                    if (tag != null && tag.TimeUTC == lastAddedMeritTime.Value)
                    {
                        grid.FirstDisplayedScrollingRowIndex = Math.Max(0, i - 3);
                        break;
                    }
                }
            }
        }

        private void RenderGrouped(IEnumerable<MeritRow> rows, IReadOnlyList<GroupingLevel> levels, int levelIndex, int indent, string path, HashSet<string> defaultExpandCycles)
        {
            var level = levels[levelIndex];

            var grouped = rows
                .GroupBy(level.KeySelector)
                .Select(g => new
                {
                    Key = g.Key,
                    Rows = g,
                    Latest = g.Max(r => r.TimeUTC)
                })
                .OrderByDescending(g => g.Latest)
                .ToList();

            foreach (var group in grouped)
            {
                string nodePath = string.IsNullOrEmpty(path) ? group.Key : path + "|" + group.Key;

                bool defaultExpanded = GetDefaultExpandedState(levelIndex, group, defaultExpandCycles);
                bool expanded = expandStates.TryGetValue(nodePath, out bool stored) ? stored : defaultExpanded;
                bool hasChildren = levelIndex < levels.Count - 1 || group.Rows.Any();

                int meritsTotal = group.Rows.Sum(r => r.MeritsGained);
                int indexHeader = grid.Rows.Add();
                var header = grid.Rows[indexHeader];

                string indicator = hasChildren ? (expanded ? "[-] " : "[+] ") : "    ";
                header.Cells[0].Value = new string(' ', indent * 2) + indicator + level.LabelFormatter(group.Key);
                header.Cells[1].Value = "Group Total";
                header.Cells[2].Value = meritsTotal;
                header.Cells[3].Value = "";
                header.Cells[4].Value = "";
                header.Tag = new GroupRow { Path = nodePath, Expanded = expanded, HasChildren = hasChildren, Level = levelIndex };

                if (expanded)
                {
                    if (levelIndex < levels.Count - 1)
                    {
                        RenderGrouped(group.Rows, levels, levelIndex + 1, indent + 1, nodePath, defaultExpandCycles);
                    }
                    else
                    {
                        foreach (var r in group.Rows.OrderByDescending(x => x.TimeUTC))
                        {
                            int idx = grid.Rows.Add();
                            var row = grid.Rows[idx];
                            row.Cells[0].Value = new string(' ', (indent + 1) * 2) + r.TimeUTC.ToString();
                            row.Cells[1].Value = r.Power;
                            row.Cells[2].Value = r.MeritsGained;
                            row.Cells[3].Value = r.TotalMerits;
                            row.Cells[4].Value = r.SystemName;
                            row.Tag = r;

                            // Highlight newly added row
                            if (lastAddedMeritTime.HasValue && r.TimeUTC == lastAddedMeritTime.Value)
                            {
                                row.DefaultCellStyle.BackColor = Theme.Current.GridHighlightBack;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Context Menu

        private void contextMenuGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            findInHistoryPanelToolStripMenuItem.Visible = findInJournalToolStripMenuItem.Visible = rightClickMeritRow != null;
            copyMeritsToolStripMenuItem.Visible = IsSystemLevelGroupRow(rightClickGroupRow);
            copyMeritsReportToolStripMenuItem.Visible = rightClickGroupRow != null;
            copySystemNameToolStripMenuItem.Visible = rightClickMeritRow != null || IsSystemLevelGroupRow(rightClickGroupRow);
        }

        private void findInJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightClickMeritRow?.HE != null)
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Journal, new RequestJournalToJID { JID = rightClickMeritRow.HE.Journalid, MakeVisible = true });             //// Switch to Journal tab and navigate to entry
        }

        private void findInHistoryPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightClickMeritRow?.HE != null)
                RequestPanelOperationOpen(PanelInformation.PanelIDs.HistoryGrid, new RequestHistoryToJID { JID = rightClickMeritRow.HE.Journalid, MakeVisible = true });
        }


        private void copyMeritsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightClickGroupRow == null)
                return;

            var merits = GetMeritsForGroupPath(rightClickGroupRow.Path);
            Clipboard.SetText(merits.Sum(m => m.MeritsGained).ToString());
        }

        private void copyMeritsReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightClickGroupRow == null)
                return;

            var merits = GetMeritsForGroupPath(rightClickGroupRow.Path);
            Clipboard.SetText(GenerateMeritsReport(merits));
        }

        private void copySystemNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string systemName = null;

            if (rightClickMeritRow != null)
            {
                systemName = rightClickMeritRow.SystemName;
            }
            else if (IsSystemLevelGroupRow(rightClickGroupRow))
            {
                // Extract system name from the group path
                var pathParts = rightClickGroupRow.Path.Split('|');
                int systemLevelIndex = (checkCycle.Checked ? 1 : 0) + (checkSession.Checked ? 1 : 0);
                if (pathParts.Length > systemLevelIndex)
                {
                    systemName = pathParts[systemLevelIndex];
                }
            }

            if (!string.IsNullOrEmpty(systemName) && systemName != "Unknown System")
            {
                Clipboard.SetText(systemName);
            }
        }

        #endregion

        #region Duplicate Detection

        /// <summary>
        /// Attempts to process a merit event. Returns true if the merit was added to the UI,
        /// false if it was queued or discarded as a duplicate.
        /// </summary>
        private bool TryProcessMerit(HistoryEntry he, JournalPowerplayMerits ppm)
        {
            System.Diagnostics.Debug.WriteLine($"[PPMerits] TryProcessMerit: Time={he.EventTimeUTC:HH:mm:ss.fff}, MeritsGained={ppm.MeritsGained}, TotalMerits={ppm.TotalMerits}, TrackedTotal={trackedMeritTotal}");

            QueueMerit(he, ppm);

            if (trackedMeritTotal < 0)
            {
                System.Diagnostics.Debug.WriteLine("[PPMerits]   -> WAITING: No initial Powerplay status received yet");
                return false;
            }

            return ResolvePendingQueue(false);
        }

        private void QueueMerit(HistoryEntry he, JournalPowerplayMerits ppm)
        {
            System.Diagnostics.Debug.WriteLine($"[PPMerits] QueueMerit: Adding to queue (QueueSize={pendingMeritsQueue.Count + 1}), MeritsGained={ppm.MeritsGained}, TotalMerits={ppm.TotalMerits}");
            pendingMeritsQueue.Add(new PendingMerit
            {
                HE = he,
                PPM = ppm,
                QueuedAtUTC = DateTime.UtcNow,
                SessionId = nextSessionId
            });
        }

        /// <summary>
        /// Process the pending queue, attempting to match queued merits to the current tracked total.
        /// Also cleans up expired and duplicate entries.
        /// </summary>
        private bool ResolvePendingQueue(bool allowTailCommit)
        {
            if (trackedMeritTotal < 0 || pendingMeritsQueue.Count == 0)
                return false;

            System.Diagnostics.Debug.WriteLine($"[PPMerits] ResolvePendingQueue: Starting with QueueSize={pendingMeritsQueue.Count}, TrackedTotal={trackedMeritTotal}, AllowTailCommit={allowTailCommit}");

            bool anyProcessed;
            bool anyAdded = false;
            int iteration = 0;
            do
            {
                anyProcessed = false;
                iteration++;

                // First, clean up expired and obvious duplicates
                int beforeCleanup = pendingMeritsQueue.Count;
                CleanupPendingQueue();
                if (pendingMeritsQueue.Count != beforeCleanup)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits]   Iteration {iteration}: Cleanup removed {beforeCleanup - pendingMeritsQueue.Count} items, QueueSize now={pendingMeritsQueue.Count}");
                }

                if (pendingMeritsQueue.Count == 0)
                    break;

                if (!allowTailCommit && pendingMeritsQueue.Count == 1)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits]   Iteration {iteration}: Waiting for more evidence before committing the final queued merit");
                    break;
                }

                var firstPending = pendingMeritsQueue[0];
                bool canAcceptFirst = DoesMeritMatch(trackedMeritTotal, firstPending.PPM);
                int acceptScore = canAcceptFirst ? 1 + CountGreedyMatches(1, firstPending.PPM.TotalMerits) : int.MinValue;
                int skipScore = CountGreedyMatches(1, trackedMeritTotal);

                if (allowTailCommit && pendingMeritsQueue.Count == 1 && canAcceptFirst)
                {
                    acceptScore = 1;
                }

                if (acceptScore > skipScore && acceptScore > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits]   Iteration {iteration}: ACCEPTED queued merit: MeritsGained={firstPending.PPM.MeritsGained}, TotalMerits={firstPending.PPM.TotalMerits}, AcceptScore={acceptScore}, SkipScore={skipScore}");
                    AddMerit(firstPending.HE, firstPending.PPM, firstPending.SessionId);
                    trackedMeritTotal = firstPending.PPM.TotalMerits;
                    pendingMeritsQueue.RemoveAt(0);
                    anyProcessed = true;
                    anyAdded = true;
                }
                else if (skipScore > acceptScore)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits]   Iteration {iteration}: SKIPPED queued merit as inferred duplicate: MeritsGained={firstPending.PPM.MeritsGained}, TotalMerits={firstPending.PPM.TotalMerits}, AcceptScore={acceptScore}, SkipScore={skipScore}");
                    pendingMeritsQueue.RemoveAt(0);
                    anyProcessed = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits]   Iteration {iteration}: Ambiguous queued merit, keeping for more evidence: MeritsGained={firstPending.PPM.MeritsGained}, TotalMerits={firstPending.PPM.TotalMerits}, AcceptScore={acceptScore}, SkipScore={skipScore}");
                    break;
                }
            } while (anyProcessed && pendingMeritsQueue.Count > 0);

            System.Diagnostics.Debug.WriteLine($"[PPMerits] ResolvePendingQueue: Finished after {iteration} iteration(s), QueueSize={pendingMeritsQueue.Count}, TrackedTotal={trackedMeritTotal}, Added={anyAdded}");
            return anyAdded;
        }

        private bool ReconcileFromAuthoritativeTotal(double authoritativeTotal)
        {
            bool changed = false;
            double runningTotal = trackedMeritTotal;

            if (runningTotal >= 0 && pendingMeritsQueue.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[PPMerits] ReconcileInit: Evaluating {pendingMeritsQueue.Count} queued merit(s) against authoritative total {authoritativeTotal}");

                for (int i = 0; i < pendingMeritsQueue.Count;)
                {
                    var pending = pendingMeritsQueue[i];

                    if (!DoesMeritMatch(runningTotal, pending.PPM))
                    {
                        System.Diagnostics.Debug.WriteLine($"[PPMerits] ReconcileInit: Dropping non-matching queued merit at index {i}");
                        pendingMeritsQueue.RemoveAt(i);
                        changed = true;
                        continue;
                    }

                    if (pending.PPM.TotalMerits > authoritativeTotal + MeritTolerance)
                    {
                        System.Diagnostics.Debug.WriteLine($"[PPMerits] ReconcileInit: Dropping overshoot queued merit at index {i}, Total={pending.PPM.TotalMerits}, Authoritative={authoritativeTotal}");
                        pendingMeritsQueue.RemoveAt(i);
                        changed = true;
                        continue;
                    }

                    AddMerit(pending.HE, pending.PPM, pending.SessionId);
                    runningTotal = pending.PPM.TotalMerits;
                    pendingMeritsQueue.RemoveAt(i);
                    changed = true;

                    if (Math.Abs(runningTotal - authoritativeTotal) < MeritTolerance)
                    {
                        if (pendingMeritsQueue.Count > 0)
                        {
                            changed = true;
                            pendingMeritsQueue.Clear();
                        }

                        break;
                    }
                }

                if (pendingMeritsQueue.Count > 0)
                {
                    changed = true;
                    pendingMeritsQueue.Clear();
                }
            }

            int previousSessionId = nextSessionId - 1;
            if (previousSessionId >= 0)
            {
                var previousSessionRows = meritRows.Where(r => r.SessionId == previousSessionId)
                                                  .OrderBy(r => r.TimeUTC)
                                                  .ToList();

                if (previousSessionRows.Count > 0)
                {
                    var lastRow = previousSessionRows[previousSessionRows.Count - 1];
                    if (Math.Abs(lastRow.TotalMerits - authoritativeTotal) >= MeritTolerance)
                    {
                        int lastMatchingIndex = previousSessionRows.FindLastIndex(r => Math.Abs(r.TotalMerits - authoritativeTotal) < MeritTolerance);
                        if (lastMatchingIndex >= 0 && lastMatchingIndex < previousSessionRows.Count - 1)
                        {
                            var rowsToRemove = new HashSet<MeritRow>(previousSessionRows.Skip(lastMatchingIndex + 1));
                            int removed = meritRows.RemoveAll(r => rowsToRemove.Contains(r));
                            if (removed > 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"[PPMerits] ReconcileInit: Removed {removed} tail merit(s) from previous session to match authoritative total {authoritativeTotal}");
                                changed = true;
                            }
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Remove expired entries (older than 10 minutes) and duplicates (total <= current tracked total).
        /// </summary>
        private void CleanupPendingQueue()
        {
            var now = DateTime.UtcNow;

            for (int i = pendingMeritsQueue.Count - 1; i >= 0; i--)
            {
                var pending = pendingMeritsQueue[i];

                // Remove if older than expiry time
                if (now - pending.QueuedAtUTC > QueueExpiryTime)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits] CleanupPendingQueue: EXPIRED - Removing merit at index {i}, Age={now - pending.QueuedAtUTC}, MeritsGained={pending.PPM.MeritsGained}, TotalMerits={pending.PPM.TotalMerits}");
                    pendingMeritsQueue.RemoveAt(i);
                    continue;
                }

                // Remove if total is <= current tracked total (likely a duplicate)
                if (pending.PPM.TotalMerits <= trackedMeritTotal)
                {
                    System.Diagnostics.Debug.WriteLine($"[PPMerits] CleanupPendingQueue: DUPLICATE - Removing merit at index {i}, TotalMerits={pending.PPM.TotalMerits} <= TrackedTotal={trackedMeritTotal}, MeritsGained={pending.PPM.MeritsGained}");
                    pendingMeritsQueue.RemoveAt(i);
                    continue;
                }
            }
        }

        private int CountGreedyMatches(int startIndex, double currentTotal)
        {
            int matches = 0;

            for (int i = startIndex; i < pendingMeritsQueue.Count; i++)
            {
                var pending = pendingMeritsQueue[i];
                if (DoesMeritMatch(currentTotal, pending.PPM))
                {
                    matches++;
                    currentTotal = pending.PPM.TotalMerits;
                }
            }

            return matches;
        }

        private bool DoesMeritMatch(double currentTotal, JournalPowerplayMerits ppm)
        {
            return Math.Abs((currentTotal + ppm.MeritsGained) - ppm.TotalMerits) < MeritTolerance;
        }

        #endregion

        #region Helper Methods

        private void UpdateDuplicateTailWarning()
        {
            bool showWarning = false;

            if (meritRows.Count >= 2)
            {
                int last = meritRows.Count - 1;
                showWarning = meritRows[last].MeritsGained == meritRows[last - 1].MeritsGained;
            }

            labelDuplicateWarning.Visible = showWarning;
        }

        private string ComputeCycleKey(DateTime timeUtc)
        {
            // Calculate days since the most recent Thursday (or today if today is Thursday)
            int daysFromThursday = ((int)timeUtc.Date.DayOfWeek - (int)DayOfWeek.Thursday + 7) % 7;
            var thursday = timeUtc.Date.AddDays(-daysFromThursday);

            // If it's Thursday but before 07:00 UTC, use the previous week's Thursday
            if (daysFromThursday == 0 && timeUtc.TimeOfDay < TimeSpan.FromHours(7))
            {
                thursday = thursday.AddDays(-7);
            }

            return $"{thursday:yyyy-MM-dd}";
        }

        private bool GetDefaultExpandedState(int levelIndex, dynamic group, HashSet<string> defaultExpandCycles)
        {
            if (levelIndex == 0 && checkCycle.Checked)
            {
                // At cycle level: only expand current and previous cycles
                return defaultExpandCycles.Contains(((IEnumerable<MeritRow>)group.Rows).First().CycleKey);
            }
            
            // At session/system level: expand sessions by default
            return levelIndex == 1;
        }

        private bool IsSystemLevelGroupRow(GroupRow groupRow)
        {
            if (groupRow == null || !checkSystem.Checked)
                return false;

            int systemLevel = (checkCycle.Checked ? 1 : 0) + (checkSession.Checked ? 1 : 0);
            return groupRow.Level == systemLevel;
        }

        private List<MeritRow> GetMeritsForGroupPath(string groupPath)
        {
            var pathParts = groupPath.Split('|');
            var filtered = meritRows.AsEnumerable();

            int levelIndex = 0;
            if (checkCycle.Checked && levelIndex < pathParts.Length)
            {
                string cycleKey = pathParts[levelIndex];
                filtered = filtered.Where(m => m.CycleKey == cycleKey);
                levelIndex++;
            }

            if (checkSession.Checked && levelIndex < pathParts.Length)
            {
                string sessionKey = pathParts[levelIndex];
                int sessionId = int.Parse(sessionKey.Replace("Session ", ""));
                filtered = filtered.Where(m => m.SessionId == sessionId);
                levelIndex++;
            }

            if (checkSystem.Checked && levelIndex < pathParts.Length)
            {
                string systemName = pathParts[levelIndex];
                filtered = filtered.Where(m => (string.IsNullOrEmpty(m.SystemName) ? "Unknown System" : m.SystemName) == systemName);
            }

            return filtered.ToList();
        }

        private string GenerateMeritsReport(List<MeritRow> merits)
        {
            var sb = new StringBuilder();
            string cycleHeader = merits.Any() 
                ? $"Cycle {merits.OrderBy(m => m.TimeUTC).First().CycleKey}" 
                : "Unknown Cycle";

            sb.AppendLine($"__**Powerplay report for {cycleHeader}**__");
            sb.AppendLine();

            // Group by system
            var systemGroups = merits
                .GroupBy(m => string.IsNullOrEmpty(m.SystemName) ? "Unknown System" : m.SystemName)
                .OrderBy(g => g.Key);

            foreach (var sysGroup in systemGroups)
            {
                sb.AppendLine($"> **{sysGroup.Key}**");
                sb.AppendLine($">    Merits Earned : {sysGroup.Sum(m => m.MeritsGained):N0}");

                var (itemsDelivered, itemsCollected) = GetPowerplayItems(sysGroup);

                if (itemsDelivered.Any())
                {
                    sb.AppendLine(">    Items Delivered :");
                    foreach (var item in itemsDelivered.OrderBy(kvp => kvp.Key))
                    {
                        sb.AppendLine($">        {item.Key} | {item.Value:N0}");
                    }
                }

                if (itemsCollected.Any())
                {
                    sb.AppendLine(">    Items Collected :");
                    foreach (var item in itemsCollected.OrderBy(kvp => kvp.Key))
                    {
                        sb.AppendLine($">        {item.Key} | {item.Value:N0}");
                    }
                }
            }

            return sb.ToString();
        }

        private (Dictionary<string, int> delivered, Dictionary<string, int> collected) GetPowerplayItems(IGrouping<string, MeritRow> systemGroup)
        {
            var delivered = new Dictionary<string, int>();
            var collected = new Dictionary<string, int>();

            foreach (var merit in systemGroup.Where(m => m.HE != null))
            {
                var nearbyEntries = DiscoveryForm.History.EntryOrder()
                    .Where(he => Math.Abs((he.EventTimeUTC - merit.HE.EventTimeUTC).TotalMinutes) < 5)
                    .Where(he => he.System?.Name == merit.SystemName || string.IsNullOrEmpty(merit.SystemName));

                foreach (var he in nearbyEntries)
                {
                    switch (he.journalEntry)
                    {
                        case JournalPowerplayCollect collect:
                            AddOrUpdate(collected, collect.Type_Localised, collect.Count);
                            break;
                        case JournalPowerplayDeliver deliver:
                            AddOrUpdate(delivered, deliver.Type_Localised, deliver.Count);
                            break;
                    }
                }
            }

            return (delivered, collected);
        }

        private void AddOrUpdate(Dictionary<string, int> dict, string key, int value)
        {
            if (dict.ContainsKey(key))
                dict[key] += value;
            else
                dict[key] = value;
        }

        #endregion

    }
}
