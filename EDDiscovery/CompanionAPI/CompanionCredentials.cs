using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace EDDiscovery.CompanionAPI
{
    /// <summary>Storage of credentials for a single Elite: Dangerous user to access the Companion App</summary>
    public class CompanionCredentials
    {
        [JsonProperty("emailadr")]
        public string EmailAdr { get; set; }
        [JsonProperty("password")]
        private string encPassword;

        [JsonIgnore]
        public string Password {
            get
            {
                return encPassword == null ? null : rijndaelCrypt.Decrypt(encPassword);
            }
            set
            {
                if (value == null)
                    value = "";
                encPassword = rijndaelCrypt.Encrypt(value);
            }
        }

        [JsonProperty("AppId")]
        public string appId { get; set; }
        [JsonProperty("machineid")]
        public string machineId { get; set; }
        [JsonProperty("machinetoken")]
        public string machineToken { get; set; }


        private RijndaelCrypt rijndaelCrypt = new RijndaelCrypt();

        public static CompanionCredentials FromFile(string filename=null)
        {
            if (filename == null)
            {
                filename = Path.Combine(Tools.GetAppDataDirectory(), "credentials.json");
            }

            CompanionCredentials credentials = new CompanionCredentials();
            try
            {
                string credentialsData = File.ReadAllText(filename);
                credentials = JsonConvert.DeserializeObject<CompanionCredentials>(credentialsData);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to read companion credentials" + ex.Message);
            }

            return credentials;
        }

        /// <summary>
        /// Clear the information held by credentials.
        /// </summary>
        public void Clear()
        {
            appId = null;
            machineId = null;
            machineToken = null;
        }


        public void ToFile()
        {
            string filename = Path.Combine(Tools.GetAppDataDirectory(), "credentials.json");

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filename, json);
        }
    }
}
