using EDDiscovery.HTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace EDDiscovery.CompanionAPI
{
    public class CompanionAPIClass
    {
        private static string BASE_URL = "https://companion.orerve.net";
        private static string ROOT_URL = "/";
        private static string LOGIN_URL = "/user/login";
        private static string CONFIRM_URL = "/user/confirm";
        private static string PROFILE_URL = "/profile";
        
        public enum State
        {
            NEEDS_LOGIN,
            NEEDS_CONFIRMATION,
            READY
        };

        public State CurrentState;
        public CompanionCredentials Credentials;
        
        private static CompanionAPIClass instance;
        private static readonly object instanceLock = new object();
        public static CompanionAPIClass Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            Trace.WriteLine("No companion API instance: creating one");
                            instance = new CompanionAPIClass();
                        }
                    }
                }
                return instance;
            }
        }

        private CompanionAPIClass()
        {
            CurrentState = State.NEEDS_LOGIN;
        }

        #region Login and Confirmation

        static public State CommanderCredentialsState( string cmdrname )
        {
            CompanionCredentials c = CompanionCredentials.FromFile(cmdrname);
            if (c != null)
                return c.Confirmed ? State.READY : State.NEEDS_CONFIRMATION;
            else
                return State.NEEDS_LOGIN;
        }

        /// <summary>
        /// Log out and remove local credentials
        /// </summary>
        public void Logout()
        {
            CurrentState = State.NEEDS_LOGIN;
        }

        public void RemoveCredentials()
        {
            if (Credentials != null)
            {
                Credentials.Clear();
                Credentials.ToFile();
                CurrentState = State.NEEDS_LOGIN;
                Credentials = null;
            }
        }

        ///<summary>Log in.  Throws an exception if it fails</summary>

        public void LoginAs(string cmdrname)                          // login with previous credientials stored
        {
            if (CurrentState == State.NEEDS_LOGIN)
            {
                Credentials = CompanionCredentials.FromFile(cmdrname);      // if file name missing, will not be complete for login

                if (Credentials != null )
                {
                    if (Credentials.Confirmed)                          // if we have confirmed..
                        CurrentState = State.READY;
                    else
                        CurrentState = State.NEEDS_CONFIRMATION;
                }
                else
                    throw new CompanionAppException("No stored credentials");
            }
            else
                throw new CompanionAppIllegalStateException("Service in incorrect state to login (" + CurrentState + ")");
        }

        public void LoginAs(string cmdrname, string emailadr, string password)
        {
            if (CurrentState == State.NEEDS_LOGIN)
            {
                Credentials = new CompanionCredentials(cmdrname, emailadr, password);         // NOW ready for LOGIN
                Login();
            }
            else
                throw new CompanionAppIllegalStateException("Service in incorrect state to login (" + CurrentState + ")");
        }

        private void Login()
        {
            HttpWebRequest request = GetRequest(BASE_URL + LOGIN_URL);

            // Send the request
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            string encodedUsername = WebUtility.UrlEncode(Credentials.EmailAdr);
            string encodedPassword = WebUtility.UrlEncode(Credentials.Password);
            byte[] data = Encoding.UTF8.GetBytes("email=" + encodedUsername + "&password=" + encodedPassword);
            request.ContentLength = data.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = GetResponse(request))
            {
                if (response == null)
                {
                    throw new CompanionAppException("Failed to contact API server");
                }
                if (response.StatusCode == HttpStatusCode.Found && response.Headers["Location"] == CONFIRM_URL)
                {
                    CurrentState = State.NEEDS_CONFIRMATION;
                    Credentials.SetNeedsConfirmation();
                }
                else if (response.StatusCode == HttpStatusCode.Found && response.Headers["Location"] == ROOT_URL)
                {
                    CurrentState = State.READY;
                    Credentials.SetConfirmed(); // ensure its marked this way
                }
                else
                {
                    throw new CompanionAppAuthenticationException("Username or password incorrect");
                }
            }
        }

        ///<summary>Confirm a login.  Throws an exception if it fails</summary>
        public void Confirm(string code)
        {
            if (CurrentState != State.NEEDS_CONFIRMATION)
            {
                // Shouldn't be here
                throw new CompanionAppIllegalStateException("Service in incorrect state to confirm login (" + CurrentState + ")");
            }

            HttpWebRequest request = GetRequest(BASE_URL + CONFIRM_URL);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            string encodedCode = WebUtility.UrlEncode(code);
            byte[] data = Encoding.UTF8.GetBytes("code=" + encodedCode);
            request.ContentLength = data.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = GetResponse(request))
            {
                if (response == null)
                {
                    throw new CompanionAppException("Failed to contact API server");
                }

                if (response.StatusCode == HttpStatusCode.Found && response.Headers["Location"] == ROOT_URL)
                {
                    CurrentState = State.READY;
                }
                else if (response.StatusCode == HttpStatusCode.Found && response.Headers["Location"] == LOGIN_URL)
                {
                    CurrentState = State.NEEDS_LOGIN;
                    throw new CompanionAppAuthenticationException("Confirmation code incorrect or expired");
                }
            }
        }

        // Set up a request with the correct parameters for talking to the companion app
        private HttpWebRequest GetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            CookieContainer cookieContainer = new CookieContainer();
            AddCompanionAppCookie(cookieContainer, Credentials);
            AddMachineIdCookie(cookieContainer, Credentials);
            AddMachineTokenCookie(cookieContainer, Credentials);
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = false;
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;
            request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Mobile/11D257";

            return request;
        }

        // Obtain a response, ensuring that we obtain the response's cookies
        private HttpWebResponse GetResponse(HttpWebRequest request)
        {
            HttpCom.WriteLog("Companion Requesting ", request.RequestUri.ToNullSafeString());
            //Trace.WriteLine("Requesting " + request.RequestUri);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                Trace.WriteLine("Failed to obtain response, error code " + wex.Status);
                return null;
            }
            HttpCom.WriteLog("Companion Response ", JsonConvert.SerializeObject(response));
            //Trace.WriteLine("Response is " + JsonConvert.SerializeObject(response));

            UpdateCredentials(response);
            Credentials.ToFile();

            return response;
        }

        private void UpdateCredentials(HttpWebResponse response)
        {
            // Obtain the cookies from the raw information available to us
            string cookieHeader = response.Headers[HttpResponseHeader.SetCookie];
            if (cookieHeader != null)
            {
                Match companionAppMatch = Regex.Match(cookieHeader, @"CompanionApp=([^;]+)");
                if (companionAppMatch.Success)
                {
                    Credentials.appId = companionAppMatch.Groups[1].Value;
                }
                Match machineIdMatch = Regex.Match(cookieHeader, @"mid=([^;]+)");
                if (machineIdMatch.Success)
                {
                    Credentials.machineId = machineIdMatch.Groups[1].Value;
                }
                Match machineTokenMatch = Regex.Match(cookieHeader, @"mtk=([^;]+)");
                if (machineTokenMatch.Success)
                {
                    Credentials.machineToken = machineTokenMatch.Groups[1].Value;
                }
            }
        }


        private static void AddCompanionAppCookie(CookieContainer cookies, CompanionCredentials credentials)
        {
            if (cookies != null && credentials.appId != null)
            {
                var appCookie = new Cookie();
                appCookie.Domain = "companion.orerve.net";
                appCookie.Path = "/";
                appCookie.Name = "CompanionApp";
                appCookie.Value = credentials.appId;
                appCookie.Secure = false;
                cookies.Add(appCookie);
            }
        }

        private static void AddMachineIdCookie(CookieContainer cookies, CompanionCredentials credentials)
        {
            if (cookies != null && credentials.machineId != null)
            {
                var machineIdCookie = new Cookie();
                machineIdCookie.Domain = "companion.orerve.net";
                machineIdCookie.Path = "/";
                machineIdCookie.Name = "mid";
                machineIdCookie.Value = credentials.machineId;
                machineIdCookie.Secure = true;
                // The expiry is embedded in the cookie value
                if (credentials.machineId.IndexOf("%7C") == -1)
                {
                    machineIdCookie.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    string expiryseconds = credentials.machineId.Substring(0, credentials.machineId.IndexOf("%7C"));
                    DateTime expiryDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    try
                    {
                        expiryDateTime = expiryDateTime.AddSeconds(Convert.ToInt64(expiryseconds));
                        machineIdCookie.Expires = expiryDateTime;
                    }
                    catch (Exception)
                    {
                        Trace.WriteLine("Failed to handle machine id expiry seconds " + expiryseconds);
                        machineIdCookie.Expires = DateTime.Now.AddDays(7);
                    }
                }
                cookies.Add(machineIdCookie);
            }
        }

        private static void AddMachineTokenCookie(CookieContainer cookies, CompanionCredentials credentials)
        {
            if (cookies != null && credentials.machineToken != null)
            {
                var machineTokenCookie = new Cookie();
                machineTokenCookie.Domain = "companion.orerve.net";
                machineTokenCookie.Path = "/";
                machineTokenCookie.Name = "mtk";
                machineTokenCookie.Value = credentials.machineToken;
                machineTokenCookie.Secure = true;
                // The expiry is embedded in the cookie value
                if (credentials.machineToken.IndexOf("%7C") == -1)
                {
                    machineTokenCookie.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    string expiryseconds = credentials.machineToken.Substring(0, credentials.machineToken.IndexOf("%7C"));
                    DateTime expiryDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    try
                    {
                        expiryDateTime = expiryDateTime.AddSeconds(Convert.ToInt64(expiryseconds));
                        machineTokenCookie.Expires = expiryDateTime;
                    }
                    catch (Exception)
                    {
                        Trace.WriteLine("Failed to handle machine token expiry seconds " + expiryseconds);
                        machineTokenCookie.Expires = DateTime.Now.AddDays(7);
                    }
                }
                cookies.Add(machineTokenCookie);
            }
        }

        #endregion

        #region Profile
        // above does not rely on this bit..really.

        // We cache the profile to avoid spamming the service
        public CProfile profile;
        private string cachedJsonProfile;
        private DateTime cachedProfileExpires;
        
        public string GetProfileString(bool forceRefresh = false)
        {
            
            if (CurrentState != State.READY)
            {
                // Shouldn't be here
                Trace.WriteLine("Service in incorrect state to provide profile (" + CurrentState + ")");
                
                throw new CompanionAppIllegalStateException("Service in incorrect state to provide profile (" + CurrentState + ")");
            }
            if ((!forceRefresh) && cachedProfileExpires > DateTime.Now)
            {
                // return the cached version
                Trace.WriteLine("Returning cached profile");
                
                return cachedJsonProfile;
            }

            string data = DownloadProfile();

            if (data == null || data == "Profile unavailable")
            {
                // Happens if there is a problem with the API.  Logging in again might clear this...
                relogin();
                data = DownloadProfile();
                if (data == null || data == "Profile unavailable")
                {
                    // No luck with a relogin; give up
                    //SpeechService.Instance.Say(null, "Access to companion API data has been lost.  Please update the companion app information to re-establish the connection.", false);
                    Logout();
                    throw new CompanionAppException("Failed to obtain data from Frontier server (" + CurrentState + ")");
                }
            }

            cachedJsonProfile = data;

            if (cachedJsonProfile != null)
            {
                cachedProfileExpires = DateTime.Now.AddSeconds(30);
                Trace.WriteLine("Profile: " + cachedJsonProfile);
            }


            CreateProfile(cachedJsonProfile);

            return cachedJsonProfile;
        }

        private void CreateProfile(string json)
        {
            if (json != null)
            {
                JObject jo = JObject.Parse(json);

                profile = new CProfile(jo);
            }
        }

        private string DownloadProfile()
        {
            HttpWebRequest request = GetRequest(BASE_URL + PROFILE_URL);
            using (HttpWebResponse response = GetResponse(request))
            {
                if (response == null)
                {
                    Trace.WriteLine("Failed to contact API server");
                    
                    throw new CompanionAppException("Failed to contact API server");
                }

                if (response.StatusCode == HttpStatusCode.Found && response.Headers["Location"] == LOGIN_URL)
                {
                    return null;
                }

                return getResponseData(response);
            }
        }

        /**
         * Try to relogin if there is some issue that requires it.
         * Throws an exception if it failed to log in.
         */
        private void relogin()
        {
            // Need to log in again.
            CurrentState = State.NEEDS_LOGIN;
            Login();
            if (CurrentState != State.READY)
            {
                Trace.WriteLine("Service in incorrect state to provide profile (" + CurrentState + ")");
                
                throw new CompanionAppIllegalStateException("Service in incorrect state to provide profile (" + CurrentState + ")");
            }
        }

        /**
         * Obtain the response data from an HTTP web response
         */
        private string getResponseData(HttpWebResponse response)
        {
            // Obtain and parse our response
            var encoding = response.CharacterSet == ""
                    ? Encoding.UTF8
                    : Encoding.GetEncoding(response.CharacterSet);

            Trace.WriteLine("Reading response");
            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, encoding);
                string data = reader.ReadToEnd();
                if (data == null || data.Trim() == "")
                {
                    HttpCom.WriteLog("Companion No data returned", "");
                    return null;
                }
                HttpCom.WriteLog("Companion Data is ", data);
                return data;
            }
        }

        #endregion
    }
}
