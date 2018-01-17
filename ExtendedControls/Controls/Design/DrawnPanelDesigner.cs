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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ExtendedControls.Controls.Design
{
    /// <summary>
    /// Extends the design-mode behaviour of a <see cref="DrawnPanel"/> <see cref="Control"/>.
    /// </summary>
    public class DrawnPanelDesigner : ControlDesigner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawnPanelDesigner"/> class.
        /// </summary>
        public DrawnPanelDesigner() : base() { }

        /// <summary>
        /// Initializes a newly created <see cref="DrawnPanel"/>.
        /// </summary>
        /// <param name="defaultValues">A non-generic <see cref="IDictionary"/> containing the default
        /// <see cref="Control"/> properties with values.</param>
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            // By default, base will assign .Name to .Text, even if edited via defaultValues. Override that behaviour.
            this.Control.Text = string.Empty;
        }
    }
}
