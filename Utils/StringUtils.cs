using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class StringUtils
    {
        public static List<string> GetEmptyStrings(int length)
        {
            var output = new List<string>();
            for(int i=0;i<length;i++)
            {
                output.Add(new string(new char[0]));
            }

            return output;
        }

        public static string SetStringToSize(string toSet,int targetSize)
        {
            if (toSet.Length > targetSize)
            {
                toSet = toSet.Substring(0, targetSize);
            }
            else if (toSet.Length < targetSize)
            {
                toSet += new string(' ', targetSize - toSet.Length);
            }

            return toSet;
        }

    }
}
