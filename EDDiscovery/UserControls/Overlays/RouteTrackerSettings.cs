using System;
using System.Collections.Generic;
using System.Drawing;
using EliteDangerousCore;
namespace EDDiscovery.UserControls
{
    public partial class SurveyorPanel
    {
        private int commanderId = int.MinValue;
        private int commanderGeneration;
        private bool loadingCommander;
        internal static string CommanderKey(int id)
        {
            return "RouteTracker_C" + id.ToString(System.Globalization.CultureInfo.InvariantCulture) + "_";
        }
        private void SelectCommander(bool reload)
        {
            if (this is UserControlRouteTracker) ApplyCommander(DiscoveryForm.History.CommanderId, reload);
        }
        internal void ApplyCommander(int next, bool reload)
        {
            if (next == commanderId) return;
            if (commanderId >= 0)
            {
                PutSetting(dbpinstate, rollUpPanelTop.PinState);
                PutSetting(dbRouteManualPos, manualTarget);
            }
            commanderId = next;
            commanderGeneration++;
            DBBaseName = CommanderKey(next);
            loadingCommander = true;
            try
            {
                // Invalidate the previous commander's route before reloading controls.
                currentRoute = null;
                manualTarget = -1;
                lastsystemonroute = null;
                instartjump = false;
                is_latest = false;
                eventsseen = 0;
                cur_sys = null;
                shipinfo = null;
                shipfsdinfo = null;
                playedtriggers = new Dictionary<string, HashSet<string>>();
                starclass = "";
                bodies_found = 0;
                all_found = false;
                scansummarytext = "";
                lock (extPictureBoxScrollSystemDetails)
                {
                    drawsystemtext.Clear();
                    drawsystemsignallist = "";
                    drawsystemvalue = 0;
                    extPictureBoxSystemDetails.ClearImageList();
                    extPictureBoxScrollSystemDetails.Render();
                }
                extPictureBoxScanSummary.ClearImageList();
                extPictureBoxScanSummary.Render();
                extPictureBoxTitle.ClearImageList();
                extPictureBoxTitle.Render();
                extPictureBoxRoute.ClearImageList();
                extPictureBoxRoute.Render();
                if (!reload) return;
                drawsystemupdatetimer?.Stop();
                PopulateCtrlList();
                extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
                fsssignalstodisplay = GetSetting(dbfsssignals, "");
                displayfont = BaseUtils.FontHandler.GetFontFromSetting(GetSetting(dbFont, ""), null);
                routecontrolsettings = GetSetting(dbroutecontrol, "showJumps;showwaypoints;shownotetext");
                rollUpPanelTop.PinState = GetSetting(dbpinstate, true);
                edsmSpanshButton.Init(this, "EDSMSpansh", "");
                LoadRoute(GetSetting(dbRouteName, ""), GetSetting(dbRouteManualPos, -1));
                // Show a deliberate empty state even for a commander with no history.
                DrawRoute(null);
                SetVisibility();
            }
            finally { loadingCommander = false; }
        }
    }
}
