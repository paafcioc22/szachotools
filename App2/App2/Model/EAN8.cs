using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model
{
    public static class EAN8
    {
        public static string CalcChechSum(string code)
        {
            string EAN = "";
            int sum = 0;
            int odd = 0;
            int even = 0;
            int Checksum = 0;

            if (code.Length == 7)
            {
                char[] digit = code.ToCharArray();
                #region Wyliczanie sumy kontrolnej
                odd = int.Parse(digit[1].ToString()) + int.Parse(digit[3].ToString()) + int.Parse(digit[5].ToString());
                even = int.Parse(digit[0].ToString()) + int.Parse(digit[2].ToString()) + int.Parse(digit[4].ToString()) + int.Parse(digit[6].ToString());
                sum = (3 * even) + odd;
                Checksum = 10 - (sum % 10);
                if (Checksum == 10)
                {
                    Checksum = 0;
                }
                #endregion

                #region Scalenie kodu z sumą kontrolną i stworzenie tablicy znaków
                code = code + Checksum.ToString();
                EAN = code;
                #endregion

            }
            return EAN;
        }
    }
}
