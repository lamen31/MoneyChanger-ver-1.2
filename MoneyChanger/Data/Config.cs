using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using System.IO;

namespace MoneyChanger.Data
{
    public class Config
    {
        public const string PARAM_CONVERSION_EUR = "param.conversion.eur";
        public const string PARAM_CONVERSION_AUD = "param.conversion.aud";
        public const string PARAM_CONVERSION_CNY = "param.conversion.cny";
        public const string PARAM_CONVERSION_SGD = "param.conversion.sgd";
        public const string PARAM_CONVERSION_USD = "param.conversion.usd";
        public const string PARAM_CONVERSION_THB = "param.conversion.thb";
        public const string PARAM_CONVERSION_PHP = "param.conversion.php";
        public const string PARAM_CONVERSION_MYR = "param.conversion.myr";
        public const string PARAM_CONVERSION_GBP = "param.conversion.gbp";
        public const string PARAM_CONVERSION_JPY = "param.conversion.jpy";
        public const string PARAM_CONVERSION_HKD = "param.conversion.hkd";
        public const string PARAM_CONVERSION_TWD = "param.conversion.twd";
        public const string PARAM_PORT_BILL_ACCEPTOR = "param.port.bill.acceptor";
        public const string PARAM_PORT_CASH_DISPENSER = "param.port.cash.dispenser";

        private FileIniDataParser iniFile = new FileIniDataParser();
        private IniData iniData = new IniData();

        private string filename = "MoneyChanger_Config.properties";

        public Config()
        {
            filename = Directory.GetCurrentDirectory() + "\\MoneyChanger_Config.properties";
        }

        public void Init()
        {
            iniData = iniFile.ReadFile(filename);
        }

        public string Read(string strSection, string strName)
        {
            string result = string.Empty;
            Init();

            result = iniData[strSection][strName];
            return result;
        }

        public void Write(string strSection, string strName, string strValue)
        {
            Init();

            try
            {
                iniData.Sections.AddSection(strSection);

                iniData[strSection][strName] = strValue;
                iniData.Configuration.NewLineStr = "\r\n";
                iniFile.WriteFile(filename, iniData);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
