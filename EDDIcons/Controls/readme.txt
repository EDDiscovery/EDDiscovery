Adding icons is complicated

1. First make the icon
2. Make the folder in Controls - say SysInfo
3. Add your icon to that folder (lets call it firstdiscover.png)
4. Mark the icon as an embedded resource in properties

5. Open the resource file in Fakes to view the icons
6. Drag the icon from the folder into the resource view to add it.
7. Rename, in the resource editor, the resource entry so its not generic if required - add on a prefix

Controls.Designer.CS should have:

        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap firstdiscover {
            get {
                object obj = ResourceManager.GetObject("firstdiscover", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
Controls.resx should show the icon and have:
  <data name="firstdiscover" type="System.Resources.ResXFileRef, System.Windows.Forms">
    <value>..\sysinfo\firstdiscover.png;System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</value>
  </data>

7. Update controls.cs to add on this:

        #region EDDiscovery.UserControls.UserControlSysInfo
        public static Image firstdiscover { get { return IconSet.GetIcon("Controls.SysInfo.firstdiscover"); } }
        #endregion

Note the firstdiscovery name MUST MATCH incl case the resx name..  EVEN though it looks like it does need to,
it must, as the designer must be using some reflection to match the name with the resx file.
This is a trap for young players!

!! Beware of case in all of these - keep it consistent

8. Rebuild manually EDDICONS

9.You should be able to add the icon to your designer manually:

            this.panelFD.BackgroundImage = global::EDDiscovery.Icons.Controls.firstdiscover;

The designer should work - you may need to rebuild a few times..

Its not easy!

Rob



