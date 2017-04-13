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

                // It is possible that we have valid cookies at this point so don't log in, but we did
                // need the credentials
                if (CompanionAPIClass.Instance.CurrentState == CompanionAPIClass.State.NEEDS_LOGIN)
                {
                    CompanionAPIClass.Instance.LoginAs(EDCommander.Current.Name, textBoxMail.Text.Trim(), textBoxPassword.Text.Trim());
                }

                if (CompanionAPIClass.Instance.CurrentState == CompanionAPIClass.State.NEEDS_CONFIRMATION)
                {
                    setUpConfirm();
                }
                else if (CompanionAPIClass.Instance.CurrentState == CompanionAPIClass.State.READY)
                {
                    if (profileJson == null)
                    {
                        profileJson = CompanionAPIClass.Instance.GetProfileString();
                    }
                    if (profileJson == null)
                    {
                        setUpLoggedIn("Login OK.  Waiting fo user profile");
                    }
                    else
                    {
                        setUpLoggedIn("ED server connection ok");
                        richTextBox1.AppendText(profileJson);
                        //setShipyardFromConfiguration();
                    }

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
                // All done - see if it works
                profileJson = CompanionAPIClass.Instance.GetProfileString();
                if (profileJson != null)
                {
                    setUpLoggedIn("Login OK");
                    richTextBox1.AppendText(profileJson);
                    //setShipyardFromConfiguration();
                }
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

                profileJson = CompanionAPIClass.Instance.GetProfileString();
                if (profileJson != null)
                {
                    richTextBox1.AppendText(profileJson);
                }

            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
                setUpLogin();
            }
        }
    }
}
