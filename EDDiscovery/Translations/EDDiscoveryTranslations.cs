using System;
using System.Collections.Generic;
using System.Globalization;

// procedure for translation normalisation
// 1. cd \code\eddiscovery.
// 2. Use eddtest scanforenums stdenums . *.cs  - check for any enums not in use
// 3. Use eddtest  normalisetranslate c:\code\eddiscovery\eddiscovery\translations 2 example-ex - "NS NoOutput" c:\code\renames.lst stdenums
//      check example against enum list and check all ids are there vs enums
// 4. run normalisetranslate.bat to fix up other languages to example

namespace EDDiscovery
{
    internal enum EDTx
    {
        Warning, // Warning
        Today, // Today
        t24h, // 24h
        t7days, // 7 days
        All, // All
        OK, // OK
        Cancel, // Cancel
        Delete, // Delete
        Campaign, // Campaign
        NoScan, // No Scan
        GameTime, // Game Time
        Time, // Time
        NoData, // No Data
        None, // None
        on, // on
        Off, // Off
        Unknown, // Unknown
        Information, // Information
        NoPos, // No Pos

        MessageBoxTheme_No,
        MessageBoxTheme_Yes,

        TBD, // TBD - to be done

        StartUp_DUPLOAD,        // EDDiscovery is already running. Launch anyway? in program.cs

        EDDiscoveryForm_addTabToolStripMenuItem, // ToolStrip control 'Insert Tab with panel..'
        EDDiscoveryForm_removeTabToolStripMenuItem, // ToolStrip control 'Remove Tab'
        EDDiscoveryForm_renameTabToolStripMenuItem, // ToolStrip control 'Rename Tab'
        EDDiscoveryForm_popOutPanelToolStripMenuItem, // ToolStrip control 'Pop Out Panel..'
        EDDiscoveryForm_helpTabToolStripMenuItem, // ToolStrip control 'Help'
        EDDiscoveryForm_notifyIconMenu_Open, // ToolStrip control '&Open EDDiscovery'
        EDDiscoveryForm_notifyIconMenu_Hide, // ToolStrip control '&Hide Tray Icon'
        EDDiscoveryForm_notifyIconMenu_Exit, // ToolStrip control 'E&xit'
        EDDiscoveryForm_toolsToolStripMenuItem, // ToolStrip control '&Tools'
        EDDiscoveryForm_toolsToolStripMenuItem_settingsToolStripMenuItem, // ToolStrip control 'Settings'
        EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem, // ToolStrip control '&Pop-outs'
        EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_showAllInTaskBarToolStripMenuItem, // ToolStrip control '&Show All In Task Bar'
        EDDiscoveryForm_toolsToolStripMenuItem_showAllPopoutsInTaskBarToolStripMenuItem_turnOffAllTransparencyToolStripMenuItem, // ToolStrip control '&Turn Off All Transparency'
        EDDiscoveryForm_toolsToolStripMenuItem_exitToolStripMenuItem, // ToolStrip control 'E&xit'
        EDDiscoveryForm_adminToolStripMenuItem, // ToolStrip control 'A&dmin'
        EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem, // ToolStrip control 'Synchronise with EDSM'
        EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_sendUnsyncedEDSMJournalsToolStripMenuItem, // ToolStrip control 'Send Unsynced Journals To EDSM'
        EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_fetchLogsAgainToolStripMenuItem, // ToolStrip control 'Fetch Logs Again'
        EDDiscoveryForm_adminToolStripMenuItem_syncEDSMSystemsToolStripMenuItem_fetchStarDataAgainToolStripMenuItem, // ToolStrip control 'Fetch Star Data Again'
        EDDiscoveryForm_adminToolStripMenuItem_rescanAllJournalFilesToolStripMenuItem, // ToolStrip control 'Re-scan all journal files'
        EDDiscoveryForm_adminToolStripMenuItem_sendHistoricDataToInaraToolStripMenuItem, // ToolStrip control 'Send to Inara historic data (previous ships, stored modules)'
        EDDiscoveryForm_adminToolStripMenuItem_rebuildUserDBIndexesToolStripMenuItem, // ToolStrip control 'Rebuild User DB Indexes'
        EDDiscoveryForm_adminToolStripMenuItem_rebuildSystemDBIndexesToolStripMenuItem, // ToolStrip control 'Rebuild System DB Indexes'
        EDDiscoveryForm_adminToolStripMenuItem_updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem, // ToolStrip control 'Update systems with unknown co-ordinates'
        EDDiscoveryForm_adminToolStripMenuItem_showLogfilesToolStripMenuItem, // ToolStrip control 'Show journal files directory of current commander'
        EDDiscoveryForm_adminToolStripMenuItem_dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem, // ToolStrip control 'Reset all history to current commander'
        EDDiscoveryForm_adminToolStripMenuItem_deleteDuplicateFSDJumpEntriesToolStripMenuItem, // ToolStrip control 'Delete duplicate FSD Jump entries'
        EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem, // ToolStrip control 'Read 2.1 and former log files'
        EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem_load21ToolStripMenuItem, // ToolStrip control 'Scan Netlogs'
        EDDiscoveryForm_adminToolStripMenuItem_read21AndFormerLogFilesToolStripMenuItem_read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem, // ToolStrip control 'Force reload logs'
        EDDiscoveryForm_addOnsToolStripMenuItem, // ToolStrip control '&Add-Ons'
        EDDiscoveryForm_addOnsToolStripMenuItem_manageAddOnsToolStripMenuItem, // ToolStrip control '&Manage Add-Ons'
        EDDiscoveryForm_addOnsToolStripMenuItem_configureAddOnActionsToolStripMenuItem, // ToolStrip control '&Edit Add-On Action Files'
        EDDiscoveryForm_addOnsToolStripMenuItem_editLastActionPackToolStripMenuItem, // ToolStrip control 'Edit Last Action Pack'
        EDDiscoveryForm_addOnsToolStripMenuItem_stopCurrentlyRunningActionProgramToolStripMenuItem, // ToolStrip control '&Stop currently running Action Program'
        EDDiscoveryForm_helpToolStripMenuItem, // ToolStrip control '&Help'
        EDDiscoveryForm_helpToolStripMenuItem_aboutToolStripMenuItem, // ToolStrip control '&About'
        EDDiscoveryForm_helpToolStripMenuItem_wikiHelpToolStripMenuItem, // ToolStrip control '&View Help'
        EDDiscoveryForm_helpToolStripMenuItem_viewHelpVideosToolStripMenuItem, // ToolStrip control 'View Help Videos'
        EDDiscoveryForm_helpToolStripMenuItem_eDDiscoveryChatDiscordToolStripMenuItem, // ToolStrip control '&Discord - EDD Community Chat'
        EDDiscoveryForm_helpToolStripMenuItem_frontierForumThreadToolStripMenuItem, // ToolStrip control '&Frontier Forum Thread'
        EDDiscoveryForm_helpToolStripMenuItem_gitHubToolStripMenuItem, // ToolStrip control '&Project Page (GitHub)'
        EDDiscoveryForm_helpToolStripMenuItem_reportIssueIdeasToolStripMenuItem, // ToolStrip control '&Report Issue / Idea'
        EDDiscoveryForm_helpToolStripMenuItem_toolStripMenuItemListBindings, // ToolStrip control 
        EDDiscoveryForm_helpToolStripMenuItem_checkForNewReleaseToolStripMenuItem, // ToolStrip control '&Check for Updates'
        EDDiscoveryForm_tabControlMain_ToolTip, // ToolTip 'Right click to add/remove tabs, Left click drag to reorder'
        EDDiscoveryForm_comboBoxCommander_ToolTip, // ToolTip 'Select the commander to view'
        EDDiscoveryForm_buttonExtRefresh_ToolTip, // ToolTip 'Refresh the history'
        EDDiscoveryForm_comboBoxCustomProfiles_ToolTip, // ToolTip 'Use to select new profile or edit profile settings'
        EDDiscoveryForm_buttonExtManageAddOns_ToolTip, // ToolTip 'Manage Add-Ons'
        EDDiscoveryForm_buttonExtEditAddOns_ToolTip, // ToolTip 'Edit Add-Ons'
        EDDiscoveryForm_buttonExtPopOut_ToolTip, // ToolTip 'Click to select a pop out panel to display'
        EDDiscoveryForm_buttonReloadActions_ToolTip, // ToolTip 'DEBUG reload action system'

        EDDiscoveryForm_TSF,
        EDDiscoveryForm_ConfirmSyncToEDSM, // You have disabled sync to EDSM for this commander.  Are you sure you want to send unsynced events to EDSM?
        EDDiscoveryForm_ConfirmSyncToEDSMCaption, // Confirm EDSM sync

        EDDiscoveryForm_SendEDSMTitle,
        EDDiscoveryForm_SendEDSMCaption,
        EDDiscoveryForm_SendEDSMAll,
        EDDiscoveryForm_SendEDSM24,
        EDDiscoveryForm_SendEDSMFrom,
        EDDiscoveryForm_SendEDSMNone,
        EDDiscoveryForm_SendEDSMCancel,

        EDDiscoveryForm_RTABL, // Name:
        EDDiscoveryForm_RTABT, // Rename Tab
        EDDiscoveryForm_RTABTT, // Enter a new name for the tab
        EDDiscoveryForm_DLLW, // The following application extension DLLs have been found
        EDDiscoveryForm_DLLL, // DLLs loaded: {0}
        EDDiscoveryForm_DLLF, // DLLs failed to load: {0}
        EDDiscoveryForm_DLLDIS, // DLLs disabled: {0}
        EDDiscoveryForm_PROFL, // Profile {0} Loaded
        EDDiscoveryForm_NI, // New EDDiscovery installer available: {0}
        EDDiscoveryForm_NRA, // New Release Available!
        EDDiscoveryForm_Arrived, // Arrived at system {0} Visit No. {1}
        EDDiscoveryForm_PE1, // Profile reports errors in triggers:
        EDDiscoveryForm_Closing, // Closing, please wait!
        EDDiscoveryForm_SDDis, // Star Data download is disabled. Use Settings to reenable it
        EDDiscoveryForm_SDSyncErr, // Synchronisation to databases is in operation or pending, please wait
        EDDiscoveryForm_EDSMQ, // This can take a considerable amount of time and bandwidth
        EDDiscoveryForm_ResetCMDR, // Confirm you wish to reset all history entries to the current commander
        EDDiscoveryForm_NoRel, // No new release found
        EDDiscoveryForm_RevFSD, // Confirm you remove any duplicate FSD entries from the current commander
        EDDiscoveryForm_FSDRem, // Removed {0} FSD entries
        EDDiscoveryForm_InaraW, // Inara historic upload is disabled until 1 hour has elapsed from the last try to prevent server flooding
        EDDiscoveryForm_IndexW, // Are you sure to Rebuild Indexes? It may take a long time.
        EDDiscoveryForm_FillPos, // Are you sure to Rebuild Indexes? It may take a long time.
        EDDiscoveryForm_RH, // Refresh History.
        EDDiscoveryForm_NoEDSMAPI, // No EDSM API key set
        EDDiscoveryForm_EP, // Edit Profiles
        EDDiscoveryForm_PL, // Profile {0} Loaded
        EDDiscoveryForm_WSE, // Web server enabled
        EDDiscoveryForm_WSF, // Web server failed to start
        EDDiscoveryForm_WSERR, // Web server disabled due to incorrect folder or missing zip file
        EDDiscoveryForm_WSD, // *** Web server is disabled ***
        EDDiscoveryForm_CloseWarning, // EDDiscovery is updating the EDSM and EDDB databases

        EDDiscoveryController_CD, // Closing down, please wait..
        EDDiscoveryController_EDSM, // Get galactic mapping from EDSM.
        EDDiscoveryController_LN, // Loaded Notes, Bookmarks and Galactic mapping.
        EDDiscoveryController_RTH, // Reading travel history
        EDDiscoveryController_SyncEDSM, // Please continue running ED Discovery until refresh is complete.
        EDDiscoveryController_SyncOff, // Synchronisation to EDSM and EDDB disabled. Use Settings panel to reenable
        EDDiscoveryController_EDSMU, // EDSM update complete with {0} systems
        EDDiscoveryController_PLF, // Processing log file {0}
        EDDiscoveryController_RD, // Refresh Displays
        EDDiscoveryController_HRC, // History refresh complete.
        EDDiscoveryController_EXPD, // Checking for new Expedition data

        UserControlForm_extButtonDrawnShowTitle_ToolTip, // ToolTip 'Toggle title visibility for this window when transparent'
        UserControlForm_extButtonDrawnMinimize_ToolTip, // ToolTip 'Minimise'
        UserControlForm_extButtonDrawnOnTop_ToolTip, // ToolTip 'Toggle window on top of others'
        UserControlForm_extButtonDrawnTaskBarIcon_ToolTip, // ToolTip 'Toggle show taskbar icon for this window'
        UserControlForm_extButtonDrawnTransparentMode_ToolTip, // ToolTip 'Toggle window transparency thru four settings\r\nOff - normal window\r\nOn (T) - transparent with controls active when mouse not inside window\r\nOn (Tc) - transparent with control active, to activate hold down the activate key\r\nOn (Tf) - fully transparent and inert, to activate hold down the activate key\r\n\r\nSee the settings page for configuring which key is the activate key.  \r\nMouse must be within the boundaries of the window and the key held down for\r\n500ms approx.\r\n\r\n\r\n'
        UserControlForm_extButtonDrawnClose_ToolTip, // ToolTip 'Close'

        AddOnManagerForm_buttonExtGlobals,   // Control 'Globals'

        AddOnManagerForm_AddOnTitle, // Add-On Manager
        AddOnManagerForm_EditTitle, // Edit Add-Ons
        AddOnManagerForm_Locallymodified, // Locally modified
        AddOnManagerForm_UptoDate, // Up to Date
        AddOnManagerForm_LocalOnly, // Local Only
        AddOnManagerForm_Newversion, // New version
        AddOnManagerForm_Newer, // Newer EDD required
        AddOnManagerForm_Old, // Too old for EDD
        AddOnManagerForm_Modwarn, // Modified locally, do you wish to overwrite the changes
        AddOnManagerForm_Failed, // Add-on failed to update. Check files for read only status
        AddOnManagerForm_DeleteWarn, // Do you really want to delete {0}
        AddOnManagerForm_Type, // Type
        AddOnManagerForm_Name, // Name
        AddOnManagerForm_Version, // Version
        AddOnManagerForm_Description, // Description
        AddOnManagerForm_Status, // Status
        AddOnManagerForm_Action, // Action
        AddOnManagerForm_Delete, // Delete
        AddOnManagerForm_Enabled, // Enabled
        AddOnManagerForm_Install, // Install
        AddOnManagerForm_Update, // Update
        AddOnManagerForm_Edit, // Edit

        BookmarkForm_buttonEDSM, // Control 'EDSM'
        BookmarkForm_labelName, // Control 'Name'
        BookmarkForm_checkBoxTarget, // Control 'Make Target'
        BookmarkForm_labelBookmarkNotes, // Control 'Bookmark Notes'
        BookmarkForm_labelTravelNote, // Control 'Travel History Note'
        BookmarkForm_labelTimeMade, // Control 'Time Made'

        BookmarkForm_NSB, // New System Bookmark
        BookmarkForm_ESN, // Enter a system name...
        BookmarkForm_GMO, // Galactic Mapping Object
        BookmarkForm_SB, // New System Bookmark
        BookmarkForm_UB, // Update Bookmark
        BookmarkForm_RB, // Create Region Bookmark
        BookmarkForm_RN, // Enter a region name...
        BookmarkForm_SI, // System Information
        BookmarkForm_Update, // Update
        BookmarkForm_Description, // Description

