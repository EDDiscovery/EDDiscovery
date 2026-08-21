/*
 * Copyright 2026-2026 EDDiscovery development team
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

using System;
using System.Drawing;

namespace EDDiscovery.UserControls
{
    public partial class Bindings : UserControlCommonBase
    {
        public Bindings()
        {
            InitializeComponent();
            DBBaseName = "Bindings";
        }

        protected override void Init()
        {
            var frontierpresetfilebindingfilename = EliteDangerousCore.BindingsFile.FindBindingsFile(EDDOptions.Instance.FrontierBindingsFolder, true);
            bindingsEditor.Init(EDDOptions.Instance.FrontierBindingsFolder, frontierpresetfilebindingfilename);
            bindingsEditor.ChangedBindings += (s) =>
            {
                if (DiscoveryForm.FrontierBindings.FileName.EqualsIIC(s) || !DiscoveryForm.FrontierBindings.IsLoaded)      // if same name, or not loaded, try and load
                    DiscoveryForm.LoadFrontierBindings();       // reload, 
            };
            bindingsEditor.ChangedDefault += (s) =>
            {
                if (!DiscoveryForm.FrontierBindings.FileName.EqualsIIC(s))      // if default is not the same as the current filename.
                    DiscoveryForm.LoadFrontierBindings();       // reload, 
            };
        }

        protected override void InitialDisplay()
        {
        }

        protected override void Closing()
        {
        }

        public override bool AllowClose()
        {
            if ( bindingsEditor.IsDirty)
            {

            }

            return true;
        }

    }
}
