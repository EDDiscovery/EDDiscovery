using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Collections;
using System.IO.Compression;

namespace EDDiscovery.Icons
{
    public static class IconSet
    {
        private static Dictionary<string, Image> defaultIcons;
        private static Dictionary<string, Image> icons;

        static IconSet()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resnames = asm.GetManifestResourceNames();
            defaultIcons = new Dictionary<string, Image>(StringComparer.InvariantCultureIgnoreCase);
            string basename = typeof(IconSet).Namespace + ".";

            foreach (string resname in resnames)
            {
                if (resname.StartsWith(basename) && resname.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = resname.Substring(basename.Length, resname.Length - basename.Length - 4);
                    defaultIcons[name] = Image.FromStream(asm.GetManifestResourceStream(resname));
                }
            }

            ResetIcons();
        }

        public static void ResetIcons()
        {
            icons = defaultIcons.ToArray().ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.InvariantCultureIgnoreCase);
        }

        public static void LoadIconsFromDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.png", SearchOption.AllDirectories))
                {
                    string name = file.Substring(path.Length + 1).Replace('/', '.').Replace('\\', '.');
                    Image img = null;

                    try
                    {
                        img = Image.FromFile(file);
                    }
                    catch
                    {
                        // Ignore any bad images
                        continue;
                    }

                    icons[name] = img;
                }
            }
        }

        public static void LoadIconsFromZipFile(string path)
        {
            if (File.Exists(path))
            {
                using (var zipfile = ZipFile.Open(path, ZipArchiveMode.Read))
                {
                    foreach (var entry in zipfile.Entries)
                    {
                        if (entry.FullName.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string name = entry.FullName.Substring(0, entry.FullName.Length - 4).Replace('/', '.').Replace('\\', '.');
                            Image img = null;

                            try
                            {
                                using (var zipstrm = entry.Open())
                                {
                                    var memstrm = new MemoryStream(); // Image will own this
                                    zipstrm.CopyTo(memstrm);
                                    img = Image.FromStream(memstrm);
                                }
                            }
                            catch
                            {
                                // Ignore any bad images
                                continue;
                            }

                            icons[name] = img;
                        }
                    }
                }
            }
        }

        public static Image GetIcon(string name)
        {
            return icons.ContainsKey(name) ? icons[name] : (defaultIcons.ContainsKey(name) ? defaultIcons[name] : null);
        }
    }
}
