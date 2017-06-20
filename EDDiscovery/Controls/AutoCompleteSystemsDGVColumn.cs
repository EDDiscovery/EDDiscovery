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
using System;
using System.ComponentModel;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery.DB;
using ExtendedControls;

namespace EDDiscovery.Controls
{
    /// <summary>
    /// A string-backed <see cref="AutoCompleteDGVColumn"/> for interracting with system names.
    /// </summary>
    public class AutoCompleteSystemsDGVColumn : AutoCompleteDGVColumn
    {
        /// <summary>
        /// Constructs a new <see cref="AutoCompleteSystemsDGVColumn"/> instance.
        /// </summary>
        public AutoCompleteSystemsDGVColumn() : base()
        {
            AutoCompleteGenerator += SystemClass.ReturnOnlySystemsListForAutoComplete;
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="AutoCompleteSystemsDGVColumn"/>.
        /// </summary>
        /// <returns>An <see cref="object"/> that represents the cloned <see cref="AutoCompleteSystemsDGVColumn"/>.</returns>
        public override object Clone()
        {
            return base.Clone() as AutoCompleteSystemsDGVColumn;
        }

        /// <summary>
        /// Apply themeing to the edit control. Since the edit control is randomly generated, short lived (but not internally!),
        /// and unable to track theme changes, this method will be called every time the edit control is shown in a different cell.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to associated with the event.</param>
        protected override void ApplyThemeToEditControl(Control ctrl)
        {
            EDDTheme.Instance.ApplyToControls(ctrl);
        }
    }
}
