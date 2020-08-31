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
        private Function function = new Function();
        private int totalcash = 0;
        private string datatext;
        private string[] data;
        private string[] column;
        private string tipeuang;
        private string nilaiuang;
        private int listpointer = 1;
        private bool looping = false;
        private static string datapath = @"C:\PercobaanNulisLog\NulisLogData.txt";
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
            int n = 1;
            looping = true;
            transaksi.AddTotalCash(totalcash.ToString());
            onelapsed?.Invoke();
            datatext = File.ReadAllText(datapath);
            if (datatext != "" && listpointer < listpointer+1)
            {
                data = datatext.Split("\r\n");
                for (int i = 0; i < data.Length; i++)
                {
                    //if (i == 0)
                    //    tipeuang = data[i];
                    //else if (i == 1)
                    //    nilaiuang = data[i];
                    string[] column = data[i].Split(",");
                    for (int j = 0; j < column.Length; j++)
                    {
                        if (j == 0)
                            tipeuang = column[j];
                        if (j == 1)
                            nilaiuang = column[j];
                    }

                    //result = ConvertCash(tipeuang, nilaiuang);
                    //transaksi.AddTotalCash(result.ToString());
                }
                transaksi.AddListAcceptor(tipeuang, nilaiuang);
                File.WriteAllText(datapath, "");
                int count = transaksi.CountListAcceptor();
                transaksi.RemoveListAcceptor(listpointer, count-listpointer);
                //using (StreamWriter streamwriter = new StreamWriter(datapath))
                //{
                //    streamwriter.WriteLine("");
                //}
                //if(looping)
                //{
                //    result = ConvertCash(tipeuang, nilaiuang);
                //    transaksi.AddTotalCash(result.ToString());
                //    looping;
                //}
                while(looping)
                {
                    result = ConvertCash(tipeuang, nilaiuang);
                    n += 1;
                    if(n < 2)
                        transaksi.AddTotalCash(result.ToString());
                    looping = false;
                }
                listpointer += 1;
                //n += 1;
            }
        }
        private int ConvertCash(string strtipe, string strnilai)
        {
            int result;
            int n = 1;
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
            if(n < 2)
                totalcash += convert;
            n += 1;
            result = totalcash;
            return result;
        }
    }
}
