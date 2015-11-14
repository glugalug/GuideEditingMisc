using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchedulesDirectGrabber
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            UsernameTextBox.Text = config_.username;
        }

        private void ShowPasswordCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !(sender as CheckBox).Checked;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string password_hash = SDTokenManager.HashPassword(PasswordTextBox.Text);
            token_manager_ = new SDTokenManager(UsernameTextBox.Text, password_hash);
            try
            {
                string token = token_manager_.token;
                config_.username = UsernameTextBox.Text;
                config_.pwhash = password_hash;
                ConfigManager.instance.SaveConfig();
                this.Close();
            } catch(SDTokenManager.ServerDownException)
            {
                MessageBox.Show("Schedules Direct JSON API server is currently down.  Try again in one hour.");
                Application.Exit();
            }
        }

        private static SDTokenManager token_manager_ = null;

        private static ConfigManager.SDGrabberConfig config_ = ConfigManager.config;
        public static SDTokenManager LoginAndGetTokenManager()
        {
            if (!(string.IsNullOrEmpty(config_.username) && string.IsNullOrEmpty(config_.pwhash)))
            {
                try
                {
                    token_manager_ = new SDTokenManager(config_.username, config_.pwhash);
                    string token = token_manager_.token;
                    return token_manager_;
                } catch { }  // Failed to get token, assume incorrect login info.
            }
            LoginForm login_form = new LoginForm();
            login_form.ShowDialog();
            return token_manager_;
        }
    }
}
