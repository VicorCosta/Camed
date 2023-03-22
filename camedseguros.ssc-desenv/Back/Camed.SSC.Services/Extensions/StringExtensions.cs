using System.Text.RegularExpressions;

namespace Camed.SSC.Application.Extensions
{
    public static class StringExtensions
    {
        public static string SomenteNumeros(this string input)
        {
            return Regex.Replace(input, @"[^\d]", "");
        }
    }
}
