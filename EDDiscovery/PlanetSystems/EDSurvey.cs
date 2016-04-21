using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{

    public class EDSurvey : EDObject
    {
        public int worldId;
        public int basecampId; // Basecamps are optional now. Surveys now are anchored to worlds.
        public string commander; // Only the original commander can update a survey. Unless it's an error flagging thing
        public string resource;
        //public string notes;
        public string imageUrl;
        public List<string> surveyedBy;
        public DateTime surveyedAt;

        // If any of these 3 attributes are submitted the request will
        // be rejected if other attributes are received. Unless it's from the original
        // commander
        public bool errorFlag;
        public string errorDescription;
        public string errorUpdater;

        // Not sure how materials are going to be stored, but they're integer for
        // this table. But if the resource type is "BINARY" it can be 1s and 0s. As per
        // the Log sheet of the Monster spreadsheet.
        //
        // eg:
        // public int carbon;

        public EDSurvey()
        {
        }

        public bool ParseJson(JObject jo)
        {

            id = jo["id"].Value<int>();
            var attributes = jo["attributes"];
            worldId = attributes["world-id"].Value<int>();
            commander = attributes["commander"].Value<string>();
            resource = attributes["resource"].Value<string>();
            notes = attributes["notes"].Value<string>();
            imageUrl = attributes["image-url"].Value<string>();
            // surveyedBy is an array of strings. Leaving it for now;
            // surveyedAt is a date. Leaving it for now;

            //TODO: Not quite sure how to work with materials object in their current state, 
            //      but something like this should do it - Greg
            
            //foreach (var mat in mlist)
            //{
            //    materials[mat.material] = attributes[mat.Name.ToLower()].Value<int>();
            //}
            return true;
        }

    }
}
    