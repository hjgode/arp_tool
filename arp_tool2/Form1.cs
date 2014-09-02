using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using OpenNETCF.Net.NetworkInformation;

namespace arp_tool2
{
    public partial class Form1 : Form
    {
        NetInfo netinfo = new NetInfo();
        public Form1()
        {
            InitializeComponent();
            loadData();
        }

        void loadData()
        {
            dataGrid1.DataSource = netinfo.arp_entries;
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuExport_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // fileLoc is a global string variable, set in StreamReader example.
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                    {
                        OpenNETCF.Net.NetworkInformation.ArpEntry[] nr = (OpenNETCF.Net.NetworkInformation.ArpEntry[])dataGrid1.DataSource;
                        //ds.WriteXml("XMLFileOut.xml",XmlWriteMode.IgnoreSchema);
                        foreach (OpenNETCF.Net.NetworkInformation.ArpEntry dr in nr)
                        {
                            sw.Write(dr.NetworkInterface + "\t" + dr.IPAddress + "\t" + dr.PhysicalAddress
                             + "\t" + dr.ArpEntryType.ToString());
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

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            newArpEntry dlg = new newArpEntry();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ArpEntry ae=dlg._ArpEntry;
                NetInfo.createIPnetEntry(ae);
                loadData();
            }
        }
    }
}