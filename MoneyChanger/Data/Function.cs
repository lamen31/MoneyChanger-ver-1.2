using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyChanger.Data
{
    public class Function
    {
        private int totalcash;
        private Transaksi transaksi = new Transaksi();
        public int ConvertCash(string strtipe, string strnilai)
        {
            int result;
            totalcash = Convert.ToInt32(transaksi.totalcash);
            int convert = Convert.ToInt32(strnilai);
            switch (strtipe)
            {
                case "USD":
                    {
                        convert = convert * 14000;
                        break;
                    }
                case "EUR":
                    {
                        convert = convert * 16000;
                        break;
                    }
                case "JPY":
                    {
                        convert = convert * 132;
                        break;
                    }
                case "CNY":
                    {
                        convert = convert * 2000;
                        break;
                    }
                case "KRW":
                    {
                        convert = convert * 12;
                        break;
                    }
                case "GBP":
                    {
                        convert = convert * 18000;
                        break;
                    }
                case "AUD":
                    {
                        convert = convert * 10000;
                        break;
                    }
                case "TWD":
                    {
                        convert = convert * 477;
                        break;
                    }
                case "PHP":
                    {
                        convert = convert * 282;
                        break;
                    }
                case "HKD":
                    {
                        convert = convert * 2000;
                        break;
                    }
                case "SGD":
                    {
                        convert = convert * 10000;
                        break;
                    }
                case "MYR":
                    {
                        convert = convert * 3300;
                        break;
                    }
                case "THB":
                    {
                        convert = convert * 453;
                        break;
                    }
            }
            totalcash += convert;
            result = totalcash;
            return result;
        }
    }
}
