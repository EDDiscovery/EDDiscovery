/*
 * Copyright 2019-2021 Robbyxp1 @ github.com
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

using GLOFC.GL4.Controls;
using System.Drawing;
using static GLOFC.GL4.Controls.GLBaseControl;

namespace EDDiscovery.UserControls.Map3D
{
    public static class MapThemer
    { 
        public static void Theme(GLBaseControl s)      // run on each control during add, theme it
        {
            Color formback = Color.FromArgb(220, 60, 60, 70);
            Color buttonface = Color.FromArgb(255, 128, 128, 128);
            Color texc = Color.Orange;

            var but = s as GLButton;
            if (but != null)
            {
                but.ButtonFaceColor = buttonface;
                but.ForeColor = texc;
                but.BackColor = buttonface;
                but.BorderColor = buttonface;
                return;
            }

            var cb = s as GLCheckBox;
            if (cb != null)
            {
                cb.ButtonFaceColor = buttonface;
                return;
            }
            var cmb = s as GLComboBox;
            if (cmb != null)
            {
                cmb.BackColor = formback ;
                cmb.ForeColor = cmb.DropDownForeColor = texc;
                cmb.FaceColor = cmb.DropDownBackgroundColor = buttonface;
                cmb.BorderColor = formback;
                return;
            }

            var dt = s as GLDateTimePicker;
            if (dt != null)
            {
                dt.BackColor = buttonface;
                dt.ForeColor = texc;
                dt.Calendar.ButLeft.ForeColor = dt.Calendar.ButRight.ForeColor = texc;
                dt.SelectedColor = Color.FromArgb(255, 160, 160, 160);
                return;
            }

            var fr = s as GLForm;
            if (fr != null)
            {
                fr.BackColor = formback;
                fr.ForeColor = texc;
                return;
            }

            var tb = s as GLMultiLineTextBox;
            if (tb != null)
            {
                tb.BackColor = formback;
                tb.ForeColor = texc;
                return;
            }

            Color cmbck = Color.FromArgb(255, 128, 128, 128);

            var ms = s as GLMenuStrip;
            if (ms != null)
            {
                ms.BackColor = cmbck;
                ms.IconStripBackColor = cmbck.Multiply(1.2f);
                return;
            }

            var mi = s as GLMenuItem;
            if (mi != null)
            {
                mi.BackColor = cmbck;
                mi.ButtonFaceColor = cmbck;
                mi.ForeColor = texc;
                mi.BackDisabledScaling = 1.0f;
                return;
            }

            var dgv = s as GLDataGridView;
            if ( dgv != null )
            {
                dgv.DefaultCellStyle.Padding = new PaddingType(2);

                dgv.BackColor = Color.FromArgb(200, 20, 20, 20);
                dgv.DefaultColumnHeaderStyle.ForeColor = dgv.DefaultRowHeaderStyle.ForeColor =
                dgv.DefaultCellStyle.ForeColor = dgv.DefaultAltRowCellStyle.ForeColor = texc;

                dgv.UpperLeftBackColor = dgv.DefaultColumnHeaderStyle.BackColor = dgv.DefaultRowHeaderStyle.BackColor = Color.FromArgb(200, 64,64,64);

                dgv.DefaultCellStyle.BackColor = Color.FromArgb(200, 40, 40, 40);
                dgv.DefaultAltRowCellStyle.BackColor = Color.FromArgb(200, 50, 50, 50);

                dgv.ScrollBarTheme.BackColor = Color.Transparent;
                dgv.ScrollBarTheme.SliderColor = Color.FromArgb(0, 64, 64, 64);
                dgv.ScrollBarTheme.ThumbButtonColor = Color.DarkOrange;
                dgv.ScrollBarTheme.MouseOverButtonColor = Color.Orange;
                dgv.ScrollBarTheme.MousePressedButtonColor = Color.FromArgb(255, 255, 192, 0);
                dgv.ScrollBarTheme.ArrowButtonColor = Color.Transparent;
                dgv.ScrollBarTheme.ArrowColor = Color.DarkOrange;

                dgv.DefaultRowHeaderStyle.SelectedColor = dgv.DefaultCellStyle.SelectedColor = Color.FromArgb(128, 128, 128, 0);
            }

            var trb = s as GLTrackBar;
            if ( trb != null )
            {
                trb.BackColor = formback;
                trb.TickColor = texc;
                trb.ForeColor = buttonface;     // of bar
                trb.ButtonFaceColor = Color.FromArgb(255, 190, 190, 190);
                trb.FaceColorScaling = 0.5f;
            }
        }
    }
}
