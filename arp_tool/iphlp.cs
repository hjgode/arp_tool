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

    public static class MyAdapters{
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref Int64 pBufOutLen);

        const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        const int ERROR_BUFFER_OVERFLOW = 111;
        const int MAX_ADAPTER_NAME_LENGTH = 256;
        const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        const int MIB_IF_TYPE_OTHER = 1;
        const int MIB_IF_TYPE_ETHERNET = 6;
        const int MIB_IF_TYPE_TOKENRING = 9;
        const int MIB_IF_TYPE_FDDI = 15;
        const int MIB_IF_TYPE_PPP = 23;
        const int MIB_IF_TYPE_LOOPBACK = 24;
        const int MIB_IF_TYPE_SLIP = 28;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct IP_ADDRESS_STRING
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string Address;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct IP_ADDR_STRING
        {
            public IntPtr Next;
            public IP_ADDRESS_STRING IpAddress;
            public IP_ADDRESS_STRING IpMask;
            public Int32 Context;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct IP_ADAPTER_INFO
        {
            public IntPtr Next;
            public Int32 ComboIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_NAME_LENGTH + 4)]
            public string AdapterName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
            public string AdapterDescription;
            public UInt32 AddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ADAPTER_ADDRESS_LENGTH)]
            public byte[] Address;
            public Int32 Index;
            public UInt32 Type;
            public UInt32 DhcpEnabled;
            public IntPtr CurrentIpAddress;
            public IP_ADDR_STRING IpAddressList;
            public IP_ADDR_STRING GatewayList;
            public IP_ADDR_STRING DhcpServer;
            public bool HaveWins;
            public IP_ADDR_STRING PrimaryWinsServer;
            public IP_ADDR_STRING SecondaryWinsServer;
            public Int32 LeaseObtained;
            public Int32 LeaseExpires;
        }

        public static void GetAdapters()
        {
           long structSize = Marshal.SizeOf( typeof( IP_ADAPTER_INFO ) );
           IntPtr pArray = Marshal.AllocHGlobal( (int)structSize );

           int ret = GetAdaptersInfo( pArray, ref structSize );

           if (ret == ERROR_BUFFER_OVERFLOW ) // ERROR_BUFFER_OVERFLOW == 111
           {
             // Buffer was too small, reallocate the correct size for the buffer.
             pArray = Marshal.ReAllocHGlobal( pArray, new IntPtr (structSize) );

             ret = GetAdaptersInfo( pArray, ref structSize );
           } // if

           if ( ret == 0 )
           {
             // Call Succeeded
             IntPtr pEntry = pArray;

             do
             {
               // Retrieve the adapter info from the memory address
               IP_ADAPTER_INFO entry = (IP_ADAPTER_INFO)Marshal.PtrToStructure( pEntry, typeof( IP_ADAPTER_INFO ));

               // ***Do something with the data HERE!***
               System.Diagnostics.Debug.WriteLine("\n");
               System.Diagnostics.Debug.WriteLine( "Index: {0}", entry.Index.ToString() );

               // Adapter Type
               string tmpString = string.Empty;
               switch( entry.Type )
               {
                 case MIB_IF_TYPE_ETHERNET  : tmpString = "Ethernet";  break;
                 case MIB_IF_TYPE_TOKENRING : tmpString = "Token Ring"; break;
                 case MIB_IF_TYPE_FDDI      : tmpString = "FDDI"; break;
                 case MIB_IF_TYPE_PPP       : tmpString = "PPP"; break;
                 case MIB_IF_TYPE_LOOPBACK  : tmpString = "Loopback"; break;
                 case MIB_IF_TYPE_SLIP      : tmpString = "Slip"; break;
                 default                    : tmpString = "Other/Unknown"; break;
               } // switch
               System.Diagnostics.Debug.WriteLine( "Adapter Type: {0}", tmpString );

               System.Diagnostics.Debug.WriteLine( "Name: {0}", entry.AdapterName );
               System.Diagnostics.Debug.WriteLine( "Desc: {0}\n", entry.AdapterDescription );

               System.Diagnostics.Debug.WriteLine( "DHCP Enabled: {0}", ( entry.DhcpEnabled == 1 ) ? "Yes" : "No" );

               if (entry.DhcpEnabled == 1)
               {
                 System.Diagnostics.Debug.WriteLine( "DHCP Server : {0}", entry.DhcpServer.IpAddress.Address );

                 // Lease Obtained (convert from "time_t" to C# DateTime)
                 DateTime pdatDate = new DateTime(1970, 1, 1).AddSeconds( entry.LeaseObtained ).ToLocalTime();
                 System.Diagnostics.Debug.WriteLine( "Lease Obtained: {0}", pdatDate.ToString() );

                 // Lease Expires (convert from "time_t" to C# DateTime)
                 pdatDate = new DateTime(1970, 1, 1).AddSeconds( entry.LeaseExpires ).ToLocalTime();
                 System.Diagnostics.Debug.WriteLine( "Lease Expires : {0}\n", pdatDate.ToString() );
               } // if DhcpEnabled

               System.Diagnostics.Debug.WriteLine( "IP Address     : {0}", entry.IpAddressList.IpAddress.Address );
               System.Diagnostics.Debug.WriteLine( "Subnet Mask    : {0}", entry.IpAddressList.IpMask.Address );
               System.Diagnostics.Debug.WriteLine( "Default Gateway: {0}", entry.GatewayList.IpAddress.Address );

               // MAC Address (data is in a byte[])
               tmpString = string.Empty;
               for (int i = 0; i < entry.AddressLength; i++)
               {
                 tmpString += string.Format("{0:X2}-", entry.Address[i]);
               }
               System.Diagnostics.Debug.WriteLine( string.Format( "MAC Address    : {0}{1:X2}\n", tmpString, entry.Address[entry.AddressLength] ));

               System.Diagnostics.Debug.WriteLine( "Has WINS: {0}", entry.HaveWins ? "Yes" : "No" );
               if (entry.HaveWins)
               {
                 System.Diagnostics.Debug.WriteLine( "Primary WINS Server  : {0}", entry.PrimaryWinsServer.IpAddress.Address );
                 System.Diagnostics.Debug.WriteLine( "Secondary WINS Server: {0}", entry.SecondaryWinsServer.IpAddress.Address );
               } // HaveWins

               // Get next adapter (if any)
               pEntry = entry.Next;

             }
             while( pEntry != IntPtr.Zero );

             Marshal.FreeHGlobal(pArray);

           } // if
           else
           {
             Marshal.FreeHGlobal(pArray);
             throw new InvalidOperationException( "GetAdaptersInfo failed: " + ret );
           }

        } // GetAdapters
    }
    public class ifentry
    {
        const int MAX_INTERFACE_NAME_LEN = 256;
        const int MAXLEN_PHYSADDR = 8;
        const int MAXLEN_IFDESCR = 256;

        [DllImport("iphlpapi.dll")]
        static extern int GetIfEntry(IntPtr pIfTable);

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        struct MIB_IFROW
        {
            [MarshalAs(UnmanagedType.ByValTStr,SizeConst=MAX_INTERFACE_NAME_LEN)]   //  256 bytes
            public string wszName;
            public UInt32 dwIndex, dwType, dwMtu, dwSpeed, dwPhysAddrLen;           //   20 bytes   5x4Bytes    Uint32 is 4Bytes
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAXLEN_PHYSADDR)]        //    8 bytes
            public byte[] bPhysAddr;
            public UInt32 dwAdminStatus, dwOperStatus, dwLastChange;                //   12 bytes   3x4Bytes
            public UInt32 dwInOctets, dwInUcastPkts, dwInNUcastPkts;                //   12 bytes
            public UInt32 dwInDiscards, dwInErrors, dwInUnknownProtos;              //   12 bytes
            public UInt32 dwOutOctets, dwOutUcastPkts, dwOutNUcastPkts;             //   12 bytes
            public UInt32 dwOutDiscards, dwOutErrors, dwOutQLen, dwDescrLen;        //   16 bytes   4x4Bytes
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAXLEN_IFDESCR)]         //  256 bytes
            public byte[] bDescr;
        }

        public static string getIFnameTODO(uint index){
            MIB_IFROW mib_ifrow = new MIB_IFROW();
            mib_ifrow.wszName = new string(' ', MAX_INTERFACE_NAME_LEN);
            mib_ifrow.dwIndex = index;

            // Allocate the memory, do it in a try/finally block, to ensure
            // that it is released.
            IntPtr buffer = IntPtr.Zero;
            int result = 0;
            try
            {
                // Allocate the memory.
                int MIB_IFROWsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MIB_IFROW));
                buffer = Marshal.AllocHGlobal(MIB_IFROWsize);
                mib_ifrow.dwIndex = index;
                //copy struct to buffer
                Marshal.StructureToPtr(mib_ifrow, buffer, true);
                // Make the call again. If it did not succeed, then
                // raise an error.
                result = GetIfEntry(buffer);
                // If the result is not 0 (no error), then throw an exception.
                if (result != 0)
                {
                    // #define ERROR_INVALID_DATA               13L
                    // Throw an exception.
                    throw new ArgumentException(result.ToString());
                }
                // Now we have the buffer, we have to marshal it. We can read
                mib_ifrow = (MIB_IFROW)Marshal.PtrToStructure(buffer, typeof(MIB_IFROW));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("exception: " + ex.Message);
            }
            finally
            {
                // Release the memory.
                Marshal.FreeHGlobal(buffer);
            }
            string s = mib_ifrow.wszName;
            return s;
        }
    }
}
