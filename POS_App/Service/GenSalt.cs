using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service
{
    public class genSalt
    {
        private static readonly char[] letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string RandSequence(int n)
        {
            Random random = new Random();
            char[] b = new char[n];

            for (int i = 0; i < n; i++)
            {
                b[i] = letters[random.Next(letters.Length)];
            }

            return new string(b);
        }

        public static string GenSalt(int length)
        {
            if (length <= 0)
            {
                length = 50;
            }
            return RandSequence(length);
        }

       
    }
}
