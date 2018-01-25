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


        // in order of documentation

        public Color colToolStripButtonCheckedBack = Color.Green; // Button is checked
        public override Color ButtonCheckedGradientBegin { get { return colToolStripButtonCheckedBack; } } 
        public override Color ButtonCheckedGradientEnd { get { return colToolStripButtonCheckedBack; } } 
        public override Color ButtonCheckedGradientMiddle { get { return colToolStripButtonCheckedBack; } }

        // ButtonCheckedHighlight
        // ButtonCheckedHighlightBorder

        public override Color ButtonPressedBorder { get { return colToolStripButtonBorder; } } // MS: Unused; Mono: Border of button, checked

        public Color colToolStripButtonPressedBack = Color.Tomato;
        public override Color ButtonPressedGradientBegin { get { return colToolStripButtonPressedBack; } } // Top of button, when pressed
        public override Color ButtonPressedGradientEnd { get { return colToolStripButtonPressedBack; } } // Bottom of button
        public override Color ButtonPressedGradientMiddle { get { return colToolStripButtonPressedBack; } } // Unused

        // ButtonPressedHighlight
        // ButtonPressedHighlightBorder

        public Color colToolStripButtonBorder = Color.Silver;
        public override Color ButtonSelectedBorder { get { return colToolStripButtonBorder; } } // MS: Border of button, checked or hovered; Mono: Border of button, hovered

        public Color colToolStripButtonSelectedBack = Color.Pink;
        public override Color ButtonSelectedGradientBegin { get { return colToolStripButtonSelectedBack; } } // MS: Top of button or dropdown.. unchecked and hovered; Mono: Top of button or menu item, unchecked and hovered
        public override Color ButtonSelectedGradientEnd { get { return colToolStripButtonSelectedBack; } }
        public override Color ButtonSelectedGradientMiddle { get { return colToolStripButtonSelectedBack; } } // Unused

        // ButtonSelectedHighlight
        // ButtonSelectedHighlightBorder

        public Color colCheckButtonChecked = Color.Yellow;
        public override Color CheckBackground { get { return colCheckButtonChecked; } }      // Colour of checkboxes when checked

        public Color colCheckButtonPressed = Color.Blue;// Colour of checkboxes when pressed down
        public override Color CheckPressedBackground { get { return colCheckButtonPressed; } }

        public Color colCheckButtonHighlighted = Color.Pink;     // when hover over and ticked.
        public override Color CheckSelectedBackground { get { return colCheckButtonHighlighted; } }

        public Color colGripper = Color.Yellow;    // gripper on the left of a toolbar
        public override Color GripDark { get { return colGripper; } } // Top-left dots of toolstrip grip
        public override Color GripLight { get { return colGripper; } } // Bottom-right dots of toolstrip grip


        public Color colToolStripDropDownMenuImageMargin = Color.Purple;
        public override Color ImageMarginGradientBegin { get { return colToolStripDropDownMenuImageMargin; } }
        public override Color ImageMarginGradientEnd { get { return colToolStripDropDownMenuImageMargin; } }
        public override Color ImageMarginGradientMiddle { get { return colToolStripDropDownMenuImageMargin; } }

        public Color colToolStripDropDownMenuImageRevealed = Color.AliceBlue;
        public override Color ImageMarginRevealedGradientBegin { get { return colToolStripDropDownMenuImageRevealed; } }
        public override Color ImageMarginRevealedGradientEnd { get { return colToolStripDropDownMenuImageRevealed; } }
        public override Color ImageMarginRevealedGradientMiddle { get { return colToolStripDropDownMenuImageRevealed; } }


        public Color colMenuBorder = Color.Crimson; // All menu borders on Menu Strip
        public override Color MenuBorder { get { return colMenuBorder; } }

        public Color colMenuHighlightBorder = Color.White; // around the selector
        public override Color MenuItemBorder { get { return colMenuHighlightBorder; } } // ToolStripMenuItem

        public override Color MenuItemPressedGradientBegin { get { return colMenuSelectedBack; } } // ToolStripMenuItem
        public override Color MenuItemPressedGradientEnd { get { return colMenuSelectedBack; } } // ToolStripMenuItem
        //Middle

        public Color colMenuHighlightBack = Color.LightGray;    // background
        public override Color MenuItemSelected { get { return colMenuHighlightBack; } } // ToolStripMenuItem Background of hovered menu or toolstrip dropdown item

        public Color colMenuSelectedBack = Color.Orange;      // no evidence i can get gradients working
        public override Color MenuItemSelectedGradientBegin { get { return colMenuSelectedBack; } } // ToolStripMenuItem
        public override Color MenuItemSelectedGradientEnd { get { return colMenuSelectedBack; } } // ToolStripMenuItem

        public Color colMenuBarBackground = Color.Yellow;       // Menu strip holding menus across the top of the screen
        public override Color MenuStripGradientBegin { get { return colMenuBarBackground; } } // Left of menustrip render..
        public override Color MenuStripGradientEnd { get { return colMenuBarBackground; } } // Right of menu


        public Color colOverflowButton = Color.DeepSkyBlue; // no evidence
        public override Color OverflowButtonGradientBegin { get { return colOverflowButton; } } // Top of overflow button
        public override Color OverflowButtonGradientEnd { get { return colOverflowButton; } } // Bottom of overflow button
        public override Color OverflowButtonGradientMiddle { get { return colOverflowButton; } } // MS: Middle of overflow button; Mono: Unused

        //RaftingContainerGradientBegin
        //RaftingContainerGradientEnd

        public Color colToolStripSeparator = Color.White;
        public override Color SeparatorDark { get { return colToolStripSeparator; } } // Left edge of vertical separator, colour of horizontal separator
        public override Color SeparatorLight { get { return colToolStripSeparator; } } // Right edge of vertical separator


        public Color colStatusStrip = Color.Green;  // no evidence  .. seems to respond to BackColour only
        public override Color StatusStripGradientBegin { get { return colStatusStrip; } } // Unused
        public override Color StatusStripGradientEnd { get { return colStatusStrip; } } // Unused


        public Color colToolStripBorder = Color.Pink;
        public override Color ToolStripBorder { get { return colToolStripBorder; } } // Toolstrip border

        // ToolStripContentPanelGradiantBegin
        // ToolStripContentPanelGradiantEnd

        public Color colMenuBackground = Color.Purple;  // drop down menu colour of both menus and pop up menus
        public override Color ToolStripDropDownBackground { get { return colMenuBackground; } } // Background of menu or toolstrip dropdown

        public Color colToolStripBackground = Color.Lavender;   // background of a tool strip.  make sure you don't assign to back color (set it to control)
        public override Color ToolStripGradientBegin { get { return colToolStripBackground; } } // MS: Top of toolstrip; Mono: Top of toolstrip or active top-level menu item, Left of dropdown margin
        public override Color ToolStripGradientEnd { get { return colToolStripBackground; } } // MS: Bottom of toolstrip; Mono: Bottom of toolstrip or active top-level menu item, Right of dropdown margin
        public override Color ToolStripGradientMiddle { get { return colToolStripBackground; } } // MS: Middle of toolstrip; Mono: Unused

        // ToolStripPanelGradientBegin
        // ToolStripPanelGradientEnd

        // NOT in table, additions, used by Renderer
        public Color colMenuText = Color.White;
        public Color colMenuSelectedText = Color.DeepSkyBlue;
    }
}
