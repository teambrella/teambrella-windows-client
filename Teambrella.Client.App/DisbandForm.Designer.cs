namespace Teambrella.Client.App
{
    partial class DisbandForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._textBoxAuxAddress = new System.Windows.Forms.TextBox();
            this.buttonCopyAux = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this._labelAuxFunds = new System.Windows.Forms.Label();
            this._dataGridViewCosigners = new System.Windows.Forms.DataGridView();
            this._cosignerColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this._statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this._labelFunds = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this._labelAuxWalletStatus = new System.Windows.Forms.Label();
            this._labelCosignerOfStatus = new System.Windows.Forms.Label();
            this.buttonCopy = new System.Windows.Forms.Button();
            this._textBoxAddressFrom = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControlWithdrawal = new System.Windows.Forms.TabControl();
            this.Current = new System.Windows.Forms.TabPage();
            this.Previous = new System.Windows.Forms.TabPage();
            this._labelWithdrawalStatus = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._dataGridViewCosignerOf = new System.Windows.Forms.DataGridView();
            this._ownerColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this._ownerStatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this._textBoxAddressTo = new System.Windows.Forms.Label();
            this.buttonCopyTo = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewCosigners)).BeginInit();
            this.tabControlWithdrawal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewCosignerOf)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(434, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "During Disbanding and Server-agnostic witdrawal cosigners and wallet holders exch" +
    "ange data on the blockchain.";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(434, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "In order to do this, you need to have enough funds in your data transport wallet." +
    "";
            // 
            // _textBoxAuxAddress
            // 
            this._textBoxAuxAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxAuxAddress.Location = new System.Drawing.Point(64, 116);
            this._textBoxAuxAddress.Name = "_textBoxAuxAddress";
            this._textBoxAuxAddress.ReadOnly = true;
            this._textBoxAuxAddress.Size = new System.Drawing.Size(228, 13);
            this._textBoxAuxAddress.TabIndex = 8;
            this._textBoxAuxAddress.Text = "1address";
            // 
            // buttonCopyAux
            // 
            this.buttonCopyAux.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyAux.Location = new System.Drawing.Point(377, 110);
            this.buttonCopyAux.Name = "buttonCopyAux";
            this.buttonCopyAux.Size = new System.Drawing.Size(70, 23);
            this.buttonCopyAux.TabIndex = 3;
            this.buttonCopyAux.Text = "&Copy";
            this.buttonCopyAux.UseVisualStyleBackColor = true;
            this.buttonCopyAux.Click += new System.EventHandler(this.buttonAuxCopy_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(458, 76);
            this.panel1.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Funds:";
            // 
            // _labelAuxFunds
            // 
            this._labelAuxFunds.AutoSize = true;
            this._labelAuxFunds.Location = new System.Drawing.Point(61, 138);
            this._labelAuxFunds.Name = "_labelAuxFunds";
            this._labelAuxFunds.Size = new System.Drawing.Size(45, 13);
            this._labelAuxFunds.TabIndex = 14;
            this._labelAuxFunds.Text = "0 mBTC";
            // 
            // _dataGridViewCosigners
            // 
            this._dataGridViewCosigners.AllowUserToAddRows = false;
            this._dataGridViewCosigners.AllowUserToDeleteRows = false;
            this._dataGridViewCosigners.AllowUserToResizeRows = false;
            this._dataGridViewCosigners.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridViewCosigners.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dataGridViewCosigners.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._dataGridViewCosigners.BackgroundColor = System.Drawing.SystemColors.Window;
            this._dataGridViewCosigners.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this._dataGridViewCosigners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridViewCosigners.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._cosignerColumn,
            this._statusColumn});
            this._dataGridViewCosigners.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._dataGridViewCosigners.GridColor = System.Drawing.SystemColors.ControlLight;
            this._dataGridViewCosigners.Location = new System.Drawing.Point(12, 319);
            this._dataGridViewCosigners.MultiSelect = false;
            this._dataGridViewCosigners.Name = "_dataGridViewCosigners";
            this._dataGridViewCosigners.ReadOnly = true;
            this._dataGridViewCosigners.RowHeadersVisible = false;
            this._dataGridViewCosigners.ShowCellToolTips = false;
            this._dataGridViewCosigners.Size = new System.Drawing.Size(435, 143);
            this._dataGridViewCosigners.TabIndex = 17;
            this._dataGridViewCosigners.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dataGridViewCosigners_CellContentClick);
            // 
            // Cosigner
            // 
            this._cosignerColumn.ActiveLinkColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this._cosignerColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this._cosignerColumn.HeaderText = "Cosigner";
            this._cosignerColumn.Name = "Cosigner";
            this._cosignerColumn.ReadOnly = true;
            this._cosignerColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._cosignerColumn.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // Status
            // 
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this._statusColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._statusColumn.HeaderText = "Status";
            this._statusColumn.Name = "Status";
            this._statusColumn.ReadOnly = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(0, 498);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(10, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(460, 26);
            this.label7.TabIndex = 18;
            this.label7.Text = "Wallets Cosigned by Me";
            // 
            // _labelFunds
            // 
            this._labelFunds.AutoSize = true;
            this._labelFunds.Location = new System.Drawing.Point(63, 270);
            this._labelFunds.Name = "_labelFunds";
            this._labelFunds.Size = new System.Drawing.Size(45, 13);
            this._labelFunds.TabIndex = 20;
            this._labelFunds.Text = "0 mBTC";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.label11.Location = new System.Drawing.Point(0, 1);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(10, 7, 0, 0);
            this.label11.Size = new System.Drawing.Size(460, 26);
            this.label11.TabIndex = 21;
            this.label11.Text = "&Data Transport Wallet";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(460, 2);
            this.label12.TabIndex = 22;
            // 
            // _labelAuxWalletStatus
            // 
            this._labelAuxWalletStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._labelAuxWalletStatus.BackColor = System.Drawing.Color.LightGray;
            this._labelAuxWalletStatus.ForeColor = System.Drawing.Color.Green;
            this._labelAuxWalletStatus.Location = new System.Drawing.Point(220, 2);
            this._labelAuxWalletStatus.Name = "_labelAuxWalletStatus";
            this._labelAuxWalletStatus.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this._labelAuxWalletStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._labelAuxWalletStatus.Size = new System.Drawing.Size(224, 25);
            this._labelAuxWalletStatus.TabIndex = 23;
            this._labelAuxWalletStatus.Text = "Funded";
            this._labelAuxWalletStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _labelCosignerOfStatus
            // 
            this._labelCosignerOfStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._labelCosignerOfStatus.BackColor = System.Drawing.Color.LightGray;
            this._labelCosignerOfStatus.ForeColor = System.Drawing.Color.DimGray;
            this._labelCosignerOfStatus.Location = new System.Drawing.Point(212, 498);
            this._labelCosignerOfStatus.Name = "_labelCosignerOfStatus";
            this._labelCosignerOfStatus.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this._labelCosignerOfStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._labelCosignerOfStatus.Size = new System.Drawing.Size(232, 26);
            this._labelCosignerOfStatus.TabIndex = 24;
            this._labelCosignerOfStatus.Text = "-";
            this._labelCosignerOfStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(377, 243);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(70, 23);
            this.buttonCopy.TabIndex = 27;
            this.buttonCopy.Text = "C&opy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // _textBoxAddressFrom
            // 
            this._textBoxAddressFrom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxAddressFrom.Location = new System.Drawing.Point(66, 248);
            this._textBoxAddressFrom.Name = "_textBoxAddressFrom";
            this._textBoxAddressFrom.ReadOnly = true;
            this._textBoxAddressFrom.Size = new System.Drawing.Size(228, 13);
            this._textBoxAddressFrom.TabIndex = 26;
            this._textBoxAddressFrom.Text = "1address";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(0, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(460, 2);
            this.label9.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Funds:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Address:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "From:";
            // 
            // tabControlWithdrawal
            // 
            this.tabControlWithdrawal.Controls.Add(this.Current);
            this.tabControlWithdrawal.Controls.Add(this.Previous);
            this.tabControlWithdrawal.Location = new System.Drawing.Point(9, 177);
            this.tabControlWithdrawal.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlWithdrawal.Name = "tabControlWithdrawal";
            this.tabControlWithdrawal.Padding = new System.Drawing.Point(14, 6);
            this.tabControlWithdrawal.SelectedIndex = 0;
            this.tabControlWithdrawal.Size = new System.Drawing.Size(441, 26);
            this.tabControlWithdrawal.TabIndex = 31;
            this.tabControlWithdrawal.SelectedIndexChanged += new System.EventHandler(this.tabControlWithdrawal_SelectedIndexChanged);
            // 
            // Current
            // 
            this.Current.BackColor = System.Drawing.SystemColors.Control;
            this.Current.Location = new System.Drawing.Point(4, 28);
            this.Current.Name = "Current";
            this.Current.Padding = new System.Windows.Forms.Padding(3);
            this.Current.Size = new System.Drawing.Size(433, 0);
            this.Current.TabIndex = 0;
            this.Current.Text = "Your Current Wallet: -";
            // 
            // Previous
            // 
            this.Previous.Location = new System.Drawing.Point(4, 28);
            this.Previous.Name = "Previous";
            this.Previous.Padding = new System.Windows.Forms.Padding(3);
            this.Previous.Size = new System.Drawing.Size(433, 0);
            this.Previous.TabIndex = 1;
            this.Previous.Text = "Your Previous Wallet:  -";
            this.Previous.UseVisualStyleBackColor = true;
            // 
            // _labelWithdrawalStatus
            // 
            this._labelWithdrawalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._labelWithdrawalStatus.BackColor = System.Drawing.Color.LightGray;
            this._labelWithdrawalStatus.ForeColor = System.Drawing.Color.DimGray;
            this._labelWithdrawalStatus.Location = new System.Drawing.Point(212, 205);
            this._labelWithdrawalStatus.Name = "_labelWithdrawalStatus";
            this._labelWithdrawalStatus.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this._labelWithdrawalStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._labelWithdrawalStatus.Size = new System.Drawing.Size(232, 26);
            this._labelWithdrawalStatus.TabIndex = 34;
            this._labelWithdrawalStatus.Text = "-";
            this._labelWithdrawalStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.LightGray;
            this.label16.Location = new System.Drawing.Point(0, 205);
            this.label16.Name = "label16";
            this.label16.Padding = new System.Windows.Forms.Padding(10, 6, 0, 0);
            this.label16.Size = new System.Drawing.Size(460, 26);
            this.label16.TabIndex = 33;
            this.label16.Text = "Withdrawal";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Location = new System.Drawing.Point(-2, 496);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(460, 2);
            this.label17.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(-1, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(460, 2);
            this.label4.TabIndex = 10;
            // 
            // _dataGridViewCosignerOf
            // 
            this._dataGridViewCosignerOf.AllowUserToAddRows = false;
            this._dataGridViewCosignerOf.AllowUserToDeleteRows = false;
            this._dataGridViewCosignerOf.AllowUserToResizeRows = false;
            this._dataGridViewCosignerOf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridViewCosignerOf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dataGridViewCosignerOf.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._dataGridViewCosignerOf.BackgroundColor = System.Drawing.SystemColors.Window;
            this._dataGridViewCosignerOf.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this._dataGridViewCosignerOf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridViewCosignerOf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._ownerColumn,
            this._ownerStatusColumn});
            this._dataGridViewCosignerOf.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._dataGridViewCosignerOf.GridColor = System.Drawing.SystemColors.ControlLight;
            this._dataGridViewCosignerOf.Location = new System.Drawing.Point(13, 540);
            this._dataGridViewCosignerOf.MultiSelect = false;
            this._dataGridViewCosignerOf.Name = "_dataGridViewCosignerOf";
            this._dataGridViewCosignerOf.ReadOnly = true;
            this._dataGridViewCosignerOf.RowHeadersVisible = false;
            this._dataGridViewCosignerOf.ShowCellToolTips = false;
            this._dataGridViewCosignerOf.Size = new System.Drawing.Size(435, 143);
            this._dataGridViewCosignerOf.TabIndex = 36;
            this._dataGridViewCosignerOf.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dataGridViewCosignerOf_CellContentClick);
            // 
            // Owner
            // 
            this._ownerColumn.ActiveLinkColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this._ownerColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this._ownerColumn.HeaderText = "Owner";
            this._ownerColumn.Name = "Owner";
            this._ownerColumn.ReadOnly = true;
            this._ownerColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._ownerColumn.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // OwnerStatus
            // 
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this._ownerStatusColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this._ownerStatusColumn.HeaderText = "Status";
            this._ownerStatusColumn.Name = "OwnerStatus";
            this._ownerStatusColumn.ReadOnly = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(378, 719);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(70, 23);
            this.buttonClose.TabIndex = 37;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 292);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "To:";
            // 
            // _textBoxAddressTo
            // 
            this._textBoxAddressTo.AutoSize = true;
            this._textBoxAddressTo.Location = new System.Drawing.Point(63, 292);
            this._textBoxAddressTo.Name = "_textBoxAddressTo";
            this._textBoxAddressTo.Size = new System.Drawing.Size(45, 13);
            this._textBoxAddressTo.TabIndex = 38;
            this._textBoxAddressTo.Text = "0 mBTC";
            // 
            // buttonCopyTo
            // 
            this.buttonCopyTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyTo.Location = new System.Drawing.Point(377, 287);
            this.buttonCopyTo.Name = "buttonCopyTo";
            this.buttonCopyTo.Size = new System.Drawing.Size(70, 23);
            this.buttonCopyTo.TabIndex = 40;
            this.buttonCopyTo.Text = "C&opy";
            this.buttonCopyTo.UseVisualStyleBackColor = true;
            this.buttonCopyTo.Click += new System.EventHandler(this.buttonCopyTo_Click);
            // 
            // DisbandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 754);
            this.Controls.Add(this.buttonCopyTo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._textBoxAddressTo);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this._dataGridViewCosignerOf);
            this.Controls.Add(this.label17);
            this.Controls.Add(this._labelWithdrawalStatus);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tabControlWithdrawal);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this._textBoxAddressFrom);
            this.Controls.Add(this._labelCosignerOfStatus);
            this.Controls.Add(this._labelAuxWalletStatus);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this._labelFunds);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._dataGridViewCosigners);
            this.Controls.Add(this.label9);
            this.Controls.Add(this._labelAuxFunds);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCopyAux);
            this.Controls.Add(this._textBoxAuxAddress);
            this.Name = "DisbandForm";
            this.Text = "Team Disbanding Progress";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewCosigners)).EndInit();
            this.tabControlWithdrawal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dataGridViewCosignerOf)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _textBoxAuxAddress;
        private System.Windows.Forms.Button buttonCopyAux;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label _labelAuxFunds;
        private System.Windows.Forms.DataGridView _dataGridViewCosigners;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label _labelFunds;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label _labelAuxWalletStatus;
        private System.Windows.Forms.Label _labelCosignerOfStatus;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.TextBox _textBoxAddressFrom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControlWithdrawal;
        private System.Windows.Forms.TabPage Current;
        private System.Windows.Forms.TabPage Previous;
        private System.Windows.Forms.Label _labelWithdrawalStatus;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView _dataGridViewCosignerOf;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridViewLinkColumn _cosignerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _statusColumn;
        private System.Windows.Forms.DataGridViewLinkColumn _ownerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _ownerStatusColumn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label _textBoxAddressTo;
        private System.Windows.Forms.Button buttonCopyTo;
    }
}