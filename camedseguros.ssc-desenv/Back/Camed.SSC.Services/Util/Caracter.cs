using Camed.SSC.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Camed.SSC.Application.Util
{
    public class Caracter : ICaracter
    {
        public string RemoveSpecialCharactersRegex(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public string RemoveSpecialCharacter2(string stringToReplace)
        {
            if (stringToReplace == null)
                return null;

            return stringToReplace.ToLower()
                .Replace(" ", "")
                .Replace("!", "")
                .Replace("!", "")
                .Replace("-", "")
                .Replace("-", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("%", "")
                .Replace("&", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(".", "")
                .Replace("@", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("^", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("~", "")
                .Replace("+", "")
                .Replace("=", "")
                .Replace("§", "")
                .Replace("ª", "")
                .Replace("á", "a")
                .Replace("à", "a")
                .Replace("â", "a")
                .Replace("ä", "a")
                .Replace("ã", "a")
                .Replace("å", "a")
                .Replace("ç", "c")
                .Replace("é", "e")
                .Replace("è", "e")
                .Replace("ê", "e")
                .Replace("ë", "e")
                .Replace("í", "i")
                .Replace("ì", "i")
                .Replace("î", "i")
                .Replace("ï", "i")
                .Replace("ñ", "n")
                .Replace("º", "")
                .Replace("ó", "o")
                .Replace("ò", "o")
                .Replace("ô", "o")
                .Replace("ö", "o")
                .Replace("õ", "o")
                .Replace("ß", "")
                .Replace("ú", "u")
                .Replace("ù", "u")
                .Replace("û", "u")
                .Replace("ü", "u")
                .Replace("ý", "y");
        }
    }
}
