using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    // Relationships:
    //   A Survey belongs to a single World
    //   A Survey can optionally have a basecamp
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
            //Reminder - JSONAPI attributes and relationships structure

            id = jo["id"].Value<int>();

            var attributes = jo["attributes"];
            commander = attributes["commander"].Value<string>();
            resource = attributes["resource"].Value<string>();
            notes = attributes["notes"].Value<string>();
            imageUrl = attributes["image-url"].Value<string>();

            var relationships = jo["relationships"];
            var world = relationships["world"];
            worldId = world["id"].Value<int>();

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


        // Obtain a world object using the world-id
        public EDWorld GetWorld()
        {
            if (worldId > 0)
            {
                Repositories.World worldRepo = new Repositories.World();
                return worldRepo.GetForId(worldId);
            }
            else
            {
                return null;
            }
        }

        // Go to the associated basecamp if there is one
        public EDBasecamp GetBasecamp()
        {
            if (basecampId > 0)
            {
                Repositories.Basecamp basecampRepo = new Repositories.Basecamp();
                return basecampRepo.GetForId(basecampId);
            }
            else
            {
                return null;
            }
        }


    }
}
    