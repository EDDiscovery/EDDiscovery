using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public class EDMaterials
    {
        public Dictionary<MaterialEnum, int> materials;
        static private List<Material> mlist = Material.GetMaterialList;

        public string System;
        public string Name;



        public EDMaterials()
        {
            materials = new Dictionary<MaterialEnum, int>();
            // Create an empty dictionary
            foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
            {
                if (mat != MaterialEnum.Unknown)
                    materials[mat] = 0;
            }

        }


        public void Add(EDMaterials m2)
        {
            foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
            {
                if (mat != MaterialEnum.Unknown)
                    materials[mat] += m2.materials[mat];
            }
        }



            //lvi.UseItemStyleForSubItems = false;


            //            for (int ii = 0; ii<mlist.Count; ii++)
            //            {
            //                ListViewItem.ListViewSubItem lvsi;
            //                if (planet.materials[mlist[ii].material])
            //                    lvsi = lvi.SubItems.Add("X");
            //                else
            //                    lvsi = lvi.SubItems.Add(" ");

            //                lvsi.BackColor = mlist[ii].RareityColor;
            //            }


            //private void SetMaterials(EDWorld obj, CheckedListBox box)
            //{
            //    for (int i = 0; i < box.Items.Count; i++)
            //    {
            //        string item = (string)box.Items[i];
            //        MaterialEnum mat = obj.MaterialFromString(item);
            //        box.SetItemChecked(i, obj.materials[mat]);
            //    }
            //}

            //private void GetMaterials(ref EDWorld obj, CheckedListBox box)
            //{
            //    for (int i = 0; i < box.Items.Count; i++)
            //    {
            //        string item = (string)box.Items[i];
            //        MaterialEnum mat = obj.MaterialFromString(item);
            //        obj.materials[mat] = box.GetItemChecked(i);
            //    }
            //}


        }
}
