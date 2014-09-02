using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Net;
using OpenNETCF.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace arp_tool2
{
    class NetInfo
    {
        //arp entries
        ArpTable _arpTable;
        public ArpTable arp_table
        {
            get {
                _arpTable = ArpTable.GetArpTable();
                return _arpTable; 
            }
        }
        ArpEntry[] _arpEntries;
        public ArpEntry[] arp_entries
        {
            get { return _arpEntries; }
        }
        public NetInfo()
        {
            List<ArpEntry> list=new List<ArpEntry>();
            foreach (ArpEntry ae in ArpTable.GetArpTable())
                list.Add(ae);
            _arpEntries = list.ToArray();
        }
        static NetworkInterface getNewNetworkInterface()
        {
            NetworkInterface ni = (NetworkInterface)(ArpTable.GetArpTable()[0].NetworkInterface);
            return ni;
        }

        /// <summary>
        /// bug in OpenNetCF 2.3, which uses coredll.dll for CreateIpNetEntry
        /// </summary>
        /// <param name="pArpEntry"></param>
        /// <returns></returns>
        [DllImport("iphlpapi.dll", SetLastError=true)]
        static extern int CreateIpNetEntry(byte[] pArpEntry);

        public static int createIPnetEntry(ArpEntry entry)
        {
            byte[] buf = getBytesArpEntry(entry);
            int iRes = -1;
            try
            {
                iRes = CreateIpNetEntry(buf); // #define ERROR_ADAP_HDW_ERR               57L
            }
            catch (Exception ex) { }
            return iRes;
        }
        static byte[] getBytesArpEntry(ArpEntry item)
        {
            /*
                DWORD		dwIndex;                    //uint32
                DWORD		dwPhysAddrLen;
                BYTE		bPhysAddr[MAXLEN_PHYSADDR]; //#define MAXLEN_PHYSADDR 8
                DWORD		dwAddr;
                DWORD		dwType;
            */
            UInt32 dwIndex = Convert.ToUInt32(item.NetworkInterface.Id);
            UInt32 dwPhysAddrLen = Convert.ToUInt32(6);
            
            byte[] bPhysAddr = new byte[8];
            byte[] bMac6 = item.PhysicalAddress.GetAddressBytes();
            for (int i = 0; i < bMac6.Length; i++)
                bPhysAddr[i] = bMac6[i];
            
            UInt32 dwAddr = BitConverter.ToUInt32(item.IPAddress.GetAddressBytes(),0);
            UInt32 dwType = (UInt32)item.ArpEntryType;
            
            List<byte> buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes(dwIndex));
            buffer.AddRange(BitConverter.GetBytes(dwPhysAddrLen));
            buffer.AddRange(bPhysAddr);
            buffer.AddRange(BitConverter.GetBytes(dwAddr));
            buffer.AddRange(BitConverter.GetBytes(dwType));
            return buffer.ToArray();
        }

        public static PhysicalAddress getPhysicalAddress(string sMac){
            PhysicalAddress pMac = PhysicalAddress.None;
            sMac = sMac.Replace("-", ""); sMac = sMac.Replace(":", "");
            UInt64 u64 = 0;
            byte[] bMac;
            try
            {
                u64 = Convert.ToUInt64(sMac, 16);
                bMac = BitConverter.GetBytes(u64);
                byte[] bMac6 = new byte[6];
                for (int i = 0; i < 6; i++)
                    bMac6[5-i] = bMac[i];
                pMac = new PhysicalAddress(bMac6);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in getPhysicalAddress: " + ex.Message);
            }
            return pMac;
        }
        
        public static ArpEntry getNewArpEntry()
        {
            IPAddress ipaddress=IPAddress.None;
            PhysicalAddress mac= new PhysicalAddress(new byte[]{0,0,0,0,0,0});
            ArpEntry arpentry = new OpenNETCF.Net.NetworkInformation.ArpEntry(getNewNetworkInterface(), ipaddress, mac, ArpEntryType.Invalid);
            return arpentry;
        }

        public static string[] getNetworkInterfaces()
        {
            List<string> list=new List<string>();
            foreach (ArpEntry ae in ArpTable.GetArpTable())
            {
                if(!list.Contains(ae.NetworkInterface.Name))
                    list.Add(ae.NetworkInterface.Name);
            }
            return list.ToArray();
        }
        public static string[] getArpTypes()
        {
            List<string> list = new List<string>();
            for (int i = 1; i < 5; i++)
                list.Add(((ArpEntryType)i).ToString());
            return list.ToArray();
        }
    }
}
