using EDDiscovery.CompanionAPI;
using EDDiscovery.EliteDangerous;
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
        public FormCAPI()
        {
            InitializeComponent();
            buttonStoredLogin.Enabled = false;

            if (CompanionAPIClass.CommanderCredentialsState(EDCommander.Current.Name) == CompanionAPIClass.State.NEEDS_CONFIRMATION)
                setUpConfirm();
            else if (CompanionAPIClass.CommanderCredentialsState(EDCommander.Current.Name) == CompanionAPIClass.State.READY)
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
                CompanionAPIClass.Instance.Logout();
                setUpLogin();
            }
        }

        private void Login()
        {
            try
            {
                if (CompanionAPIClass.Instance.NeedLogin)
                {
                    CompanionAPIClass.Instance.LoginAs(EDCommander.Current.Name, textBoxMail.Text.Trim(), textBoxPassword.Text.Trim());
                }

                if (CompanionAPIClass.Instance.NeedConfirmation)
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
                CompanionAPIClass.Instance.Confirm(code);
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
            CompanionAPIClass.Instance.RemoveCredentials();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                CompanionAPIClass.Instance.LoginAs(EDCommander.Current.Name);
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
            profileJson = CompanionAPIClass.Instance.GetProfileString();

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
    }
}