        SurfaceBookmarkUserControl_BodyName, // Column Header 'Planetary Body'
        SurfaceBookmarkUserControl_SurfaceName, // Column Header 'Name'
        SurfaceBookmarkUserControl_SurfaceDesc, // Column Header 'Description'
        SurfaceBookmarkUserControl_Latitude, // Column Header 'Latitude'
        SurfaceBookmarkUserControl_Longitude, // Column Header 'Longitude'
        SurfaceBookmarkUserControl_Valid, // Column Header 'Saveable'
        SurfaceBookmarkUserControl_labelSurface, // Control 'Surface Bookmarks In System'
        SurfaceBookmarkUserControl_sendToCompassToolStripMenuItem, // ToolStrip control 'Send to compass'
        SurfaceBookmarkUserControl_deleteToolStripMenuItem, // ToolStrip control 'Delete Row'
        SurfaceBookmarkUserControl_addPlanetManuallyToolStripMenuItem, // ToolStrip control 'Manually Add Planet'
        SurfaceBookmarkUserControl_Enter, // Enter a name
        SurfaceBookmarkUserControl_PL, // Planet:
        SurfaceBookmarkUserControl_EPN, // Enter planet name
        SurfaceBookmarkUserControl_PresstoAccept, // Press to Accept
        SurfaceBookmarkUserControl_PresstoCancel, // Press to Cancel
        SurfaceBookmarkUserControl_MAP, // Manually Add Planet

        CSV_Helpers_OpenFailed, // Failed to open<spc>
        CSV_Helpers_WriteFailed, // Failed to write to<spc>

        UserControlCompass_labelBookmark, // Control 'System Bookmarks'
        UserControlCompass_labelTargetLat, // Control 'Target'
        UserControlCompass_checkBoxHideTransparent, // Control 'Hide In Transparent'


        ExportForm, // Control 'Export'
        ExportForm_buttonExport, // Control 'Export'
        ExportForm_ImportTitle,
        ExportForm_ImportButton,
        ExportForm_labelCVSSep, // Control 'CSV Separator'
        ExportForm_radioButtonComma, // Control 'Comma'
        ExportForm_radioButtonSemiColon, // Control 'Semicolon'
        ExportForm_checkBoxIncludeHeader, // Control 'Include Header'
        ExportForm_checkBoxCustomAutoOpen, // Control 'Open'
        ExportForm_labelUTCEnd, // Control 'UTC'
        ExportForm_labelUTCStart, // Control 'UTC'

        Form2DMap_LastWeek, // Last Week
        Form2DMap_LastMonth, // Last Month
        Form2DMap_LastYear, // Last Year
        Form2DMap_All, // All
        Form2DMap_Custom, // Custom

        GalaxySectorSelect, // Control 'Galaxy Sector Select'
        GalaxySectorSelect_buttonExtSet, // Control 'Set'
        GalaxySectorSelect_labelSectorName, // Control 'Sector'
        GalaxySectorSelect_Custom, // Custom
        GalaxySectorSelect_Reset, // Reset
        GalaxySectorSelect_All, // All
        GalaxySectorSelect_None, // None
        GalaxySectorSelect_Bubble, // Bubble
        GalaxySectorSelect_ExtendedBubble, // Extended Bubble
        GalaxySectorSelect_BC, // Bubble+Colonia
        GalaxySectorSelect_RD, // You have added new sectors!
        GalaxySectorSelect_NoSync, // Synchronisation to star data disabled in settings.
        GalaxySectorSelect_GALSELEX, // ED Discovery downloads star data from EDSM/EDDB which is used to give you additional data.  Select how much data you want to store.  The more of the galaxy you select, the bigger the storage needed
        GalaxySectorSelect_GALSELEN, // Enter number to jump to or near to
        GalaxySectorSelect_Select, // GalaxySectorSelect
        GalaxySectorSelect_PresstoAccept, // Press to Accept
        GalaxySectorSelect_GALSELTitle, // Select EDSM Galaxy Data
        GalaxySectorSelect_RS, // You have removed sectors!

        NewReleaseForm, // Control 'EDDiscovery Release'
        NewReleaseForm_labelName, // Control 'Name'
        NewReleaseForm_btnClose, // Control '&Close'
        NewReleaseForm_labelURL, // Control 'GitHub URL'
        NewReleaseForm_labelDownload, // Control 'Download'
        NewReleaseForm_buttonPortablezip, // Control '&Portable Zip'
        NewReleaseForm_buttonUrlOpen, // Control '&Open'
        NewReleaseForm_buttonExeInstaller, // Control '&Exe Installer'
        NewReleaseForm_labelRelease, // Control 'Release info'

        MoveToCommander, // Control 'MoveToCommander'
        MoveToCommander_buttonTransfer, // Control 'Transfer'
        MoveToCommander_label1, // Control 'Move selected history to commander.'

        ProfileEditor, // Control 'ProfileEditor'
        ProfileEditor_buttonExtGlobals, // Control 'Globals'
        ProfileEditor_Custom, // Custom
        ProfileEditor_Trigger, // Trigger
        ProfileEditor_Back, // Back
        ProfileEditor_Default, // Default
        ProfileEditor_TrigEdit, // Edit Profile {0} Trigger
        ProfileEditor_BackEdit, // Edit Profile {0} Back Trigger
        ProfileEditor_DeleteWarning, // Do you wish to delete profile {0}?

        SafeModeForm, // Control 'EDDiscovery Safe Mode'
        SafeModeForm_buttonCancel, // Control 'Exit'
        SafeModeForm_buttonRun, // Control 'Run'
        SafeModeForm_buttonRemoveJournals, // Control 'Remove Journal Entries'
        SafeModeForm_buttonDeleteUserDB, // Control 'Delete/Rebuild User DB'
        SafeModeForm_buttonDeleteSystemDB, // Control 'Delete/Rebuild System DB'
        SafeModeForm_buttonResetDBLoc, // Control 'Reset DB Location'
        SafeModeForm_buttonBackup, // Control 'Backup Database'
        SafeModeForm_buttonDbs, // Control 'Move Databases'
        SafeModeForm_buttonLang, // Control 'Reset Language to English'
        SafeModeForm_buttonActionPacks, // Control 'Remove all Action Packs'
        SafeModeForm_buttonRemoveDLLs, // Control 'Remove all Extension DLLs'
        SafeModeForm_buttonResetTabs, // Control 'Reset Tabs, Remove PopOuts'
        SafeModeForm_buttonPositions, // Control 'Reset Window Positions'
        SafeModeForm_buttonResetTheme, // Control 'Reset Theme'
        SafeModeForm_buttonRemoveJournalsCommanders,

        SetNoteForm, // Control 'Set Note'
        SetNoteForm_labelTTimestamp, // Control 'Timestamp:'
        SetNoteForm_buttonSave, // Control 'Save'
        SetNoteForm_labelTSystem, // Control 'System:'
        SetNoteForm_labelTSummary, // Control 'Summary:'
        SetNoteForm_labelTDetails, // Control 'Details:'

        DataGridViewDialogs_EN,
        DataGridViewDialogs_Jumpto,
        DataGridViewDialogs_PresstoAccept,
        DataGridViewDialogs_PresstoCancel,
        DataGridViewDialogs_VNV,

        UserControlSettings_groupBoxCommanders, // Control 'Commanders'
        UserControlSettings_ColumnCommander, // Column Header 'Commander'
        UserControlSettings_EdsmName, // Column Header 'EDSM Name'
        UserControlSettings_JournalDirCol, // Column Header 'Journal Folder'
        UserControlSettings_NotesCol, // Column Header 'Notes'
        UserControlSettings_btnDeleteCommander, // Control 'Delete'
        UserControlSettings_buttonEditCommander, // Control 'Edit'
        UserControlSettings_buttonAddCommander, // Control 'Add'
        UserControlSettings_groupBoxTheme, // Control 'Theme'
        UserControlSettings_button_edittheme, // Control 'Edit Theme'
        UserControlSettings_buttonSaveTheme, // Control 'Save Theme'

        UserControlSettings_groupBoxCustomHistoryLoad, // Control 'History'
        UserControlSettings_checkBoxOrderRowsInverted, // Control 'Number Rows by Date Ascending'
        UserControlSettings_labelTimeDisplay, // Control 'Time:'
        UserControlSettings_extGroupBoxWebServer, // Control 'Web Server'
        UserControlSettings_labelPortNo, // Control 'Port'
        UserControlSettings_extButtonTestWeb, // Control 'Test'
        UserControlSettings_extCheckBoxWebServerEnable, // Control 'Enable'
        UserControlSettings_groupBoxInteraction, // Control 'Interaction'
        UserControlSettings_labelTKey, // Control 'Key to activate transparent windows'
        UserControlSettings_groupBoxMemory, // Control 'Memory'
        UserControlSettings_labelHistoryEssItems, // Control 'Essential entries:'
        UserControlSettings_labelHistorySel, // Control 'Entries to read:'
        UserControlSettings_groupBoxCustomScreenShots, // Control 'Screenshots conversion'

        UserControlSettings_buttonExtScreenshot, // Control 'Configure'
        UserControlSettings_checkBoxCustomEnableScreenshots, // Control 'Enable'
        UserControlSettings_groupBoxCustomEDSM, // Control 'EDSM Control'
        UserControlSettings_buttonExtEDSMConfigureArea, // Control 'Select Galaxy Sectors'
        UserControlSettings_checkBoxCustomEDSMDownload, // Control 'Enable Star Data Download'
        UserControlSettings_groupBoxPopOuts, // Control 'Window Options'
        UserControlSettings_checkBoxPanelSortOrder, // Control 'Panel List Sorted Alphanumerically'
        UserControlSettings_checkBoxKeepOnTop, // Control 'Keep on Top'
        UserControlSettings_checkBoxCustomResize, // Control 'Redraw the screen during resizing'
        UserControlSettings_checkBoxMinimizeToNotifyIcon, // Control 'Minimize to notification area icon'
        UserControlSettings_checkBoxUseNotifyIcon, // Control 'Show notification area icon'
        UserControlSettings_extGroupBoxDLLPerms, // Control 'DLLs'
        UserControlSettings_extButtonDLLConfigure, // Control 'Configure'
        UserControlSettings_extButtonDLLPerms, // Control 'Permissions'

        UserControlSettings_groupBoxCustomLanguage, // Control 'Language'
        UserControlSettings_groupBoxCustomSafeMode, // Control 'Advanced'
        UserControlSettings_buttonExtSafeMode, // Control 'Restart in Safe Mode'
        UserControlSettings_labelSafeMode, // Control 'Click this to perform special operations, such as to move system databases to another drive, reset UI, and other maintenance tasks...\r\n\r\n'
        UserControlSettings_btnDeleteCommander_ToolTip, // ToolTip 'Delete selected commander'
        UserControlSettings_buttonEditCommander_ToolTip, // ToolTip 'Edit selected commander'
        UserControlSettings_buttonAddCommander_ToolTip, // ToolTip 'Add a new commander'
        UserControlSettings_comboBoxTheme_ToolTip, // ToolTip 'Select the theme to use'
        UserControlSettings_button_edittheme_ToolTip, // ToolTip 'Edit theme and change colours fonts'
        UserControlSettings_buttonSaveTheme_ToolTip, // ToolTip 'Save theme to disk'
        UserControlSettings_checkBoxOrderRowsInverted_ToolTip, // ToolTip 'Number oldest entry 1, latest entry highest'
        
        UserControlSettings_comboBoxClickThruKey_ToolTip, // ToolTip 'Select the key to hold down for at least 500 ms\r\nto show the form of a transparent pop out, when\r\nthe pop out transparency mode if Tc (Click thru,\r\ncontrols still active) or Tf (controls inactive)\r\nNot all keys are guaranteed active on all keyboards'
        UserControlSettings_comboBoxCustomEssentialEntries_ToolTip, // ToolTip 'Select which items you consider essential to load older than the time above'
        UserControlSettings_comboBoxCustomHistoryLoadTime_ToolTip, // ToolTip 'Reduce Memory use. Select either load all records, or load only essential items of records older than a set time before now'
        UserControlSettings_buttonExtScreenshot_ToolTip, // ToolTip 'Configure further screenshot options'
        UserControlSettings_checkBoxCustomEnableScreenshots_ToolTip, // ToolTip 'Screen shot conversion on/off'
        UserControlSettings_buttonExtEDSMConfigureArea_ToolTip, // ToolTip 'Configure what parts of the galaxy is stored in the databases'
        UserControlSettings_checkBoxCustomEDSMDownload_ToolTip, // ToolTip 'Click to enable downloading of stars from EDSM. Will apply at next start.'
        UserControlSettings_checkBoxPanelSortOrder_ToolTip, // ToolTip 'Panel lists sorted alphanumerically instead of ordered in groups. Note Requires Restart'
        UserControlSettings_checkBoxKeepOnTop_ToolTip, // ToolTip 'This window, and its children, top'
        UserControlSettings_checkBoxCustomResize_ToolTip, // ToolTip 'Check to allow EDD to redraw the screen during main window resize. Only disable if its too slow'
        UserControlSettings_checkBoxMinimizeToNotifyIcon_ToolTip, // ToolTip 'Minimize the main window to the system notification area (system tray) icon.'
        UserControlSettings_checkBoxUseNotifyIcon_ToolTip, // ToolTip 'Show a system notification area (system tray) icon for EDDiscovery.'
        UserControlSettings_buttonExtSafeMode_ToolTip, // ToolTip 'Safe Mode allows you to perform special operations, such as moving the databases, resetting the UI, resetting the action packs,  DLLs etc.'

        UserControlSettings_GameTime, // Game Time
        UserControlSettings_Local, // Local
        UserControlSettings_WSQ, // If you have previously done this on this same port number you can click No and the enable will work
        UserControlSettings_WSF, // Did not start - click OK to configure windows
        UserControlSettings_FS, // All entries for Statistics
        UserControlSettings_SM, // Safe Mode
        UserControlSettings_CSM, // Confirm restart to safe mode
        UserControlSettings_DLA, // Disabled-Load All
        UserControlSettings_7daysold, // >7 days old
        UserControlSettings_30daysold, // >30 days old
        UserControlSettings_60daysold, // >60 days old
        UserControlSettings_90daysold, // >90 days old
        UserControlSettings_180daysold, // >180 days old
        UserControlSettings_270daysold, // >270 days old
        UserControlSettings_365daysold, // > 365 days old
        UserControlSettings_AddC, // Commander name is not valid or duplicate
        UserControlSettings_AddT, // Cannot create Commander
        UserControlSettings_Language, // Applies at next restart of ED Discovery
        UserControlSettings_RemoveSectors, // Remove Sectors
        UserControlSettings_GalRemove, // Removing {0} Sector(s).
        UserControlSettings_ESM, // Scans,Cargo,Missions,State,Jumps etc
        UserControlSettings_EJS, // Jumps and Scans
        UserControlSettings_EJ, // Jumps
        UserControlSettings_EN, // Nothing
        UserControlSettings_DelCmdr, // delete commander
        UserControlSettings_Font, // The font used by this theme is not available on your system.
        UserControlSettings_NoMap, // No map downloaded
        UserControlSettings_GalFini, // Finished, Please close the window.

        UserControlMaterialCommodities_NameCol, // Column Header 'Name'
        UserControlMaterialCommodities_ShortName, // Column Header 'Abv'
        UserControlMaterialCommodities_Category, // Column Header 'Category'
        UserControlMaterialCommodities_Type, // Column Header 'Type'
        UserControlMaterialCommodities_Number, // Column Header 'Number'
        UserControlMaterialCommodities_Price, // Column Header 'Avg. Price'
        UserControlMaterialCommodities_buttonFilter_ToolTip, // ToolTip 'Filter out items'
        UserControlMaterialCommodities_textBoxItems1_ToolTip, // ToolTip 'Count of Items'
        UserControlMaterialCommodities_textBoxItems2_ToolTip, // ToolTip 'Count of Items'
        UserControlMaterialCommodities_checkBoxShowZeros_ToolTip, // ToolTip 'Green will show materials with zero counts, red means remove them'
        UserControlMaterialCommodities_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'

