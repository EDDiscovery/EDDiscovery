﻿using System;
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
        EDDiscoveryForm_ThemeW, // The theme stored has missing colors or other missing information
        EDDiscoveryForm_DLLW, // The following application extension DLLs have been found
        EDDiscoveryForm_DLLL, // DLLs loaded: {0}
        EDDiscoveryForm_DLLF, // DLLs failed to load: {0}
        EDDiscoveryForm_DLLDIS, // DLLs disabled: {0}
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
        EDDiscoveryForm_IndexW, // Are you sure to Rebuild Indexes? It may take a long time.
        EDDiscoveryForm_FillPos, // Are you sure to Rebuild Indexes? It may take a long time.
        EDDiscoveryForm_RemoveDLLPerms, // remove dll etc.
        EDDiscoveryForm_RH, // Refresh History.
        EDDiscoveryForm_NoEDSMAPI, // No EDSM API key set
        EDDiscoveryForm_EDSMSyncE, // EDSM Sync failed: {0}
        EDDiscoveryForm_EP, // Edit Profiles
        EDDiscoveryForm_PL, // Profile {0} Loaded
        EDDiscoveryForm_WSE, // Web server enabled
        EDDiscoveryForm_WSF, // Web server failed to start
        EDDiscoveryForm_WSERR, // Web server disabled due to incorrect folder or missing zip file
        EDDiscoveryForm_WSD, // *** Web server is disabled ***
        EDDiscoveryForm_CloseWarning, // EDDiscovery is updating the EDSM and EDDB databases
        EDDiscoveryForm_SafeMode, // To start in safe mode, exit the program, hold down the shift key

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

        CSV_Helpers_OpenFailed, // Failed to open<spc>
        CSV_Helpers_WriteFailed, // Failed to write to<spc>

        ExportForm_ECH, // Export current History view to {0}
        ExportForm_JF, // Journal File
        ExportForm_ImportTitle,
        ExportForm_ImportButton,
        ExportForm_ImportData,


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

        UserControlMaterialCommodities_Data, // Data !! NOTE
        UserControlMaterialCommodities_Mats, // Data !! NOTE
        UserControlMaterialCommodities_Total, // Data !! NOTE
        UserControlMaterialCommodities_Rare, // Rare !! NOTE
        UserControlMaterialCommodities_Recipes, // Recipes !! NOTE
        UserControlMaterialCommodities_ShipLocker,
        UserControlMaterialCommodities_BackPack,

        UserControlMissions_MPlural, //  Missions
        UserControlMissions_MSingular, //  Mission
        UserControlMissions_Name, // Name
        UserControlMissions_Value, // Value (cr):
        UserControlMissions_ValueN, // Value (cr)
        UserControlMissions_ValueC, // Value:

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
        UserControlModules_ItemLocalised, // Type
        UserControlModules_ItemCol, // Item
        UserControlModules_Mass, // Mass
        UserControlModules_BluePrint, // BluePrint
        UserControlModules_PriorityEnable, // P/E
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


        UserControlStats_Lastdock, // Last dock
        UserControlStats_Trip, // Trip
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


        UserControlShoppingList_NM, // Needed Mats
        UserControlShoppingList_FS, // Filling Shopping List would exceed capacity for:
        UserControlShoppingList_NoMat, // No materials currently required.
        UserControlShoppingList_FSD, // Max FSD Injections
        UserControlShoppingList_MO, // Materials on {0}

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

        UserControlJournalGrid_TT1, //  showing {0} original {1}
        UserControlJournalGrid_TTFilt1, // Filtered {0}
        UserControlJournalGrid_TTSelAge, // Select the entries by age,
        UserControlJournalGrid_TTFilt2, // Filtered {0}
        UserControlJournalGrid_TTEvent, // Filter out entries based on event type,
        UserControlJournalGrid_TTFilt3, // Total filtered out {0}
        UserControlJournalGrid_TTTotal, // Filter out entries matching the field selection,
        UserControlJournalGrid_JHF, // Journal: Filter out fields
        UserControlJournalGrid_NotSynced, // System could not be found - has not been synched or EDSM is unavailable

        UserControlTravelGrid_THF, // History: Filter out fields
        UserControlTravelGrid_TT1, //  showing {0} original {1}
        UserControlTravelGrid_TTFilt1, // Filtered {0}
        UserControlTravelGrid_TTSelAge, // Select the entries by age,
        UserControlTravelGrid_TTFilt2, // Filtered {0}
        UserControlTravelGrid_TTEvent, // Filter out entries based on event type,
        UserControlTravelGrid_TTFilt3, // Total filtered out {0}
        UserControlTravelGrid_TTTotal, // Filter out entries matching the field selection,
        UserControlTravelGrid_CSTART, // Clear Start marker
        UserControlTravelGrid_CSTOP, // Clear Stop marker
        UserControlTravelGrid_SETSTOPTC, // Set Stop marker for travel calculations
        UserControlTravelGrid_SETSTARTTC, // Set Start marker for travel calculations
        UserControlTravelGrid_SETSTSTOP, // Set Start/Stop point for travel calculations
        UserControlTravelGrid_NotSynced, // System could not be found - has not been synched or EDSM is unavailable
        UserControlTravelGrid_Remove, // Confirm you wish to remove this entry

        UserControlStarList_Total, // Total
        UserControlStarList_CS, // {0} Star(s)
        UserControlStarList_FSD, // This system has materials for FSD boost:
        UserControlStarList_NoEDSM, // System could not be found - has not been synched or EDSM is unavailable
        UserControlStarList_OB, // {0} Other bodies
        UserControlStarList_JUMP,
        UserControlStarList_valueables,
        UserControlStarList_scanorganics,

        UserControlCaptainsLog_Diary, // Diary
        UserControlCaptainsLog_Entries, // Entries

        CaptainsLogDiary_Daysofweek, // Sun;Mon;Tue;Wed;Thu;Fri;Sat
        CaptainsLogEntries_DTF, // Bad Date Time format
        CaptainsLogEntries_Note, // Note:
        CaptainsLogEntries_EnterNote, // Enter Note
        CaptainsLogEntries_CFN, // Do you really want to delete {0} notes?
        CaptainsLogEntries_CF, // Do you really want to delete the note for {0}
        CaptainsLogEntries_SysU, // System could not be found - has not been synched or EDSM is unavailable
        CaptainsLogEntries_NSS, // No such system
        CaptainsLogEntries_SetTags, // Set Tags

        UserControlBookmarks_CFN, // Do you really want to delete {0} bookmarks?
        UserControlBookmarks_CF, // Do you really want to delete the bookmark for {0}
        UserControlBookmarks_SysU, // System could not be found - has not been synched or EDSM is unavailable

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

        UserControlMissionOverlay_IL, //  Left

        UserControlRouteTracker_Selectroute, // Select route
        UserControlRouteTracker_PresstoCancel, // Press to Cancel
        UserControlRouteTracker_Enterroute, // Enter route
        UserControlRouteTracker_J1, // jump
        UserControlRouteTracker_TF, // To First WP
        UserControlRouteTracker_JS, // jumps
        UserControlRouteTracker_FL, // From Last WP
        UserControlRouteTracker_LF, // , Left {0:N1}ly
        UserControlRouteTracker_ToWP, // (WP {0})
        UserControlRouteTracker_NoFSD, //  No Ship FSD Information
        UserControlRouteTracker_Dev, // , Dev {0:N1}ly
        UserControlRouteTracker_NoRoute, // Please set a route, by right clicking
        UserControlRouteTracker_NoWay, // Route contains no waypoints
        UserControlRouteTracker_Unk, // Unknown location
        UserControlRouteTracker_NoCo, // No systems in route have known co-ords
        UserControlRouteTracker_Note, // Note

        UserControlMiningOverlay_Limcargo,
        UserControlMiningOverlay_Proscoll,
        UserControlMiningOverlay_ref,
        UserControlMiningOverlay_pros,
        UserControlMiningOverlay_coll,
        UserControlMiningOverlay_ratio,
        UserControlMiningOverlay_avg,
        UserControlMiningOverlay_min,
        UserControlMiningOverlay_max,
        UserControlMiningOverlay_mload,
        UserControlMiningOverlay_hml,
        UserControlMiningOverlay_discv,
        UserControlMiningOverlay_content,
        UserControlMiningOverlay_above,
        UserControlMiningOverlay_dist,
        UserControlMiningOverlay_astpros,

        UserControlSpanel_Target, // Target
        UserControlSpanel_TVE, // View system on EDSM
        UserControlSpanel_SPF, // Summary Panel: Filter out fields

        UserControlSurveyor_Scan, //  Scan
        UserControlSurveyor_Systemscancomplete, // System scan complete.
        UserControlSurveyor_bodiesfound, //  bodies found.
        UserControlSurveyor_fuel, //  bodies found.
        UserControlSurveyor_navroute, //  bodies found.
        UserControlSurveyor_fsssignals,

        UserControlSysInfo_NoMissions, // No Missions
        UserControlSysInfo_NA, // N/A
        UserControlSysInfo_SysUnk, // System unknown to EDSM
        UserControlSysInfo_Pos, // Position is {0:0.00},{1:0.00},{2:0.00}
        UserControlSysInfo_Target, // On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter
        UserControlSysInfo_OnFoot, 

        UserControlTrippanel_jump, // jump
        UserControlTrippanel_jumps, // jumps
        UserControlTrippanel_NoT, //  -> Target not set
        UserControlTrippanel_FDEDSM, // Shows if EDSM indicates your it's first discoverer
        UserControlTrippanel_UKN, // System {0} unknown to EDSM
        UserControlTrippanel_Left, // left

        UserControlExpedition_Unsaved, // Expedition - There are unsaved changes to the current route.
        UserControlExpedition_Conflict, // The current route name conflicts with a well-known expedition.
        UserControlExpedition_Overwrite, // Warning: route already exists. Would you like to overwrite it?
        UserControlExpedition_EDSMUnk,
        UserControlExpedition_UnkSystem, 
        UserControlExpedition_NoScanInfo,
        UserControlExpedition_Specify, // Please specify a name for the route.
        UserControlExpedition_UnknownS, // Unknown system, system is without co-ordinates
        UserControlExpedition_Createroute, // Please create a route on a route panel
        UserControlExpedition_NoRouteExport, // There is no route to export
        UserControlExpedition_NoRoute, // No route set up. Please add at least two systems.
        UserControlExpedition_Delete, // Are you sure you want to delete this route?
        UserControlExpedition_AddSys, 

        UserControlExploration_LoseAllChanges, // Exploration, Confirm you want to lose all changes
        UserControlExploration_Saved, // Saved to {0} Exploration Set
        UserControlExploration_SelectSet, // Select a exploration set file
        UserControlExploration_OpenE, // There was a problem opening file {0}
        UserControlExploration_NoSys, // The imported file contains no known system names
        UserControlExploration_NoRoute, // There is no route to export
        UserControlExploration_Export, // Export route
        UserControlExploration_ErrorW, // Error exporting route. Is file {0} open?
        UserControlExploration_Clear, // Are you sure you want to clear the route list?
        UserControlExploration_AddSys, // Add Systems
        UserControlExploration_EDSMUnk, // System not known to EDSM
        UserControlExploration_UnknownS, // Unknown system, system is without co-ordinates

        UserControlRoute_Confirm, // This will result in a large number ({0})) of jumps
        UserControlRoute_NoRoute, // No route set up, retry
        UserControlRoute_M1, 
        UserControlRoute_M2, 
        UserControlRoute_M3, 
        UserControlRoute_M4, 
        UserControlRoute_M5, 
        UserControlRoute_M6, 

        UserControlWebBrowser_EnterSys, // Show System
        UserControlWebBrowser_System, // System:

        UserControlEstimatedValues_SV, // Estimated Scan Values for {0}

        UserControlLocalMap_3dmap, // 3D Map of closest systems from {0}


        UserControlPlot_Top, // Top
        UserControlPlot_Front, // Front
        UserControlPlot_Side, // Side
        UserControlPlot_Grid, // Grid
        UserControlPlot_Report, // Report
        UserControlPlot_2d, // 2D Plot of systems in range from {0}
        UserControlPlot_PTOP, // Plot around {0}, viewed from the top
        UserControlPlot_PFRONT, // Plot around {0}, viewed from the front
        UserControlPlot_PSIDE, // Plot around {0}, viewed from the side
        UserControlPlot_SysAROUND, // Systems around {0}, from {1} to {2}, Ly: {3}
        UserControlPlot_Vs, //
        UserControlPlot_Nt, //
        UserControlPlot_D1, //
        UserControlPlot_D2, //
        UserControlPlot_D3, //

        UserControlScan_System, // System:
        UserControlScan_EnterSys, // Show System
        UserControlScan_VLMT, // Set Valuable Minimum

        UserControlScan_StatusIcons, // Show body status icons
        UserControlScan_SystemValue, // Show syatem and value in main display
        UserControlScan_MatFull, // Hide materials which have reached their storage limit
        UserControlScan_ShowMaterials, //  Show Materials
        UserControlScan_ShowRaresOnly, // Show rare materials only
        UserControlScan_ShowMoons, // Show Moons
        UserControlScan_Belt,
        UserControlScan_Star,
        UserControlScan_Barycentre,
        UserControlScan_Body,
        UserControlScan_AllG,
        UserControlScan_HabZone,
        UserControlScan_PlanetClass,
        UserControlScan_StarClass,
        UserControlScan_Distance,
        UserControlScan_Expired,

        UserControlStarDistance_From, // From {0}
        UserControlStarDistance_NoEDSMSys, // System could not be found - has not been synched or EDSM is unavailable

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

        SearchMaterialsCommodities_RareCommodity, // , Rare Commodity
        SearchMaterialsCommodities_AND, // AND
        SearchMaterialsCommodities_OR, // OR
        SearchMaterialsCommodities_DIS, // Discovered at
        SearchMaterialsCommodities_COL, // Collected at
        SearchMaterialsCommodities_MR, // Mission Reward at

        SearchScans_Select, // Select
        SearchScans_Name, // Name:
        SearchScans_SN, // Enter Search Name:
        SearchScans_DEL, // Confirm deletion of
        SearchScans_DELNO, // Cannot delete this entry
        SearchScans_CNV, // Condition is not valid
        SearchScans_CD, // Condition

        UserControlSearch_Stars, // Stars
        UserControlSearch_MaterialsCommodities, // Materials Commodities
        UserControlSearch_Scans, // Scans

        UserControlMarketData_Conly, // Cargo only, no market data on this item
        UserControlMarketData_LEntry, // Travel History Entry Last

        UserControlOutfitting_Date, // Date
        UserControlOutfitting_Yard, // Yard
        UserControlOutfitting_Item, // Item
        UserControlOutfitting_Distance, // Distance
        UserControlOutfitting_Price, // Price
        UserControlOutfitting_Type, // Type
        UserControlOutfitting_Info, // Info
        UserControlOutfitting_Mass, // Mass
        UserControlOutfitting_TravelHistoryEntry, // Travel History Entry

        UserControlLedger_CashTransactions, // Cash Transactions
        UserControlLedger_NOLG, // No Ledger available

        UserControlPanelSelector_NOADDONS, // NO ADD ONS!
        UserControlPanelSelector_AddOns, //  Add Ons
        UserControlPanelSelector_TTA, // Click to add or remove Add Ons
        UserControlPanelSelector_TTB, // Add ons are essential additions to your EDD experience!
        UserControlPanelSelector_PP1, // Pop out in a new window
        UserControlPanelSelector_MT1, // Open as a new menu tab

        UserControlShipYards_TravelHistoryEntry, // Travel History Entry
        UserControlShipYards_Date, // Date
        UserControlShipYards_Yard, // Yard
        UserControlShipYards_Distance, // Distance
        UserControlShipYards_Price, // Price
        UserControlShipYards_Ship, // Ship
        UserControlShipYards_Manufacturer, // Manufacturer
        UserControlShipYards_MS, // Mass/Speed

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

        UserControlCommonBase_Copyingtexttoclipboardfailed, // Copying text to clipboard failed

        TagsForm_TN, // Tag name
        TagsForm_SI, // Select image for this tag
        TagsForm_DE, // Delete entry

        SurfaceBookmarkUserControl_Enter, // Enter a name
        SurfaceBookmarkUserControl_PL, // Planet:
        SurfaceBookmarkUserControl_EPN, // Enter planet name
        SurfaceBookmarkUserControl_PresstoAccept, // Press to Accept
        SurfaceBookmarkUserControl_PresstoCancel, // Press to Cancel
        SurfaceBookmarkUserControl_MAP, // Manually Add Planet

        StatsTimeUserControl_Summary, // Summary
        StatsTimeUserControl_Day, // Day
        StatsTimeUserControl_Week, // Week
        StatsTimeUserControl_Month, // Month
        StatsTimeUserControl_Custom, // Custom
        StatsTimeUserControl_labelTime, // Time

        FindSystemsUserControl_Cannotfindsystem, // Cannot find system
        FindSystemsUserControl_EDSM, // new warning..

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

        UserControlMaterialTrader_Raw, // Raw
        UserControlMaterialTrader_Encoded, // Encoded
        UserControlMaterialTrader_Manufactured, // Manufactured
        // not used UserControlMaterialTrader_Trade, // Trade
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

        UserControlSuitsWeapons_Confirm,

        CommanderForm_CAPILoggingin,
        CommanderForm_CAPIDisabled,
        CommanderForm_CAPILogout,
        CommanderForm_CAPILoggedin,
        CommanderForm_CAPILogin,
        CommanderForm_CAPIAwaitLogin,
    }

    public static class EDTranslatorExtensions
    {
        static public string T(this string s, EDTx value)              // use the enum.
        {
            return BaseUtils.Translator.Instance.Translate(s, value.ToString().Replace("_", "."));
        }
    }
}
