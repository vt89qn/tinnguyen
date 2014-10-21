using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading.Tasks;
using DotRas;

namespace FB.App_Common
{
    public static class MobileModermController
    {
        private static RasDialer d1 = new RasDialer();
        public const int ERROR_SUCCESS = 0;
        const int MAX_PATH = 260;
        const int RAS_MaxDeviceType = 16;
        const int RAS_MaxPhoneNumber = 128;
        const int RAS_MaxEntryName = 256;
        const int RAS_MaxDeviceName = 128;

        const int RAS_Connected = 0x2000;
        private static int rasConnectionsAmount;

        [DllImport("rasapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int RasEnumConnections([In, Out] RASCONN[] lprasconn, ref int lpcb, ref int lpcConnections);

        [DllImport("rasapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RasHangUp(IntPtr hRasConn);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct RASCONN
        {
            public int dwSize;
            public IntPtr hrasconn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxEntryName + 1)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceType + 1)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RAS_MaxDeviceName + 1)]
            public string szDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPhonebook;
            public int dwSubEntry;
        }

        public static bool Connect()
        {
            d1.EntryName = AppSettings.Name3G;
            d1.PhoneNumber = "*99#";
            d1.Dial();
            return true;
        }

        public static void Disconnect()
        {
            RASCONN[] rasStructs = GetRasConnections();

            // Przejście przez każdą strukturę RASCONN
            for (int i = 0; i < rasConnectionsAmount; i++)
            {
                // Pobranie pojedynczej struktury
                RASCONN rStruct = rasStructs[i];

                // Jeżeli uchwyt do połączenia wynosi 0, to brak połączenia
                if (rStruct.hrasconn == IntPtr.Zero) continue; // i następna struktura...
                // Rozłączenie...
                var t = RasHangUp(rStruct.hrasconn);
                t = RasHangUp(rStruct.hrasconn);
            }
        }

        private static RASCONN[] GetRasConnections()
        {
            // Stworzenie tablicy, którą później należy przekazać funkcjom
            int rasEnumReturn;
            RASCONN[] rasconnStructs = new RASCONN[256];
            rasconnStructs.Initialize(); // inicjalizacja wszystkich pól struktury
            rasconnStructs[0].dwSize = Marshal.SizeOf(typeof(RASCONN)); // inicjalizacja pierwszego pola pierwszej struktury na wartość wielkości tej struktury
            int sizeOfRasconnStruct = rasconnStructs[0].dwSize * rasconnStructs.Length; // wielkość pojedynczej struktury * ilosc

            // Wywołanie RasEnumConnections do zdobycia wszystkich aktywnych połączeń RAS
            rasEnumReturn = RasEnumConnections(rasconnStructs, ref sizeOfRasconnStruct, ref rasConnectionsAmount);

            // jeżeli RasEnumConnections nie zwróciło ERROR_SUCCESS
            if (rasEnumReturn != 0) throw new Win32Exception(rasEnumReturn);
            return rasconnStructs;
        }
    }
}
