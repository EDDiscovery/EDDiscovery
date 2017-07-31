/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace EliteDangerousCore.CompanionAPI
{
    /// <summary>Storage of credentials for a single Elite: Dangerous user to access the Companion App</summary>
    public class CompanionCredentials
    {
        [JsonProperty("commander")]
        public string Commander { get; set; }
        [JsonProperty("emailadr")]
        public string EmailAdr { get; set; }
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }             // Means it was Confirmed ONCE.. server may require re-confirm..

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

        [JsonIgnore]
        public bool IsComplete
        {
            get
            {
                if (string.IsNullOrEmpty(EmailAdr) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(machineId) || string.IsNullOrEmpty(machineToken))
                    return false;
                else
                    return true;
            }
        }

        [JsonProperty("AppId")]
        public string appId { get; set; }
        [JsonProperty("machineid")]
        public string machineId { get; set; }
        [JsonProperty("machinetoken")]
        public string machineToken { get; set; }

        private RijndaelCrypt rijndaelCrypt = new RijndaelCrypt();

        private CompanionCredentials()
        { }

        public CompanionCredentials(string commander, string email, string pwd)
        {
            Commander = commander;
            EmailAdr = email;
            Password = pwd;
        }

        public static CompanionCredentials FromFile(string cmdrname=null)
        {
            string filename = Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "credentials" + cmdrname.SafeFileString() + ".json");

            try
            {
                if (File.Exists(filename))
                {
                    string credentialsData = File.ReadAllText(filename);
                    CompanionCredentials credentials = new CompanionCredentials();
                    credentials = JsonConvert.DeserializeObject<CompanionCredentials>(credentialsData);
                    return credentials;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to read companion credentials" + ex.Message);
            }

            return null;
        }

        public enum State
        {
            NO_CREDENTIALS,
            NEEDS_CONFIRMATION,
            CONFIRMED,
        };

        static public State CredentialState(string cmdrname)
        {
            CompanionCredentials c = CompanionCredentials.FromFile(cmdrname);
            if (c != null && c.IsComplete)
                return c.Confirmed ? State.CONFIRMED : State.NEEDS_CONFIRMATION;
            else
                return State.NO_CREDENTIALS;
        }

        static public void DeleteCredentials(string cmdrname)
        {
            CompanionCredentials c = CompanionCredentials.FromFile(cmdrname);
            if (c != null)
            {
                c.Clear();
                c.ToFile();
            }
        }

        /// <summary>
        /// Clear the information held by credentials.
        /// </summary>
        public void Clear()
        {
            Password = null;
            appId = null;
            machineId = null;
            machineToken = null;
            Confirmed = false;
        }

        public void ToFile()
        {
            string filename = Path.Combine(EliteDangerousCore.EliteConfigInstance.InstanceOptions.AppDataDirectory, "credentials" + Commander.SafeFileString() + ".json");
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filename, json);
        }

        public void SetConfirmed()
        {
            if (!Confirmed)
            {
                System.Diagnostics.Debug.WriteLine("Credentials marked confirmed");
                Confirmed = true;
                ToFile();
            }
        }

        public void SetNeedsConfirmation()
        {
            if (Confirmed)
            {
                System.Diagnostics.Debug.WriteLine("Credentials need confirm");
                Confirmed = false;
                ToFile();
            }
        }
    }
}
