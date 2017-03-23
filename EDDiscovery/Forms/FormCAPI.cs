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
                CompanionAPIClass.Instance.Credentials.EmailAdr = textBoxMail.Text.Trim();
                CompanionAPIClass.Instance.Credentials.Password = textBoxPassword.Text.Trim();

                // It is possible that we have valid cookies at this point so don't log in, but we did
                // need the credentials
                if (CompanionAPIClass.Instance.CurrentState == CompanionAPIClass.State.NEEDS_LOGIN)
                {
                    CompanionAPIClass.Instance.Login();
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
                    //setShipyardFromConfiguration();
                }
            }
            catch (CompanionAppAuthenticationException ex)
            {
                setUpLogin(ex.Message);
            }
            catch (CompanionAppErrorException ex)
            {
                setUpLogin(ex.Message);
            }
            catch (Exception ex)
            {
                setUpLogin("Unexpected problem\r\nPlease report this at http://github.com/CmdrMcDonald/EliteDangerousDataProvider/issues\r\n" + ex);
            }
        }
    



        private void setUpLogin(string message = null)
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

    }
}
