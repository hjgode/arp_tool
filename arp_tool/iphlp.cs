using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Net;

namespace arp_tool
{
    class iphlp
    {
        [DllImport("iphlpapi.dll", EntryPoint = "GetIpNetTable")]
        static extern int GetIpNetTable(IntPtr pIpNetTable, ref int pdwSize, bool bOrder);

        public enum MIB_IPNET_TYPE:uint
        {
            MIB_IPNET_TYPE_OTHER,
            MIB_IPNET_TYPE_INVALID,
            MIB_IPNET_TYPE_DYNAMIC,
            MIB_IPNET_TYPE_STATIC
        }
        // Define the MIB_IPNETROW structure.      
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_IPNETROW
        {
            /// <summary>
            /// the index of the network adapter for this entry
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwIndex;
            //public int dwIndex;
            /// <summary>
            /// the length of the mac address field
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwPhysAddrLen;
            //public int dwPhysAddrLen;
            /// <summary>
            /// the mac address stored in bytes mac0 to mac7
            /// </summary>
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            /// <summary>
            /// the IP address stored in 4 bytes
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public uint dwAddr;
            //public int dwAddr;
            /// <summary>
            /// the type of the entry
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public MIB_IPNET_TYPE dwType;//uint
            //public int dwType;
        }

        // The insufficient buffer error.
        const int ERROR_INSUFFICIENT_BUFFER = 122;
        public class MBIPNETROW
        {
            uint _index = 0;
            public uint index
            {
                get { return _index; }
            }
            //string _interfaceName = "";
            //public string interfaceNameTODO
            //{
            //    get { return ifentry.getIFnameTODO(_index); }
            //}
            public uint phsyAddrLength=0;
            public byte[] mac = new byte[8];
            public ulong uMAC = 0;
            public string sMAC
            {
                get { return macstrdotted(); }
            }
            IPAddress _ipaddress = IPAddress.Any;
            public IPAddress ipaddress { 
                get { return _ipaddress; } 
            }
            public MIB_IPNET_TYPE _ipnettype = MIB_IPNET_TYPE.MIB_IPNET_TYPE_INVALID;
            public MIB_IPNET_TYPE ipnettype
            {
                get { return _ipnettype; }
            }
            MIB_IPNETROW _netrow;

            public MBIPNETROW(MIB_IPNETROW netrow){
                _netrow = netrow;
                _index = netrow.dwIndex;
                phsyAddrLength = netrow.dwPhysAddrLen;
                mac[0] = netrow.mac0;
                mac[1] = netrow.mac1;
                mac[2] = netrow.mac2;
                mac[3] = netrow.mac3;
                mac[4] = netrow.mac4;
                mac[5] = netrow.mac5;
                if(netrow.dwPhysAddrLen>6)
                    mac[6] = netrow.mac6;
                if (netrow.dwPhysAddrLen > 7)
                    mac[7] = netrow.mac7;
                _ipaddress = new IPAddress(netrow.dwAddr);
                uMAC = Convert.ToUInt64(macstr(), 16);
                _ipnettype = netrow.dwType;
            }
            public override string ToString()
            {
                string s = "";
                s += "index: " + _index.ToString();
                //s += "'" + ifentry.getIFnameTODO(_index) + "', ";
                s += "\t" + macstrdotted();
                s += "\t" + ipaddress.ToString();
                s += "\t" + ipnettype.ToString();
                s += "\t" + uMAC.ToString("x012");
                return s;
            }
            string macstr()
            {
                string sm = "";
                if(_netrow.dwPhysAddrLen==6)
                    sm = string.Format("{0:X02}{1:X02}{2:X02}{3:X02}{4:X02}{5:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
                else if (_netrow.dwPhysAddrLen == 7)
                    sm = string.Format("{0:X02}{1:X02}{2:X02}{3:X02}{4:X02}{5:X02}{6:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], mac[6]);
                else if (_netrow.dwPhysAddrLen == 7)
                    sm = string.Format("{0:X02}{1:X02}{2:X02}{3:X02}{4:X02}{5:X02}{6:X02}{7:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], mac[6], mac[7]);

                return sm;
            }
            public string macstrdotted()
            {
                string sm = "";
                if(_netrow.dwPhysAddrLen==6)
                    sm = string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);
                else if (_netrow.dwPhysAddrLen == 7)
                    sm = string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}:{6:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], mac[6]);
                else if (_netrow.dwPhysAddrLen == 7)
                    sm = string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}:{6:X02}:{7:X02}",
                        mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], mac[6], mac[7]);

