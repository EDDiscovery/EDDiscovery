using EDDiscovery.CompanionAPI;
using EDDiscovery.EliteDangerous;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class FormCAPI : Form
    {
        private string profileJson;

        CompanionAPIClass capi = new CompanionAPIClass();

        public FormCAPI()
        {
            InitializeComponent();
            buttonStoredLogin.Enabled = false;

            if (CompanionCredentials.CredentialState(EDCommander.Current.Name) == CompanionCredentials.State.NEEDS_CONFIRMATION)
                setUpConfirm();
            else if (CompanionCredentials.CredentialState(EDCommander.Current.Name) == CompanionCredentials.State.CONFIRMED)
            {
                setUpLoggedIn("Credentials ready");
                buttonStoredLogin.Enabled = true;
                buttonLogin.Enabled = false;
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (buttonLogin.Text.Equals("Login"))
            {
                Login();
                return;
            }
            if (buttonLogin.Text.Equals("Confirm"))
            {
                Confirm();
                return;
            }
            if (buttonLogin.Text.Equals("Logout"))
            {
                // Logged in - handle logout
                capi.Logout();
                setUpLogin();
            }
        }

        private void Login()
        {
            try
            {
                if (capi.NeedLogin)
                {
                    capi.LoginAs(EDCommander.Current.Name, textBoxMail.Text.Trim(), textBoxPassword.Text.Trim());
                }

                if (capi.NeedConfirmation)
                {
                    setUpConfirm();
                }
            }
            catch (CompanionAppAuthenticationException ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
            catch (CompanionAppErrorException ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("Unexpected problem" + ex);
            }
        }


        private void Confirm()
        {
            // Stage 2 of authentication - confirmation
            string code = textBoxConfirmationCode.Text.Trim();
            try
            {
                capi.Confirm(code);
                setUpLoggedIn("Login OK after confirm");
            }
            catch (CompanionAppAuthenticationException ex)
            {
                richTextBox1.AppendText(ex.Message);
                setUpLogin();
            }
            catch (CompanionAppErrorException ex)
            {
                richTextBox1.AppendText(ex.Message);
                setUpLogin();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
                setUpLogin();
            }
        }
    

        private void setUpLogin()
        {
            textBoxMail.Enabled = true;
            textBoxPassword.Enabled = true; 
            textBoxConfirmationCode.Enabled = false;
            buttonLogin.Text = "Login";
            buttonLogin.Enabled = true;

        }
        private void setUpConfirm()
        {
            textBoxMail.Enabled = false;
            textBoxPassword.Enabled = false;
            textBoxConfirmationCode.Enabled = true;
            buttonLogin.Text = "Confirm";
            buttonLogin.Enabled = true;
        }

        private void setUpLoggedIn(string message = null)
        {
            if (message == null)
            {
                richTextBox1.AppendText("Login OK");
            }
            else
            {
                richTextBox1.AppendText(message);
            }

            textBoxMail.Enabled = true;
            textBoxPassword.Enabled = true;
            textBoxConfirmationCode.Enabled = false;
            buttonLogin.Text = "Logout";
            buttonLogin.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CompanionCredentials.DeleteCredentials(EDCommander.Current.Name);
            capi.Logout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                capi.LoginAs(EDCommander.Current.Name);
                setUpLoggedIn("Logged in");
                buttonStoredLogin.Enabled = false;
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
                setUpLogin();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            profileJson = capi.GetProfileString();

            if (profileJson == null)
            {
                setUpLoggedIn("Login OK. No user profile available");
            }
            else
            {
                richTextBox1.AppendText(profileJson);
                //setShipyardFromConfiguration();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            profileJson = capi.GetProfileString();

            if (profileJson == null)
            {
                setUpLoggedIn("Login OK. No user profile available");
            }
            else
            {
                CProfile profile = new CProfile(profileJson);

                EDDN.EDDNClass eddn = new EDDN.EDDNClass();

                eddn.commanderName = capi.Credentials.Commander;
                JObject msg = eddn.CreateEDDNCommodityMessage(profile.StarPort.commodities, profile.CurrentStarSystem.name, profile.StarPort.name, DateTime.UtcNow );

                if (msg != null)
                {
                    if (eddn.PostMessage(msg))
                    {
                    }
                }
            }

            richTextBox1.AppendText(profileJson);
            
            
        }
    }
}
