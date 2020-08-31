using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace MoneyChanger.Data
{
    public class EKtp
    {
        private bool initialized = false;
        public IntPtr hContext = IntPtr.Zero;

        byte[] pbPcid = null;
        byte[] pbConf = null;

        public void EKtp_Connect()
        {
            try
            {
                EKtp_Sdk.EktpConnect(ref hContext, pbPcid, 16, pbConf, 32);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
