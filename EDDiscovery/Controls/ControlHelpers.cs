using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedControls
{
    class ControlHelpers
    {
        public static Bitmap ReplaceColourInBitmap(Bitmap source, System.Drawing.Imaging.ColorMap[] remap)
        {
            Bitmap newmap = new Bitmap(source.Width, source.Height);

            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
            ia.SetRemapTable(remap, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            using (Graphics gr = Graphics.FromImage(newmap))
            {
                gr.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, ia);
            }

            return newmap;
        }

        public static void DrawTextCentreIntoBitmap(ref Bitmap img, string text, Font dp, Color c)
        {
            using (Graphics bgr = Graphics.FromImage(img))
            {
                SizeF sizef = bgr.MeasureString(text, dp);

                using (Brush textb = new SolidBrush(c))
                {
                    bgr.DrawString(text, dp, textb, img.Width/2 - (int)(sizef.Width / 2) , img.Height/2 - (int)(sizef.Height / 2));
                }
            }
        }

        public static Bitmap DrawTextIntoAutoSizedBitmap(string text, Font dp, Color c)
        {
            Bitmap t = new Bitmap(1, 1);

            using (Graphics bgr = Graphics.FromImage(t))
            {
                SizeF sizef = bgr.MeasureString(text, dp);

                Bitmap img = new Bitmap((int)(sizef.Width + 1), (int)(sizef.Height + 1));

                using (Graphics dgr = Graphics.FromImage(img))
                {
                    using (Brush textb = new SolidBrush(c))
                    {
                        dgr.DrawString(text, dp, textb, 0, 0);

                        return img;
                    }
                }
            }
        }
    }
}
