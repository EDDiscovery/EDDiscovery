/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EDDiscovery.ExtendedControls
{
    #region public class DataGridViewExt : DoubleBufferedDataGridView

#if false
    public class DataGridViewExt : DoubleBufferedDataGridView
    {
        // More to come here...

        // Effortless index+1 row header numbering (SavedRouteControl, maybe materials, recipes, etc).
        // Row header numbering for HistoryEntry-type controls that use more arbitrary numbers.
        // Genericized (easily accessible) Drag & Drop handling.
        
    }
#endif

    #endregion // public class DataGridViewExt : DoubleBufferedDataGridView


    #region public class DoubleBufferedDataGridView : DataGridView

    /// <summary>
    /// Displays data in a customizable grid, with double-buffering enabled by default, and a quick
    /// design-capable accessor to avoid manual interractions with <c>DefaultCellStyle.WrapMode</c>.
    /// </summary>
    public class DoubleBufferedDataGridView : DataGridView
    {
        private const bool DefaultDoubleBufferedValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleBufferedDataGridView"/> class.
        /// </summary>
        public DoubleBufferedDataGridView()
        {
            base.DoubleBuffered = DefaultDoubleBufferedValue;
        }

        /// <summary>
        /// Gets or sets a value indicating whether textual content in the <see cref="DoubleBufferedDataGridView"/>
        /// cells is wrapped to subsequent lines or truncated when it is too long to fit on a single line.
        /// <para>This prop is merely a designer-friendly accessor for <c>DefaultCellStyle.WrapMode</c>,
        /// as that prop doesn't always serialize properly.</para>
        /// </summary>
        [Category("Layout")]
        [DefaultValue(DataGridViewTriState.NotSet)]
        [Description("Indicates whether textual content will wrap or truncate inside of all cells.")]
        public DataGridViewTriState DefaultCellStyleWrapMode
        {
            get
            {
                return DefaultCellStyle.WrapMode;
            }
            set
            {
                DefaultCellStyle.WrapMode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DoubleBufferedDataGridView"/>
        /// should redraw its surface using a secondary buffer to reduce or prevent flickering.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(DefaultDoubleBufferedValue)]
        [Description("Indicates whether the control uses a secondary drawing buffer to reduce or prevent flickering.")]
        public new bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                if (!InvokeRequired)
                    base.DoubleBuffered = value;
                else
                    Invoke(new Action(() => { base.DoubleBuffered = value; }));
            }
        }
    }

    #endregion // public class DoubleBufferedDataGridView : DataGridView
}