                return sm;
            }
        }


        public MBIPNETROW[] getMBIPNETROW()
        {
            //List<MIB_IPNETROW> ipnet_table = new List<MIB_IPNETROW>();
            List<MBIPNETROW> ipnettable = new List<MBIPNETROW>();
            string sCon = "";

            // The number of bytes needed.
            int bytesNeeded = 0;
            // The result from the API call.
            int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);
            // Call the function, expecting an insufficient buffer.
            if (result != ERROR_INSUFFICIENT_BUFFER)
            {
                // Throw an exception.
                throw new ArgumentException(result.ToString());
            }
            // Allocate the memory, do it in a try/finally block, to ensure
            // that it is released.
            IntPtr buffer = IntPtr.Zero;
            // Try/finally.
            try
            {
                // Allocate the memory.
                buffer = Marshal.AllocCoTaskMem(bytesNeeded);
                // Make the call again. If it did not succeed, then
                // raise an error.
                result = GetIpNetTable(buffer, ref bytesNeeded, false);
                // If the result is not 0 (no error), then throw an exception.
                if (result != 0)
                {
                    // Throw an exception.
                    throw new ArgumentException(result.ToString());
                }
                // Now we have the buffer, we have to marshal it. We can read
                // the first 4 bytes to get the length of the buffer.
                int entries = Marshal.ReadInt32(buffer);
                // Increment the memory pointer by the size of the int.
                IntPtr currentBuffer = new IntPtr(buffer.ToInt64() +
                   Marshal.SizeOf(typeof(int)));

                // Allocate an array of entries.
                MIB_IPNETROW[] table = new MIB_IPNETROW[entries];
                // Cycle through the entries.
                for (int index = 0; index < entries; index++)
                {
                    // Call PtrToStructure, getting the structure information.
                    table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() + (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))), typeof(MIB_IPNETROW));
                    //ipnet_table.Add(table[index]);
                    ipnettable.Add(new MBIPNETROW(table[index]));
                }
                for (int index = 0; index < entries; index++)
                {
                    IPAddress ip = new IPAddress(table[index].dwAddr);
                    sCon+="\r\n"+table[index].dwIndex.ToString()+" IP:" + ip.ToString() + "\t\tMAC:";
                    byte b;
                    b = table[index].mac0;
                    sCon+= string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}",
                        table[index].mac0, table[index].mac1, table[index].mac2, table[index].mac3,
                        table[index].mac4, table[index].mac5);
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(buffer);
            }
            System.Diagnostics.Debug.WriteLine(sCon);
            //return ipnet_table.ToArray();
            return ipnettable.ToArray();
        }
    }
    public class ifentry
    {
        const int MAX_INTERFACE_NAME_LEN = 256;
        const int MAXLEN_PHYSADDR = 8;
        const int MAXLEN_IFDESCR = 256;

        [DllImport("iphlpapi.dll")]
        static extern uint GetIfEntry(ref MIB_IFROW pIfTable);

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        struct MIB_IFROW
        {
            [MarshalAs(UnmanagedType.ByValTStr,SizeConst=MAX_INTERFACE_NAME_LEN)]
            public string wszName;
            public uint dwIndex, dwType, dwMtu, dwSpeed, dwPhysAddrLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAXLEN_PHYSADDR)]
            public byte[] bPhysAddr;
            public uint dwAdminStatus, dwOperStatus, dwLastChange;
            public uint dwInOctets, dwInUcastPkts, dwInNUcastPkts;
            public uint dwInDiscards, dwInErrors, dwInUnknownProtos;
            public uint dwOutOctets, dwOutUcastPkts, dwOutNUcastPkts;
            public uint dwOutDiscards, dwOutErrors, dwOutQLen, dwDescrLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAXLEN_IFDESCR)]
            public byte[] bDescr;
        }

        public static string getIFnameTODO(uint index){
            MIB_IFROW mib_ifrow = new MIB_IFROW();
            mib_ifrow.wszName = new string(' ', MAX_INTERFACE_NAME_LEN);
            mib_ifrow.dwIndex = index;
            uint uRes = GetIfEntry(ref mib_ifrow);
            string s = mib_ifrow.wszName;
            return s;
        }
    }
}
