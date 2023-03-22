﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Util
{
    public static class FormatCnpjCpf
    {
        public static string FormatCNPJQuiver(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"000\.000\.000\/0000\-00");
        }

        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormatCPF(string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }

        public static string SemFormatacao(string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }
    }
}
