namespace arp_tool2
{
    partial class newArpEntry
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuOK = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listNetworkInterfaces = new System.Windows.Forms.ComboBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtMAC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuCancel);
            this.mainMenu1.MenuItems.Add(this.mnuOK);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.mnuCancel_Click);
            // 
            // mnuOK
            // 
            this.mnuOK.Text = "OK";
            this.mnuOK.Click += new System.EventHandler(this.mnuOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.Text = "interface:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 18);
            this.label2.Text = "IP address:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 18);
            this.label3.Text = "MAC address:";
            // 
            // listNetworkInterfaces
            // 
            this.listNetworkInterfaces.Location = new System.Drawing.Point(106, 77);
            this.listNetworkInterfaces.Name = "listNetworkInterfaces";
            this.listNetworkInterfaces.Size = new System.Drawing.Size(119, 22);
            this.listNetworkInterfaces.TabIndex = 3;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(106, 150);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(118, 21);
            this.txtIP.TabIndex = 4;
            // 
            // txtMAC
            // 
            this.txtMAC.Location = new System.Drawing.Point(106, 114);
            this.txtMAC.Name = "txtMAC";
            this.txtMAC.Size = new System.Drawing.Size(118, 21);
            this.txtMAC.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(14, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 41);
            this.label4.Text = "Define a new Arp entry";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 18);
            this.label5.Text = "type:";
            // 
            // listTypes
            // 
            this.listTypes.Location = new System.Drawing.Point(105, 187);
            this.listTypes.Name = "listTypes";
            this.listTypes.Size = new System.Drawing.Size(119, 22);
            this.listTypes.TabIndex = 3;
            // 
            // newArpEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMAC);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.listTypes);
            this.Controls.Add(this.listNetworkInterfaces);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "newArpEntry";
            this.Text = "newArpEntry";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox listNetworkInterfaces;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtMAC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox listTypes;
    }
}