        UserControlMaterialCommodities_Data, // Data !! NOTE
        UserControlMaterialCommodities_Mats, // Data !! NOTE
        UserControlMaterialCommodities_Total, // Data !! NOTE
        UserControlMaterialCommodities_Rare, // Rare !! NOTE
        UserControlMaterialCommodities_Recipes, // Recipes !! NOTE
        UserControlMaterialCommodities_ShipLocker, // Discrete
        UserControlMaterialCommodities_BackPack, // Discrete


        MissionListUserControl_PcolName, // Column Header 'Name'
        MissionListUserControl_pColStart, // Column Header 'Start Date'
        MissionListUserControl_pColEnd, // Column Header 'End Date'
        MissionListUserControl_pColOrigin, // Column Header 'Origin'
        MissionListUserControl_pColFromFaction, // Column Header 'Faction'
        MissionListUserControl_pColDestSys, // Column Header 'Destination'
        MissionListUserControl_pColTargetFaction, // Column Header 'Target Faction'
        MissionListUserControl_pColResult, // Column Header 'Result (cr)'
        MissionListUserControl_pColInfo, // Column Header 'Info'
        MissionListUserControl_labelTo, // Control 'to'
        MissionListUserControl_labelSearch, // Control 'Search'

        UserControlMissions_MPlural, //  Missions
        UserControlMissions_MSingular, //  Mission
        UserControlMissions_Name, // Name
        UserControlMissions_Value, // Value (cr):
        UserControlMissions_ValueN, // Value (cr)
        UserControlMissions_ValueC, // Value:

        UserControlModules_ItemLocalised, // Column Header 'Type'
        UserControlModules_ItemCol, // Column Header 'Item'
        UserControlModules_BluePrint, // Column Header 'BluePrint'
        UserControlModules_PriorityEnable, // Column Header 'P/E'
        UserControlModules_labelShip, // Control 'Ship'
        UserControlModules_labelVehicle, // Control 'Unknown'

        UserControlModules_FullBluePrint, // Show full blueprint information

        UserControlModules_comboBoxShips_ToolTip, // ToolTip 'Select ship to view'
        UserControlModules_extButtonShowControl_ToolTip, // ToolTip 'Display Settings'
        UserControlModules_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'
        UserControlModules_buttonExtCoriolis_ToolTip, // ToolTip 'Send to Coriolis'
        UserControlModules_buttonExtEDShipyard_ToolTip, // ToolTip 'Send to ED Ship Yard'
        UserControlModules_buttonExtConfigure_ToolTip, // ToolTip 'Configure extra data missing from Elite Journal Output'
        UserControlModules_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'l'

        UserControlModules_ForceSell, // Force Sell
        UserControlModules_ConfirmForceSell, // Confirm sell of ship
        UserControlModules_StoredModules, // Stored Modules
        UserControlModules_TravelHistoryEntry, // Travel History Entry
        UserControlModules_System, // System
        UserControlModules_TxTime, // Tx Time
        UserControlModules_Cost, // Cost
        UserControlModules_SlotCol, // Slot
        UserControlModules_ItemInfo, // Info
        UserControlModules_Value, // Value
        UserControlModules_Stored, // Stored
        UserControlModules_AllModules, // All Modules

        UserControlModules_FSDAvgJump, // FSD Avg Jump
        UserControlModules_HT, // Half tank, no cargo
        UserControlModules_MassFDUnladen, // Mass FD Unladen
        UserControlModules_FuelReserveCapacity, // Fuel Reserve Capacity
        UserControlModules_HullHealth, // Hull Health (Loadout)
        UserControlModules_Mass, // Mass
        UserControlModules_FSDMaxRange, // FSD Max Range
        UserControlModules_FT, // Full Tank, no cargo
        UserControlModules_FSDMaximumFuelperjump, // FSD Maximum Fuel per jump
        UserControlModules_HullValue, // Hull Value
        UserControlModules_ModulesValue, // Modules Value
        UserControlModules_TotalCost, // Total Cost
        UserControlModules_RebuyCost, // Rebuy Cost
        UserControlModules_MassHull, // Mass Hull
        UserControlModules_MassUnladen, // Mass Unladen

        UserControlModules_MassModules, // Mass Modules
        UserControlModules_Manufacturer, // Manufacturer
        UserControlModules_FuelCapacity, // Fuel Capacity
        UserControlModules_FuelLevel, // Fuel Level
        UserControlModules_FuelWarning, // Fuel Warning %
        UserControlModules_PadSize, // Pad Size
        UserControlModules_MainThrusterSpeed, // Main Thruster Speed
        UserControlModules_MainThrusterBoost, // Main Thruster Boost
        UserControlModules_Storedat, // Stored at
        UserControlModules_CargoCapacity, // Cargo Capacity
        UserControlModules_InTransit, // In Transit
        UserControlModules_InTransitto, // In Transit
        UserControlModules_EDSURL, // Enter ED Shipyard URL
        UserControlModules_CURL, // Enter Coriolis URL
        UserControlModules_FW, // Fuel Warning:
        UserControlModules_TTF, // Enter fuel warning level in % (0 = off, 1-100%)
        UserControlModules_PresstoAccept, // Press to Accept
        UserControlModules_PresstoCancel, // Press to Cancel
        UserControlModules_NValid, // A Value is not valid
        UserControlModules_SC, // Ship Configure
        UserControlModules_MI, // Module Information
        UserControlModules_NOSI, // No ship information available

        UserControlStats_tabControlCustomStats_tabPageGeneral, // Control 'General'
        UserControlStats_tabControlCustomStats_tabPageGeneral_ItemName, // Column Header 'Item'
        UserControlStats_tabControlCustomStats_tabPageGeneral_Information, // Column Header 'Information'
        UserControlStats_tabControlCustomStats_tabPageTravel, // Control 'Travel'
        UserControlStats_tabControlCustomStats_tabPageScan, // Control 'Scan'
        UserControlStats_tabControlCustomStats_tabPageRanks, // Control 'Scan'
        UserControlStats_tabControlCustomStats_tabPageGameStats, // Control 'In Game'
        UserControlStats_tabControlCustomStats_tabPageByShip, // Control 'By Ship'
        UserControlStats_tabControlCustomStats_tabPageCombat,
        UserControlStats_tabControlCustomStats_tabPageLedger,
        UserControlStats_labelStart, // Control 'Start'
        UserControlStats_labelEndDate, // Control 'To'
        UserControlStats_TotalNoofjumps, // Total No of jumps:
        UserControlStats_FSDjumps, // FSD jumps:
        UserControlStats_Beltcluster, // Belt cluster
        UserControlStats_JumpHistory, // Jump History
        UserControlStats_24hc, // 24 Hours:
        UserControlStats_OneWeek, // , One Week:
        UserControlStats_30Days, // , 30 Days:
        UserControlStats_OneYear, // , One Year:
        UserControlStats_MostNorth, // Most North
        UserControlStats_MostSouth, // Most South
        UserControlStats_MostEast, // Most East
        UserControlStats_MostWest, // Most West
        UserControlStats_MostHighest, // Most Highest
        UserControlStats_MostLowest, // Most Lowest
        UserControlStats_MostVisited, // Most Visited
        UserControlStats_Jumps, // Jumps
        UserControlStats_TravelledLy, // Travelled Ly
        UserControlStats_PremiumBoost, // Premium Boost
        UserControlStats_StandardBoost, // Standard Boost
        UserControlStats_BasicBoost, // Basic Boost
        UserControlStats_JetConeBoost, // Jet Cone Boost
        UserControlStats_Scans, // Scans
        UserControlStats_Mapped, // Mapped
        UserControlStats_Scanvalue, // Scan value
        UserControlStats_OrganicScans, // Scan value
        UserControlStats_Lastdock, // Last dock
        UserControlStats_Trip, // Trip
        UserControlStats_NoTrip, // No Trip
        UserControlStats_All, // All
        UserControlStats_Type, // Type
        UserControlStats_BankAccount, // Bank Account
        UserControlStats_Combat, // Combat
        UserControlStats_Crime, // Crime
        UserControlStats_Smuggling, // Smuggling
        UserControlStats_Trading, // Trading
        UserControlStats_Mining, // Mining
        UserControlStats_Exploration, // Exploration
        UserControlStats_Passengers, // Passengers
        UserControlStats_SearchandRescue, // Search and Rescue
        UserControlStats_Crafting, // Crafting
        UserControlStats_Crew, // Crew
        UserControlStats_Multi, // Multi-crew
        UserControlStats_MaterialsTrader, // Materials Trader
        UserControlStats_CQC, // CQC
        UserControlStats_FLEETCARRIER, // Fleetcarrier
        UserControlStats_Exobiology, // Exobiology
        UserControlStats_Name, // Name
        UserControlStats_Ident, // Ident
        UserControlStats_BodiesScanned, // Bodies Scanned
        UserControlStats_Destroyed, // Destroyed
        // used by faction panels now, not stats
        UserControlStats_GoodsBought, // Goods Bought
        UserControlStats_GoodsSold, // Goods Sold
        UserControlStats_GoodsProfit, // Goods Sold

        UserControlStats_tabControlCustomStats_tabPageLedger_dataGridViewTextBoxColumnLedgerDate,
        UserControlStats_tabControlCustomStats_tabPageLedger_dataGridViewTextBoxColumnNumericCredits,

        UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnRank, 
        UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnAtStart, 
        UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxAtEnd, 
        UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextBoxColumnLastPromotionDate,
        UserControlStats_tabControlCustomStats_tabPageRanks_dataGridViewTextColumnRankProgressNumeric,

        UserControlStats_Powerplay,
        UserControlStats_Squadron,
        UserControlStats_Bounties,
        UserControlStats_Bountyvalue,
        UserControlStats_Bountiesonships,
        UserControlStats_Crimes,
        UserControlStats_CrimeCost,
        UserControlStats_FactionKillBonds,
        UserControlStats_FKBValue,
        UserControlStats_InterdictionPlayerSucceeded,
        UserControlStats_InterdictionPlayerFailed,
        UserControlStats_InterdictionNPCSucceeded,
        UserControlStats_InterdictionNPCFailed,
        UserControlStats_InterdictedPlayerSucceeded,
        UserControlStats_InterdictedPlayerFailed,
        UserControlStats_InterdictedNPCSucceeded,
        UserControlStats_InterdictedNPCFailed,
        UserControlStats_PVPKills,
        UserControlStats_BountiesThargoid,
        UserControlStats_BountiesOnFootNPC,
        UserControlStats_BountiesSkimmers,
        UserControlStats_ShipsUnknown,
        UserControlStats_ShipsElite,
        UserControlStats_ShipsDeadly,
        UserControlStats_ShipsDangerous,
        UserControlStats_ShipsMaster,
        UserControlStats_ShipsExpert,
        UserControlStats_ShipsCompetent,
        UserControlStats_ShipsNovice,
        UserControlStats_ShipsHarmless,
        UserControlStats_PVPElite,
        UserControlStats_PVPDeadly,
        UserControlStats_PVPDangerous,
        UserControlStats_PVPMaster,
        UserControlStats_PVPExpert,
        UserControlStats_PVPCompetent,
        UserControlStats_PVPNovice,
        UserControlStats_PVPHarmless,


        UserControlOrganics_extCheckBoxShowIncomplete_ToolTip, // ToolTip 'Display incomplete scans'
        UserControlOrganics_extButtonShowControl_ToolTip, // ToolTip 'Display Settings'
        UserControlOrganics_extButtonFont_ToolTip, // ToolTip 'Font'
        UserControlOrganics_extCheckBoxWordWrap_ToolTip, // ToolTip 'Word Wrap'
        UserControlOrganics_extButtonAlignment_ToolTip, // ToolTip 'Word Wrap'
        UserControlOrganics_at, // At {0}
        UserControlOrganics_sysinfo, // 




        UserControlShoppingList_NM, // Needed Mats
        UserControlShoppingList_FS, // Filling Shopping List would exceed capacity for:
        UserControlShoppingList_NoMat, // No materials currently required.
        UserControlShoppingList_FSD, // Max FSD Injections
        UserControlShoppingList_MO, // Materials on {0}

        UserControlContainerSplitter_sizeRatioToolStripMenuItem, // ToolStrip control 'Set Size Ratio'

        UserControlContainerSplitter_RC, // Right click on splitter bar to change orientation
        UserControlContainerSplitter_ChangetoHorizontalSplit, // Change to Horizontal Split
        UserControlContainerSplitter_ChangetoVerticalSplit, // Change to Vertical Split
        UserControlContainerSplitter_SplitLeftPanel, // Split Left Panel
        UserControlContainerSplitter_SplitTopPanel, // Split Top Panel
        UserControlContainerSplitter_SplitRightPanel, // Split Right Panel
        UserControlContainerSplitter_SplitBottomPanel, // Split Bottom Panel
        UserControlContainerSplitter_MergeLeftPanel, // Merge Left Panel
        UserControlContainerSplitter_MergeTopPanel, // Merge Top Panel
        UserControlContainerSplitter_MergeRightPanel, // Merge Right Panel
        UserControlContainerSplitter_MergeBottomPanel, // Merge Bottom Panel

        UserControlContainerGrid_buttonExtTile_ToolTip, // ToolTip 'Tile the grid panels'
        UserControlContainerGrid_buttonExtDelete_ToolTip, // ToolTip 'Remove selected panel'
        UserControlContainerGrid_buttonExtPopOut_ToolTip, // ToolTip 'Click to select a pop out panel to display'

        UserControlJournalGrid_ColumnTime, // Column Header 'Time'
        UserControlJournalGrid_Event, // Column Header 'Event'
        UserControlJournalGrid_ColumnType, // Column Header 'Description'
        UserControlJournalGrid_ColumnText, // Column Header 'Information'
        UserControlJournalGrid_labelTime, // Control 'Time'
        UserControlJournalGrid_labelSearch, // Control 'Search'
        UserControlJournalGrid_removeSortingOfColumnsToolStripMenuItem, // ToolStrip control 'Remove sorting of columns'
        UserControlJournalGrid_jumpToEntryToolStripMenuItem, // ToolStrip control 'Jump to Entry'
        UserControlJournalGrid_mapGotoStartoolStripMenuItem, // ToolStrip control 'Go to star on 3D Map'
        UserControlJournalGrid_viewOnEDSMToolStripMenuItem, // ToolStrip control 'View on EDSM'
        UserControlJournalGrid_toolStripMenuItemStartStop, // ToolStrip control 'Set Start/Stop point for travel calculations'
        UserControlJournalGrid_runActionsOnThisEntryToolStripMenuItem, // ToolStrip control 'Run Actions on this entry'
        UserControlJournalGrid_copyJournalEntryToClipboardToolStripMenuItem, // ToolStrip control 'Copy journal entry to clipboard'
        UserControlJournalGrid_comboBoxTime_ToolTip, // ToolTip 'Select the entries selected by age'
        UserControlJournalGrid_textBoxSearch_ToolTip, // ToolTip 'Enter text to search in any fields for an item'
        UserControlJournalGrid_buttonFilter_ToolTip, // ToolTip 'Filter out entries based on event type'
        UserControlJournalGrid_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'
        UserControlJournalGrid_checkBoxCursorToTop_ToolTip, // ToolTip 'Automatically move the cursor to the latest entry when it arrives'
        UserControlJournalGrid_TT1, //  showing {0} original {1}
        UserControlJournalGrid_TTFilt1, // Filtered {0}
        UserControlJournalGrid_TTSelAge, // Select the entries by age,
        UserControlJournalGrid_JHF, // Journal: Filter out fields
        UserControlJournalGrid_NotSynced, // System could not be found - has not been synched or EDSM is unavailable

