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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class ToolStripCustomColourTable : ProfessionalColorTable
    {
        public ToolStripCustomColourTable()
        {
            this.UseSystemColors = false;
        }

        // used by Renderer
        public Color colMenuText = Color.White;
        public Color colMenuSelectedText = Color.DeepSkyBlue;

        public Color colMenuBackground = Color.Yellow;  // drop down menu colour of both menus and pop up menus
        public override Color ToolStripDropDownBackground { get { return colMenuBackground; } } // Background of menu or toolstrip dropdown

        public Color colMenuBarBackground = Color.Yellow;       // Menu strip holding menus across the top of the screen
        public override Color MenuStripGradientBegin { get { return colMenuBarBackground; } } // Left of menustrip render..
        public override Color MenuStripGradientEnd { get { return colMenuBarBackground; } } // Right of menu

        public Color colMenuBorder = Color.Crimson; // All menu borders
        public override Color MenuBorder { get { return colMenuBorder; } }

        public Color colMenuSelectedBack = Color.DeepPink;      // no evidence i can get gradients working
        public override Color MenuItemSelectedGradientBegin { get { return colMenuSelectedBack; } } // MS: Top of top-level menu item; Mono: Unused
        public override Color MenuItemSelectedGradientEnd { get { return colMenuSelectedBack; } } // MS: Bottom of top-level menu item; Mono: Unused
        public override Color MenuItemPressedGradientBegin { get { return colMenuSelectedBack; } } // Top of active top-level menu item
        public override Color MenuItemPressedGradientEnd { get { return colMenuSelectedBack; } } // Bottom of active top-level menu item

        public Color colMenuHighlightBorder = Color.White; // around the selector
        public Color colMenuHighlightBack = Color.LightGray;    // background
        public override Color MenuItemBorder { get { return colMenuHighlightBorder; } } // Border of menuitem, or of menu or toolstrip dropdown item
        public override Color MenuItemSelected { get { return colMenuHighlightBack; } } // Background of hovered menu or toolstrip dropdown item

        public Color colCheckButtonChecked = Color.Yellow;
        public override Color CheckBackground { get { return colCheckButtonChecked; } }      // Colour of checkboxes when checked

        public Color colCheckButtonPressed = Color.Blue;// Colour of checkboxes when pressed down
        public override Color CheckPressedBackground { get { return colCheckButtonPressed; } }

        public Color colCheckButtonHighlighted = Color.Pink;     // when hover over and ticked.
        public override Color CheckSelectedBackground { get { return colCheckButtonHighlighted; } }

        public Color colToolStripButtonCheckedBack = Color.Green;
        public override Color ButtonCheckedGradientBegin { get { return colToolStripButtonCheckedBack; } } // Top of button, checked
        public override Color ButtonCheckedGradientEnd { get { return colToolStripButtonCheckedBack; } } // Bottom of button, checked
        public override Color ButtonCheckedGradientMiddle { get { return colToolStripButtonCheckedBack; } } // Unused

        public Color colToolStripButtonPressedBack = Color.Tomato;
        public override Color ButtonPressedGradientBegin { get { return colToolStripButtonPressedBack; } } // Top of button, when pressed
        public override Color ButtonPressedGradientEnd { get { return colToolStripButtonPressedBack; } } // Bottom of button
        public override Color ButtonPressedGradientMiddle { get { return colToolStripButtonPressedBack; } } // Unused

        public Color colToolStripButtonSelectedBack = Color.Yellow;
        public override Color ButtonSelectedGradientBegin { get { return colToolStripButtonSelectedBack; } } // MS: Top of button, unchecked and hovered; Mono: Top of button or menu item, unchecked and hovered
        public override Color ButtonSelectedGradientEnd { get { return colToolStripButtonSelectedBack; } } // MS: Bottom of button, unchecked and hovered; Mono: Bottom of button or menu item, unchecked and hovered
        public override Color ButtonSelectedGradientMiddle { get { return colToolStripButtonSelectedBack; } } // Unused

        public Color colToolStripButtonBorder = Color.Silver;
        public override Color ButtonPressedBorder { get { return colToolStripButtonBorder; } } // MS: Unused; Mono: Border of button, checked
        public override Color ButtonSelectedBorder { get { return colToolStripButtonBorder; } } // MS: Border of button, checked or hovered; Mono: Border of button, hovered
        
        public Color colLeftSelectionMargin = Color.Purple;         // margin on left of screen, where buttons are
        public override Color ImageMarginGradientBegin { get { return colLeftSelectionMargin; } } // MS: Left of dropdown margin; Mono: Unused
        public override Color ImageMarginGradientEnd { get { return colLeftSelectionMargin; } } // MS: Right of dropdown margin; Mono: Unused
        public override Color ImageMarginGradientMiddle { get { return colLeftSelectionMargin; } } // MS: Middle of dropdown margin; Mono: Unused

        public Color colToolStripBackground = Color.Lavender;   // background of a tool strip.  make sure you don't assign to back color (set it to control)
        public override Color ToolStripGradientBegin { get { return colToolStripBackground; } } // MS: Top of toolstrip; Mono: Top of toolstrip or active top-level menu item, Left of dropdown margin
        public override Color ToolStripGradientEnd { get { return colToolStripBackground; } } // MS: Bottom of toolstrip; Mono: Bottom of toolstrip or active top-level menu item, Right of dropdown margin
        public override Color ToolStripGradientMiddle { get { return colToolStripBackground; } } // MS: Middle of toolstrip; Mono: Unused

        public Color colToolStripBorder = Color.Pink;     
        public override Color ToolStripBorder { get { return colToolStripBorder; } } // Toolstrip border

        public Color colToolStripSeparator = Color.White;
        public override Color SeparatorDark { get { return colToolStripSeparator; } } // Left edge of vertical separator, colour of horizontal separator
        public override Color SeparatorLight { get { return colToolStripSeparator; } } // Right edge of vertical separator

        public Color colGripper = Color.Yellow;    // gripper on the left of a toolbar
        public override Color GripDark { get { return colGripper; } } // Top-left dots of toolstrip grip
        public override Color GripLight { get { return colGripper; } } // Bottom-right dots of toolstrip grip

        public Color colStatusStrip = Color.Green;  // no evidence  .. seems to respond to BackColour only
        public override Color StatusStripGradientBegin { get { return colStatusStrip; } } // Unused
        public override Color StatusStripGradientEnd { get { return colStatusStrip; } } // Unused

        public Color colOverflowButton = Color.DeepSkyBlue; // no evidence
        public override Color OverflowButtonGradientBegin { get { return colOverflowButton; } } // Top of overflow button
        public override Color OverflowButtonGradientEnd { get { return colOverflowButton; } } // Bottom of overflow button
        public override Color OverflowButtonGradientMiddle { get { return colOverflowButton; } } // MS: Middle of overflow button; Mono: Unused
    }
}
