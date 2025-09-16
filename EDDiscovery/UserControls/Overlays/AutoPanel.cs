/*
 * Copyright 2025 - 2025 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.UIEvents;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery.UserControls
{
    public partial class AutoPanel : UserControlCommonBase
    {
        public AutoPanel()
        {
            InitializeComponent();
            DBBaseName = "AutoPanel";
        }

        private enum PanelMode { Unknown, Supercruising, FSDJump,
                    Surveying, Compass, NormalSpace, Combat, Landed , Docked, OnFootInterior, Mining, 
                    GlideMode, Organics, OnGroundCombat, Docking };

        private PanelMode mode = PanelMode.Unknown;
        private bool hidden = true;


        UIOverallStatus uistatus;
        HistoryEntry lasthe;

        protected override void Init()
        {
            DiscoveryForm.OnNewUIEvent += DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange;

            // if opened after start, we need to pick up state from discoveryform

            uistatus = DiscoveryForm.UIOverallStatus;
            lasthe = DiscoveryForm.History.GetLast;
        }

        protected override void InitialDisplay()
        {
            UpdateState();
        }


        protected override void Closing()
        {
            DiscoveryForm.OnNewUIEvent -= DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= DiscoveryForm_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
        }

        private void DiscoveryForm_OnHistoryChange()
        {
            lasthe = DiscoveryForm.History.GetLast;
            UpdateState();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            System.Diagnostics.Debug.WriteLine($"Autopanel NewHistory {he.EventTimeUTC}");
            lasthe = DiscoveryForm.History.GetLast;
            UpdateState();
        }


        private void DiscoveryForm_OnNewUIEvent(EliteDangerousCore.UIEvent ui)
        {
            if (ui is UIOverallStatus os)      // if opened at start we get this, and we get it every time flags changes
            {
                System.Diagnostics.Debug.WriteLine($"AutoPanel UI {ui.EventTypeID} : {ui.ToString()}");
                uistatus = os;
                UpdateState();
            }
        }


        void UpdateState()
        {
            PanelMode newmode = PanelMode.Unknown;

            Ship ship = lasthe?.ShipInformation;        // may be null

            UIMode.ModeType mt = uistatus.Mode;

            //var t1 = UIMode.ModeWithoutMulticrewTaxi(UIMode.ModeType.TaxiDockedPlanet);
            //var t2 = UIMode.ModeWithoutMulticrewTaxi(UIMode.ModeType.MulticrewSRV);

            // in the order of enum ModeType:

            if ( uistatus.Focus != 0)
            {
                hidden = true;
            }
            else
            {
                hidden = false;
            }


            if (mt == UIMode.ModeType.MainShipNormalSpace)
            {
                bool glidemode = uistatus.Flags.Contains(UITypeEnum.GlideMode);
                bool hardpointdeployed = uistatus.Flags.Contains(UITypeEnum.HardpointsDeployed);
                bool hasminingequipment = ship?.HasMiningEquipment() ?? false;
                bool hasweapons = ship?.HasWeapons() ?? false;
                bool docking = lasthe?.Status.DockingPad>0;
                bool isinmininglocation = BodyDefinitions.IsBodyNameRing(uistatus.BodyName); 

                System.Diagnostics.Debug.WriteLine($"Autopanel UpdateState Docking: {docking}");

                newmode = PanelMode.NormalSpace;

                if (glidemode)
                {
                    newmode = PanelMode.GlideMode;
                }
                else if ( docking )
                {
                    newmode = PanelMode.Docking;
                }
                else if (uistatus.Pos.ValidPosition) // above planet
                {
                    if (hasweapons && hardpointdeployed)
                        newmode = PanelMode.Combat;
                    else
                        newmode = PanelMode.Compass;
                }
                else if (hardpointdeployed)
                {
                    if (hasminingequipment && isinmininglocation)           // mining location, and mining equipment
                        newmode = PanelMode.Mining;
                    else if (hasweapons)
                        newmode = PanelMode.NormalSpace;
                }
                else if (isinmininglocation)        // mining location, no hardpoints, mining screen
                {
                    newmode = PanelMode.Mining;
                }
            }
            else if (mt == UIMode.ModeType.MainShipDockedPlanet || mt == UIMode.ModeType.MainShipDockedStarPort)
            {
                newmode = PanelMode.Docked;
            }
            else if (mt == UIMode.ModeType.MainShipSupercruise)
            {
                bool injump = uistatus.Flags.Contains(UITypeEnum.FsdJump);

                if (injump)
                {
                    newmode = PanelMode.FSDJump;
                }
                else if (uistatus.Pos.ValidPosition)
                {
                    newmode = PanelMode.Compass;
                }
                else if (uistatus.Flags.Contains(UITypeEnum.HUDInAnalysisMode))
                {
                    newmode = PanelMode.Surveying;
                }
                else
                {
                    newmode = PanelMode.Supercruising;
                }
            }
            else if (mt == UIMode.ModeType.MainShipLanded)
            {
                newmode = PanelMode.Landed;
            }
            else if (mt == UIMode.ModeType.SRV)
            {
                newmode = PanelMode.Compass;
            }
            else if (mt == UIMode.ModeType.Fighter)
            {
                newmode = PanelMode.Combat;
            }
            else if (mt == UIMode.ModeType.OnFootPlanet)
            {
                if (uistatus.HandItem?.Name == "Genetic Sampler")
                {
                    newmode = PanelMode.Organics;
                }
                else if (uistatus.HandItem is ItemData.Weapon)
                {
                    newmode = PanelMode.OnGroundCombat;
                }
                else
                {
                    newmode = PanelMode.Compass;
                }
            }
            else if (uistatus.UIMode.OnFoot)
            {
                newmode = PanelMode.OnFootInterior;
            }
            else
            {
                // UI status not valid, probably due to running without elite active

                if (lasthe?.Status.IsDocked == true)
                {
                    newmode = PanelMode.Docked;
                }
                else if (lasthe?.Status.IsInSupercruise == true)
                {
                    newmode = PanelMode.Supercruising;
                }
                else if (lasthe?.Status.IsLanded == true)
                {
                    newmode = PanelMode.Landed;
                }
                else if (lasthe?.Status.IsSRV == true)
                {
                    newmode = PanelMode.Compass;
                }
                else if (lasthe?.Status.IsFighter == true)
                {
                    newmode = PanelMode.Combat;
                }
                else if (lasthe?.Status.TravelState == HistoryEntryStatus.TravelStateType.OnFootPlanet)
                {
                    newmode = PanelMode.Compass;
                }
                else if (lasthe?.Status.OnFoot == true)
                {
                    newmode = PanelMode.OnFootInterior;
                }
                else
                {
                    newmode = PanelMode.NormalSpace;
                }
            }

            mode = newmode;

            labelMode.Text = "Panel Mode: " + mode.ToString() + (hidden ? " (Hidden) " :"");
            label2.Text = "UIMode: " + uistatus.UIMode.ToString();
            label3.Text = "HETS:" + lasthe?.Status.TravelState.ToString();
        }



        public override bool SupportTransparency => true;
        protected override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            this.panel1.BackColor = curcol;
        }
    }
}
