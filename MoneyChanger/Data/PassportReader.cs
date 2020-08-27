using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace MoneyChanger.Data
{
    public class PassportReader
    {
        public static class myDllCard
        {
            [DllImport("IDCard.dll", EntryPoint = "InitIDCard")]
            public static extern int InitIDCard([MarshalAs(UnmanagedType.LPWStr)] String userID, int nType, [MarshalAs(UnmanagedType.LPWStr)] String lpDirectory);
            [DllImport("IDCard.dll", EntryPoint = "GetFieldNameEx")]
            public static extern int GetFieldNameEx(int nAttribute, int nIndex, [MarshalAs(UnmanagedType.LPWStr)] String ArrBuffer, ref int nBufferLen);
            [DllImport("IDCard.dll", EntryPoint = "GetRecogResultEx")]
            public static extern int GetRecogResultEx(int nAttribute, int nIndex, [MarshalAs(UnmanagedType.LPWStr)] String lpBuffer, ref int nBufferLen);
            [DllImport("IDCard.dll", EntryPoint = "GetCurrentDevice")]
            public static extern int GetCurrentDevice([MarshalAs(UnmanagedType.LPWStr)] String ArrDeviceName, int nLength);
            [DllImport("IDCard.dll", EntryPoint = "GetVersionInfo")]
            public static extern int GetVersionInfo([MarshalAs(UnmanagedType.LPWStr)] String ArrVersion, int nLength);
            [DllImport("IDCard.dll", EntryPoint = "CheckDeviceOnline")]
            public static extern bool CheckDeviceOnline();
            [DllImport("IDCard.dll", EntryPoint = "ResetIDCardID")]
            public static extern void ResetIDCardID();
            [DllImport("IDCard.dll", EntryPoint = "AddIDCardID")]
            public static extern int AddIDCardID(int nMainID, int[] nSubID, int nSubIdCount);
            [DllImport("IDCard.dll", EntryPoint = "DetectDocument")]
            public static extern int DetectDocument();
            [DllImport("IDCard.dll", EntryPoint = "SetRecogDG")]
            public static extern void SetRecogDG(int nDG);
            [DllImport("IDCard.dll", EntryPoint = "SetRecogVIZ")]
            public static extern void SetRecogVIZ(bool bRecogVIZ);
            [DllImport("IDCard.dll", EntryPoint = "SetSaveImageType")]
            public static extern void SetSaveImageType(int nImageType);
            [DllImport("IDCard.dll", EntryPoint = "SetConfigByFile")]
            public static extern int SetConfigByFile([MarshalAs(UnmanagedType.LPWStr)] String strConfigFile);
            [DllImport("IDCard.dll", EntryPoint = "FreeIDCard")]
            public static extern void FreeIDCard();
            [DllImport("IDCard.dll", EntryPoint = "GetDeviceSN")]
            public static extern int GetDeviceSN([MarshalAs(UnmanagedType.LPWStr)] String ArrSn, int nLength);
            [DllImport("IDCard.dll", EntryPoint = "SaveImageEx")]
            public static extern int SaveImageEx([MarshalAs(UnmanagedType.LPWStr)] String lpFileName, int nType);
            [DllImport("IDCard.dll", EntryPoint = "AutoProcessIDCard")]
            public static extern int AutoProcessIDCard(ref int nCardType);
        }

        delegate int SDT_OpenPort(int iPort);
        delegate int SDT_ClosePort(int iPort);
        delegate int SDT_StartFindIDCard(int iPort, ref byte pRAPDU, int iIfOpen);
        delegate int SDT_SelectIDCard(int iPort, ref byte pRAPDU, int iIfOpen);
        delegate int SDT_ReadBaseMsg(int iPort, ref byte pucCHMsg, ref int puiCHMsgLen, ref byte pucPHMsg, ref int puiPHMsgLen, int iIfOpen);
        delegate int SDT_ReadNewAppMsg(int iPort, ref byte pucAppMsg, ref int puiAppMsgLen, int iIfOpen);
        delegate int GetBmp(string filename, int nType);
        delegate int SDT_GetSAMIDToStr(int iPortID, ref byte pcSAMIDStr, int iIfOpen);
        delegate int SDT_GetSAMID(int iPortID, ref byte pcSAMID, int iIfOpen);

        SDT_OpenPort sdtopenport;
        SDT_ClosePort sdtcloseport;
        SDT_StartFindIDCard sdtstartfindidcard;
        SDT_SelectIDCard sdtselectidcard;
        SDT_ReadBaseMsg sdtreadbasemsg;
        SDT_ReadNewAppMsg sdtreadnewappmsg;
        GetBmp getbmp;
        SDT_GetSAMIDToStr sdtgetsamidtostr;
        SDT_GetSAMID sdtgetsamid;

        public static class myDll
        {
            [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
            public static extern int LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

            [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
            public static extern int GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

            [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
            public static extern bool FreeLibrary(int hModule);
        }

        bool boolIsIDCardLoaded = false;
        bool boolDevice = false;
        bool boolLoad = false;

        int intCount = 0;
        int intOpenPort = 0;

        Transaksi transaksi = new Transaksi();
        public string stringUserID { get; set; }
        public string stringDllPath = Directory.GetCurrentDirectory();
        public string stringDeviceSN { get; set; }
        public string stringDeviceStatus { get; set; }
        public string stringDeviceName { get; set; }
        public string stringSDKVersion { get; set; }
        public string stringRFIDSN { get; set; }
        public string[] stringArrContent { get; set; }
        public string stringname { get; set; }
        public string stringmrz2 { get; set; }
        public string stringnationality { get; set; }

        public string LoadKernel()
        {
            stringUserID = "66915733240623479851";
            string UserID = new string(stringUserID);

            stringDllPath = Directory.GetCurrentDirectory();

            int intRet = -1;
            int intConfig;
            string stringTextConfigPath = stringDllPath + "\\IDCardConfig.ini";
            intRet = myDllCard.InitIDCard(UserID, 1, stringDllPath);
            if (intRet != 0)
            {
                string stringtemp = "Error" + intRet.ToString();
                return stringtemp;
            }
            intConfig = myDllCard.SetConfigByFile(stringTextConfigPath);
            boolIsIDCardLoaded = true;
            stringDeviceSN = fnGetDeviceSN();
            stringDeviceStatus = fnDeviceOnlineStatus();
            stringDeviceName = fnGetDeviceName();
            stringSDKVersion = fnGetSDKVersion();

            LoadDll();
            stringRFIDSN = fnGetRFIDSN();

            return "Load Kernel Success";
        }

        private string fnGetDeviceSN()
        {

            if (!boolIsIDCardLoaded)
            {
                return "Error: 1";
            }
            String cArrSN = new String('\0', 16);
            int nRet = myDllCard.GetDeviceSN(cArrSN, 16);
            if (nRet == 0)
                return cArrSN;
            else
                return "Error: 2";
        }

        private string fnDeviceOnlineStatus()
        {
            if (!boolIsIDCardLoaded)
            {
                return "Error: 1";
            }

            //bool bRet = pCheckDeviceOnline();
            bool boolRet = myDllCard.CheckDeviceOnline();
            if (boolRet)
            {
                boolDevice = true;
                return "Device Online";
            }
            else
            {
                boolDevice = false;
                intCount++;
                return "Device Offline";
            }
        }

        private string fnGetDeviceName()
        {
            if (!boolIsIDCardLoaded)
            {
                return "Error: 1";
            }

            String cArrDeviceName = new String('\0', 128);
            //pGetCurrentDevice(cArrDeviceName, 128);
            myDllCard.GetCurrentDevice(cArrDeviceName, 128);
            stringDeviceName += cArrDeviceName;

            return cArrDeviceName;
        }

        private string fnGetSDKVersion()
        {
            if (!boolIsIDCardLoaded)
            {
                return "Error: 1";
            }

            String cArrVersion = new String('\0', 128);
            //pGetVersionInfo(cArrVersion, 128);
            myDllCard.GetVersionInfo(cArrVersion, 128);
            return cArrVersion;
        }

        private bool LoadDllAPI()
        {
            stringDllPath = Directory.GetCurrentDirectory();
            stringDllPath += "\\sdtapi.dll";
            int hmodulesdtapi = myDll.LoadLibrary(@stringDllPath);
            if (hmodulesdtapi == 0)
            {
                return false;
            }

            IntPtr intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_OpenPort");
            sdtopenport = (SDT_OpenPort)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_OpenPort));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_ClosePort");
            sdtcloseport = (SDT_ClosePort)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_ClosePort));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_StartFindIDCard");
            sdtstartfindidcard = (SDT_StartFindIDCard)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_StartFindIDCard));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_SelectIDCard");
            sdtselectidcard = (SDT_SelectIDCard)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_SelectIDCard));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_ReadBaseMsg");
            sdtreadbasemsg = (SDT_ReadBaseMsg)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_ReadBaseMsg));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_ReadNewAppMsg");
            sdtreadnewappmsg = (SDT_ReadNewAppMsg)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_ReadNewAppMsg));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_GetSAMIDToStr");
            sdtgetsamidtostr = (SDT_GetSAMIDToStr)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_GetSAMIDToStr));

            intptr = (IntPtr)myDll.GetProcAddress(hmodulesdtapi, "SDT_GetSAMID");
            sdtgetsamid = (SDT_GetSAMID)Marshal.GetDelegateForFunctionPointer(intptr, typeof(SDT_GetSAMID));

            stringDllPath = Directory.GetCurrentDirectory();
            stringDllPath += "\\WltRS.dll";
            int hmodulewltrs = myDll.LoadLibrary(@stringDllPath);
            if (hmodulewltrs == 0)
            {
                return false;
            }

            intptr = (IntPtr)myDll.GetProcAddress(hmodulewltrs, "GetBmp");
            getbmp = (GetBmp)Marshal.GetDelegateForFunctionPointer(intptr, typeof(GetBmp));

            if (sdtopenport == null || sdtcloseport == null || sdtstartfindidcard == null || sdtselectidcard == null || sdtreadbasemsg == null ||
                sdtreadnewappmsg == null || sdtgetsamidtostr == null || sdtgetsamid == null)
            {
                myDll.FreeLibrary(hmodulesdtapi);
                return false;
            }

            if (getbmp == null)
            {
                myDll.FreeLibrary(hmodulewltrs);
                return false;
            }

            return true;
        }

        private void LoadDll()
        {

            if (boolLoad == false)
            {
                if (!LoadDllAPI())
                {
                    return;
                }
                else
                {
                    for (int iPort = 1001; iPort < 1017; iPort = iPort + 1)
                    {
                        if (sdtopenport(iPort) == 0x90)
                        {
                            intOpenPort = iPort;
                            boolLoad = true;
                            break;
                        }
                    }
                }
            }
        }

        private string fnGetRFIDSN()
        {

            if (!boolLoad)
            {
                return "Error: 1";
            }

            byte[] cArrSN = new byte[1024];
            int nRet = sdtgetsamidtostr(intOpenPort, ref cArrSN[0], 0);
            if (nRet == 0x90)
                return Encoding.UTF8.GetString(cArrSN);
            else
                return "Failed to get Device Sequence";
        }

        private string[] GetContent()
        {
            int[] intIndexArray = { 3, 13, 12 };
            string[] strContent = new string[intIndexArray.Length];
            int MAX_CH_NUM = 128;
            int nBufLen = MAX_CH_NUM * sizeof(byte);
            for (int i = 0; i < intIndexArray.Length; i++)
            {
                String cArrFieldValue = new String('\0', MAX_CH_NUM);
                String cArrFieldName = new String('\0', MAX_CH_NUM);
                //int nRet = pGetRecogResultEx(1, intIndexArray[i], cArrFieldValue, ref nBufLen);
                int nRet = myDllCard.GetRecogResultEx(1, intIndexArray[i], cArrFieldValue, ref nBufLen);
                strContent[i] = cArrFieldValue;
            }
            return strContent;
        }

        public void ScanPassport()
        {
            if (!boolIsIDCardLoaded)
            {
                return;
            }
            stringDeviceStatus = fnDeviceOnlineStatus();
            if (!boolDevice)
            {
                return;
            }
            int nRet = 0;

            int nCardType = 13;
            if (nCardType <= 0)
            {
                return;
            }

            int[] nSubID = new int[1];
            nSubID[0] = 0;
            myDllCard.ResetIDCardID();
            nRet = myDllCard.AddIDCardID(nCardType, nSubID, 1);

            int ncardType = 0;


            nRet = myDllCard.AutoProcessIDCard(ref ncardType);

            if (nRet > 0)
            {
                stringArrContent = GetContent();
            }
        }

        public PassportReaderTransaksi GetPRData()
        {
            string strReport = LoadKernel();
            ScanPassport();
            PassportReaderTransaksi passportdata = new PassportReaderTransaksi();
            passportdata.stringName = stringArrContent[0].Substring(0, stringArrContent[0].IndexOf("\0"));
            passportdata.stringMRZ2 = stringArrContent[1].Substring(0, stringArrContent[1].IndexOf("\0"));
            passportdata.stringNationality = stringArrContent[2].Substring(0, stringArrContent[2].IndexOf("\0"));
            return passportdata;
        }

        public Task<PassportReaderTransaksi[]> GetData()
        {
            string strReport = LoadKernel();
            ScanPassport();
            return Task.FromResult(Enumerable.Range(1, 3).Select(index => new PassportReaderTransaksi
            {
                stringName = stringArrContent[0],
                stringMRZ2 = stringArrContent[1],
                stringNationality = stringArrContent[2]
            }).ToArray());
        }
    }
}