        UserControlTravelGrid_quickMarkToolStripMenuItem,
        UserControlTravelGrid_removeSortingOfColumnsToolStripMenuItem, // ToolStrip control 'Remove Sorting of Columns'
        UserControlTravelGrid_gotoEntryNumberToolStripMenuItem, // ToolStrip control 'Jump to Entry'
        UserControlTravelGrid_setNoteToolStripMenuItem, // ToolStrip control 'Set Note'
        UserControlTravelGrid_createEditBookmarkToolStripMenuItem, // ToolStrip control 'Create/Edit Bookmark'
        UserControlTravelGrid_toolStripMenuItemStartStop, // ToolStrip control 'Set Start/Stop point for travel calculations'
        UserControlTravelGrid_gotoNextStartStopMarkerToolStripMenuItem, // ToolStrip control 'Jump to next Start/Stop marker'
        UserControlTravelGrid_mapGotoStartoolStripMenuItem, // ToolStrip control 'Go to star on 3D Map'
        UserControlTravelGrid_viewOnEDSMToolStripMenuItem, // ToolStrip control 'View on EDSM'
        UserControlTravelGrid_starMapColourToolStripMenuItem, // ToolStrip control 'Star Map Colour...'
        UserControlTravelGrid_addToTrilaterationToolStripMenuItem, // ToolStrip control 'Add to ...'
        UserControlTravelGrid_addToTrilaterationToolStripMenuItem_trilaterationToolStripMenuItem, // ToolStrip control 'System on Trilateration Panel'
        UserControlTravelGrid_addToTrilaterationToolStripMenuItem_wantedSystemsToolStripMenuItem, // ToolStrip control 'Wanted Systems on Trilateration Panel'
        UserControlTravelGrid_addToTrilaterationToolStripMenuItem_bothToolStripMenuItem, // ToolStrip control 'Both of the above'
        UserControlTravelGrid_addToTrilaterationToolStripMenuItem_expeditionToolStripMenuItem, // ToolStrip control 'Expedition Panel'

        UserControlTravelGrid_moveToAnotherCommanderToolStripMenuItem, // ToolStrip control 'Move Entries to another Commander'
        UserControlTravelGrid_hideSystemToolStripMenuItem, // ToolStrip control 'Hide Entries'
        UserControlTravelGrid_removeJournalEntryToolStripMenuItem, // ToolStrip control 'Remove Journal Entry'
        UserControlTravelGrid_runActionsOnThisEntryToolStripMenuItem, // ToolStrip control 'Run Actions on this entry'
        UserControlTravelGrid_copyJournalEntryToClipboardToolStripMenuItem, // ToolStrip control 'Copy journal entry to clipboard/Log'
        UserControlTravelGrid_writeEventInfoToLogDebugToolStripMenuItem, // ToolStrip control 'Write event class info to Log (Debug)'

        UserControlTravelGrid_runActionsAcrossSelectionToolSpeechStripMenuItem, // ToolStrip control 'Run actions across selection for speech debugging (Debug)'
        UserControlTravelGrid_runSelectionThroughInaraSystemToolStripMenuItem, // ToolStrip control 'Run selection through Inara System (Debug)'
        UserControlTravelGrid_runEntryThroughProfileSystemToolStripMenuItem, // ToolStrip control 'Run entry through Profile System (Debug)'
        UserControlTravelGrid_runSelectionThroughIGAUDebugToolStripMenuItem, // ToolStrip control 'Run selection through IGAU (Debug)'
        UserControlTravelGrid_runSelectionThroughEDDNThruTestToolStripMenuItem, // ToolStrip control 'Run selection through EDDN Test Server (Debug)'
        UserControlTravelGrid_runSelectionThroughEDAstroDebugToolStripMenuItem, // ToolStrip control 'Run selection through EDAstro (Debug)'
        UserControlTravelGrid_sendJournalEntriesToDLLsToolStripMenuItem, // ToolStrip control 'Send selection to DLLs (Debug)'
        UserControlTravelGrid_showSystemVisitedForeColourToolStripMenuItem, // ToolStrip control 'Show lines in System Visited Fore Colour'
        UserControlTravelGrid_travelGridInDebugModeToolStripMenuItem, // ToolStrip control 'Travel grid in Debug Mode'

        UserControlTravelGrid_outliningOnOffToolStripMenuItem, // ToolStrip control 'Outlining On'
        UserControlTravelGrid_scanEventsOutliningOnOffToolStripMenuItem, // ToolStrip control 'Scan Events Outlining'
        UserControlTravelGrid_toolStripRollUpOlderOutlines, // ToolStrip control 'Roll up older entries'
        UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpOffToolStripMenuItem, // ToolStrip control 'Off'
        UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpAfterFirstToolStripMenuItem, // ToolStrip control 'After First'
        UserControlTravelGrid_toolStripRollUpOlderOutlines_rollUpAfter5ToolStripMenuItem, // ToolStrip control 'After 5'

        UserControlTravelGrid_comboBoxTime_ToolTip, // ToolTip 'Select the entries by age'
        UserControlTravelGrid_textBoxSearch_ToolTip, // ToolTip 'Enter text to search in any fields for an item'
        UserControlTravelGrid_buttonFilter_ToolTip, // ToolTip 'Filter out entries based on event type'
        UserControlTravelGrid_buttonField_ToolTip, // ToolTip 'Filter out entries matching the field selection'
        UserControlTravelGrid_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'
        UserControlTravelGrid_checkBoxCursorToTop_ToolTip, // ToolTip 'Automatically move the cursor to the latest entry when it arrives'
        UserControlTravelGrid_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'
        UserControlTravelGrid_extCheckBoxOutlines_ToolTip, // ToolTip 'Control Outlining'

        UserControlTravelGrid_ColumnTime, // Column Header 'Time'
        UserControlTravelGrid_Icon, // Column Header 'Event'
        UserControlTravelGrid_ColumnSystem, // Column Header 'Description'
        UserControlTravelGrid_ColumnDistance, // Column Header 'Information'
        UserControlTravelGrid_ColumnNote, // Column Header 'Note'
        UserControlTravelGrid_labelTime, // Control 'Time'
        UserControlTravelGrid_labelSearch, // Control 'Search'
        UserControlTravelGrid_THF, // History: Filter out fields
        UserControlTravelGrid_TT1, //  showing {0} original {1}
        UserControlTravelGrid_TTFilt1, // Filtered {0}
        UserControlTravelGrid_TTSelAge, // Select the entries by age,
        UserControlTravelGrid_CSTART, // Clear Start marker
        UserControlTravelGrid_CSTOP, // Clear Stop marker
        UserControlTravelGrid_SETSTOPTC, // Set Stop marker for travel calculations
        UserControlTravelGrid_SETSTARTTC, // Set Start marker for travel calculations
        UserControlTravelGrid_SETSTSTOP, // Set Start/Stop point for travel calculations
        UserControlTravelGrid_NotSynced, // System could not be found - has not been synched or EDSM is unavailable
        UserControlTravelGrid_Remove, // Confirm you wish to remove this entry

        UserControlTravelGrid_SearchTerms,      // you must define this one fully if you include it

        UserControlStarList_ColumnTime, // Column Header 'Last Visit'
        UserControlStarList_ColumnSystem, // Column Header 'System'
        UserControlStarList_ColumnVisits, // Column Header 'Visits'
        UserControlStarList_ColumnInformation, // Column Header 'Information'
        UserControlStarList_Value, // Column Header 'Scan Value'
        UserControlStarList_labelTime, // Control 'Time'
        UserControlStarList_labelSearch, // Control 'Search'
        UserControlStarList_removeSortingOfColumnsToolStripMenuItem, // ToolStrip control 'Remove sorting of columns'
        UserControlStarList_mapGotoStartoolStripMenuItem, // ToolStrip control 'Go to star on 3D Map'
        UserControlStarList_viewOnEDSMToolStripMenuItem, // ToolStrip control 'View on EDSM'
        UserControlStarList_setNoteToolStripMenuItem, // ToolStrip control 'Set Note'
        UserControlStarList_viewScanDisplayToolStripMenuItem, // ToolStrip control 'View Scan Display'
        UserControlStarList_comboBoxTime_ToolTip, // ToolTip 'Select the entries by age'
        UserControlStarList_checkBoxEDSM_ToolTip, // ToolTip 'Show/Hide Body data from EDSM.'
        UserControlStarList_textBoxSearch_ToolTip, // ToolTip 'Enter text to search in any fields for an item'
        UserControlStarList_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'
        UserControlStarList_checkBoxCursorToTop_ToolTip, // ToolTip 'Automatically move the cursor to the latest entry when it arrives'
        UserControlStarList_Total, // Total
        UserControlStarList_CS, // {0} Star(s)
        UserControlStarList_FSD, // This system has materials for FSD boost:
        UserControlStarList_NoEDSM, // System could not be found - has not been synched or EDSM is unavailable
        UserControlStarList_OB, // {0} Other bodies
        UserControlStarList_JUMP, // discrete
        UserControlStarList_valueables, // discrete
        UserControlStarList_scanorganics, // discrete
        UserControlStarList_showcodex, // discrete

        UserControlCaptainsLog_Diary, // Diary
        UserControlCaptainsLog_Entries, // Entries

        CaptainsLogEntries_ColTime, // Column Header 'Time'
        CaptainsLogEntries_ColSystem, // Column Header 'System'
        CaptainsLogEntries_ColBodyName, // Column Header 'Body'
        CaptainsLogEntries_ColNote, // Column Header 'Note'
        CaptainsLogEntries_ColTags, // Column Header 'Tags'
        CaptainsLogEntries_labelDateStart, // Control 'Start Date:'
        CaptainsLogEntries_labelEndDate, // Control 'End Date:'
        CaptainsLogEntries_labelSearch, // Control 'Search'
        CaptainsLogEntries_toolStripMenuItemGotoStar3dmap, // ToolStrip control 'Goto in 3D Map'
        CaptainsLogEntries_openInEDSMToolStripMenuItem, // ToolStrip control 'Open in EDSM'
        CaptainsLogEntries_openAScanPanelViewToolStripMenuItem, // ToolStrip control 'Open a Scan Panel View'
        CaptainsLogEntries_textBoxFilter_ToolTip, // ToolTip 'Search for Log Entries'
        CaptainsLogEntries_buttonNew_ToolTip, // ToolTip 'New Entry'
        CaptainsLogEntries_buttonDelete_ToolTip, // ToolTip 'Delete selected entries'
        CaptainsLogEntries_buttonTags_ToolTip, // ToolTip 'Edit Tags'

        CaptainsLogDiary_Daysofweek, // Sun;Mon;Tue;Wed;Thu;Fri;Sat
        CaptainsLogEntries_DTF, // Bad Date Time format
        CaptainsLogEntries_Note, // Note:
        CaptainsLogEntries_EnterNote, // Enter Note
        CaptainsLogEntries_CFN, // Do you really want to delete {0} notes?
        CaptainsLogEntries_CF, // Do you really want to delete the note for {0}
        CaptainsLogEntries_SysU, // System could not be found - has not been synched or EDSM is unavailable
        CaptainsLogEntries_NSS, // No such system
        CaptainsLogEntries_SetTags, // Set Tags

        TagsForm_buttonMore_ToolTip, // ToolTip 'Add more tags'
        TagsForm_TN, // Tag name
        TagsForm_SI, // Select image for this tag
        TagsForm_DE, // Delete entry

        UserControlBookmarks_ColType, // Column Header 'Bookmark Type'
        UserControlBookmarks_ColBookmarkName, // Column Header 'Bookmark Name'
        UserControlBookmarks_ColDescription, // Column Header 'Description'
        UserControlBookmarks_labelSearch, // Control 'Search'
        UserControlBookmarks_toolStripMenuItemGotoStar3dmap, // ToolStrip control 'Goto in 3D Map'
        UserControlBookmarks_openInEDSMToolStripMenuItem, // ToolStrip control 'Open in EDSM'
        UserControlBookmarks_textBoxFilter_ToolTip, // ToolTip 'Search for Bookmark'
        UserControlBookmarks_buttonNew_ToolTip, // ToolTip 'New Bookmark'
        UserControlBookmarks_buttonEdit_ToolTip, // ToolTip 'Edit Selected Bookmark'
        UserControlBookmarks_extButtonEditSystem_ToolTip, // ToolTip 'Edit Bookmark on current system'
        UserControlBookmarks_buttonDelete_ToolTip, // ToolTip 'Delete selected bookmark'
        UserControlBookmarks_buttonExtExcel_ToolTip, // ToolTip 'Export bookmarks to CSV file'
        UserControlBookmarks_buttonExtImport_ToolTip, // ToolTip 'Import bookmarks to EDD from CSV file'
        UserControlBookmarks_extButtonNewRegion_ToolTip,
        UserControlBookmarks_CFN, // Do you really want to delete {0} bookmarks?
        UserControlBookmarks_CF, // Do you really want to delete the bookmark for {0}
        UserControlBookmarks_SysU, // System could not be found - has not been synched or EDSM is unavailable

        UserControlCombatPanel_Time, // Column Header 'Time'
        UserControlCombatPanel_Event, // Column Header 'Event'
        UserControlCombatPanel_Description, // Column Header 'Description'
        UserControlCombatPanel_Reward, // Column Header 'Reward'
        UserControlCombatPanel_labelCredits, // Control 'Credits'
        UserControlCombatPanel_labelTotalKills, // Control 'Kills'
        UserControlCombatPanel_labelFactionKills, // Control 'Faction Kills'
        UserControlCombatPanel_labelBalance, // Control 'Balance'
        UserControlCombatPanel_labelTotalCrimes, // Control 'Crimes'
        UserControlCombatPanel_labelTarget, // Control 'Target'
        UserControlCombatPanel_labelFaction, // Control 'Faction'
        UserControlCombatPanel_labelTotalReward, // Control 'TotalReward'
        UserControlCombatPanel_labelFactionReward, // Control 'FactionReward'
        UserControlCombatPanel_buttonExtEditCampaign, // Control 'Edit'
        UserControlCombatPanel_checkBoxCustomGridOn, // Control 'Grid'
        UserControlCombatPanel_labelCredits_ToolTip, // ToolTip 'Current Credits'
        UserControlCombatPanel_labelTotalKills_ToolTip, // ToolTip 'Total kills (NPC/PVP) in campaign'
        UserControlCombatPanel_labelFactionKills_ToolTip, // ToolTip 'Faction Kills'
        UserControlCombatPanel_labelBalance_ToolTip, // ToolTip 'Total reward less any costs (fines, rebuys etc)'
        UserControlCombatPanel_labelTotalCrimes_ToolTip, // ToolTip 'How many times you've been caught!'
        UserControlCombatPanel_labelFaction_ToolTip, // ToolTip 'Target Faction'
        UserControlCombatPanel_labelTotalReward_ToolTip, // ToolTip 'Total reward'
        UserControlCombatPanel_labelFactionReward_ToolTip, // ToolTip 'Reward associated with destroying the faction ships'
        UserControlCombatPanel_comboBoxCustomCampaign_ToolTip, // ToolTip 'Select Campaign to view'
        UserControlCombatPanel_buttonExtEditCampaign_ToolTip, // ToolTip 'Edit user defined campaign'
        UserControlCombatPanel_checkBoxCustomGridOn_ToolTip, // ToolTip 'Show grid when in transparent mode'
        UserControlCombatPanel_Kills, // Kills:
        UserControlCombatPanel_Faction, // Faction:
        UserControlCombatPanel_Crimes, // Crimes:
        UserControlCombatPanel_Bal, // Bal:
        UserControlCombatPanel_labelDied, // Died
        UserControlCombatPanel_NewCampaign, // New Campaign
        UserControlCombatPanel_SinceLastDock, // Since Last Dock
        UserControlCombatPanel_Name, // Name:
        UserControlCombatPanel_C1, // Give name to campaign
        UserControlCombatPanel_C2, // Optional faction to target
        UserControlCombatPanel_Start, // Start:
        UserControlCombatPanel_C3, // Select Start time
        UserControlCombatPanel_End, // End:
        UserControlCombatPanel_C4, // Select Start time
        UserControlCombatPanel_C5, // Press to Accept
        UserControlCombatPanel_C6, // Press to Cancel
        UserControlCombatPanel_C7, // Press to Delete
        UserControlCombatPanel_NoOverwrite, // Name of campaign already in use, cannot overwrite
        UserControlCombatPanel_Condel, // Confirm deletion of {0}
        UserControlCombatPanel_NC, // Name changed - can't delete
        UserControlCombatPanel_NT, // No Target

