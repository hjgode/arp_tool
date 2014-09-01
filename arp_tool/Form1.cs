using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace arp_tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getTable();
        }
        void getTable()
        {
            iphlp _iphlp = new iphlp();
            iphlp.MBIPNETROW[] list = _iphlp.getMBIPNETROW();
            foreach (iphlp.MBIPNETROW mi in list)
            {
                System.Diagnostics.Debug.WriteLine(mi.ToString());
            }
            dataGrid1.RowHeadersVisible = false;
            dataGrid1.DataSource = list;
            
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // fileLoc is a global string variable, set in StreamReader example.
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                    {
                        iphlp.MBIPNETROW[] nr = (iphlp.MBIPNETROW[]) dataGrid1.DataSource;
                        //ds.WriteXml("XMLFileOut.xml",XmlWriteMode.IgnoreSchema);
                        foreach (iphlp.MBIPNETROW dr in nr)
                        {
                            sw.Write(dr.ToString());
                            sw.WriteLine();
                        }
                    }
                    MessageBox.Show("Saved successfully to "+sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Saving to " + sfd.FileName + " failed. Exception: " +ex.Message);
                }
            }
            sfd.Dispose();
        }
    }
}