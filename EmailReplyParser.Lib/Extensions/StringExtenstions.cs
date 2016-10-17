using System;

namespace EmailReplyParser.Lib.Extensions
{
    public static class StringExtenstions
    {
        public static string Reverse(this string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
