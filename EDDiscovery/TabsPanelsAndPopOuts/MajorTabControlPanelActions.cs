/*
 * Copyright 2015-2024 EDDiscovery development team
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

using System.Windows.Forms;
using static EDDiscovery.UserControls.UserControlCommonBase;

namespace EDDiscovery
{
    public partial class MajorTabControl : ExtendedControls.ExtTabControl
    {
        // request came from primary panel
        // splitter has already distributed it around itself
        // We pass it onto other tabs, and stop if its been positively serviced

        private PanelActionState RequestPanelOperationPrimaryTab(UserControls.UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"MTC RequestOp primary request {actionobj}");

            // We should never get this because the primary tab always has a travel grid
            // and it will claim the call in the primary tab splitter
            System.Diagnostics.Debug.Assert(!(actionobj is RequestTravelToJID));      

            foreach (TabPage tp in TabPages)
            {
                if (tp.Tag == null)       // tag is null if not primary. It came in on primary, don't resent to primary, as splitter has already distributed it
                {
                    var uccb = (UserControls.UserControlCommonBase)tp.Controls[0];

                    //System.Diagnostics.Debug.WriteLine($"MTC PerformOp primary from {sender.PanelID} distribute to tab {tp.Name}: {actionobj}");
                    var res = uccb.CallPerformPanelOperation(sender, actionobj);
                    //System.Diagnostics.Debug.WriteLine($"..PerformOp Primary result {res} panel {tp.Text}");

                    if (IsPASResult(res))       // if we have a stopping result
                    {
                        //System.Diagnostics.Debug.WriteLine($"..PerformOp Primary terminated {res} panel {tp.Text}");
                        return res;
                    }
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine($"MTC RequestOp primary from {sender.PanelID} don't send back to primary: {actionobj}");
                }
            }

            // pass onto popouts since noone has claimed it
            return eddiscovery.PopOuts.PerformPanelOperation(sender, actionobj);       
        }

        // request came from secondary panel 
        private PanelActionState RequestPanelOperationSecondaryTab(TabPage page, UserControls.UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Perform Other Panel operation request {actionobj}");
            return RequestOperationOther(page, sender, actionobj);
        }

        // request came from a pop up panel
        public PanelActionState RequestPanelOperationPopOut(UserControls.UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Perform Popout Panel operation request {actionobj}");
            return RequestOperationOther(null, sender, actionobj);
        }

        // For tabs other than primary, or for senders other than a tab page (action lang, popouts: page will be null)
        // see if the request is valid, and for what tabs
        // don't distribute certain types
        // and send certain types only to primary tab

        public PanelActionState RequestOperationOther(TabPage page, UserControls.UserControlCommonBase sender, object actionobj)
        {
            // if we are pushing an operation down, but its a History push up from a secondary tab or a pop up panel, we stop it.
            // only the primary tab pushes these around

            if (IsOperationHistoryPush(actionobj))            
            {
                //System.Diagnostics.Debug.WriteLine($"..blocked as TH push from secondary tab");
                return PanelActionState.NotHandled;
            }

            // if we are pushing a operation that only the primary tab can operate on as it owns the travel grid (TravelToJID, RequestTravelHistoryPos)
            // send to primary tab only as it owns the primary travel grid

            else if (IsOperationForPrimaryTH(actionobj))
            {
                //System.Diagnostics.Debug.WriteLine($"..Send travel grid request {actionobj} to primary tab");
                UserControls.UserControlContainerSplitter pt = PrimarySplitterTab;
                return pt.CallPerformPanelOperation(sender, actionobj);        
            }

            // else push to all until we get a result
            else 
            { 
                foreach (TabPage tp in TabPages)       
                {
                    if (tp != page)     // don't resent to page which originated it - its already been distributed on that page. If page == null, all will pass
                    {
                        var uccb = (UserControls.UserControlCommonBase)tp.Controls[0];

                        //System.Diagnostics.Debug.WriteLine($"MTC PerformOp Other from {sender.PanelID} distribute to tab {tp.Name}: {actionobj}");
                        var res = uccb.CallPerformPanelOperation(sender, actionobj);
                        //System.Diagnostics.Debug.WriteLine($"..PerformOp Other result {res} panel {tp.Text}");

                        if (IsPASResult(res))
                        {
                            //System.Diagnostics.Debug.WriteLine($"..PerformOp Other terminated {res} panel {tp.Text}");
                            return res;
                        }
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine($"MTC PerformOp Other from {sender.PanelID} don't send to original sender: {actionobj}");
                    }
                }
            }

            // no result stopped the push, so push to the panels
            return eddiscovery.PopOuts.PerformPanelOperation(sender, actionobj);       // and send to all pop out forms
        }
    }
}
