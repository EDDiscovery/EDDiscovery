using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public enum MaterialEnum
    {
        Unknown = 0,
        Carbon,
        Iron,
        Nickel,
        Phosphorus,
        Sulphur,
        Arsenic,
        Chromium,
        Germanium,
        Manganese,
        Selenium,
        Vanadium,
        Zinc,
        Zirconium,
        Cadmium,
        Mercury,
        Molybdenum,
        Niobium,
        Tin,
        Tungsten,
        Antimony,
        Polonium,
        Ruthenium,
        Technetium,
        Tellurium,
        Yttrium,
    }

    public enum MaterialRarityEnum
    {
        VeryCommon = 1,
        Common,
        Rare,
        VeryRare,
    }


    public enum ProspectNodeTypeEnum
    {
        Unknown = 0,
        Mesosiderite,
        BronziteChondrite,
        MetallicMeteorite,
        OutcropGeiger,
        OutcropMetallic,
    }

   


    public class Material
    {
        public MaterialEnum material;
        public int number;


        public Material(MaterialEnum m)
        {
            material = m;
            number = 1;
        }
        public Material(MaterialEnum m, int nr)
        {
            material = m;
            number = nr;
        }

        public string Name
        {
            get
            {
                return material.ToString();
            }
        }

        public string ShortName
        {
            get
            {
                switch (material)
                {
                    case MaterialEnum.Carbon:
                        return "C";
                    case MaterialEnum.Iron:
                        return "Fe";
                    case MaterialEnum.Nickel:
                        return "Ni";
                    case MaterialEnum.Phosphorus:
                        return "P";
                    case MaterialEnum.Sulphur:
                        return "S";
                    case MaterialEnum.Arsenic:
                        return "As";
                    case MaterialEnum.Chromium:
                        return "Cr";
                    case MaterialEnum.Germanium:
                        return "Ge";
                    case MaterialEnum.Manganese:
                        return "Mn";
                    case MaterialEnum.Selenium:
                        return "Se";
                    case MaterialEnum.Vanadium:
                        return "V";
                    case MaterialEnum.Zinc:
                        return "Zn";
                    case MaterialEnum.Zirconium:
                        return "Zr";
                    case MaterialEnum.Cadmium:
                        return "Cd";
                    case MaterialEnum.Mercury:
                        return "Hg";
                    case MaterialEnum.Molybdenum:
                        return "Mo";
                    case MaterialEnum.Niobium:
                        return "Nb";
                    case MaterialEnum.Tin:
                        return "Sn";
                    case MaterialEnum.Tungsten:
                        return "W";
                    case MaterialEnum.Antimony:
                        return "Sb";
                    case MaterialEnum.Polonium:
                        return "Po";
                    case MaterialEnum.Ruthenium:
                        return "Ru";
                    case MaterialEnum.Technetium:
                        return "Tc";
                    case MaterialEnum.Tellurium:
                        return "Te";
                    case MaterialEnum.Yttrium:
                        return "Y";

                    default:
                        return "?";  // Should never happend
                }
            }
        }


        static public List<Material> GetMaterialList
        {
            get
            {
                List<Material> mlist = new List<Material>();
                foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
                {
                    if (mat != MaterialEnum.Unknown)
                        mlist.Add(new Material(mat));
                }
                return mlist;
            }
        }

        public MaterialRarityEnum Rarity
        {
            get
            {
                switch (material)
                {
                    case MaterialEnum.Carbon:
                    case MaterialEnum.Iron:
                    case MaterialEnum.Nickel:
                    case MaterialEnum.Phosphorus:
                    case MaterialEnum.Sulphur:
                        return MaterialRarityEnum.VeryCommon;

                    case MaterialEnum.Arsenic:
                    case MaterialEnum.Chromium:
                    case MaterialEnum.Germanium:
                    case MaterialEnum.Manganese:
                    case MaterialEnum.Selenium:
                    case MaterialEnum.Vanadium:
                    case MaterialEnum.Zinc:
                    case MaterialEnum.Zirconium:
                        return MaterialRarityEnum.Common;

                    case MaterialEnum.Cadmium:
                    case MaterialEnum.Mercury:
                    case MaterialEnum.Molybdenum:
                    case MaterialEnum.Niobium:
                    case MaterialEnum.Tin:
                    case MaterialEnum.Tungsten:
                        return MaterialRarityEnum.Rare;

                    case MaterialEnum.Antimony:
                    case MaterialEnum.Polonium:
                    case MaterialEnum.Ruthenium:
                    case MaterialEnum.Technetium:
                    case MaterialEnum.Tellurium:
                    case MaterialEnum.Yttrium:
                        return MaterialRarityEnum.VeryRare;

                    default:
                        return MaterialRarityEnum.Common;  // Should never happend
                }
            }

        }


        public Color RareityColor
        {
            get
            {
                switch (material)
                {
                    case MaterialEnum.Carbon:
                    case MaterialEnum.Iron:
                    case MaterialEnum.Nickel:
                    case MaterialEnum.Phosphorus:
                    case MaterialEnum.Sulphur:
                        return Color.LightBlue;

                    case MaterialEnum.Arsenic:
                    case MaterialEnum.Chromium:
                    case MaterialEnum.Germanium:
                    case MaterialEnum.Manganese:
                    case MaterialEnum.Selenium:
                    case MaterialEnum.Vanadium:
                    case MaterialEnum.Zinc:
                    case MaterialEnum.Zirconium:
                        return Color.LightGreen;

                    case MaterialEnum.Cadmium:
                    case MaterialEnum.Mercury:
                    case MaterialEnum.Molybdenum:
                    case MaterialEnum.Niobium:
                    case MaterialEnum.Tin:
                    case MaterialEnum.Tungsten:
                        return Color.LightGoldenrodYellow;

                    case MaterialEnum.Antimony:
                    case MaterialEnum.Polonium:
                    case MaterialEnum.Ruthenium:
                    case MaterialEnum.Technetium:
                    case MaterialEnum.Tellurium:
                    case MaterialEnum.Yttrium:
                        return Color.Salmon;

                    default:
                        return Color.White;  // Should never happend
                }
            }

        }

    }

    //public class Synthesis
    //{
    //    public string name;
    //    List<Material> materials;

    //}
}