        UserControlMissionOverlay_missionNameToolStripMenuItem, // ToolStrip control 'Mission Name'
        UserControlMissionOverlay_missionDescriptionToolStripMenuItem, // ToolStrip control 'Mission Description'
        UserControlMissionOverlay_startDateToolStripMenuItem, // ToolStrip control 'Start Date'
        UserControlMissionOverlay_endDateToolStripMenuItem, // ToolStrip control 'End Date'
        UserControlMissionOverlay_factionInformationToolStripMenuItem, // ToolStrip control 'Faction Information'
        UserControlMissionOverlay_rewardToolStripMenuItem, // ToolStrip control 'Reward'
        UserControlMissionOverlay_IL, //  Left

        UserControlMiningOverlay_extCheckBoxZeroRefined_ToolTip, // ToolTip 'Display items with zero refined items'
        UserControlMiningOverlay_buttonExtExcel_ToolTip, // ToolTip 'Export'
        UserControlMiningOverlay_extComboBoxChartOptions_ToolTip, // ToolTip 'Select chart options'
        UserControlMiningOverlay_Limcargo, // discrete
        UserControlMiningOverlay_Proscoll, // discrete
        UserControlMiningOverlay_ref, // discrete
        UserControlMiningOverlay_pros, // discrete
        UserControlMiningOverlay_coll, // discrete
        UserControlMiningOverlay_ratio, // discrete
        UserControlMiningOverlay_avg, // discrete
        UserControlMiningOverlay_min, // discrete
        UserControlMiningOverlay_max,// discrete 
        UserControlMiningOverlay_mload, // discrete
        UserControlMiningOverlay_hml, // discrete 
        UserControlMiningOverlay_discv, // discrete
        UserControlMiningOverlay_content, // discrete
        UserControlMiningOverlay_above, // discrete
        UserControlMiningOverlay_dist, // discrete
        UserControlMiningOverlay_astpros, // discrete

        UserControlSpanel_extButtonShowControl_ToolTip, // ToolTip 'Configure overall settings'
        UserControlSpanel_extButtonColumns_ToolTip, // ToolTip 'Configure columns shown'
        UserControlSpanel_extButtonColumnOrder_ToolTip, // ToolTip 'Configure column order'
        UserControlSpanel_extButtonHabZones_ToolTip, // ToolTip 'Configure hab zone information'
        UserControlSpanel_buttonFilter_ToolTip, // ToolTip 'Select what journal events to show'
        UserControlSpanel_buttonField_ToolTip, // ToolTip 'Filter out events'
        UserControlSpanel_extButtonScanShow_ToolTip, // ToolTip 'Configure scan display'
        UserControlSpanel_extButtoScanPos_ToolTip, // ToolTip 'Set position of scan display'
        UserControlSpanel_extButtonFont_ToolTip, // ToolTip 'Font'
        UserControlSpanel_extCheckBoxWordWrap_ToolTip, // ToolTip 'Word Wrap'
        UserControlSpanel_Target, // Target
        UserControlSpanel_TVE, // View system on EDSM
        UserControlSpanel_SPF, // Summary Panel: Filter out fields

        UserControlSurveyor_extButtonPlanets_ToolTip, // ToolTip 'Planet Filter'
        UserControlSurveyor_extButtonStars_ToolTip, // ToolTip 'Star Filter'
        UserControlSurveyor_extButtonShowControl_ToolTip, // ToolTip 'Display Settings'
        UserControlSurveyor_extButtonAlignment_ToolTip, // ToolTip 'Alignment'
        UserControlSurveyor_extButtonFSS_ToolTip, // ToolTip 'FSS Signal Selection'
        UserControlSurveyor_checkBoxEDSM_ToolTip, // ToolTip 'EDSM lookup toggle'
        UserControlSurveyor_extButtonSetRoute_ToolTip, // ToolTip 'Select route to follow'
        UserControlSurveyor_extButtonControlRoute_ToolTip, // ToolTip 'Route Settings'
        UserControlSurveyor_extButtonFont_ToolTip, // ToolTip 'Font'
        UserControlSurveyor_extCheckBoxWordWrap_ToolTip, // ToolTip 'Word Wrap'
        UserControlSurveyor_extButtonSearches_ToolTip,
        UserControlSurveyor_Scan, //  Scan
        UserControlSurveyor_Systemscancomplete, // System scan complete.
        UserControlSurveyor_bodiesdetected, //  bodies detected.
        UserControlSurveyor_bodiesfound, //  bodies found.
        UserControlSurveyor_fuel, //  bodies found.
        UserControlSurveyor_navroute, //  bodies found.
        UserControlSurveyor_fsssignals,

        UserControlSurveyor_showAllPlanetsToolStripMenuItem,
        UserControlSurveyor_planetaryClassesToolStripMenuItem_ammoniaWorldToolStripMenuItem,
        UserControlSurveyor_planetaryClassesToolStripMenuItem_earthlikeWorldToolStripMenuItem,
        UserControlSurveyor_planetaryClassesToolStripMenuItem_waterWorldToolStripMenuItem,
        UserControlSurveyor_planetaryClassesToolStripMenuItem_highMetalContentBodyToolStripMenuItem,
        UserControlSurveyor_planetaryClassesToolStripMenuItem_metalToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_terraformableToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasVolcanismToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasRingsToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_highEccentricityToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_lowRadiusToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasSignalsToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasGeologicalSignalsToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_hasBiologicalSignalsToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithAtmosphereToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableAndLargeToolStripMenuItem,
        UserControlSurveyor_bodyFeaturesToolStripMenuItem_landableWithVolcanismToolStripMenuItem,
        UserControlSurveyor_showAllStarsToolStripMenuItem,
        UserControlSurveyor_showBeltClustersToolStripMenuItem,
        UserControlSurveyor_showstarclassToolStripMenuItem,
        UserControlSurveyor_showValuesToolStripMenuItem,
        UserControlSurveyor_showMoreInformationToolStripMenuItem,
        UserControlSurveyor_showGravityToolStripMenuItem,
        UserControlSurveyor_showAtmosToolStripMenuItem,
        UserControlSurveyor_showVolcanismToolStripMenuItem,
        UserControlSurveyor_showSignalsToolStripMenuItem,
        UserControlSurveyor_autoHideToolStripMenuItem,
        UserControlSurveyor_dontHideInFSSModeToolStripMenuItem,
        UserControlSurveyor_hideAlreadyMappedBodiesToolStripMenuItem,
        UserControlSurveyor_showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem,
        UserControlSurveyor_showDividersToolStripMenuItem,
        UserControlSurveyor_textAlignToolStripMenuItem_leftToolStripMenuItem,
        UserControlSurveyor_textAlignToolStripMenuItem_centerToolStripMenuItem,
        UserControlSurveyor_textAlignToolStripMenuItem_rightToolStripMenuItem,

        // Note now in surveryor
        UserControlRouteTracker_showJumpsToolStripMenuItem,
        UserControlRouteTracker_showWaypointCoordinatesToolStripMenuItem,
        UserControlRouteTracker_showDeviationFromRouteToolStripMenuItem,
        UserControlRouteTracker_showBookmarkNotesToolStripMenuItem,
        UserControlRouteTracker_autoCopyWPToolStripMenuItem,
        UserControlRouteTracker_autoSetTargetToolStripMenuItem,
        UserControlRouteTracker_showtargetinfo,
        UserControlRouteTracker_showfuelinfo,

        UserControlRouteTracker_J1, // jump
        UserControlRouteTracker_TF, // To First WP
        UserControlRouteTracker_JS, // jumps
        UserControlRouteTracker_FL, // From Last WP
        UserControlRouteTracker_LF, // , Left {0:N1}ly
        UserControlRouteTracker_ToWP, // (WP {0})
        UserControlRouteTracker_NoFSD, //  No Ship FSD Information
        UserControlRouteTracker_Dev, // , Dev {0:N1}ly
        UserControlRouteTracker_NoWay, // Route contains no waypoints
        UserControlRouteTracker_NoCo, // No systems in route have known co-ords
        UserControlRouteTracker_Note, // Note

        UserControlNotePanel_miGMPNotes, // ToolStrip control 'Display galactic mapping notes'
        UserControlNotePanel_miSystemNotes, // ToolStrip control 'Display system notes'

        UserControlSysInfo_toolStripSystem, // ToolStrip control 'Display System Name'
        UserControlSysInfo_toolStripEDSM, // ToolStrip control 'Display EDSM Buttons'
        UserControlSysInfo_toolStripEDSMDownLine, // ToolStrip control 'EDSM buttons on separate line'
        UserControlSysInfo_toolStripVisits, // ToolStrip control 'Display Visits'
        UserControlSysInfo_toolStripBody, // ToolStrip control 'Display Body Name'
        UserControlSysInfo_displayStationButtonsToolStripMenuItem, // ToolStrip control 'Display Station Buttons'
        UserControlSysInfo_displayStationFactionToolStripMenuItem, // ToolStrip control 'Display Station Faction'
        UserControlSysInfo_toolStripPosition, // ToolStrip control 'Display Position'
        UserControlSysInfo_toolStripDistanceFrom, // ToolStrip control 'Display Distance From'
        UserControlSysInfo_toolStripSystemState, // ToolStrip control 'Display System State'
        UserControlSysInfo_displaySecurityToolStripMenuItem, // ToolStrip control 'Display Security'
        UserControlSysInfo_toolStripTarget, // ToolStrip control 'Display Target'
        UserControlSysInfo_toolStripShip, // ToolStrip control 'Display Ship Information'
        UserControlSysInfo_displayShipButtonsToolStripMenuItem, // ToolStrip control 'Display Ship Buttons'
        UserControlSysInfo_toolStripFuel, // ToolStrip control 'Display Fuel Level'
        UserControlSysInfo_toolStripCargo, // ToolStrip control 'Display Cargo Count'
        UserControlSysInfo_toolStripDataCount, // ToolStrip control 'Display Data Count'
        UserControlSysInfo_toolStripMaterialCounts, // ToolStrip control 'Display Material Count'
        UserControlSysInfo_displayMicroresourcesCountToolStripMenuItem, // ToolStrip control 'Display Microresources Count'
        UserControlSysInfo_toolStripCredits, // ToolStrip control 'Display Credits'
        UserControlSysInfo_toolStripGameMode, // ToolStrip control 'Display Game Mode'
        UserControlSysInfo_toolStripTravel, // ToolStrip control 'Display Travel Trip Statistics'
        UserControlSysInfo_toolStripMissionList, // ToolStrip control 'Display Mission List'
        UserControlSysInfo_toolStripJumpRange, // ToolStrip control 'Display Jump Range'
        UserControlSysInfo_toolStripSkinny, // ToolStrip control 'When transparent, use skinny look'
        UserControlSysInfo_toolStripReset, // ToolStrip control 'Reset'
        UserControlSysInfo_toolStripRemoveAll, // ToolStrip control 'Remove All'
        UserControlSysInfo_displayNextDestinationToolStripMenuItem,

        UserControlSysInfo_ToolTip, // ToolTip 'Hold down Ctrl Key then left drag a item to reposition, 8 columns are available'
        UserControlSysInfo_textBoxTargetDist_ToolTip, // ToolTip 'Distance to target'
        UserControlSysInfo_textBoxTarget_ToolTip, // ToolTip 'Sets the target'

        UserControlSysInfo_labelStationFaction, // Control 'Faction'
        UserControlSysInfo_extButtonEDSMTarget, // Control 'EDSM'
        UserControlSysInfo_labelSecurity, // Control 'Security'
        UserControlSysInfo_labelJumpRange, // Control 'Jump'
        UserControlSysInfo_labelTarget, // Control 'Target'
        UserControlSysInfo_labelSysName, // Control 'System'
        UserControlSysInfo_labelGamemode, // Control 'Mode'
        UserControlSysInfo_labelTravel, // Control 'Travel'
        UserControlSysInfo_labelOpenShip, // Control 'Open'
        UserControlSysInfo_labelOpenStation, // Control 'Open'
        UserControlSysInfo_labelOpen, // Control 'Open'
        UserControlSysInfo_labelCargo, // Control 'Cargo'
        UserControlSysInfo_labelCredits, // Control 'Credits'
        UserControlSysInfo_labelShip, // Control 'Ship'
        UserControlSysInfo_labelMaterials, // Control 'Materials'
        UserControlSysInfo_labelVisits, // Control 'Visits'
        UserControlSysInfo_labelMR, // Control 'MR'
        UserControlSysInfo_labelData, // Control 'Data'
        UserControlSysInfo_labelFuel, // Control 'Fuel'
        UserControlSysInfo_labelBodyName, // Control 'Body'
        UserControlSysInfo_labelPosition, // Control 'Pos'
        UserControlSysInfo_labelMissions, // Control 'Missions'
        UserControlSysInfo_labelEconomy, // Control 'Economy'
        UserControlSysInfo_labelGov, // Control 'Gov'
        UserControlSysInfo_labelAllegiance, // Control 'Allegiance'
        UserControlSysInfo_labelState, // Control 'State'
        UserControlSysInfo_labelSolDist, // Control 'Sol'
        UserControlSysInfo_labelHomeDist, // Control 'Home'
        UserControlSysInfo_labelNextDestination,
        UserControlSysInfo_NoMissions, // No Missions
        UserControlSysInfo_SysUnk, // System unknown to EDSM
        UserControlSysInfo_Pos, // Position is {0:0.00},{1:0.00},{2:0.00}
        UserControlSysInfo_Target, // On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter
        UserControlSysInfo_OnFoot, // discrete
        UserControlSysInfo_NA, // discrete

