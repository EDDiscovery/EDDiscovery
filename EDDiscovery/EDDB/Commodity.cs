using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.EDDB
{
    public class Commodity
    {
        public readonly  int id;
        public readonly string name;
        public readonly int category_id;
        public readonly int average_price;
        public readonly bool is_rare;

        public Commodity(int id, string name, int cat_id, int avg_price, bool rare)
        {
            this.id = id;
            this.name = name;
            category_id = cat_id;
            average_price = avg_price;
            is_rare = rare;
        }

        public Commodity(JObject jo)
        {
                name = jo["name"].Value<string>();

                id = jo["id"].Value<int>();
            category_id = jo["category_id"].Value<int>();


            average_price = jo["average_price"].Value<int>();

            int v = jo["is_rare"].Value<int>();

            if (v == 0)
                is_rare = false;
            else
                is_rare = true;
            
            }

        }
}
