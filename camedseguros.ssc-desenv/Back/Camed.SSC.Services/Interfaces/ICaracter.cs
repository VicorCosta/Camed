using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Interfaces
{
    public interface ICaracter
    {
        string RemoveSpecialCharactersRegex(string str);
        string RemoveSpecialCharacter2(string stringToReplace);
    }
}
