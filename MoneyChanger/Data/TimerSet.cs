using MoneyChanger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MoneyChanger.Data
{
    public class TimerSet
    {
        private Timer time;
        private Transaksi transaksi = new Transaksi();
        private int totalcash = 0;
        private string datatext;
        private string[] data;
        private string[] column;
        private string tipeuang;
        private string nilaiuang;
        private static string datapath = @"C:\PercobaanNulisLog\NulisLogDataDummy.txt";
        private static string datakosong = @"C:\PercobaanNulisLog\NulisLogDataKosong.txt";
        private static string datakosong1 = @"C:\PercobaanNulisLog\NulisLogDataKosong1.txt";

        public void SetTimer(double interval)
        {
            time = new Timer();
            time.Elapsed += new ElapsedEventHandler(TimeRefresh);
            time.Interval = interval;
            time.Enabled = true;
        }
        public event Action onelapsed;
        public void TimeRefresh(object source, ElapsedEventArgs e)
        {
            int result;
            onelapsed?.Invoke();
            datatext = File.ReadAllText(datapath);
            data = datatext.Split(",");
            for (int i = 0; i < data.Length; i++)
            {
                if (i == 0)
                    tipeuang = data[i];
                if (i == 1)
                    nilaiuang = data[i];
                //string[] column = data[i].Split(",");
                //for (int j = 0; j < column.Length; j++)
                //{
                //    if (j == 0)
                //        tipeuang = column[j];
                //    if (j == 1)
                //        nilaiuang = column[j];
                //}
                
                //result = ConvertCash(tipeuang, nilaiuang);
                //transaksi.AddTotalCash(result.ToString());
            }
            transaksi.AddListAcceptor(tipeuang, nilaiuang);
            using (StreamWriter streamwriter = new StreamWriter(datapath))
            {
                streamwriter.WriteLine("");
            }
            result = ConvertCash(tipeuang, nilaiuang);
            transaksi.AddTotalCash(result.ToString());
        }
        private int ConvertCash(string strtipe, string strnilai)
        {
            int result;
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
