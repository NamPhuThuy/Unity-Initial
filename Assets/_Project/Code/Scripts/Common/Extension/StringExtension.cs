using System.Text;

namespace NamPhuThuy.Common
{
    public static class StringExtension
    {
        public static string FillAfter(this string str, string fill, int maxLength)
        {
            return str + fill.Loop(maxLength - str.Length);
        }

        public static string FillBefore(this string str, string fill, int maxLength)
        {
            return fill.Loop(maxLength - str.Length) + str;
        }

        public static string Loop(this string str, int loop)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < loop; i++)
            {
                builder.Append(str);
            }
            return builder.ToString();
        }
    }
}