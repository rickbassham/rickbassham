namespace RepoWatcher
{
    partial class EditProjectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this._name = new System.Windows.Forms.TextBox();
            this._url = new System.Windows.Forms.TextBox();
            this._checkEveryValue = new System.Windows.Forms.NumericUpDown();
            this._checkEveryUnit = new System.Windows.Forms.ComboBox();
            this._keepLogEntries = new System.Windows.Forms.NumericUpDown();
            this._ignoreUsers = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._ok = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._checkEveryValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._keepLogEntries)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(56, 12);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(224, 20);
            this._name.TabIndex = 1;
            this._name.Validated += new System.EventHandler(this._name_Validated);
            // 
            // _url
            // 
            this._url.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._url.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this._url.Location = new System.Drawing.Point(56, 38);
            this._url.Name = "_url";
            this._url.Size = new System.Drawing.Size(224, 20);
            this._url.TabIndex = 2;
            this._url.Validated += new System.EventHandler(this._url_Validated);
            // 
            // _checkEveryValue
            // 
            this._checkEveryValue.Location = new System.Drawing.Point(89, 65);
            this._checkEveryValue.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this._checkEveryValue.Name = "_checkEveryValue";
            this._checkEveryValue.Size = new System.Drawing.Size(88, 20);
            this._checkEveryValue.TabIndex = 3;
            this._checkEveryValue.ThousandsSeparator = true;
            this._checkEveryValue.Validated += new System.EventHandler(this._checkEveryValue_Validated);
            // 
            // _checkEveryUnit
            // 
            this._checkEveryUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._checkEveryUnit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._checkEveryUnit.FormattingEnabled = true;
            this._checkEveryUnit.Items.AddRange(new object[] {
            "milliseconds",
            "seconds",
            "minutes",
            "hours"});
            this._checkEveryUnit.Location = new System.Drawing.Point(197, 64);
            this._checkEveryUnit.Name = "_checkEveryUnit";
            this._checkEveryUnit.Size = new System.Drawing.Size(83, 21);
            this._checkEveryUnit.TabIndex = 4;
            this._checkEveryUnit.Validated += new System.EventHandler(this._checkEveryUnit_Validated);
            // 
            // _keepLogEntries
            // 
            this._keepLogEntries.Location = new System.Drawing.Point(56, 91);
            this._keepLogEntries.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this._keepLogEntries.Name = "_keepLogEntries";
            this._keepLogEntries.Size = new System.Drawing.Size(78, 20);
            this._keepLogEntries.TabIndex = 5;
            this._keepLogEntries.ThousandsSeparator = true;
            this._keepLogEntries.Validated += new System.EventHandler(this._keepLogEntries_Validated);
            // 
            // _ignoreUsers
            // 
            this._ignoreUsers.Location = new System.Drawing.Point(6, 19);
            this._ignoreUsers.Multiline = true;
            this._ignoreUsers.Name = "_ignoreUsers";
            this._ignoreUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._ignoreUsers.Size = new System.Drawing.Size(256, 75);
            this._ignoreUsers.TabIndex = 6;
            this._ignoreUsers.Validated += new System.EventHandler(this._ignoreUsers_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "URL:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Check Every:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Keep";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "log entries.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._ignoreUsers);
            this.groupBox1.Location = new System.Drawing.Point(12, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ignore Users:";
            // 
            // _ok
            // 
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._ok.Location = new System.Drawing.Point(124, 223);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 12;
            this._ok.Text = "&OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _cancel
            // 
            this._cancel.CausesValidation = false;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancel.Location = new System.Drawing.Point(205, 223);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 13;
            this._cancel.Text = "&Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // EditProjectForm
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(298, 258);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._ok);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._keepLogEntries);
            this.Controls.Add(this._checkEveryUnit);
            this.Controls.Add(this._checkEveryValue);
            this.Controls.Add(this._url);
            this.Controls.Add(this._name);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(304, 280);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(304, 280);
            this.Name = "EditProjectForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Edit Project Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditProjectForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._checkEveryValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._keepLogEntries)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.TextBox _url;
        private System.Windows.Forms.NumericUpDown _checkEveryValue;
        private System.Windows.Forms.ComboBox _checkEveryUnit;
        private System.Windows.Forms.NumericUpDown _keepLogEntries;
        private System.Windows.Forms.TextBox _ignoreUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}