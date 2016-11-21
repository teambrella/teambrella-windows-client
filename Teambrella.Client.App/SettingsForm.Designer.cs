namespace Teambrella.Client.App
{
    partial class SettingsForm
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
            this._listBoxTeams = new System.Windows.Forms.ListBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelTeams = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._comboBoxPayToAddressOkAge = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this._comboBoxAutoApprovalMyGoodAddress = new System.Windows.Forms.ComboBox();
            this._comboBoxAutoApprovalMyNewAddress = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._comboBoxAutoApprovalCosignGoodAddress = new System.Windows.Forms.ComboBox();
            this._comboBoxAutoApprovalCosignNewAddress = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this._buttonDisband = new System.Windows.Forms.Button();
            this._labelStatusValue = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._labelPubKey = new System.Windows.Forms.Label();
            this._panelStatus = new System.Windows.Forms.Panel();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this._panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // _listBoxTeams
            // 
            this._listBoxTeams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._listBoxTeams.FormattingEnabled = true;
            this._listBoxTeams.Location = new System.Drawing.Point(12, 64);
            this._listBoxTeams.Name = "_listBoxTeams";
            this._listBoxTeams.Size = new System.Drawing.Size(194, 342);
            this._listBoxTeams.TabIndex = 0;
            this._listBoxTeams.SelectedValueChanged += new System.EventHandler(this.listBoxTeams_SelectedValueChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(545, 433);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelTeams
            // 
            this.labelTeams.AutoSize = true;
            this.labelTeams.Location = new System.Drawing.Point(9, 48);
            this.labelTeams.Name = "labelTeams";
            this.labelTeams.Size = new System.Drawing.Size(39, 13);
            this.labelTeams.TabIndex = 2;
            this.labelTeams.Text = "&Teams";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this._comboBoxPayToAddressOkAge);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this._comboBoxAutoApprovalMyGoodAddress);
            this.panel1.Controls.Add(this._comboBoxAutoApprovalMyNewAddress);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this._comboBoxAutoApprovalCosignGoodAddress);
            this.panel1.Controls.Add(this._comboBoxAutoApprovalCosignNewAddress);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(212, 102);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 304);
            this.panel1.TabIndex = 3;
            // 
            // _comboBoxPayToAddressOkAge
            // 
            this._comboBoxPayToAddressOkAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxPayToAddressOkAge.FormattingEnabled = true;
            this._comboBoxPayToAddressOkAge.Items.AddRange(new object[] {
            "2 days",
            "4 days",
            "6 days",
            "8 days",
            "10 days",
            "12 days",
            "14 days",
            "16 days",
            "18 days",
            "20 days"});
            this._comboBoxPayToAddressOkAge.Location = new System.Drawing.Point(222, 31);
            this._comboBoxPayToAddressOkAge.Name = "_comboBoxPayToAddressOkAge";
            this._comboBoxPayToAddressOkAge.Size = new System.Drawing.Size(148, 21);
            this._comboBoxPayToAddressOkAge.TabIndex = 18;
            this._comboBoxPayToAddressOkAge.SelectedValueChanged += new System.EventHandler(this.comboBoxPayToAddressOkAge_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "New If Known for Less Than";
            // 
            // _comboBoxAutoApprovalMyGoodAddress
            // 
            this._comboBoxAutoApprovalMyGoodAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxAutoApprovalMyGoodAddress.FormattingEnabled = true;
            this._comboBoxAutoApprovalMyGoodAddress.Items.AddRange(new object[] {
            "Do Not Auto-Approve",
            "Auto-Approve Right Away",
            "Auto-Approve In 1 day",
            "Auto-Approve In 2 days",
            "Auto-Approve In 3 days",
            "Auto-Approve In 4 days",
            "Auto-Approve In 5 days"});
            this._comboBoxAutoApprovalMyGoodAddress.Location = new System.Drawing.Point(222, 90);
            this._comboBoxAutoApprovalMyGoodAddress.Name = "_comboBoxAutoApprovalMyGoodAddress";
            this._comboBoxAutoApprovalMyGoodAddress.Size = new System.Drawing.Size(148, 21);
            this._comboBoxAutoApprovalMyGoodAddress.TabIndex = 16;
            this._comboBoxAutoApprovalMyGoodAddress.SelectedValueChanged += new System.EventHandler(this.comboBoxAutoApprovalMyGoodAddress_SelectedValueChanged);
            // 
            // _comboBoxAutoApprovalMyNewAddress
            // 
            this._comboBoxAutoApprovalMyNewAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxAutoApprovalMyNewAddress.FormattingEnabled = true;
            this._comboBoxAutoApprovalMyNewAddress.Items.AddRange(new object[] {
            "Do Not Auto-Approve",
            "Auto-Approve Right Away",
            "Auto-Approve In 1 day",
            "Auto-Approve In 2 days",
            "Auto-Approve In 3 days",
            "Auto-Approve In 4 days",
            "Auto-Approve In 5 days",
            "Auto-Approve In 6 days",
            "Auto-Approve In 7 days"});
            this._comboBoxAutoApprovalMyNewAddress.Location = new System.Drawing.Point(222, 114);
            this._comboBoxAutoApprovalMyNewAddress.Name = "_comboBoxAutoApprovalMyNewAddress";
            this._comboBoxAutoApprovalMyNewAddress.Size = new System.Drawing.Size(148, 21);
            this._comboBoxAutoApprovalMyNewAddress.TabIndex = 15;
            this._comboBoxAutoApprovalMyNewAddress.SelectedValueChanged += new System.EventHandler(this.comboBoxAutoApprovalMyNewAddress_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "When Paying to a New Address:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "When Paying to a Normal Address:";
            // 
            // _comboBoxAutoApprovalCosignGoodAddress
            // 
            this._comboBoxAutoApprovalCosignGoodAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxAutoApprovalCosignGoodAddress.FormattingEnabled = true;
            this._comboBoxAutoApprovalCosignGoodAddress.Items.AddRange(new object[] {
            "Do Not Auto-Approve",
            "Auto-Approve Right Away",
            "Auto-Approve In 1 day",
            "Auto-Approve In 2 days",
            "Auto-Approve In 3 days",
            "Auto-Approve In 4 days",
            "Auto-Approve In 5 days"});
            this._comboBoxAutoApprovalCosignGoodAddress.Location = new System.Drawing.Point(222, 171);
            this._comboBoxAutoApprovalCosignGoodAddress.Name = "_comboBoxAutoApprovalCosignGoodAddress";
            this._comboBoxAutoApprovalCosignGoodAddress.Size = new System.Drawing.Size(148, 21);
            this._comboBoxAutoApprovalCosignGoodAddress.TabIndex = 12;
            this._comboBoxAutoApprovalCosignGoodAddress.SelectedValueChanged += new System.EventHandler(this.comboBoxAutoApprovalCosignGoodAddress_SelectedValueChanged);
            // 
            // _comboBoxAutoApprovalCosignNewAddress
            // 
            this._comboBoxAutoApprovalCosignNewAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxAutoApprovalCosignNewAddress.FormattingEnabled = true;
            this._comboBoxAutoApprovalCosignNewAddress.Items.AddRange(new object[] {
            "Do Not Auto-Approve",
            "Auto-Approve Right Away",
            "Auto-Approve In 1 day",
            "Auto-Approve In 2 days",
            "Auto-Approve In 3 days",
            "Auto-Approve In 4 days",
            "Auto-Approve In 5 days",
            "Auto-Approve In 6 days",
            "Auto-Approve In 7 days"});
            this._comboBoxAutoApprovalCosignNewAddress.Location = new System.Drawing.Point(222, 195);
            this._comboBoxAutoApprovalCosignNewAddress.Name = "_comboBoxAutoApprovalCosignNewAddress";
            this._comboBoxAutoApprovalCosignNewAddress.Size = new System.Drawing.Size(148, 21);
            this._comboBoxAutoApprovalCosignNewAddress.TabIndex = 11;
            this._comboBoxAutoApprovalCosignNewAddress.SelectedValueChanged += new System.EventHandler(this.comboBoxAutoApprovalCosignNewAddress_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "When Paying to a New Address:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "When Paying to a Normal Address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Transactions That I &Cosign:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Transactions From &My Wallet:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Pay-To Addresses ";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(10, 13);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "&Status:";
            // 
            // _buttonDisband
            // 
            this._buttonDisband.Location = new System.Drawing.Point(222, 8);
            this._buttonDisband.Name = "_buttonDisband";
            this._buttonDisband.Size = new System.Drawing.Size(75, 23);
            this._buttonDisband.TabIndex = 4;
            this._buttonDisband.Text = "&Disband...";
            this._buttonDisband.UseVisualStyleBackColor = true;
            this._buttonDisband.Click += new System.EventHandler(this.buttonDisband_Click);
            // 
            // _labelStatusValue
            // 
            this._labelStatusValue.AutoSize = true;
            this._labelStatusValue.ForeColor = System.Drawing.Color.Green;
            this._labelStatusValue.Location = new System.Drawing.Point(56, 13);
            this._labelStatusValue.Name = "_labelStatusValue";
            this._labelStatusValue.Size = new System.Drawing.Size(40, 13);
            this._labelStatusValue.TabIndex = 10;
            this._labelStatusValue.Text = "Normal";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Your Public Key:";
            // 
            // _labelPubKey
            // 
            this._labelPubKey.AutoSize = true;
            this._labelPubKey.Location = new System.Drawing.Point(99, 14);
            this._labelPubKey.Name = "_labelPubKey";
            this._labelPubKey.Size = new System.Drawing.Size(30, 13);
            this._labelPubKey.TabIndex = 5;
            this._labelPubKey.Text = "[key]";
            // 
            // _panelStatus
            // 
            this._panelStatus.BackColor = System.Drawing.Color.LightGray;
            this._panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panelStatus.Controls.Add(this._buttonDisband);
            this._panelStatus.Controls.Add(this.labelStatus);
            this._panelStatus.Controls.Add(this._labelStatusValue);
            this._panelStatus.Location = new System.Drawing.Point(212, 64);
            this._panelStatus.Name = "_panelStatus";
            this._panelStatus.Size = new System.Drawing.Size(408, 41);
            this._panelStatus.TabIndex = 6;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(545, 9);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 7;
            this.buttonCopy.Text = "C&opy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 468);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this._panelStatus);
            this.Controls.Add(this._labelPubKey);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelTeams);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this._listBoxTeams);
            this.MaximumSize = new System.Drawing.Size(640, 700);
            this.MinimumSize = new System.Drawing.Size(640, 383);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this._panelStatus.ResumeLayout(false);
            this._panelStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _listBoxTeams;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelTeams;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox _comboBoxPayToAddressOkAge;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox _comboBoxAutoApprovalMyGoodAddress;
        private System.Windows.Forms.ComboBox _comboBoxAutoApprovalMyNewAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox _comboBoxAutoApprovalCosignGoodAddress;
        private System.Windows.Forms.ComboBox _comboBoxAutoApprovalCosignNewAddress;
        private System.Windows.Forms.Label _labelStatusValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _buttonDisband;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label _labelPubKey;
        private System.Windows.Forms.Panel _panelStatus;
        private System.Windows.Forms.Button buttonCopy;

    }
}