        UserControlExpedition_SystemName, // Column Header 'System Name'
        UserControlExpedition_Distance, // Column Header 'Dist.'
        UserControlExpedition_Note, // Column Header 'Note'
        UserControlExpedition_CurDist, // Column Header 'Cur. Dist'
        UserControlExpedition_Visits, // Column Header 'Visits'
        UserControlExpedition_Scans, // Column Header 'Scans'
        UserControlExpedition_FSSBodies, // Column Header 'FSS Bodies'
        UserControlExpedition_KnownBodies, // Column Header 'Known Bodies'
        UserControlExpedition_Stars, // Column Header 'Stars'
        UserControlExpedition_Info, // Column Header 'Info'
        UserControlExpedition_labelRouteName, // Control 'Route Name:'
        UserControlExpedition_labelDateStart, // Control 'Start Date:'
        UserControlExpedition_labelEndDate, // Control 'End Date:'
        UserControlExpedition_ColumnDistStart,
        UserControlExpedition_ColumnDistanceRemaining,
        UserControlExpedition_labelCml, // Control 'Cml Distance:'
        UserControlExpedition_labelP2P, // Control 'P2P Distance:'
        UserControlExpedition_buttonReverseRoute_ToolTip, // Control 'Reverse'
        UserControlExpedition_extCheckBoxWordWrap_ToolTip, // Control 'Enable or disable word wrap'
        UserControlExpedition_extButtonLoadRoute_ToolTip, // ToolTip 'Load Route'
        UserControlExpedition_extButtonNew_ToolTip, // ToolTip 'New Expedition'
        UserControlExpedition_extButtonSave_ToolTip, // ToolTip 'Save Expedition'
        UserControlExpedition_extButtonDelete_ToolTip, // ToolTip 'Delete Expedition'
        UserControlExpedition_extButtonImportFile_ToolTip, // ToolTip 'Import File'
        UserControlExpedition_extButtonImportRoute_ToolTip, // ToolTip 'Import from Route Panel'
        UserControlExpedition_extButtonImportNavRoute_ToolTip, // ToolTip 'Import a Nav Route'
        UserControlExpedition_extButtonNavRouteLatest_ToolTip, // ToolTip 'Import Latest Nav Route'
        UserControlExpedition_extButtonAddSystems_ToolTip, // ToolTip 'Import Systems from EDSM/DB'
        UserControlExpedition_buttonExtExport_ToolTip, // ToolTip 'Export to File'
        UserControlExpedition_extButtonShow3DMap_ToolTip, // ToolTip 'Show expedition on 3D Map'
        UserControlExpedition_extButtonDisplayFilters_ToolTip, // ToolTip 'Select filters on Info'
        UserControlExpedition_checkBoxEDSM_ToolTip, // ToolTip 'Show/Hide Body data from EDSM.'
        UserControlExpedition_copyToolStripMenuItem, // ToolStrip control 'Copy'
        UserControlExpedition_pasteToolStripMenuItem, // ToolStrip control 'Paste'
        UserControlExpedition_insertCopiedToolStripMenuItem, // ToolStrip control 'Insert Copied Rows'
        UserControlExpedition_deleteRowsToolStripMenuItem, // ToolStrip control 'Delete Rows'
        UserControlExpedition_setTargetToolStripMenuItem, // ToolStrip control 'Set Target'
        UserControlExpedition_editBookmarkToolStripMenuItem, // ToolStrip control 'Edit bookmark'
        UserControlExpedition_Unsaved, // Expedition - There are unsaved changes to the current route.
        UserControlExpedition_Conflict, // The current route name conflicts with a well-known expedition.
        UserControlExpedition_Overwrite, // Warning: route already exists. Would you like to overwrite it?
        UserControlExpedition_EDSMUnk,  // discrete
        UserControlExpedition_NoScanInfo, // discrete
        UserControlExpedition_Specify, // Please specify a name for the route.
        UserControlExpedition_Createroute, // Please create a route on a route panel
        UserControlExpedition_NoRouteExport, // There is no route to export
        UserControlExpedition_NoRoute, // No route set up. Please add at least two systems.
        UserControlExpedition_Delete, // Are you sure you want to delete this route?
        UserControlExpedition_AddSys,
        UserControlExpedition_GMOInfo, // Unknown system, system is without co-ordinates

        UserControlRoute_SystemCol, // Column Header 'System'
        UserControlRoute_DistanceCol, // Column Header 'Distance'
        UserControlRoute_WayPointDistCol, // Column Header 'Dist. Waypoint'
        UserControlRoute_DeviationCol, // Column Header 'Deviation'
        UserControlRoute_checkBox_FsdBoost, // Control 'Use Boosts'
        UserControlRoute_buttonExtTravelTo, // Control 'History'
        UserControlRoute_buttonExtTravelFrom, // Control 'History'
        UserControlRoute_buttonExtTargetTo, // Control 'Target'
        UserControlRoute_buttonToEDSM, // Control 'EDSM'
        UserControlRoute_buttonFromEDSM, // Control 'EDSM'
        UserControlRoute_buttonTargetFrom, // Control 'Target'
        UserControlRoute_cmd3DMap, // Control '3D Map'
        UserControlRoute_labelLy2, // Control 'ly'
        UserControlRoute_labelLy1, // Control 'ly'
        UserControlRoute_labelTo, // Control 'To'
        UserControlRoute_labelMaxJump, // Control 'Max jump'
        UserControlRoute_labelDistance, // Control 'Distance'
        UserControlRoute_labelMetric, // Control 'Metric'
        UserControlRoute_button_Route, // Control 'Find route'
        UserControlRoute_labelFrom, // Control 'From'
        UserControlRoute_showInEDSMToolStripMenuItem, // ToolStrip control 'Show in EDSM'
        UserControlRoute_copyToolStripMenuItem, // ToolStrip control 'Copy'
        UserControlRoute_showScanToolStripMenuItem, // ToolStrip control 'View Scan Display'
        UserControlRoute_checkBox_FsdBoost_ToolTip, // ToolTip 'The route finder will try and use FSD injections in case a direct route could not be found.'
        UserControlRoute_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'
        UserControlRoute_textBox_ToName_ToolTip, // ToolTip 'Alternate Name'
        UserControlRoute_textBox_FromName_ToolTip, // ToolTip 'Alternate name'
        UserControlRoute_comboBoxRoutingMetric_ToolTip, // ToolTip 'Pick the metric to use when searching for a route'
        UserControlRoute_buttonExtTravelTo_ToolTip, // ToolTip 'Copy the entry in the main travel grid to end route entry'
        UserControlRoute_buttonExtTravelFrom_ToolTip, // ToolTip 'Copy the entry in the main travel grid to start route entry'
        UserControlRoute_buttonExtTargetTo_ToolTip, // ToolTip 'Copy the target system to end route entry'
        UserControlRoute_buttonToEDSM_ToolTip, // ToolTip 'Open this end route system in EDSM'
        UserControlRoute_buttonFromEDSM_ToolTip, // ToolTip 'Open this start route system in EDSM'
        UserControlRoute_buttonTargetFrom_ToolTip, // ToolTip 'Copy the target system to start route entry'
        UserControlRoute_checkBoxEDSM_ToolTip, // ToolTip 'Lookup stars from EDSM if not found in database'
        UserControlRoute_cmd3DMap_ToolTip, // ToolTip 'Show route on 3D Map'
        UserControlRoute_textBox_From_ToolTip, // ToolTip 'Select system to start the route'
        UserControlRoute_textBox_Range_ToolTip, // ToolTip 'Give your jump range, or search range for long jumps'
        UserControlRoute_textBox_To_ToolTip, // ToolTip 'Select the system to end in'
        UserControlRoute_textBox_Distance_ToolTip, // ToolTip 'Distance between start and end'
        UserControlRoute_textBox_ToZ_ToolTip, // ToolTip 'Z Co-ord'
        UserControlRoute_textBox_ToY_ToolTip, // ToolTip 'Y (Vertical) Co-ord'
        UserControlRoute_textBox_ToX_ToolTip, // ToolTip 'X Co-Ord'
        UserControlRoute_textBox_FromZ_ToolTip, // ToolTip 'Z Co-ord'
        UserControlRoute_button_Route_ToolTip, // ToolTip 'Compute the route'
        UserControlRoute_textBox_FromY_ToolTip, // ToolTip 'Y (Vertical) Co-ord'
        UserControlRoute_textBox_FromX_ToolTip, // ToolTip 'X Co-ord'
        UserControlRoute_Confirm, // This will result in a large number ({0})) of jumps
        UserControlRoute_NoRoute, // No route set up, retry
        UserControlRoute_M1,    // discrete
        UserControlRoute_M2, // discrete
        UserControlRoute_M3, // discrete
        UserControlRoute_M4, // discrete
        UserControlRoute_M5, // discrete
        UserControlRoute_M6, // discrete

        UserControlWebBrowser_extButtonIE11Warning, // Control 'Using IE11 - click here to get WebView2'
        UserControlWebBrowser_extCheckBoxBack_ToolTip, // ToolTip 'Back'
        UserControlWebBrowser_extCheckBoxStar_ToolTip, // ToolTip 'Select another system to view'
        UserControlWebBrowser_checkBoxAutoTrack_ToolTip, // ToolTip 'Track system in history panel'
        UserControlWebBrowser_extCheckBoxAllowedList_ToolTip, // ToolTip 'URLs'
        UserControlWebBrowser_EnterSys, // Show System
        UserControlWebBrowser_System, // System:

        UserControlEstimatedValues_BodyName, // Column Header 'Body Name'
        UserControlEstimatedValues_BodyType, // Column Header 'Body Type'
        UserControlEstimatedValues_EDSM, // Column Header 'EDSM'
        UserControlEstimatedValues_Mapped, // Column Header 'Mapped'
        UserControlEstimatedValues_WasMapped, // Column Header 'Already Mapped'
        UserControlEstimatedValues_WasDiscovered, // Column Header 'Already Discovered'
        UserControlEstimatedValues_EstBase, // Column Header 'Base Value'
        UserControlEstimatedValues_MappedValue, // Column Header 'Mapped'
        UserControlEstimatedValues_FirstMappedEff, // Column Header 'First Mapped'
        UserControlEstimatedValues_FirstDiscMapped, // Column Header 'First Discovered Mapped'
        UserControlEstimatedValues_EstValue, // Column Header 'Current Value'
        UserControlEstimatedValues_checkBoxEDSM_ToolTip, // ToolTip 'Get and show information from EDSM'
        UserControlEstimatedValues_checkBoxShowZeros_ToolTip, // ToolTip 'Green will show materials with zero counts, red means remove them'
        UserControlEstimatedValues_extCheckBoxShowImpossible_ToolTip, // ToolTip 'Show all values, even ones which are impossible to get'
        UserControlEstimatedValues_SV, // Estimated Scan Values for {0}


        UserControlLog_toolStripMenuItemCopy, // ToolStrip control 'Copy'
        UserControlLog_clearLogToolStripMenuItem, // ToolStrip control 'Clear Log'

        UserControlScan_extCheckBoxStar_ToolTip, // ToolTip 'Select another system to view'
        UserControlScan_extButtonFilter_ToolTip, // ToolTip 'Filter Bodies'
        UserControlScan_extButtonDisplayFilters_ToolTip, // ToolTip 'Settings'
        UserControlScan_buttonSize_ToolTip, // ToolTip 'Select image size'
        UserControlScan_checkBoxEDSM_ToolTip, // ToolTip 'Show/Hide Body data from EDSM'
        UserControlScan_extButtonHighValue_ToolTip, // ToolTip 'Set High Value Limit'
        UserControlScan_buttonExtExcel_ToolTip, // ToolTip 'Export'
        UserControlScan_System, // System:
        UserControlScan_EnterSys, // Show System
        UserControlScan_VLMT, // Set Valuable Minimum

        UserControlScan_StatusIcons, // Show body status icons
        UserControlScan_SystemValue, // Show syatem and value in main display
        UserControlScan_MatFull, // Hide materials which have reached their storage limit
        UserControlScan_ShowMaterials, //  Show Materials
        UserControlScan_ShowRaresOnly, // Show rare materials only
        UserControlScan_ShowMoons, // Show Moons
        UserControlScan_Belt,   // discrete
        UserControlScan_Star, // discrete
        UserControlScan_Barycentre, // discrete
        UserControlScan_Body, // discrete
        UserControlScan_AllG,// discrete
        UserControlScan_HabZone,// discrete
        UserControlScan_PlanetClass,// discrete
        UserControlScan_StarClass,// discrete
        UserControlScan_Distance,// discrete
        UserControlScan_Expired,// discrete

        UserControlStarDistance_colName, // Column Header 'Name'
        UserControlStarDistance_colDistance, // Column Header 'Distance'
        UserControlStarDistance_colVisited, // Column Header 'Visited'
        UserControlStarDistance_labelExtMin, // Control 'Min'
        UserControlStarDistance_labelExtMax, // Control 'Max'
        UserControlStarDistance_checkBoxCube, // Control 'Cube'
        UserControlStarDistance_viewSystemToolStripMenuItem, // ToolStrip control 'View System'
        UserControlStarDistance_viewOnEDSMToolStripMenuItem1, // ToolStrip control 'View on EDSM'
        UserControlStarDistance_addToTrilaterationToolStripMenuItem1, // ToolStrip control 'Add to Trilateration'
        UserControlStarDistance_addToExpeditionToolStripMenuItem, // ToolStrip control 'Add to Expedition'
        UserControlStarDistance_textMinRadius_ToolTip, // ToolTip 'Minimum star distance in ly'
        UserControlStarDistance_textMaxRadius_ToolTip, // ToolTip 'Maximum star distance in ly'
        UserControlStarDistance_checkBoxCube_ToolTip, // ToolTip 'Check to indicate use a cube instead of a sphere for distances'
        UserControlStarDistance_From, // From {0}
        UserControlStarDistance_NoEDSMSys, // System could not be found - has not been synched or EDSM is unavailable

        UserControlTrilateration_ColumnSystem, // Column Header 'System'
        UserControlTrilateration_ColumnDistance, // Column Header 'Distance'
        UserControlTrilateration_ColumnCalculated, // Column Header 'Calculated'
        UserControlTrilateration_ColumnStatus, // Column Header 'Status'
        UserControlTrilateration_Source, // Column Header 'Source'
        UserControlTrilateration_dataGridViewTextBoxColumnClosestSystemsSystem, // Column Header 'Wanted System'
        UserControlTrilateration_toolStrip, // Control ''Press Start New''
        UserControlTrilateration_removeFromWantedSystemsToolStripMenuItem, // ToolStrip control 'Remove from wanted systems'
        UserControlTrilateration_viewOnEDSMToolStripMenuItem1, // ToolStrip control 'View on EDSM'
        UserControlTrilateration_deleteAllWithKnownPositionToolStripMenuItem, // ToolStrip control 'Delete all with known position'
        UserControlTrilateration_addAllLocalSystemsToolStripMenuItem, // ToolStrip control 'Add all local systems'
        UserControlTrilateration_addAllEDSMSystemsToolStripMenuItem, // ToolStrip control 'Add all EDSM systems'
        UserControlTrilateration_addAllSectorSystemsToolStripMenuItem, // ToolStrip control 'Add all Sector systems'
        UserControlTrilateration_addToWantedSystemsToolStripMenuItem, // ToolStrip control 'Add to wanted systems'
        UserControlTrilateration_viewOnEDSMToolStripMenuItem, // ToolStrip control 'View on EDSM'
        UserControlTrilateration_pasteToolStripMenuItem, // ToolStrip control 'Paste'
        UserControlTrilateration_toolStripButtonSubmitDistances, // ToolStrip control '&Submit Distances'
        UserControlTrilateration_toolStripButtonNew, // ToolStrip control 'Start &new'
        UserControlTrilateration_toolStripLabelSystem, // ToolStrip control 'From System:'
        UserControlTrilateration_toolStripLabel1, // ToolStrip control 'Visited without coordinates:'
        UserControlTrilateration_toolStripAddFromHistory, // ToolStrip control 'Add 20 oldest'
        UserControlTrilateration_toolStripAddRecentHistory, // ToolStrip control 'Add 20 newest'
        UserControlTrilateration_toolStripButtonSector, // ToolStrip control 'Sector Systems'
        UserControlTrilateration_ToolStripText, // Press Start New
        UserControlTrilateration_DUP, // Duplicate system entry is not allowed
        UserControlTrilateration_TP, // Trilateration Panel
        UserControlTrilateration_CHK, // You are about to submit distances from {0}.
        UserControlTrilateration_UPD, // Update 'From' system to current position ({0}) and submit entered distances?
        UserControlTrilateration_Sub, // Submitting system to EDSM, please wait...
        UserControlTrilateration_NOTREM, // {0} is pushed from EDSM and cannot be removed
        UserControlTrilateration_NoEDSM, // System could not be found - has not been synched or EDSM is unavailable
        UserControlTrilateration_Cmdr, // Please enter commander name before submitting the system!
        UserControlTrilateration_Horray, // EDSM submission succeeded, trilateration successful.
        UserControlTrilateration_Failed, // EDSM submission succeeded, but trilateration failed. Try adding more distances.
        UserControlTrilateration_Dead, // EDSM submission failed.
        UserControlTrilateration_Co, // Only systems with coordinates or already known to EDSM can be added
        UserControlTrilateration_PU, // Position unknown
        UserControlTrilateration_PF, // Position found
        UserControlTrilateration_NotProcGen, // Sector name could not be derived from system name (it may not be procedurally generated).  Not getting sector systems.
        UserControlTrilateration_NoSector, // No systems with unknown coordinates were found for the current sector.
        UserControlTrilateration_SectorCount, // {0} systems with unknown coordinates found in {1} sector.


