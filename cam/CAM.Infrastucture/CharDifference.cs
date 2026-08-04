using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Infrastucture
{
    public class CharDifference
    {
        public static int CalculateDifference(string strA, string strB)
        {
            int allDifferences = 0;
            int charCount1;
            int charCount2;

            //do this as long as both strings are longer than 0
            while (strA.Length > 0 && strB.Length > 0)
            {
                //get char count for the character at index 1 in string 1
                charCount1 = strA.Length - strA.Replace(strA[0].ToString(), "").Length;
                //get char count for the character at index 1 in string 1 but for string2
                charCount2 = strB.Length - strB.Replace(strA[0].ToString(), "").Length;

                //strip all chars that now have been counted from string 2
                strB = strB.Replace(strA[0].ToString(), "");
                //strip all chars that now have been counted from string 1
                strA = strA.Replace(strA[0].ToString(), "");

                //add difference to counting variable
                allDifferences += Math.Abs(@charCount1 - @charCount2);
            }

            // is there any rest length on any of those string ?
            allDifferences += Math.Abs(strA.Length - strB.Length);


            return allDifferences;
        }
    }
}
