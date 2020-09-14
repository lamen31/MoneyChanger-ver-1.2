using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyChanger.Data
{
    public class EKTP_Utility
    {
        public static string StringInsert(string strdata)
        {
            int i = 0;
            string strtemp = strdata;
            string strInsert = ",";
            for(i = strtemp.Length - 2; i > 0; i -= 2)
            {
                strtemp = strtemp.Insert(i, strInsert);
            }
            return strtemp;
        }
        public static byte[] StringToByte(string strvalue)
        {
            byte[] result = null;

            try
            {
                strvalue = strvalue.Replace(Environment.NewLine, "");
                strvalue = strvalue.Replace("0x", "");
                strvalue = strvalue.Replace(" ", "");

                string[] xbytes = strvalue.Split(",");

                result = new byte[xbytes.Length];
                int i = 0;
                foreach (string xbyte in xbytes)
                {
                    result[i] = byte.Parse(xbyte, System.Globalization.NumberStyles.HexNumber);
                    i++;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
    }
   
}