        UserControlSearch_Stars, // Stars
        UserControlSearch_MaterialsCommodities, // Materials Commodities
        UserControlSearch_Scans, // Scans

        // used across multiple panels, but so much shared with above may as well use the same 

        SearchScans_ColumnDate, // Column Header 'Date'
        SearchScans_ColumnBody, // Column Header 'Body'
        SearchScans_ColumnInformation, // Column Header 'Information'
        SearchScans_ColumnParent, // Column Header 'Parent'
        SearchScans_ColumnParentParent, // Column Header 'Parent.Parent'
        SearchScans_ColumnStar, // Column Header 'Star'
        SearchScans_ColumnStarStar, // Column Header 'Star.Star'
        SearchScans_ColumnCurrentDistance, // Column Header 'Current Distance'
        SearchScans_ColumnPosition, // Column Header 'Position'
        SearchScans_ColumnSearches, // Column Header 'Searches'
        SearchScans_Select, // Select
        SearchScans_Name, // Name:
        SearchScans_SN, // Enter Search Name:
        SearchScans_DEL, // Confirm deletion of
        SearchScans_DELNO, // Cannot delete this entry
        SearchScans_CNV, // Condition is not valid
        SearchScans_CD, // Condition
        SearchScans_comboBoxSearches_ToolTip,
        SearchScans_buttonFind_ToolTip,
        SearchScans_buttonSave_ToolTip,
        SearchScans_buttonDelete_ToolTip, 
        SearchScans_buttonExtExcel_ToolTip,
        SearchScans_extCheckBoxDebug_ToolTip,
        SearchScans_extButtonNew_ToolTip,
        SearchScans_extButtonExport_ToolTip, 
        SearchScans_extButtonImport_ToolTip,
        SearchScans_extCheckBoxWordWrap_ToolTip,
        SearchScans_labelTime,
        SearchScans_labelSearch,
        SearchScans_Export,
        SearchScans_Import,
        SearchScans_TooMany,

        SearchScans_scanSortControl_labelSort,

        UserControlDiscoveries_textBoxSearch_ToolTip, 
        UserControlDiscoveries_extButtonSearches_ToolTip,
        UserControlDiscoveries_extCheckBoxWordWrap_ToolTip, 
        UserControlDiscoveries_buttonExtExcel_ToolTip,

        SearchMaterialsCommodities_ColumnDate, // Column Header 'Date'
        SearchMaterialsCommodities_ColumnStar, // Column Header 'Star'
        SearchMaterialsCommodities_ColumnLocation, // Column Header 'Location'
        SearchMaterialsCommodities_ColumnCurrentDistance, // Column Header 'Current Distance'
        SearchMaterialsCommodities_ColumnPosition, // Column Header 'Position'
        SearchMaterialsCommodities_label2, // Control 'Item 2'
        SearchMaterialsCommodities_label1, // Control 'Item 1'
        SearchMaterialsCommodities_RareCommodity, // , Rare Commodity
        SearchMaterialsCommodities_AND, // AND
        SearchMaterialsCommodities_OR, // OR
        SearchMaterialsCommodities_DIS, // Discovered at
        SearchMaterialsCommodities_COL, // Collected at
        SearchMaterialsCommodities_MR, // Mission Reward at

        SearchMaterialsCommodities_buttonExtExcel_ToolTip, 
        SearchMaterialsCommodities_buttonExtFind_ToolTip, 
        SearchMaterialsCommodities_comboBoxCustomCMANDOR_ToolTip,
        SearchMaterialsCommodities_comboBoxCustomCM1_ToolTip,
        SearchMaterialsCommodities_comboBoxCustomCM2_ToolTip,

        SearchStars_ColumnStar, // Column Header 'Star'
        SearchStars_ColumnIndex, // Column Header 'Star'
        SearchStars_ColumnCentreDistance, // Column Header 'Centre Distance'
        SearchStars_ColumnCurrentDistance, // Column Header 'Current Distance'
        SearchStars_ColumnPosition, // Column Header 'Position'

        DataGridViewStarResults_3d, // ToolStrip control 'Go to star on 3D Map'
        DataGridViewStarResults_EDSM, // ToolStrip control 'View on EDSM'
        DataGridViewStarResults_Data, // ToolStrip control 'View Data On Entry'
        DataGridViewStarResults_Goto, // ToolStrip control 'Go to entry on grid'

        UserControlMarketData_CategoryCol, // Column Header 'Category'
        UserControlMarketData_NameCol, // Column Header 'Name'
        UserControlMarketData_SellCol, // Column Header 'Sell'
        UserControlMarketData_BuyCol, // Column Header 'Buy'
        UserControlMarketData_CargoCol, // Column Header 'Cargo'
        UserControlMarketData_DemandCol, // Column Header 'Demand'
        UserControlMarketData_SupplyCol, // Column Header 'Supply'
        UserControlMarketData_GalAvgCol, // Column Header 'Galactic Avg'
        UserControlMarketData_ProfitToCol, // Column Header 'Profit To cr/t'
        UserControlMarketData_ProfitFromCol, // Column Header 'Profit From cr/t'
        UserControlMarketData_labelLocation, // Control 'No Data'
        UserControlMarketData_labelVs, // Control 'Vs'
        UserControlMarketData_checkBoxBuyOnly, // Control 'Buy Only'
        UserControlMarketData_checkBoxHasDemand, // Control 'Has Demand'
        UserControlMarketData_checkBoxAutoSwap, // Control 'AutoSwap'
        UserControlMarketData_comboBoxCustomFrom_ToolTip, // ToolTip 'Click to select between tracking the cursor or a particular market data set'
        UserControlMarketData_comboBoxCustomTo_ToolTip, // ToolTip 'Click to select a comparision market data set'
        UserControlMarketData_checkBoxBuyOnly_ToolTip, // ToolTip 'Show items you can buy only'
        UserControlMarketData_checkBoxHasDemand_ToolTip, // ToolTip 'Show items that has demand'

        UserControlMarketData_Conly, // Cargo only, no market data on this item
        UserControlMarketData_LEntry, // Travel History Entry Last

        UserControlOutfitting_labelYardSel, // Control 'Sel'
        UserControlOutfitting_labelYard, // Control 'Unknown'
        UserControlOutfitting_comboBoxYards_ToolTip, // ToolTip 'Select ship to view'
        UserControlOutfitting_Date, // Date
        UserControlOutfitting_Yard, // Yard
        UserControlOutfitting_Item, // Item
        UserControlOutfitting_Distance, // Distance
        UserControlOutfitting_Price, // Price
        UserControlOutfitting_Type, // Type
        UserControlOutfitting_Info, // Info
        UserControlOutfitting_Mass, // Mass
        UserControlOutfitting_TravelHistoryEntry, // Travel History Entry

        UserControlLedger_TimeCol, // Column Header 'Time'
        UserControlLedger_Type, // Column Header 'Type'
        UserControlLedger_Notes, // Column Header 'Notes'
        UserControlLedger_Credits, // Column Header 'Credits'
        UserControlLedger_Debits, // Column Header 'Debits'
        UserControlLedger_Balance, // Column Header 'Balance'
        UserControlLedger_NormProfit, // Column Header 'Profit Per Unit'
        UserControlLedger_TotalProfit, // Column Header 'Total Profit'
        UserControlLedger_labelTime, // Control 'Time'
        UserControlLedger_labelSearch, // Control 'Search'
        UserControlLedger_toolStripMenuItemGotoItem, // ToolStrip control 'Go to entry on grid'
        UserControlLedger_comboBoxTime_ToolTip, // ToolTip 'Select the entries by age'
        UserControlLedger_textBoxFilter_ToolTip, // ToolTip 'Search to particular items'
        UserControlLedger_buttonFilter_ToolTip, // ToolTip 'Display entries matching this event type filter'
        UserControlLedger_buttonExtExcel_ToolTip, // ToolTip 'Send data on grid to excel'
        UserControlLedger_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'
        UserControlLedger_CashTransactions, // Cash Transactions
        UserControlLedger_NOLG, // No Ledger available

        UserControlPanelSelector_NOADDONS, // NO ADD ONS!
        UserControlPanelSelector_AddOns, //  Add Ons
        UserControlPanelSelector_TTA, // Click to add or remove Add Ons
        UserControlPanelSelector_TTB, // Add ons are essential additions to your EDD experience!
        UserControlPanelSelector_PP1, // Pop out in a new window
        UserControlPanelSelector_MT1, // Open as a new menu tab

        UserControlShipYards_labelYardSel, // Control 'Sel'
        UserControlShipYards_labelYard, // Control 'Unknown'
        UserControlShipYards_comboBoxYards_ToolTip, // ToolTip 'Select ship to view'
        UserControlShipYards_TravelHistoryEntry, // Travel History Entry
        UserControlShipYards_Date, // Date
        UserControlShipYards_Yard, // Yard
        UserControlShipYards_Distance, // Distance
        UserControlShipYards_Price, // Price
        UserControlShipYards_Ship, // Ship
        UserControlShipYards_Manufacturer, // Manufacturer
        UserControlShipYards_MS, // Mass/Speed

        UserControlScanGrid_colName, // Column Header 'Name'
        UserControlScanGrid_colClass, // Column Header 'Class'
        UserControlScanGrid_colDistance, // Column Header 'Distance'
        UserControlScanGrid_colBriefing, // Column Header 'Information'
        UserControlScanGrid_extButtonShowControl_ToolTip, // ToolTip 'Configure overall settings'
        UserControlScanGrid_extButtonHabZones_ToolTip, // ToolTip 'Configure hab zone information'
        UserControlScanGrid_checkBoxEDSM_ToolTip, // ToolTip 'EDSM lookup toggle'
        UserControlScanGrid_MainStar, // Main Star
        UserControlScanGrid_Mass, // Mass
        UserControlScanGrid_Radius, // Radius
        UserControlScanGrid_Temperature, // Temperature
        UserControlScanGrid_Terraformable, // Terraformable
        UserControlScanGrid_Moon, // Moon
        UserControlScanGrid_Landable, // Landable
        UserControlScanGrid_Geologicalactivity, // Geological activity
        UserControlScanGrid_Surfacemapped, // Surface mapped
        UserControlScanGrid_BC, // This body contains:
        UserControlScanGrid_Ring, // Has 1 ring:
        UserControlScanGrid_Belt, // Belt:
        UserControlScanGrid_Value, // Value
        UserControlScanGrid_GS, // This is a green system, as it has all existing jumponium materials available!
        UserControlScanGrid_JS, //  jumponium materials found in system.
        UserControlScanGrid_ScanSummaryfor, // Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons

        UserControlScanGrid_structuresToolStripMenuItem_beltsToolStripMenuItem,
        UserControlScanGrid_structuresToolStripMenuItem_ringsToolStripMenuItem,
        UserControlScanGrid_materialsToolStripMenuItem,
        UserControlScanGrid_valuesToolStripMenuItem,

        UserControlCommonBase_Copyingtexttoclipboardfailed, // Copying text to clipboard failed

        StatsTimeUserControl_Summary, // Summary
        StatsTimeUserControl_Day, // Day
        StatsTimeUserControl_Week, // Week
        StatsTimeUserControl_Month, // Month
        StatsTimeUserControl_Custom, // Custom

        FilterSelector_Travel, // Travel
        FilterSelector_Scan, // Scan
        FilterSelector_Missions, // Missions
        FilterSelector_Materials, // Materials
        FilterSelector_Commodities, // Commodities
        FilterSelector_Ledger, // Ledger
        FilterSelector_Ship, // Ship
        FilterSelector_Mining, // Mining
        FilterSelector_MicroResources,
        FilterSelector_Suits,
        FilterSelector_Carrier,
        FilterSelector_NewGroup,
        FilterSelector_Confirmremoval,
        FilterSelector_Newgroupname,

        ScanDisplayForm_Sys, // System
        ScanDisplayForm_Station, // Station

        TravelHistoryFilter_Hours, // {0} hours
        TravelHistoryFilter_Days, // {0} days
        TravelHistoryFilter_1Week, // One Week
        TravelHistoryFilter_Weeks, // {0} weeks
        TravelHistoryFilter_Month, // Month
        TravelHistoryFilter_Quarter, // Quarter
        TravelHistoryFilter_HYear, // Half year
        TravelHistoryFilter_Year, // Year
        TravelHistoryFilter_LastN, // Last {0} entries
        TravelHistoryFilter_LDock, // Last dock
        TravelHistoryFilter_StartEnd, // Start/End Flag

        UserControlMaterialTrader_UpgradeCol, // Column Header 'From'
        UserControlMaterialTrader_LevelCol, // Column Header 'Use'
        UserControlMaterialTrader_WantedCol, // Column Header 'Left'
        UserControlMaterialTrader_ModuleCol, // Column Header 'To'
        UserControlMaterialTrader_MaxCol, // Column Header 'Get'
        UserControlMaterialTrader_AvailableCol, // Column Header 'Total'
        UserControlMaterialTrader_buttonClear, // Control 'Clear'
        UserControlMaterialTrader_clearTradeToolStripMenuItem, // ToolStrip control 'Clear Trade'
        UserControlMaterialTrader_checkBoxCursorToTop_ToolTip, // ToolTip 'When red, use the materials at the cursor to estimate, when green always use the latest materials.'
        UserControlMaterialTrader_buttonClear_ToolTip, // ToolTip 'Set all wanted values to zero'
        UserControlMaterialTrader_Raw, // Raw
        UserControlMaterialTrader_Encoded, // Encoded
        UserControlMaterialTrader_Manufactured, // Manufactured
        UserControlMaterialTrader_Offer, // Offer
        UserControlMaterialTrader_Receive, // Receive


        UserControlFactions_CapShipVictims, // Capital ship Victims: 
        UserControlFactions_MissionsInProgress, // 
        UserControlFactions_CapShipAward, //  Capital ship Award: 
        UserControlFactions_FactionsPlural, // Factions
        UserControlFactions_MissionsFor, // Missions for

        UserControlFactions_Item, // Item
        UserControlFactions_MaterialCommodsFor, // Materials/Commodities for

        UserControlFactions_BountyBond, // Bounty/Bond
        UserControlFactions_Target, // Target
        UserControlFactions_TargetFaction, // Target Faction
        UserControlFactions_Reward, // Reward
        UserControlFactions_BountiesBondsFor, // Bounties/Bonds for


