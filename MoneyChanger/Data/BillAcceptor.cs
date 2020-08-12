using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MoneyChanger.Data
{
    public class BillAcceptor
    {
        //LibraryBillAcceptor.BillAcceptor acceptor = new LibraryBillAcceptor.BillAcceptor();

        //public void PortOpen()
        //{
        //    acceptor.OpenPort(5);
        //    Console.WriteLine("FUNCTION: BILL ACCEPTOR PORT OPEN");
        //}

        //public void EnableAcceptance()
        //{
        //    acceptor.EnableAcceptance();
        //    Console.WriteLine("FUNCTION: BILL ACCEPTOR ENABLE ACCEPTANCE");
        //}

        //public void PortClose()
        //{
        //    acceptor.ClosePort();
        //    Console.WriteLine("FUNCTION: BILL ACCEPTOR PORT CLOSE");
        //}

        public const string param_billacceptor_exe = "C:\\LibraryBillAcceptor\\bill acceptor form\\bin\\Debug\\bill acceptor.exe";
        public Process process = new Process();
        public Process currentprocess;
        public Process[] localprocessbyname;

        public async Task billacceptor()
        {
            string workingdirectory = Path.GetDirectoryName(param_billacceptor_exe);
            //Process process = new Process();
            process.StartInfo.FileName = param_billacceptor_exe;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            Console.WriteLine("FUNCTION: BILL ACCEPTOR PROCESS RUNNING");
            //process.WaitForExit();
            //if (process.HasExited)
            //{
            //    process.Close();
            //    process.Dispose();
            //}
        }

        public void KillProcess()
        {
            currentprocess = Process.GetCurrentProcess();
            localprocessbyname = Process.GetProcessesByName("bill acceptor");
            foreach(var processbyname in localprocessbyname)
            {
                processbyname.Kill();
            }
        }
    }
}
