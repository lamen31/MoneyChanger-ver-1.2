using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChanger.Data
{
    public class EKTP_Data
    {
        private string[] Demographic;

        public byte[] demographic
        {
            set
            {
                byte[] chars = value;
                int index = 0;
                Demographic = new string[21];

                for(int i = 0; i < chars.Length; i++)
                {
                    if(chars[i] == '"')
                    {
                        StringBuilder builder = new StringBuilder();
                        for(int j = i + 1; j < chars.Length; j++)
                        {
                            if (chars[j] == '"')
                            {
                                i = j;
                                break; ;
                            }
                            builder.Append((char)chars[j]);
                        }
                        Demographic[index++] = builder.ToString();
                        if (index >= Demographic.Length)
                            break;
                    }
                }
            }
        }
    }
}
