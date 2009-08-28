using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RepoWatcher
{
    public partial class EditProjectForm : Form
    {
        private bool _checkForErrorsBeforeClosing = true;

        public EditProjectForm()
        {
            InitializeComponent();
        }

        internal Project Project
        {
            get
            {
                if (IsValid())
                {
                    Project p = new Project();

                    p.Name = this._name.Text;
                    p.Url = new Uri(this._url.Text);
                    p.IgnoreUsers = new List<string>(this._ignoreUsers.Lines);
                    p.MaxHistory = (int)this._keepLogEntries.Value;

                    switch ((string)this._checkEveryUnit.SelectedItem)
                    {
                        case "milliseconds":
                            p.CheckEvery = TimeSpan.FromMilliseconds((int)this._checkEveryValue.Value);
                            break;
                        case "seconds":
                            p.CheckEvery = TimeSpan.FromSeconds((int)this._checkEveryValue.Value);
                            break;
                        case "minutes":
                            p.CheckEvery = TimeSpan.FromMinutes((int)this._checkEveryValue.Value);
                            break;
                        case "hours":
                            p.CheckEvery = TimeSpan.FromHours((int)this._checkEveryValue.Value);
                            break;
                    }

                    return p;
                }

                return null;
            }
        }

        private bool IsValid()
        {
            if ((errorProvider1.GetError(this._name).Length == 0)
                && (errorProvider1.GetError(this._url).Length == 0)
                && (errorProvider1.GetError(this._checkEveryValue).Length == 0)
                && (errorProvider1.GetError(this._checkEveryUnit).Length == 0)
                && (errorProvider1.GetError(this._keepLogEntries).Length == 0)
                )
            {
                return true;
            }

            return false;
        }

        private void EditProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_checkForErrorsBeforeClosing && !IsValid())
            {
                DialogResult result = MessageBox.Show("The form currently has errors.  If you continue, your changes will not be saved.  Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _name_Validated(object sender, EventArgs e)
        {
            if (this._name.Text.Length == 0)
            {
                errorProvider1.SetError(this._name, "Name is required.");
            }
            else
            {
                errorProvider1.SetError(this._name, string.Empty);
            }
        }

        private void _url_Validated(object sender, EventArgs e)
        {
            if (this._url.Text.Length == 0)
            {
                errorProvider1.SetError(this._url, "URL is required.");
            }
            else
            {
                errorProvider1.SetError(this._url, string.Empty);
            }
        }

        private void _checkEveryValue_Validated(object sender, EventArgs e)
        {
            if (this._checkEveryValue.Value <= 0)
            {
                errorProvider1.SetError(this._checkEveryValue, "Please provide a value > 0.");
            }
            else
            {
                errorProvider1.SetError(this._checkEveryValue, string.Empty);
            }
        }

        private void _checkEveryUnit_Validated(object sender, EventArgs e)
        {
            if (this._checkEveryUnit.SelectedIndex < 0)
            {
                errorProvider1.SetError(this._checkEveryUnit, "Please select a unit.");
            }
            else
            {
                errorProvider1.SetError(this._checkEveryUnit, string.Empty);
            }
        }

        private void _keepLogEntries_Validated(object sender, EventArgs e)
        {
            if (this._keepLogEntries.Value <= 0)
            {
                errorProvider1.SetError(this._keepLogEntries, "Please provide a value > 0.");
            }
            else
            {
                errorProvider1.SetError(this._keepLogEntries, string.Empty);
            }
        }

        private void _ignoreUsers_Validated(object sender, EventArgs e)
        {

        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            _checkForErrorsBeforeClosing = false;
            this.Close();
        }
    }
}