using System;
using System.Collections.Generic;
using System.Globalization;

namespace EDDiscovery
{
    public enum EDTx
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
        Systemnotknown, // System not known
        Travel, // Travel
        GameTime, // Game Time
        Time, // Time
        NoData, // No Data
        None, // None
        on, // on
        Off, // Off
        Unknown, // Unknown
        Information, // Information
        NoPos, // No Pos

        EDDiscoveryForm_TSF,
        EDDiscoveryForm_ConfirmSyncToEDSM, // You have disabled sync to EDSM for this commander.  Are you sure you want to send unsynced events to EDSM?
        EDDiscoveryForm_ConfirmSyncToEDSMCaption, // Confirm EDSM sync
        EDDiscoveryForm_RTABL, // Name:
        EDDiscoveryForm_RTABT, // Rename Tab
        EDDiscoveryForm_RTABTT, // Enter a new name for the tab
        EDDiscoveryForm_ThemeW, // The theme stored has missing colors or other missing information
        EDDiscoveryForm_DLLW, // The following application extension DLLs have been found
        EDDiscoveryForm_DLLL, // DLLs loaded: {0}
        EDDiscoveryForm_DLLF, // DLLs failed to load: {0}
        EDDiscoveryForm_PROFL, // Profile {0} Loaded
        EDDiscoveryForm_NI, // New EDDiscovery installer available: {0}
        EDDiscoveryForm_NRA, // New Release Available!
        EDDiscoveryForm_CAPINA, // Companion API is disabled in commander settings
        EDDiscoveryForm_CAPION, // Logged into Companion API
        EDDiscoveryForm_CAPIF, // Companion API failed: {0}
        EDDiscoveryForm_CAPIND, // CAPI not docked. Server API lagging!
        EDDiscoveryForm_CAPISS, // CAPI profileSystemRequired is {0}, profile station is {1}
        EDDiscoveryForm_CAPISN, // CAPI profileStationRequired is {0}, profile station is {1}
        EDDiscoveryForm_CAPIMN, // CAPI stationname {0}, market station is {1}
        EDDiscoveryForm_CAPIEX, // Companion API get failed:
        EDDiscoveryForm_Arrived, // Arrived at system {0} Visit No. {1}
        EDDiscoveryForm_PE1, // Profile reports errors in triggers:
        EDDiscoveryForm_EDDN, // Sent {0} event to EDDN ({1})
        EDDiscoveryForm_EDDNEX, // EDDN: Send commodities prices failed:
        EDDiscoveryForm_Closing, // Closing, please wait!
        EDDiscoveryForm_SDDis, // Star Data download is disabled. Use Settings to reenable it
        EDDiscoveryForm_SDSyncErr, // Synchronisation to databases is in operation or pending, please wait
        EDDiscoveryForm_EDSMQ, // This can take a considerable amount of time and bandwidth
        EDDiscoveryForm_ResetCMDR, // Confirm you wish to reset all history entries to the current commander
        EDDiscoveryForm_NoRel, // No new release found
        EDDiscoveryForm_RevFSD, // Confirm you remove any duplicate FSD entries from the current commander
        EDDiscoveryForm_FSDRem, // Removed {0} FSD entries
        EDDiscoveryForm_VSLNF, // Could not find VisitedStarsCache.dat file, choose file
        EDDiscoveryForm_VSLEXP, // File {0} created.
        EDDiscoveryForm_VSLRestart, // Restart Elite Dangerous to have this file read into the galaxy map
        EDDiscoveryForm_VSLFileErr, // Error writing {0} export visited stars
        EDDiscoveryForm_InaraW, // Inara historic upload is disabled until 1 hour has elapsed from the last try to prevent server flooding
        EDDiscoveryForm_RH, // Refresh History.
        EDDiscoveryForm_NoEDSMAPI, // Please ensure a commander is selected and it has a EDSM API key set
        EDDiscoveryForm_EDSMSyncE, // EDSM Sync failed: {0}
        EDDiscoveryForm_EP, // Edit Profiles
        EDDiscoveryForm_PL, // Profile {0} Loaded
        EDDiscoveryForm_WSE, // Web server enabled
        EDDiscoveryForm_WSF, // Web server failed to start
        EDDiscoveryForm_WSERR, // Web server disabled due to incorrect folder or missing zip file
        EDDiscoveryForm_WSD, // *** Web server is disabled ***
        EDDiscoveryForm_CloseWarning, // EDDiscovery is updating the EDSM and EDDB databases
        EDDiscoveryForm_SafeMode, // To start in safe mode, exit the program, hold down the shift key
        EDDiscoveryForm_ResetEDSMID, // Confirm you wish to reset the assigned EDSM ID

        EDDiscoveryController_CD, // Closing down, please wait..
        EDDiscoveryController_EDSM, // Get galactic mapping from EDSM.
        EDDiscoveryController_LN, // Loaded Notes, Bookmarks and Galactic mapping.
        EDDiscoveryController_EDMC, // EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!
        EDDiscoveryController_RTH, // Reading travel history
        EDDiscoveryController_SyncEDSM, // Please continue running ED Discovery until refresh is complete.
        EDDiscoveryController_SyncOff, // Synchronisation to EDSM and EDDB disabled. Use Settings panel to reenable
        EDDiscoveryController_EDSMU, // EDSM update complete with {0} systems
        EDDiscoveryController_EDDBU, // EDDB update complete with {0} systems
        EDDiscoveryController_SYST, // Loading completed, total of {0:N0} systems stored
        EDDiscoveryController_Refresh, // Refresh due to updating EDSM or EDDB data
        EDDiscoveryController_PLF, // Processing log file {0}
        EDDiscoveryController_RD, // Refresh Displays
        EDDiscoveryController_HRC, // History refresh complete.
        EDDiscoveryController_Maps, // Checking for new EDDiscovery maps
        EDDiscoveryController_EXPD, // Checking for new Expedition data
        EDDiscoveryController_EXPL, // Checking for new Exploration data

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

        CommanderForm_LF, // Select folder where Journal*.log files are stored by Frontier in

        ExportForm_ECH, // Export current History view to {0}
        ExportForm_JF, // Journal File

        Form2DMap_Nomapsavailable, // No maps available

        Form2DMap_LastWeek, // Last Week
        Form2DMap_LastMonth, // Last Month
        Form2DMap_LastYear, // Last Year
        Form2DMap_All, // All
        Form2DMap_Custom, // Custom


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
        GalaxySectorSelect_PresstoCancel, // Press to Cancel
        GalaxySectorSelect_GALSELTitle, // Select EDSM Galaxy Data
        GalaxySectorSelect_RS, // You have removed sectors!

        ProfileEditor_Custom, // Custom
        ProfileEditor_Trigger, // Trigger
        ProfileEditor_Back, // Back
        ProfileEditor_Default, // Default
        ProfileEditor_TrigEdit, // Edit Profile {0} Trigger
        ProfileEditor_BackEdit, // Edit Profile {0} Back Trigger
        ProfileEditor_DeleteWarning, // Do you wish to delete profile {0}?



    }

    public static class EDTranslatorExtensions
    {
        static public string T(this string s, EDTx value)              // use the enum.
        {
            return BaseUtils.Translator.Instance.Translate(s, value.ToString().Replace("_", "."));
        }
    }
}