        UserControlFactions_colFaction, // Column Header 'Faction'
        UserControlFactions_colMissions, // Column Header 'Missions'
        UserControlFactions_colReputation, // Column Header 'Rep +'
        UserControlFactions_colMissionCredits, // Column Header 'Mission Credits'
        UserControlFactions_CProfit, // Column Header 'Commds Profit'
        UserControlFactions_CrimeCommitted, // Column Header 'Crimes Committed'
        UserControlFactions_BountyKills, // Column Header 'Bounty Kills'
        UserControlFactions_BountyValue, // Column Header 'Bounty Rewards'
        UserControlFactions_BountyRewardsValue, // Column Header 'Bounty Rewards Value'
        UserControlFactions_Interdicted, // Column Header 'Interdicted'
        UserControlFactions_Interdiction, // Column Header 'Interdiction'
        UserControlFactions_KillBondVictim, // Column Header 'Kill Bonds Victim'
        UserControlFactions_KillBondsAward, // Column Header 'Kill Bonds Awarded'
        UserControlFactions_KillBondsValue, // Column Header 'Kill Bonds Value'
        UserControlFactions_CartoValue, // Column Header 'Cartographic Value'
        UserControlFactions_colInfo, // Column Header 'Other Info'
        UserControlFactions_labelTo, // Control 'to'
        UserControlFactions_showMissionsForFactionToolStripMenuItem, // ToolStrip control 'Show missions for faction'
        UserControlFactions_showCommoditymaterialTradesForFactionToolStripMenuItem, // ToolStrip control 'Show commodity/material trades for faction'
        UserControlFactions_showBountiesAndBondsForFactionToolStripMenuItem, // ToolStrip control 'Show bounties and bonds for faction'
        UserControlFactions_showFactionSystemDetailToolStripMenuItem, // ToolStrip control 'Show system detail for faction'
        UserControlFactions_startDateTime_ToolTip, // ToolTip 'Include from'
        UserControlFactions_endDateTime_ToolTip, // ToolTip 'Include to'
        UserControlFactions_SystemAddress, // System Address
        UserControlFactions_colInfluence,   // +Influence
        UserControlFactions_CBought,        // Commds +
        UserControlFactions_CSold,  // Commds -
        UserControlFactions_MBought, // Mats +
        UserControlFactions_MSold,  // Mats-
        UserControlFactions_RewardsPlural, // Rewards
        UserControlFactions_BountiesPlural, // Bounties
        UserControlFactions_BondsPlural, // Bonds
        UserControlFactions_SystemsDetailFor, // Systems Detail for


        FindSystemsUserControl_extCheckBoxExcludeVisitedSystems, // Control 'Exclude Visited Systems'
        FindSystemsUserControl_checkBoxCustomCube, // Control 'Cube'
        FindSystemsUserControl_buttonExtNames, // Control 'From DB Find Names'
        FindSystemsUserControl_buttonExtVisited, // Control 'From Visited Systems'
        FindSystemsUserControl_buttonExtDB, // Control 'From DB'
        FindSystemsUserControl_buttonExtEDSM, // Control 'From EDSM'
        FindSystemsUserControl_labelRadMax, // Control 'Max'
        FindSystemsUserControl_labelRadMin, // Control 'Radius ly Min'
        FindSystemsUserControl_labelFilter, // Control 'Star Name'
        FindSystemsUserControl_Cannotfindsystem, // Cannot find system
        FindSystemsUserControl_EDSM, // new warning..

        CommanderForm_CAPILoggingin,    // discrete 
        CommanderForm_CAPIDisabled, // discrete
        CommanderForm_CAPILogout, // discrete
        CommanderForm_CAPILoggedin, // discrete
        CommanderForm_CAPILogin, // discrete
        CommanderForm_CAPIAwaitLogin, // discrete

        UserControlSuitsWeapons_forceSellShipToolStripMenuItem, // Toolstrip control 'force'

        UserControlSuitsWeapons_CSTime, // Column Header 'Time'
        UserControlSuitsWeapons_CSName, // Column Header 'Name'
        UserControlSuitsWeapons_CSMods, // Column Header 'Mods'
        UserControlSuitsWeapons_CSPrice, // Column Header 'Price'
        UserControlSuitsWeapons_CSLoadout, // Column Header 'Loadout'
        UserControlSuitsWeapons_CSPrimary1, // Column Header 'Primary 1'
        UserControlSuitsWeapons_CSPrimary2, // Column Header 'Primary 2'
        UserControlSuitsWeapons_CSSecondary, // Column Header 'Secondary'
        UserControlSuitsWeapons_CWTime, // Column Header 'Time'
        UserControlSuitsWeapons_CWName, // Column Header 'Name'
        UserControlSuitsWeapons_CWClass, // Column Header 'Class'
        UserControlSuitsWeapons_CWMods, // Column Header 'Mods'
        UserControlSuitsWeapons_CWPrice, // Column Header 'Price'
        UserControlSuitsWeapons_CWPrimary, // Column Header 'Primary'
        UserControlSuitsWeapons_CWWType, // Column Header 'Weapon Type'
        UserControlSuitsWeapons_CWDamageType, // Column Header 'Damage Type'
        UserControlSuitsWeapons_CWFireMode, // Column Header 'Fire Mode'
        UserControlSuitsWeapons_CWDamage, // Column Header 'Damage'
        UserControlSuitsWeapons_CWRPS, // Column Header 'Rate/Sec'
        UserControlSuitsWeapons_CWDPS, // Column Header 'DPS'
        UserControlSuitsWeapons_CWClipSize, // Column Header 'Clip Size'
        UserControlSuitsWeapons_CWHopper, // Column Header 'Hopper Size'
        UserControlSuitsWeapons_CWRange, // Column Header 'Range m'
        UserControlSuitsWeapons_CWHSD, // Column Header 'Head Shot Mult'
        UserControlSuitsWeapons_Confirm, // discrete

        UserControlSynthesis_UpgradeCol, // Column Header 'Upgrade/Mat'
        UserControlSynthesis_Level, // Column Header 'Level'
        UserControlSynthesis_MaxCol, // Column Header 'Max'
        UserControlSynthesis_WantedCol, // Column Header 'Wanted'
        UserControlSynthesis_Available, // Column Header 'Avail.'
        UserControlSynthesis_Notes, // Column Header 'Notes'
        UserControlSynthesis_Recipe, // Column Header 'Recipe'
        UserControlSynthesis_buttonRecipeFilter_ToolTip, // ToolTip 'Filter the table by the synthesis type'
        UserControlSynthesis_buttonFilterLevel_ToolTip, // ToolTip 'Filter the table by the synthesis level'
        UserControlSynthesis_buttonMaterialFilter_ToolTip, // ToolTip 'Filter the table by the material type'
        UserControlSynthesis_buttonClear_ToolTip, // ToolTip 'Set all wanted values to zero'
        UserControlSynthesis_chkNotHistoric_ToolTip, // ToolTip 'When red, use the materials at the cursor to estimate, when green always use the latest materials.'
        UserControlSynthesis_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'

        UserControlEngineering_UpgradeCol, // Column Header 'Upgrade/Mat'
        UserControlEngineering_ModuleCol, // Column Header 'Module'
        UserControlEngineering_LevelCol, // Column Header 'Level'
        UserControlEngineering_MaxCol, // Column Header 'Max'
        UserControlEngineering_WantedCol, // Column Header 'Wanted'
        UserControlEngineering_AvailableCol, // Column Header 'Avail.'
        UserControlEngineering_NotesCol, // Column Header 'Notes'
        UserControlEngineering_RecipeCol, // Column Header 'Recipe'
        UserControlEngineering_EngineersCol, // Column Header 'Engineers'
        UserControlEngineering_CraftedCol, // Column Header 'Crafted'

        UserControlEngineering_buttonFilterUpgrade_ToolTip, // ToolTip 'Filter the table by upgrade type'
        UserControlEngineering_buttonFilterModule_ToolTip, // ToolTip 'Filter the table by module type'
        UserControlEngineering_buttonFilterLevel_ToolTip, // ToolTip 'Filter the table by level'
        UserControlEngineering_buttonFilterEngineer_ToolTip, // ToolTip 'Filter the table by engineer'
        UserControlEngineering_buttonFilterMaterial_ToolTip, // ToolTip 'Filter the table by material'
        UserControlEngineering_buttonClear_ToolTip, // ToolTip 'Set all wanted values to zero'
        UserControlEngineering_chkNotHistoric_ToolTip, // ToolTip 'When red, use the materials at the cursor to estimate, when green always use the latest materials.'
        UserControlEngineering_extCheckBoxWordWrap_ToolTip, // ToolTip 'Enable or disable word wrap'

        UserControlEngineers_buttonFilterEngineer_ToolTip, 
        UserControlEngineers_extCheckBoxWordWrap_ToolTip, 
        UserControlEngineers_extCheckBoxMoreInfo_ToolTip,

        UserControlShoppingList_showMaxFSDInjectionsToolStripMenuItem, // ToolStrip control 'Show Max FSD Injections'
        UserControlShoppingList_showBodyMaterialsWhenLandedToolStripMenuItem, // ToolStrip control 'Show Body Materials When Landed'
        UserControlShoppingList_showBodyMaterialsWhenLandedToolStripMenuItem_onlyCapacityToolStripMenuItem, // ToolStrip control 'Hide when storage full'
        UserControlShoppingList_showAvailableMaterialsInListWhenLandedToolStripMenuItem, // ToolStrip control 'Include Material %age on Landed Body in Shopping List'
        UserControlShoppingList_showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem, // ToolStrip control 'Show System Availability in Shopping List in flight'
        UserControlShoppingList_useEDSMDataInSystemAvailabilityToolStripMenuItem, // ToolStrip control 'Use EDSM data in System Availability'
        UserControlShoppingList_useHistoricMaterialCountsToolStripMenuItem, // ToolStrip control 'Use Historic Material Counts'
        UserControlShoppingList_toggleListPositionToolStripMenuItem, // ToolStrip control 'Toggle Shopping List Position'

        UserControlSpanel_showSystemInformationToolStripMenuItem,
        UserControlSpanel_toolStripMenuItemTargetLine,
        UserControlSpanel_blackBoxAroundTextToolStripMenuItem,
        UserControlSpanel_expandTextOverEmptyColumnsToolStripMenuItem,
        UserControlSpanel_dontShowInformationWhenToolStripMenuItem,
        UserControlSpanel_dontShowInformationWhenToolStripMenuItem_showNothingWhenDockedtoolStripMenuItem,
        UserControlSpanel_dontShowInformationWhenToolStripMenuItem_dontshowwhenInGalaxyPanelToolStripMenuItem,
        UserControlSpanel_dontShowInformationWhenToolStripMenuItem_hideTitleToolStripMenuItem,
        UserControlSpanel_EDSMButtonToolStripMenuItem,
        UserControlSpanel_toolStripMenuItemTime,
        UserControlSpanel_iconToolStripMenuItem,
        UserControlSpanel_showDescriptionToolStripMenuItem,
        UserControlSpanel_showInformationToolStripMenuItem,
        UserControlSpanel_showNotesToolStripMenuItem,
        UserControlSpanel_showXYZToolStripMenuItem,
        UserControlSpanel_showTargetToolStripMenuItem,
        UserControlSpanel_showDistancesOnFSDJumpsOnlyToolStripMenuItem,
        UserControlSpanel_OrdertoolStripMenuItem_orderDefaultToolStripMenuItem,
        UserControlSpanel_OrdertoolStripMenuItem_orderNotesAfterXYZToolStripMenuItem,
        UserControlSpanel_OrdertoolStripMenuItem_orderTargetDistanceXYZNotesToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showMetalRichPlanetsToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showWaterWorldsToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showEarthLikeToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showAmmoniaWorldsToolStripMenuItem,
        UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showIcyPlanetsToolStripMenuItem,
        UserControlSpanel_surfaceScanDetailsToolStripMenuItem_scanNoToolStripMenuItem,
        UserControlSpanel_surfaceScanDetailsToolStripMenuItem_scan15sToolStripMenuItem,
        UserControlSpanel_surfaceScanDetailsToolStripMenuItem_scan30sToolStripMenuItem,
        UserControlSpanel_surfaceScanDetailsToolStripMenuItem_scan60sToolStripMenuItem,
        UserControlSpanel_surfaceScanDetailsToolStripMenuItem_scanUntilNextToolStripMenuItem,

        UserControlSpanel_showInPositionToolStripMenuItem_scanRightMenuItem,
        UserControlSpanel_showInPositionToolStripMenuItem_scanLeftMenuItem,
        UserControlSpanel_showInPositionToolStripMenuItem_scanAboveMenuItem,
        UserControlSpanel_showInPositionToolStripMenuItem_scanBelowMenuItem,
        UserControlSpanel_showInPositionToolStripMenuItem_scanOnTopMenuItem,

        UserControlCarrier_Decommisioned,
        UserControlCarrier_DecommisionedOn,
        UserControlCarrier_Cargo,
        UserControlCarrier_Services,
        UserControlCarrier_Shippacks,
        UserControlCarrier_Modulepacks,
        UserControlCarrier_Freespace,
        UserControlCarrier_Jumprange,
        UserControlCarrier_Maxjump,
        UserControlCarrier_Fuel,
        UserControlCarrier_DockingAccess,
        UserControlCarrier_Notorious,
        UserControlCarrier_Balance,
        UserControlCarrier_Reserve,
        UserControlCarrier_Available,
        UserControlCarrier_PioneerTax,
        UserControlCarrier_ShipyardTax,
        UserControlCarrier_RearmTax,
        UserControlCarrier_OutfittingTax,
        UserControlCarrier_RefuelTax,
        UserControlCarrier_RepairTax,
        UserControlCarrier_CoreCost,
        UserControlCarrier_ServicesCost,
        UserControlCarrier_NoCarrier,
        UserControlCarrier_InstallCost,
        UserControlCarrier_CapacityAllocated,
        UserControlCarrier_Upkeepcost,
        UserControlCarrier_Suspendedupkeepcost,
        UserControlCarrier_CrewName,
        UserControlCarrier_ServiceActive,
        UserControlCarrier_ServiceSuspended,
        UserControlCarrier_ServiceNotInstalled,
        UserControlCarrier_NoPacks,
        UserControlCarrier_Tier,
        UserControlCarrier_Cost,
        UserControlCarrier_Jumping,
        UserControlCarrier_NetworkFailure,
        UserControlCarrier_NotLoggedIn,
        UserControlCarrier_OptionalService,
        UserControlCarrier_CoreService,

        UserControlCarrier_extTabControl_tabPageOverall,
        UserControlCarrier_extTabControl_tabPageOverall_imageControlOverall,
        UserControlCarrier_extTabControl_tabPageItinerary,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinDate,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinSystemAlphaInt,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinBodyAlphaInt,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinJumpDistNumeric,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinDistFromNumeric,
        UserControlCarrier_extTabControl_tabPageItinerary_colItinInformation,
        UserControlCarrier_extTabControl_tabPageFinances,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerDate,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerStarsystemAlphaInt,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerBodyAlphaInt,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerEvent,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerCreditNumeric,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerDebitNumeric,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerBalanceNumeric,
        UserControlCarrier_extTabControl_tabPageFinances_colLedgerNotes,
        UserControlCarrier_extTabControl_tabPageServices,
        UserControlCarrier_extTabControl_tabPageServices_imageControlServices,
        UserControlCarrier_extTabControl_tabPagePacks,
        UserControlCarrier_extTabControl_tabPagePacks_imageControlPacks,
        UserControlCarrier_extTabControl_tabPageOrders,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersDate,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersCommodity,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersType,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersGroup,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersPurchaseNumeric,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersSaleNumeric,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersPriceNumeric,
        UserControlCarrier_extTabControl_tabPageOrders_colOrdersBlackmarket,
        UserControlCarrier_extTabControl_tabPageCAPI3,
        UserControlCarrier_extTabControl_tabPageCAPI3_dataGridViewTextBoxColumn1,
        UserControlCarrier_extTabControl_tabPageCAPI3_dataGridViewTextBoxColumnValue,
        UserControlCarrier_extTabControl_tabPageCAPI1,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsName,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsManu,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsPriceNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsSpeedNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsBoostNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsMassNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsLandingPadNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesName,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesCat,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesMassNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesPowerNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesCostNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesStockNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesInfo,
        UserControlCarrier_extTabControl_tabPageCAPI2,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoCommodity,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoType,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoGroup,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoQuantityNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoPriceNumeric,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoStolen,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerName,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerType,
        UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerQuantityNumeric,

        ActionPackVariablesForm_gv
    }

    internal static class EDTranslatorExtensions
    {
        static public string T(this string s, EDTx value)              // use the enum.  This was invented before the shift to all Enums of feb 22
        {
            return s.TxID(value);
        }
    }
}
