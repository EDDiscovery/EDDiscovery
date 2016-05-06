using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class CustomColourTable : ProfessionalColorTable
    {
        public CustomColourTable()
        {
            this.UseSystemColors = false;
        }

        public Color colMenuBackground = Color.FromArgb(255, 188, 199, 216);//ControlPaint.LightLight(ControlPaint.Light(Color.SlateBlue));
        public Color colToolStripBorder = Color.FromArgb(255, 133, 145, 162); //0x8591A2);//ControlPaint.LightLight(Color.SlateBlue);
        public Color colToolStripBackGround = Color.LightGray;
        public Color colToolStripSeparator = Color.FromArgb(255, 133, 145, 162); //0x8591A2);//ControlPaint.LightLight(Color.SlateBlue);
        public Color colMenuSelectedBack = Color.FromArgb(255, 201, 210, 225); //ControlPaint.LightLight(ControlPaint.LightLight(Color.LightSlateGray));
        public Color colMenuSelectedText = Color.Black;
        public Color dark = Color.FromArgb(255, 41, 57, 85);
        public Color colMenuText = Color.White;
        public Color colButtonSelectedBorder = Color.FromArgb(229, 195, 101);
        public Color colButtonSelectBackLight = Color.FromArgb(255, 252, 242);
        public Color colButtonSelectBackDark = Color.FromArgb(255, 236, 181);

        public override Color ButtonCheckedGradientBegin { get { return colButtonSelectBackLight; } } // Top of button, checked
        public override Color ButtonCheckedGradientEnd { get { return colButtonSelectBackDark; } } // Bottom of button, checked
        public override Color ButtonCheckedGradientMiddle { get { return Color.Empty; } } // Unused

        public override Color ButtonPressedGradientBegin { get { return colButtonSelectBackLight; } } // Top of button, checked and hovered
        public override Color ButtonPressedGradientEnd { get { return colButtonSelectBackLight; } } // Bottom of button, checked and hovered
        public override Color ButtonPressedGradientMiddle { get { return colButtonSelectBackLight; } } // Unused
        public override Color ButtonPressedBorder { get { return colButtonSelectedBorder; } } // MS: Unused; Mono: Border of button, checked

        public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(100, colButtonSelectedBorder); } } // MS: Top of button, unchecked and hovered; Mono: Top of button or menu item, unchecked and hovered
        public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(100, colButtonSelectedBorder); } } // MS: Bottom of button, unchecked and hovered; Mono: Bottom of button or menu item, unchecked and hovered
        public override Color ButtonSelectedGradientMiddle { get { return Color.FromArgb(100, colButtonSelectedBorder); } } // Unused
        public override Color ButtonSelectedBorder { get { return dark; } } // MS: Border of button, checked or hovered; Mono: Border of button, hovered

        public override Color CheckBackground { get { return colButtonSelectBackLight; } }
        public override Color CheckPressedBackground { get { return colButtonSelectBackDark; } }
        public override Color CheckSelectedBackground { get { return colButtonSelectBackDark; } }

        public override Color ToolStripGradientBegin { get { return colToolStripBackGround; } } // MS: Top of toolstrip; Mono: Top of toolstrip or active top-level menu item, Left of dropdown margin
        public override Color ToolStripGradientEnd { get { return colToolStripBackGround; } } // MS: Bottom of toolstrip; Mono: Bottom of toolstrip or active top-level menu item, Right of dropdown margin
        public override Color ToolStripGradientMiddle { get { return colToolStripBackGround; } } // MS: Middle of toolstrip; Mono: Unused
        public override Color ToolStripBorder { get { return colToolStripBorder; } } // Toolstrip border

        public override Color MenuStripGradientBegin { get { return colMenuBackground; } } // Left of menu
        public override Color MenuStripGradientEnd { get { return colMenuBackground; } } // Right of menu
        public override Color MenuBorder { get { return Color.Empty; } } // Border of drop-down menu

        public override Color ToolStripDropDownBackground { get { return colMenuBackground; } } // Background of menu or toolstrip dropdown

        public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(200, colMenuSelectedBack); } } // MS: Top of top-level menu item; Mono: Unused
        public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(200, colMenuSelectedBack); } } // MS: Bottom of top-level menu item; Mono: Unused
        public override Color MenuItemBorder { get { return dark; } } // Border of menuitem, or of menu or toolstrip dropdown item

        public override Color MenuItemPressedGradientBegin { get { return colMenuSelectedBack; } } // Top of active top-level menu item
        public override Color MenuItemPressedGradientEnd { get { return colMenuSelectedBack; } } // Bottom of active top-level menu item

        public override Color SeparatorDark { get { return colToolStripSeparator; } } // Left edge of vertical separator, colour of horizontal separator
        public override Color SeparatorLight { get { return colToolStripSeparator; } } // Right edge of vertical separator

        public override Color GripDark { get { return colToolStripSeparator; } } // Top-left dots of toolstrip grip
        public override Color GripLight { get { return colMenuBackground; } } // Bottom-right dots of toolstrip grip

        public override Color ImageMarginGradientBegin { get { return colButtonSelectBackLight; } } // MS: Left of dropdown margin; Mono: Unused
        public override Color ImageMarginGradientEnd { get { return colButtonSelectBackLight; } } // MS: Right of dropdown margin; Mono: Unused
        public override Color ImageMarginGradientMiddle { get { return colButtonSelectBackLight; } } // MS: Middle of dropdown margin; Mono: Unused

        public override Color StatusStripGradientBegin { get { return colToolStripBackGround; } } // Unused
        public override Color StatusStripGradientEnd { get { return colToolStripBackGround; } } // Unused

        public override Color MenuItemSelected { get { return colMenuSelectedBack; } } // Background of hovered menu or toolstrip dropdown item

        public override Color OverflowButtonGradientBegin { get { return colMenuSelectedBack; } } // Top of overflow button
        public override Color OverflowButtonGradientEnd { get { return colMenuSelectedBack; } } // Bottom of overflow button
        public override Color OverflowButtonGradientMiddle { get { return colMenuSelectedBack; } } // MS: Middle of overflow button; Mono: Unused
    }
}
