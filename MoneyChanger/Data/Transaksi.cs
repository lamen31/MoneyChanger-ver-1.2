using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyChanger.Data
{
    public class Transaksi
    {
        public string totalcash { get; set; }
        public string[] converter { get; set; }
        public class Acceptor
        {
            public string Type { get; set; }
            public string Value { get; set; }
            public int Count { get; set; }
        }
        public List<Acceptor> listacceptor = new List<Acceptor>();
        public void AddListAcceptor(string strtype, string strvalue)
        {
            Acceptor acceptor = new Acceptor();
            int laccCount = listacceptor.Count;
            bool newBankNote = true;
            acceptor.Type = strtype;
            acceptor.Value = strvalue;
            acceptor.Count = 1;
                for (int i = 0;i<laccCount;i++)
                {
                    if (acceptor.Type == listacceptor[i].Type)
                    {
                        if (acceptor.Value == listacceptor[i].Value)
                        {
                            listacceptor[i].Count += 1;
                            newBankNote = false;
                            break;
                        }
                    }
                }
                if(newBankNote)
                listacceptor.Add(acceptor);
        }
        public void RemoveListAcceptor(int intposition, int intrange)
        {
            listacceptor.RemoveRange(intposition, intrange);
        }
        public int CountListAcceptor()
        {
            int result = listacceptor.Count();
            return result;
        }
        public void AddConverter(string[] strconverter)
        {
            converter = strconverter;
        }
        public void AddTotalCash(string strtotalcash)
        {
            totalcash = strtotalcash;
        }
        public void Clear()
        {
            totalcash = string.Empty;
        }
    }
}
