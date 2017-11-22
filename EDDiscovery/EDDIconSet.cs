using EDDiscovery.Icons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using System.Windows.Forms;
using ExtendedControls;
using ExtendedControls.Controls;

namespace EDDiscovery
{
    public class EDDIconSet : IEliteIconSet
    {
        public class IconGroup<T> : IReadOnlyDictionary<T, Image>
        {
            protected Dictionary<T, Image> icons;

            public IconGroup(string basedir, Func<string, Image> geticon)
            {
                Init(basedir, Enum.GetValues(typeof(T)).OfType<T>(), geticon);
            }

            protected void Init(string basedir, IEnumerable<T> keys, Func<string, Image> geticon)
            {
                icons = keys.ToDictionary(e => e, e => geticon(basedir + "." + e.ToString()));
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

        public class IconReplacer
        {
            public string BaseName { get; private set; }
            protected Func<string, Image> GetImage { get; set; }

            public IconReplacer(IIconPackControl control, Func<string, Image> getimage)
            {
                BaseName = control.BaseName;
                GetImage = getimage;
            }

            public void ReplaceImage(Action<Image> setter, string name)
            {
                Image newimg = GetImage("Controls." + BaseName + "." + name);
                if (newimg != null)
                {
                    setter(newimg);
                }
            }
        }

        private static EDDIconSet _instance;
        private Func<string, Image> getIcon;

        private EDDIconSet()
        {
            StarTypeIcons = new IconGroup<EDStar>("Stars", this.GetIcon);
            PlanetTypeIcons = new IconGroup<EDPlanet>("Planets", this.GetIcon);
            JournalTypeIcons = new IconGroup<JournalTypeEnum>("Journal", this.GetIcon);
            GalMapTypeIcons = new IconGroup<GalMapTypeEnum>("GalMap", this.GetIcon);
            PanelTypeIcons = new IconGroup<PanelInformation.PanelIDs>("Panels", this.GetIcon);
        }

        public static EDDIconSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDIconSet();
                    EliteConfigInstance.InstanceIconSet = _instance;
                }

                return _instance;
            }
            set
            {
                _instance = value;
                EliteConfigInstance.InstanceIconSet = value;
            }
        }

        public static void Init(Func<string, Image> geticon)
        {
            EliteConfigInstance.InstanceIconSet = Instance;
        }

        public IReadOnlyDictionary<PanelInformation.PanelIDs, Image> PanelTypeIcons { get; private set; }
        public IReadOnlyDictionary<EDStar, Image> StarTypeIcons { get; private set; }
        public IReadOnlyDictionary<EDPlanet, Image> PlanetTypeIcons { get; private set; }
        public IReadOnlyDictionary<JournalTypeEnum, Image> JournalTypeIcons { get; private set; }
        public IReadOnlyDictionary<GalMapTypeEnum, Image> GalMapTypeIcons { get; private set; }

        public Image GetIcon(string name)
        {
            if (!name.Contains("."))
            {
                name = "Legacy." + name;
            }

            return getIcon(name);
        }

        public void ReplaceIcons(Control control)
        {
            if (control is IIconPackControl)
            {
                IIconPackControl ctrl = ((IIconPackControl)control);
                IconReplacer replacer = new IconReplacer(ctrl, GetIcon);
                ctrl.ReplaceImages(replacer.ReplaceImage);
            }

            if (control is TabStrip)
            {
                TabStrip ts = (TabStrip)control;
                ts.ImageList = PanelInformation.GetPanelImages();
            }

            if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ReplaceIcons(child);
                }
            }
        }
    }
}
