namespace arp_tool2
{
    partial class Form1
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
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuExport = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuAdd = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(240, 268);
            this.dataGrid1.TabIndex = 1;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuFile);
            this.mainMenu1.MenuItems.Add(this.mnuEdit);
            // 
            // mnuFile
            // 
            this.mnuFile.MenuItems.Add(this.mnuExit);
            this.mnuFile.MenuItems.Add(this.mnuExport);
            this.mnuFile.Text = "File";
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuExport
            // 
            this.mnuExport.Text = "Export...";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click_1);
            // 
            // mnuEdit
            // 
            this.mnuEdit.MenuItems.Add(this.mnuAdd);
            this.mnuEdit.Text = "Edit";
            // 
            // mnuAdd
            // 
            this.mnuAdd.Text = "Add new";
            this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.dataGrid1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "arp_tool2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuExport;
        private System.Windows.Forms.MenuItem mnuEdit;
        private System.Windows.Forms.MenuItem mnuAdd;
    }
}

