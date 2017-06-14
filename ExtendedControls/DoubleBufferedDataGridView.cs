﻿/*
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    /// <summary>
    /// Displays data in a customizable grid, with double-buffering enabled by default.
    /// </summary>
    public class DoubleBufferedDataGridView : DataGridView
    {
        #region Public interfaces

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleBufferedDataGridView"/> class.
        /// </summary>
        public DoubleBufferedDataGridView()
        {
            base.DoubleBuffered = DefaultDoubleBufferedValue;
        }

        #endregion // ctors

        #region Properties

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

        #endregion // Properties

        #endregion // Public interfaces

        private const bool DefaultDoubleBufferedValue = true;
    }
}
