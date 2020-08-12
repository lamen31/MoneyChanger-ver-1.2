using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyChanger.Data
{
    public class Transaksi
    {
        public string totalcash { get; set; }
        public class Acceptor
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
        public List<Acceptor> listacceptor = new List<Acceptor>();
        public void AddListAcceptor(string strtype, string strvalue)
        {
            Acceptor acceptor = new Acceptor();
            acceptor.Type = strtype;
            acceptor.Value = strvalue;
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
        public void AddTotalCash(string strtotalcash)
        {
            totalcash = strtotalcash;
        }
    }
}
