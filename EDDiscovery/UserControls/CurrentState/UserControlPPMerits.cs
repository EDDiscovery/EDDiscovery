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

        #endregion

        #region Initialization

        public UserControlPPMerits()
        {
            InitializeComponent();
            DBBaseName = "PPMerits";

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
        }

        protected override void Init()
        {
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

            if (he.journalEntry is JournalPowerplayMerits ppm)
            {
                AddMerit(he, ppm);
                // Debounce redraws to avoid flicker on burst updates
                pendingRedraw = true;
                redrawDebounceTimer.Stop();
                redrawDebounceTimer.Start();
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
            meritRows.Clear();
            nextSessionId = 0;

            foreach (var he in DiscoveryForm.History.EntryOrder())
            {
                if (he.journalEntry is JournalLoadGame)
                {
                    nextSessionId++;
                }

                if (he.journalEntry is JournalPowerplayMerits ppm)
                {
                    AddMerit(he, ppm);
                }
            }

            Redraw();
        }

        private void AddMerit(HistoryEntry he, JournalPowerplayMerits ppm)
        {
            var row = new MeritRow
            {
                TimeUTC = he.EventTimeUTC,
                Power = ppm.Power,
                MeritsGained = (int)ppm.MeritsGained,
                TotalMerits = (int)ppm.TotalMerits,
                SystemName = he.System != null ? he.System.Name : string.Empty,
                CycleKey = ComputeCycleKey(he.EventTimeUTC),
                SessionId = nextSessionId,
                HE = he
            };
            meritRows.Add(row);
            lastAddedMeritTime = row.TimeUTC;
        }

        #endregion

        #region Rendering

        private void Redraw()
        {
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
                RequestPanelOperationOpen(PanelInformation.PanelIDs.TravelGrid, new RequestTravelToJID { JID = rightClickMeritRow.HE.Journalid, MakeVisible = true });
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

        #region Helper Methods

        private string ComputeCycleKey(DateTime timeUtc)
        {
            var monday = timeUtc.Date.AddDays(-(int)timeUtc.Date.DayOfWeek + (int)DayOfWeek.Monday);
            return $"{monday:yyyy-MM-dd}";
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
