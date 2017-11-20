using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Collections;
using EliteDangerousCore;
    
namespace EDDiscovery.Icons
{
    public static class Icons
    {
        private static Dictionary<string, Image> icons;

        static Icons()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resnames = asm.GetManifestResourceNames();
            icons = new Dictionary<string, Image>();
            string basename = typeof(Icons).Namespace + ".";

            foreach (string resname in resnames)
            {
                if (resname.StartsWith(basename) && resname.EndsWith(".png"))
                {
                    string name = resname.Substring(basename.Length, resname.Length - basename.Length - 4);
                    icons[name] = Image.FromStream(asm.GetManifestResourceStream(resname));
                }
            }
        }

        public static Image GetIcon(string name)
        {
            return icons[name];
        }
    }

    public abstract class Icons<T> : IReadOnlyDictionary<T, Image>
    {
        protected Dictionary<T, Image> icons;

        protected void Init(string basedir, IEnumerable<T> keys)
        {
            icons = keys.ToDictionary(e => e, e => Icons.GetIcon(basedir + "." + e.ToString()));
        }

        public Image this[T key] => icons[key];
        public IEnumerable<T> Keys => icons.Keys;
        public IEnumerable<Image> Values => icons.Values;
        public int Count => icons.Count;
        public bool ContainsKey(T key) => icons.ContainsKey(key);
        public IEnumerator<KeyValuePair<T, Image>> GetEnumerator() => icons.GetEnumerator();
        public bool TryGetValue(T key, out Image value) => icons.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => icons.GetEnumerator();
    }

    public class PlanetIcons : Icons<EDPlanet>
    {
        public PlanetIcons()
        {
            Init("Planets", Enum.GetValues(typeof(EDPlanet)).OfType<EDPlanet>());
        }
    }

    public class StarIcons : Icons<EDStar>
    {
        public StarIcons()
        {
            Init("Stars", Enum.GetValues(typeof(EDStar)).OfType<EDStar>());
        }
    }

    public class JournalIcons : Icons<JournalTypeEnum>
    {
        public JournalIcons()
        {
            Init("Journal", Enum.GetValues(typeof(JournalTypeEnum)).OfType<JournalTypeEnum>());
        }
    }
}
