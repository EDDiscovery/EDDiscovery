/*
 * Copyright © 2016 - 2023 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    /// <summary>
    /// Defines a class for extending IEnumerable functionalities. (Chunking)
    /// </summary>
    public static class EnumerableExtensions
    {
        
        /// <summary>
        /// Splits the source enumerable into chunks of a specified size.
        /// </summary>
        /// <typeparam name="T">The type of items in the enumerable.</typeparam>
        /// <param name="source">The source enumerable to split into chunks.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>An enumerable of chunks containing items from the source.</returns>
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            // Throws an exception if the chunk size is not positive.
            if (chunkSize <= 0)
                throw new ArgumentException("Chunk size must be greater than 0.", nameof(chunkSize));

            // Initializes a new list to hold the current chunk of items.
            var chunk = new List<T>(chunkSize);

            // Iterates over each item in the source enumerable.
            foreach (var item in source)
            {
                // Adds the current item to the chunk.
                chunk.Add(item);
                // If the chunk has reached the specified size, yield return it and start a new chunk.
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk = new List<T>(chunkSize);
                }
            }

            // If there are any items left in the chunk after iterating through the source, yield return it.
            if (chunk.Any())
            {
                yield return chunk;
            }
        }
    }
    
    /// <summary>
    /// UserControlJournalGrid is responsible for displaying the journal entries in a grid format.
    /// It allows filtering, searching, and sorting of journal entries based on various criteria.
    /// </summary>
    public partial class UserControlJournalGrid : UserControlCommonBase
    {
        private JournalFilterSelector cfs;
        private BaseUtils.ConditionLists fieldfilter = new BaseUtils.ConditionLists();
        private Dictionary<long, DataGridViewRow> rowsbyjournalid = new Dictionary<long, DataGridViewRow>();

        private static readonly string dbFilter = "EventFilter2";
        private static readonly string dbHistorySave = "EDUIHistory";
        private static readonly string dbFieldFilter = "ControlFieldFilter";
        private static readonly string dbUserGroups = "UserGroups";

        public event Action OnPopOut;

        private HistoryList current_historylist; // the last one set, for internal refresh purposes on sort

        private string searchterms = "system:body:station:stationfaction";

        #region Init

        private static class Columns
        {
            public const int Time = 0;
            public const int Event = 1;
            public const int Type = 2;
            public const int Text = 3;
        }

        private Timer searchtimer;
        private Timer todotimer;
        private Queue<Action> todo = new Queue<Action>();
        private Queue<HistoryEntry> queuedadds = new Queue<HistoryEntry>();

        private int fdropdown; // filter totals

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlJournalGrid"/> class.
        /// </summary>
        public UserControlJournalGrid()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes and sets up the journal grid.
        /// </summary>
        public override void Init()
        {
            DBBaseName = "JournalGrid";

            SetupDataGridView();
            SetupFilterSelector();
            SetupTimers();
            SubscribeToEvents();
            ApplyTranslations();
            InitializeComboBox();
        }

        /// <summary>
        /// Sets up the data grid view.
        /// </summary>
        private void SetupDataGridView()
        {
            dataGridViewJournal.MakeDoubleBuffered();
            dataGridViewJournal.RowTemplate.MinimumHeight = 26; // enough for the icon
            dataGridViewJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewJournal.AllowRowHeaderVisibleSelection = true;
        }

        /// <summary>
        /// Sets up the filter selector.
        /// </summary>
        private void SetupFilterSelector()
        {
            cfs = new JournalFilterSelector();
            cfs.UC.AddAllNone();
            cfs.AddJournalExtraOptions();
            cfs.AddJournalEntries();
            cfs.AddUserGroups(GetSetting(dbUserGroups, ""));
            cfs.SaveSettings += EventFilterChanged;
            checkBoxCursorToTop.Checked = true;

            string filter = GetSetting(dbFieldFilter, "");
            if (!string.IsNullOrEmpty(filter))
                fieldfilter.FromJSON(filter); // load filter
        }


        /// <summary>
        /// Sets up the timers (To-Do and Search)
        /// </summary>
        private void SetupTimers()
        {
            searchtimer = new Timer { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

            todotimer = new Timer { Interval = 10 };
            todotimer.Tick += Todotimer_Tick;
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void SubscribeToEvents()
        {
            DiscoveryForm.OnHistoryChange += HistoryChanged;
            DiscoveryForm.OnNewEntry += AddNewEntry;
        }

        /// <summary>
        /// Applies translations to UI elements based on the current language settings.
        /// </summary>
        /// </summary>
        private void ApplyTranslations()
        {
            var enumlist = new Enum[] { EDTx.UserControlJournalGrid_ColumnTime, EDTx.UserControlJournalGrid_ColumnEvent, EDTx.UserControlJournalGrid_ColumnDescription, 
                EDTx.UserControlJournalGrid_ColumnInformation, EDTx.UserControlJournalGrid_labelTime, EDTx.UserControlJournalGrid_labelSearch };

            var enumlistcms = new Enum[] { EDTx.UserControlJournalGrid_removeSortingOfColumnsToolStripMenuItem, EDTx.UserControlJournalGrid_jumpToEntryToolStripMenuItem, 
                EDTx.UserControlJournalGrid_mapGotoStartoolStripMenuItem, EDTx.UserControlJournalGrid_viewOnEDSMToolStripMenuItem,
                EDTx.UserControlJournalGrid_viewOnSpanshToolStripMenuItem,
                EDTx.UserControlJournalGrid_viewScanDisplayToolStripMenuItem,
                EDTx.UserControlJournalGrid_toolStripMenuItemStartStop, 
                EDTx.UserControlJournalGrid_runActionsOnThisEntryToolStripMenuItem, EDTx.UserControlJournalGrid_copyJournalEntryToClipboardToolStripMenuItem };

            var enumlisttt = new Enum[] { EDTx.UserControlJournalGrid_comboBoxTime_ToolTip, EDTx.UserControlJournalGrid_textBoxSearch_ToolTip, 
                EDTx.UserControlJournalGrid_buttonFilter_ToolTip, EDTx.UserControlJournalGrid_buttonExtExcel_ToolTip, EDTx.UserControlJournalGrid_checkBoxCursorToTop_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(historyContextMenu, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
        }

        /// <summary>
        /// Initializes the combo box.
        /// </summary>
        private void InitializeComboBox()
        {
            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbHistorySave, ""), true, true, true);

            if (TranslatorExtensions.TxDefined(EDTx.UserControlTravelGrid_SearchTerms)) // if translator has it defined, use it (share with travel grid)
                searchterms = searchterms.TxID(EDTx.UserControlTravelGrid_SearchTerms);
        }

        /// <summary>
        /// Loads the layout of the journal grid.
        /// </summary>
        public override void LoadLayout()
        {
            dataGridViewJournal.RowTemplate.MinimumHeight = Math.Max(28, Font.ScalePixels(28));
            DGVLoadColumnLayout(dataGridViewJournal, rowheaderselection: dataGridViewJournal.AllowRowHeaderVisibleSelection);
        }

        /// <summary>
        /// Handles the Closing event of the UserControlJournalGrid control.
        /// </summary>
        public override void Closing()
        {
            todo.Clear();
            todotimer.Stop();
            searchtimer.Stop();
            DGVSaveColumnLayout(dataGridViewJournal);
            PutSetting(dbUserGroups, cfs.GetUserGroups());
            DiscoveryForm.OnHistoryChange -= HistoryChanged;
            DiscoveryForm.OnNewEntry -= AddNewEntry;
            searchtimer.Dispose();
        }

        #endregion

        #region Display

        /// <summary>
        /// The initial display of the journal grid.
        /// </summary>
        public override void InitialDisplay()
        {
            Display(DiscoveryForm.History, false);
        }


        /// <summary>
        /// Handles the HistoryChange event of the DiscoveryForm control.
        /// </summary>
        private void HistoryChanged()
        {
            Display(DiscoveryForm.History, false);
        }

        /// <summary>
        /// Displays the specified history list in the journal grid.
        /// </summary>
        /// <param name="hl">The history list to display.</param>
        /// <param name="disablesorting">if set to <c>true</c> [disablesorting].</param>
        private void Display(HistoryList hl, bool disablesorting)
        {
            todo.Clear(); // Ensure the todo queue is in a quiet state before proceeding
            queuedadds.Clear(); // Clear any queued additions
            todotimer.Stop(); // Stop the timer to process todo actions

            this.dataGridViewJournal.Cursor = Cursors.WaitCursor; // Set cursor to wait state during operation

            // Disable buttons during display operation and store their enabled state
            buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = false;
            bool enableButtons = true;

            current_historylist = hl; // Cache the current history list

            // Get the current selected position in the DataGridView, if any
            var selpos = dataGridViewJournal.GetSelectedRowOrCellPosition();
            Tuple<long, int> pos = selpos != null ? Tuple.Create(((HistoryEntry)(dataGridViewJournal.Rows[selpos.Item1].Tag)).Journalid, selpos.Item2) : Tuple.Create(-1L, 0);

            // Handle sorting state
            SortOrder sortorder = dataGridViewJournal.SortOrder;
            int sortcol = dataGridViewJournal.SortedColumn?.Index ?? -1;
            if (sortcol >= 0 && disablesorting)
            {
                dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = SortOrder.None; // Clear sort direction if sorting is disabled
                sortcol = -1; // Reset sort column index
            }

            // Apply time filter to history entries
            var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
            List<HistoryEntry> result = filter.Filter(hl.EntryOrder());
            fdropdown = hl.Count - result.Count(); // Update filter dropdown count

            // Clear existing rows and reset journal ID map
            dataGridViewJournal.Rows.Clear();
            rowsbyjournalid.Clear();

            // Update time column header based on user settings
            dataGridViewJournal.Columns[0].HeaderText = EDDConfig.Instance.GetTimeTitle();

            // Prepare search terms and event filter
            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);
            HistoryEventFilter hef = new HistoryEventFilter(GetSetting(dbFilter, "All"), fieldfilter, DiscoveryForm.Globals);

            System.Diagnostics.Stopwatch swtotal = System.Diagnostics.Stopwatch.StartNew(); // Start stopwatch for performance measurement

            // Process filtered history entries in chunks for efficiency
            foreach (var chunk in result.ChunkBy(500))
            {
                todo.Enqueue(() =>
                {
                    var rowstoadd = chunk.Select(item => CreateHistoryRow(item, sst, hef)).Where(row => row != null).ToArray();
                    dataGridViewJournal.Rows.AddRange(rowstoadd); // Add rows to DataGridView in a single operation

                    if (dataGridViewJournal.SelectAndMove(rowsbyjournalid, ref pos, false))
                        FireChangeSelection(); // Fire selection change event if necessary
                });
            }

            // Final todo action to update UI state after all entries have been processed
            todo.Enqueue(() =>
            {
                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCount + " JG TOTAL TIME " + swtotal.ElapsedMilliseconds); // Log total processing time

                UpdateToolTipsForFilter(); // Update tooltips based on the current filter

                if (dataGridViewJournal.SelectAndMove(rowsbyjournalid, ref pos, true))
                    FireChangeSelection(); // Fire selection change event if necessary

                if (sortcol >= 0)
                {
                    dataGridViewJournal.Sort(dataGridViewJournal.Columns[sortcol], sortorder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridViewJournal.Columns[sortcol].HeaderCell.SortGlyphDirection = sortorder; // Update sort glyph
                }

                this.dataGridViewJournal.Cursor = Cursors.Default; // Restore cursor to default state
                buttonExtExcel.Enabled = buttonFilter.Enabled = buttonField.Enabled = enableButtons;

                while (queuedadds.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("JG Dequeue paused adds");
                    AddEntry(queuedadds.Dequeue()); // Add each queued entry
                }
            });

            todotimer.Start(); // Restart the todo timer to process queued actions
        }
        
        /// <summary>
        /// Handles the Tick event of the timer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Todotimer_Tick(object sender, EventArgs e)
        {
            // If there are actions in the queue, dequeue and execute the next action
            if (todo.Count != 0)
            {
                var act = todo.Dequeue();
                act();
            }
            else
            {
                // If the queue is empty, stop the timer
                todotimer.Stop();
            }
        }

        /// <summary>
        /// Adds a new entry to the history.
        /// </summary>
        /// <param name="he">The history entry to add.</param>
        private void AddNewEntry(HistoryEntry he)               // on new entry from discovery system
        {
            // If the todo timer is running, add the entry to the queue
            if (todotimer.Enabled)       // if we have the todotimer running.. we add to the queue.  better than the old loadcomplete, no race conditions
            {
                queuedadds.Enqueue(he);
            }
            else
            {
                // If the timer is not running, add the entry immediately
                AddEntry(he);
            }
        }

       
        /// <summary>
        /// Adds a new entry to the journal grid.
        /// </summary>
        /// <param name="he">The history entry to add.</param>
        private void AddEntry(HistoryEntry he)
        { 
            // Prepare search terms and event filter for the new entry
            var sst = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);
            HistoryEventFilter hef = new HistoryEventFilter(GetSetting(dbFilter, "All"), fieldfilter, DiscoveryForm.Globals);

            // Create a row for the new entry if it passes the search and filter criteria
            var row = CreateHistoryRow(he, sst, hef);     // we might be filtered out by search
            if (row != null)
            { 
                // If the row is not null, insert it at the beginning of the DataGridView
                dataGridViewJournal.Rows.Insert(0, row);

                // Check if a maximum number of items is set and remove excess items if necessary
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;

                if (filter.MaximumNumberOfItems.HasValue)        // this one won't remove the latest one
                {
                    int rowsToRemove = dataGridViewJournal.Rows.Count - filter.MaximumNumberOfItems.Value;
                    while (rowsToRemove-- > 0)
                    {
                        dataGridViewJournal.Rows.RemoveAt(dataGridViewJournal.Rows.Count - 1);
                    }
                }

                // If the option is selected, move the cursor to the top of the DataGridView
                if (checkBoxCursorToTop.Checked) // Move focus to first row
                {
                    dataGridViewJournal.ClearSelection();
                    dataGridViewJournal.SetCurrentAndSelectAllCellsOnRow(0);       // its the current cell which needs to be set, moves the row marker as well
                    FireChangeSelection();
                }
            }
        }
        /// <summary>
        /// Creates a DataGridViewRow for a given history entry based on search and filter criteria.
        /// </summary>
        /// <param name="he">The history entry to create a row for.</param>
        /// <param name="search">The search terms to match against the history entry.</param>
        /// <param name="hef">The event filter to apply to the history entry.</param>
        /// <returns>A DataGridViewRow representing the history entry, or null if the entry does not match the search and filter criteria.</returns>
        private DataGridViewRow CreateHistoryRow(HistoryEntry he, BaseUtils.StringSearchTerms search, HistoryEventFilter hef)
        {
            // Check if the history entry is included in the event filter
            if (!hef.IsIncluded(he)) return null;

            // Convert the event time to the selected time zone
            DateTime time = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC);
            // Fill in the event description and detailed information
            he.FillInformation(out string eventDescription, out string eventDetailedInfo);
            // Combine the description and detailed info with a newline separator
            string detail = $"{eventDescription}{Environment.NewLine}{eventDetailedInfo}";

            // If search is enabled and the entry does not match the search terms, return null
            if (search.Enabled && !IsSearchMatch(he, time, detail, search))
            {
                return null;
            }

            // Clone the row template and create cells for the history entry
            var rw = dataGridViewJournal.RowTemplate.Clone() as DataGridViewRow;
            rw.CreateCells(dataGridViewJournal, time, "", he.EventSummary, detail);
            // Tag the row with the history entry for future reference
            rw.Tag = he;
            // Map the row to the journal ID for quick access
            rowsbyjournalid[he.Journalid] = rw;

            return rw;
        }

        /// <summary>
        /// Determines if a history entry matches the given search terms.
        /// </summary>
        /// <param name="he">The history entry to check.</param>
        /// <param name="time">The time of the history entry.</param>
        /// <param name="detail">The combined event description and detailed information.</param>
        /// <param name="search">The search terms to match against.</param>
        /// <returns>True if the history entry matches the search terms, otherwise false.</returns>
        private bool IsSearchMatch(HistoryEntry he, DateTime time, string detail, BaseUtils.StringSearchTerms search)
        {
            // Determine the entry number string based on row order setting
            string entryNumberStr = EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber.ToStringInvariant() : (DiscoveryForm.History.Count - he.EntryNumber + 1).ToStringInvariant();
            // Check if any search term matches the history entry's time, summary, detail, entry number, or related names
            return search.Terms.Any(term => term != null && (
                time.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                he.EventSummary.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                detail.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                entryNumberStr.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                he.System.Name.WildCardMatch(term, true) ||
                he.Status.BodyName?.WildCardMatch(term, true) == true ||
                he.Status.StationName?.WildCardMatch(term, true) == true ||
                he.Status.StationFaction?.WildCardMatch(term, true) == true
            ));
        }
       
        // Updates the tooltip for the filter dropdown based on the current filter state
        private void UpdateToolTipsForFilter()
        {
            // Construct the message suffix indicating the number of rows displayed and total entries
            string messageSuffix = $" showing {dataGridViewJournal.Rows.Count} original {current_historylist?.Count ?? 0}".T(EDTx.UserControlJournalGrid_TT1);
            // Determine the tooltip text based on whether a filter is applied
            string tooltipText = fdropdown > 0 ? $"Filtered {fdropdown}{messageSuffix}".T(EDTx.UserControlJournalGrid_TTFilt1) : $"Select the entries by age, {messageSuffix}".T(EDTx.UserControlJournalGrid_TTSelAge);
            // Set the tooltip dynamically on the comboBoxTime control
            comboBoxTime.SetTipDynamically(toolTip, tooltipText);
        }

        #endregion

        #region Buttons

        /// <summary>
        /// Event handler for the filter button click
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            // Open the filter selection dialog
            cfs.Open(GetSetting(dbFilter, "All"), sender as Button, this.FindForm());
        }

        /// <summary>
        /// Handles the click event of the filter button, opening the filter selection dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void EventFilterChanged(string newset, Object e)
        {
            // Check if the new filter setting is different from the current one and update it
            if (GetSetting(dbFilter, "All") != newset)
            {
                PutSetting(dbFilter, newset);
                // Refresh the display with the new filter applied
                Display(current_historylist, false);
            }
        }

        /// <summary>
        /// Event handler for the search timer tick.
        /// Stops the search timer and performs the search/display update.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            // Stop the search timer and perform the search/display update
            searchtimer.Stop();
            Display(current_historylist, false);
        }
        
        /// <summary>
        /// Handles the text changed event for the search text box.
        /// Restarts the search timer to delay the search operation until typing has paused.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // Restart the search timer to delay search until typing has paused
            searchtimer.Stop(); // Stop the timer
            searchtimer.Start(); // Start the timer
        }

        /// <summary>
        /// Event handler for when the selected index of the journal window combo box changes.
        /// Saves the new journal window setting and refreshes the display.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void comboBoxJournalWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the new journal window setting and refresh the display
            PutSetting(dbHistorySave, comboBoxTime.Text);
            Display(current_historylist, false);
        }

        /// <summary>
        /// Event handler for the field filter button click.
        /// Shows the field filter dialog and updates the filter if a new one is selected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void buttonField_Click(object sender, EventArgs e)
        {
            // Show the field filter dialog and update the filter if a new one is selected
            var result = HistoryFilterHelpers.ShowDialog(FindForm(), fieldfilter, DiscoveryForm, "Journal: Filter out fields".T(EDTx.UserControlJournalGrid_JHF));
            if (result != null)
            {
                fieldfilter = result;
                // Save the new field filter setting and refresh the display
                PutSetting(dbFieldFilter, fieldfilter.GetJSON());
                Display(current_historylist, false);
            }
        }
        #endregion
        
        /// <summary>
        /// Handles the painting of rows in the journal DataGridView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void dataGridViewJournal_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Check if the row's tag contains a HistoryEntry object
            if (dataGridViewJournal.Rows[e.RowIndex].Tag is HistoryEntry he)
            {
                // Calculate the row number based on the configuration and entry number
                int rowno = EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (DiscoveryForm.History.Count - he.EntryNumber + 1);
                // Paint the event column for the row
                PaintHelpers.PaintEventColumn(dataGridViewJournal, e, rowno, he, Columns.Event, false);
            }
        }

        #region Mouse Clicks

        /// <summary>
        /// Prepares the context menu before opening based on the selected history entry
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // Check if a history entry is selected with a right click
            bool hasRightClickHe = rightclickhe != null;
            // Enable or disable menu items based on whether a history entry is selected
            toolStripMenuItemStartStop.Enabled = hasRightClickHe;
            mapGotoStartoolStripMenuItem.Enabled = hasRightClickHe && rightclickhe.System.HasCoordinate;
            viewOnEDSMToolStripMenuItem.Enabled = hasRightClickHe;
            viewOnSpanshToolStripMenuItem.Enabled = hasRightClickHe;
            viewScanDisplayToolStripMenuItem.Enabled = hasRightClickHe;
            removeSortingOfColumnsToolStripMenuItem.Enabled = dataGridViewJournal.SortedColumn != null;
            jumpToEntryToolStripMenuItem.Enabled = dataGridViewJournal.Rows.Count > 0;
        }

        // Variables to hold the history entry for right and left clicks
        HistoryEntry rightclickhe = null;
        HistoryEntry leftclickhe = null;

        
        /// <summary>
        /// Handles mouse down events on the journal DataGridView to determine selected history entries
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void dataGridViewJournal_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine the history entry for right and left clicks based on the row validity
            rightclickhe = dataGridViewJournal.RightClickRowValid ? dataGridViewJournal.Rows[dataGridViewJournal.RightClickRow].Tag as HistoryEntry : null;
            leftclickhe = dataGridViewJournal.LeftClickRowValid ? dataGridViewJournal.Rows[dataGridViewJournal.LeftClickRow].Tag as HistoryEntry : null;
        }

        /// <summary>
        /// Handles double-click events on cells in the journal DataGridView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void dataGridViewJournal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the double-clicked row is valid and a history entry is selected
            if (dataGridViewJournal.LeftClickRowValid && leftclickhe != null) 
            {
                // Create and display an info form with details about the selected history entry
                ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                leftclickhe.FillInformation(out string EventDescription, out string EventDetailedInfo);
                string infodetailed = EventDescription.AppendPrePad(EventDetailedInfo, Environment.NewLine);
                info.Info(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(leftclickhe.EventTimeUTC) + ": " + leftclickhe.EventSummary,
                    FindForm().Icon, infodetailed);
                info.Size = new Size(1200, 800);
                info.Show(FindForm());
            }
        }
        
        /// <summary>
        /// Handles single cell click events in the journal DataGridView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void dataGridViewJournal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FireChangeSelection(); // Trigger selection change event
        }

        /// <summary>
        /// Opens the 3D map at the system of the right-clicked history entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscoveryForm.Open3DMap(rightclickhe?.System); // Open 3D map for selected system
        }

        /// <summary>
        /// Displays the scan or market form for the right-clicked history entry
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void viewScanDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), rightclickhe, DiscoveryForm.History); // Show scan or market form     
        }


        /// <summary>
        /// Handles the click event for the EDSM menu item.
        /// Opens the EDSM page for the system of the right-clicked history entry.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null) // Check if a history entry is selected
            {
                EDSMClass edsm = new EDSMClass(); // Create a new EDSM class instance
                // Attempt to show the system in EDSM and display a message if unsuccessful
                if (!edsm.ShowSystemInEDSM(rightclickhe.System.Name))
                    ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlJournalGrid_NotSynced));
            }
        }

        /// <summary>
        /// Handles the click event for the Spansh menu item.
        /// Opens the Spansh website for the system of the right-clicked history entry.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the system has a valid address and launch the browser for it
            if (rightclickhe?.System.SystemAddress.HasValue == true)
                EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(rightclickhe.System.SystemAddress.Value);
        }

        /// <summary>
        /// Toggles the start/stop flag for the right-clicked history entry and refreshes the display
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null) // Check if a history entry is selected
            {
                this.dataGridViewJournal.Cursor = Cursors.WaitCursor; // Set cursor to wait

                rightclickhe.SetStartStop(); // Toggle start/stop flag
                DiscoveryForm.History.RecalculateTravel(); // Recalculate travel history

                // Update display for affected rows
                foreach (DataGridViewRow row in dataGridViewJournal.Rows) 
                {
                    HistoryEntry he = row.Tag as HistoryEntry;
                    if (he != null && (he.IsFSD || he.StopMarker || he == rightclickhe))
                    {
                        he.FillInformation(out string eventdescription, out string unuseddetailinfo); // Recalculate information
                        row.Cells[ColumnInformation.Index].Value = eventdescription; // Update cell value
                    }
                }

                dataGridViewJournal.Refresh(); // Refresh DataGridView to show changes

                RequestPanelOperation(this, new TravelHistoryStartStopChanged()); // Notify other components

                this.dataGridViewJournal.Cursor = Cursors.Default; // Reset cursor
            }
        }

        /// <summary>
        /// Executes configured actions for the right-clicked history entry
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void runActionsOnThisEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null) // Check if a history entry is selected
                DiscoveryForm.ActionRunOnEntry(rightclickhe, Actions.ActionEventEDList.UserRightClick(rightclickhe)); // Run actions
        }

        
        /// <summary>
        /// Copies the journal entry of the right-clicked history entry to the clipboard
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe?.journalEntry != null) // Check if a journal entry is associated
            {
                string json = rightclickhe.journalEntry.GetJsonString(); // Get JSON string
                if (!string.IsNullOrEmpty(json))
                {
                    SetClipboardText(json); // Copy to clipboard
                }
            }
        }

        /// <summary>
        /// Removes sorting from columns in the DataGridView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display(current_historylist, true); // Display history list without sorting
        }
        #endregion

        /// <summary>
        /// Performs a panel operation based on the specified action object
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="actionobj">The action object to perform.</param>
        /// <returns>The state of the panel operation.</returns>
        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            // Determine if the action object is of type TravelHistoryStartStopChanged and return the appropriate PanelActionState
            return actionobj is UserControlCommonBase.TravelHistoryStartStopChanged ? PanelActionState.HandledContinue : PanelActionState.NotHandled;
        }

        /// <summary>
        /// Placeholder for future implementation
        /// </summary>
        public void FireChangeSelection() // Placeholder for future implementation
        {

        }

       
        /// <summary>
        /// Handles the jump to entry menu item click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void jumpToEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Calculate the entry index for the right-clicked history entry, defaulting to 0 if none is selected
            int curi = rightclickhe != null ? CalculateEntryIndex(rightclickhe) : 0;

            // Open the jump dialog and get the selected row index
            int selrow = dataGridViewJournal.JumpToDialog(this.FindForm(), curi, r => CalculateEntryIndex(r.Tag as HistoryEntry));

            // If a valid row is selected, select it in the DataGridView
            if (selrow >= 0)
            {
                SelectRow(selrow);
            }
        }

        /// <summary>
        /// Calculates the entry index for the specified history entry
        /// </summary>
        /// <param name="he">The history entry to calculate the index for.</param>
        /// <returns>The calculated entry index.</returns>
        private int CalculateEntryIndex(HistoryEntry he)
        {
            // Calculate the entry index based on whether row order is inverted, defaulting to 0 if the history entry is null
            return he != null ? (EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (DiscoveryForm.History.Count - he.EntryNumber + 1)) : 0;
        }

        /// <summary>
        /// Selects the specified row in the DataGridView
        /// </summary>
        /// <param name="rowIndex">The index of the row to select.</param>
        private void SelectRow(int rowIndex)
        {
            // Clear any existing selection and select the specified row
            dataGridViewJournal.ClearSelection();
            dataGridViewJournal.SetCurrentAndSelectAllCellsOnRow(rowIndex);
            // Trigger any actions associated with changing the selection
            FireChangeSelection();
        }

        /// <summary>
        /// Handles the sort compare event for the DataGridView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void dataGridViewJournal_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // If the first column is being sorted, use the custom date sorting method
            if (e.Column.Index == 0)
            {
                e.SortDataGridViewColumnDate();
            }
        }

        #region Excel

        /// <summary>
        /// Handles the click event for the Excel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            // Initialize the export form with options for CSV and journal exports
            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export(new[] { "Export Current View", "Export as Journals" },
                new[] { Forms.ImportExportForm.ShowFlags.ShowDateTimeCSVOpenInclude, Forms.ImportExportForm.ShowFlags.ShowDateTimeOpenInclude },
                new[] { "CSV export| *.csv", "Journal Export|*.log" });

            // Show the export form dialog and proceed if the user confirms
            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                // Handle journal export
                if (frm.SelectedIndex == 1)
                {
                    try
                    {
                        // Open a stream writer to the specified path
                        using (StreamWriter writer = new StreamWriter(frm.Path))
                        {
                            // Iterate through all DataGridView rows
                            foreach (DataGridViewRow dgvr in dataGridViewJournal.Rows)
                            {
                                HistoryEntry he = dgvr.Tag as HistoryEntry;
                                // Check if the row is visible and within the specified time range
                                if (dgvr.Visible && he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 && he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0)
                                {
                                    // Prepare the journal entry for export, removing new lines and unnecessary spaces
                                    string forExport = he.journalEntry.GetJsonString().Replace("\r\n", "");
                                    if (!string.IsNullOrEmpty(forExport))
                                    {
                                        forExport = System.Text.RegularExpressions.Regex.Replace(forExport, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                                        // Write the formatted entry to the file
                                        writer.WriteLine(forExport);
                                    }
                                }
                            }
                        }

                        // Automatically open the file after export if specified
                        if (frm.AutoOpen)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(frm.Path);
                            }
                            catch
                            {
                                // Show an error message if the file fails to open
                                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to open " + frm.Path, "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                    catch
                    {
                        // Show an error message if the export fails
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else // Handle CSV export
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                    // Define how to determine the line status for each row
                    grd.GetLineStatus += (int r) =>
                    {
                        if (r < dataGridViewJournal.Rows.Count)
                        {
                            HistoryEntry he = dataGridViewJournal.Rows[r].Tag as HistoryEntry;
                            // Determine if the row should be included in the export
                            return (dataGridViewJournal.Rows[r].Visible &&
                                    he.EventTimeUTC.CompareTo(frm.StartTimeUTC) >= 0 &&
                                    he.EventTimeUTC.CompareTo(frm.EndTimeUTC) <= 0) ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.Skip;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    // Define how to retrieve data for each line
                    grd.GetLine += (int r) =>
                    {
                        DataGridViewRow rw = dataGridViewJournal.Rows[r];
                        // Return the data to be included in the CSV for this row
                        return new object[] { rw.Cells[0].Value, rw.Cells[2].Value, rw.Cells[3].Value };
                    };

                    // Define how to retrieve headers for the CSV file
                    grd.GetHeader += (int c) =>
                    {
                        // Return the header for each column, if headers are included
                        return (c < 3 && frm.IncludeHeader) ? dataGridViewJournal.Columns[c + ((c > 0) ? 1 : 0)].HeaderText : null;
                    };

                    // Write the CSV file
                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        #endregion

    }
}
