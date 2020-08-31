using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MoneyChanger.Data
{
    public struct CBXITEM
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Byte[] cxbox;
    }
    public struct CDMCFGSTATUS
    {
        public Byte major_no;
        public Byte minor_no;
        public Byte cbx_type;
    }
    public struct CDMSTATUS
    {
        public Byte error_cd;
        public Byte reject_cd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] sensor;
    }
    public struct CDMDIAGNOSTIC
    {
        public Byte error_cd;
        public Byte reject_cd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] sensor;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
        public char[] result_msg;
    }
    public struct CDMMULTIDISPENSE
    {
        public Byte error_cd;
        public Byte reject_cd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] sensor;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Byte[] count;
    }
    public struct CDMLASTDISPENSE
    {
        public Byte last_cmd;
        public Byte last_error_cd;
        public Byte last_cbx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Byte[] last_dispense_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Byte[] last_divert_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Byte[] last_pick_count;
    }
    public class CashDispenser
    {
        public int Result { get; set; }
        public UInt16 comport { get; set; }
        public int[] Slot = { 0, 0, 0, 0 };
        public static class myDll
        {
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_OpenPort")]
            extern public static bool MFSCommCDM_OpenPort(IntPtr hwnd, UInt16 portnum);
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_Reset")]
            extern public static bool MFSCommCDM_Reset();
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_MultiDispense")]
            extern public static bool MFSCommCDM_MultiDispense(ref CBXITEM cbx, ref CDMMULTIDISPENSE ret);
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_ClosePort")]
            extern public static bool MFSCommCDM_ClosePort();
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_CfgStatus")]
            extern public static bool MFSCommCDM_CfgStatus(ref CDMCFGSTATUS cdmcfg);
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_Status")]
            extern public static bool MFSCommCDM_Status(ref CDMSTATUS cdmstatus);
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_Diagnostic")]
            extern public static bool MFSCommCDM_Diagnostic(ref CDMDIAGNOSTIC cdmdiagnostic);
            [DllImport("MFSCOMMCDM.DLL", EntryPoint = "MFSCommCDM_LastDispense")]
            extern public static bool MFSCommCDM_LastDispense(ref CDMLASTDISPENSE cdmlastdispense);
        }

        public void PortOpen(int intcomport)
        {
            comport = Convert.ToUInt16(intcomport);
            IntPtr hwnd;
            hwnd = Process.GetCurrentProcess().MainWindowHandle;

            if (myDll.MFSCommCDM_OpenPort(hwnd, comport))
            {
                Console.WriteLine("PORT OPEN SUCCESS");
            }
            else
            {
                Console.WriteLine("PORT OPEN FAILED");
            }
        }

        public void PortClose()
        {
            if (myDll.MFSCommCDM_ClosePort())
            {
                Console.WriteLine("PORT CLOSE SUCCESS");
            }
            else
            {
                Console.WriteLine("PORT CLOSE FAILED");
            }
        }

        public void Reset()
        {
            if (myDll.MFSCommCDM_Reset())
            {
                Console.WriteLine("RESET SUCCESS");
            }
            else
            {
                Console.WriteLine("RESET FAILED");
            }
        }

        public void SetResult(int intresult)
        {
            Result = intresult;
        }

        public void Config()
        {
            CDMCFGSTATUS cdmcfgstatus = new CDMCFGSTATUS();
            if (myDll.MFSCommCDM_CfgStatus(ref cdmcfgstatus))
            {
                Console.WriteLine("CFG SUCCESS");
            }
            else
            {
                Console.WriteLine("CFG FAILED");
            }
        }

        public void CashConvert()
        {
            int cashvalue = Result;
            cashvalue = cashvalue / 1000;

            do
            {
                if (cashvalue >= 100)
                {
                    Slot[3] += 1;
                    cashvalue -= 100;
                }
                else if (cashvalue >= 50 && cashvalue < 100)
                {
                    Slot[2] += 1;
                    cashvalue -= 50;
                }
                else if (cashvalue >= 10 && cashvalue < 50)
                {
                    Slot[1] += 1;
                    cashvalue -= 10;
                }
                else if (cashvalue >= 5 && cashvalue < 10)
                {
                    Slot[0] += 1;
                    cashvalue -= 5;
                }
            } while (cashvalue != 0);
        }

        public void MultiDispense()
        {
            CDMMULTIDISPENSE cdmmultidispense = new CDMMULTIDISPENSE();
            CBXITEM cbxitem = new CBXITEM();
            cbxitem.cxbox = new byte[6];
            Byte test = 0;

            test = (Byte)Convert.ToSByte(Slot[0]);
            cbxitem.cxbox[0] = test;

            test = (Byte)Convert.ToSByte(Slot[1]);
            cbxitem.cxbox[1] = test;

            test = (Byte)Convert.ToSByte(Slot[2]);
            cbxitem.cxbox[2] = test;

            test = (Byte)Convert.ToSByte(Slot[3]);
            cbxitem.cxbox[3] = test;
            cbxitem.cxbox[4] = 0;
            cbxitem.cxbox[5] = 0;

            if(!myDll.MFSCommCDM_MultiDispense(ref cbxitem, ref cdmmultidispense))
            {
                Console.WriteLine("MULTI DISPENSE FAILED");
            }
            else
            {
                Console.WriteLine("MULTI DISPENSE SUCCESS");
            }
        }
    }
}
