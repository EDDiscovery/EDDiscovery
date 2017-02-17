/*
 * Copyright © 2017 EDDiscovery development team
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
 * 
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionLedger : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "JID of event", UserData, "Configure Ledger Command" );
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "L_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Ledger");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                bool nextvalidentry = false;

                if (cmdname != null && cmdname.Equals("ATORBEFORE", StringComparison.InvariantCultureIgnoreCase))
                {
                    nextvalidentry = true;  // means, find next ledger entry BEFORE this JID..
                    cmdname = sp.NextWord();
                }

                if (cmdname != null)
                {
                    long jid;
                    if (!cmdname.InvariantParse(out jid))
                    {
                        ap.ReportError("Non integer JID in Ledger");
                        return true;
                    }

                    EDDiscovery2.DB.MaterialCommoditiesLedger ml = ap.actioncontroller.HistoryList.materialcommodititiesledger;
                    EDDiscovery2.DB.MaterialCommoditiesLedger.Transaction tx = ml.Transactions.Find(x => x.jid == jid);// try and find it in the ledger
                    int jidindex = ap.actioncontroller.HistoryList.EntryOrder.FindIndex(x => x.Journalid == jid);    // find it in the journal

                    if ( tx == null && nextvalidentry ) // if not directly found..
                    {
                        while ( jidindex > 0 )      // go back, to 0.  if jidindex is -1 above, nothing happens
                        {
                            jidindex--;            // predec so we don't test first one
                            jid = ap.actioncontroller.HistoryList.EntryOrder[jidindex].Journalid;
                            tx = ml.Transactions.Find(x => x.jid == jid);
                            if (tx != null)
                                break;
                        }
                    }

                    if (tx == null )
                    {
                        ap.ReportError("Cannot find entry in Ledger");
                        return true;
                    }

                    ap.currentvars[prefix + "JID"] = jid.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap.currentvars[prefix + "IndexOf"] = ap.actioncontroller.HistoryList.EntryOrder[jidindex].Indexno.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap.currentvars[prefix + "UTCTime"] = tx.utctime.ToString("MM/dd/yyyy HH:mm:ss");
                    ap.currentvars[prefix + "EntryType"] = tx.jtype.ToString();
                    ap.currentvars[prefix + "Notes"] = tx.notes;
                    ap.currentvars[prefix + "Value"] = tx.cashadjust.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap.currentvars[prefix + "PPU"] = tx.profitperunit.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    ap.currentvars[prefix + "Credits"] = tx.cash.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                    ap.ReportError("Missing JID in Ledger");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }


}
