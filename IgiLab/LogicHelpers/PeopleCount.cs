using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgiLab.LogicHelpers
{
    public static class PeopleCount
    {
        private const string MILLION_POSTFIX = "Mil.";
        private const string THOUSAND_POSTFIX = "Kil.";

        public static string GetNumber(int num)
        {
            string postfix = "";

            if (num >= 1000000)
            {
                num /= 1000000;
                postfix = MILLION_POSTFIX;
            }
            else if (num >= 1000)
            {
                num /= 1000;
                postfix = THOUSAND_POSTFIX;
            }


            return num.ToString() + " " + postfix;
        }
    }
}
