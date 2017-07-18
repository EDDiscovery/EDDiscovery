/*
 * Copyright © 2016 EDDiscovery development team
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
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EDDiscovery
{
    public class EDDToolStripRenderer : ToolStripProfessionalRenderer//ToolStripSystemRenderer
    {
        public ToolStripCustomColourTable colortable;

        public EDDToolStripRenderer() : base(new ToolStripCustomColourTable())
        {
            this.colortable = (ToolStripCustomColourTable)ColorTable;
            this.RoundedEdges = true;
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)            // called to determine text colour..
        {
            if (e.Item.Selected || e.Item.Pressed)
                e.TextColor = colortable.colMenuSelectedText;
            else
                e.TextColor = colortable.colMenuText;

            base.OnRenderItemText(e);
        }
    }
}
