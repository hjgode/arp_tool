using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenNETCF.Net.NetworkInformation;

namespace arp_tool2
{
    public partial class newArpEntry : Form
    {
        ArpEntry _arpentry;
        public ArpEntry _ArpEntry
        {
            get{return _arpentry;}
        }

        string[] netIfaces = NetInfo.getNetworkInterfaces();
        string[] arpTypes = NetInfo.getArpTypes();

        public newArpEntry()
        {
            InitializeComponent();
            _arpentry = NetInfo.getNewArpEntry(); 
            loadValues();
        }
        void loadValues()
        {
            listNetworkInterfaces.DataSource = netIfaces;
            listTypes.DataSource = arpTypes;

            selectIface(_arpentry.NetworkInterface.Name);

            listTypes.SelectedIndex=((int)_arpentry.ArpEntryType + 1);

            txtIP.Text = _arpentry.IPAddress.ToString();
            txtMAC.Text = _arpentry.PhysicalAddress.ToString();
        }

        bool saveValues()
        {
            bool bRet = false;

            // IP convert
            string sIP = txtIP.Text;
            System.Net.IPAddress ip=System.Net.IPAddress.None;
            try
            {
                ip = System.Net.IPAddress.Parse(sIP);
                txtIP.BackColor = Color.White;
                bRet = true;
            }
            catch (Exception)
            {
                txtIP.BackColor = Color.Pink;
            }
            if (!bRet)
                return bRet;

            //mac convert
            string sMac = txtMAC.Text;
            PhysicalAddress pMac = NetInfo.getPhysicalAddress(sMac);
            if (pMac == PhysicalAddress.None)
            {
                txtMAC.BackColor = Color.Pink;
                bRet = false;
                return bRet;
            }
            else
            {
                txtMAC.BackColor = Color.White;
            }

            ArpEntryType arpType = (ArpEntryType)(listTypes.SelectedIndex + 1);

            NetworkInterface netIface = (NetworkInterface)NetworkInterface.GetAllNetworkInterfaces()[listNetworkInterfaces.SelectedIndex];

            _arpentry = new ArpEntry(netIface, ip, pMac, arpType);

            return bRet;
        }

        void selectIface(string s)
        {
            listNetworkInterfaces.SelectedIndex = 0;
            for (int i = 0; i < listNetworkInterfaces.Items.Count; i++)
                if ((string)listNetworkInterfaces.Items[i] == s)
                {
                    listNetworkInterfaces.SelectedIndex = i;
                    continue;
                }
        }

        private void mnuOK_Click(object sender, EventArgs e)
        {
            if (saveValues())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}