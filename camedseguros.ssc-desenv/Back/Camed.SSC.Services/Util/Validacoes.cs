using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Util
{
    public static class Validacoes
    {
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return true;

            string valor = cpf.Replace(".", "").Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = 11 - (soma % 11);

            if (resultado >= 10)
                resultado = 0;

            if (numeros[9] != resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = 11 - (soma % 11);

            if (resultado >= 10)
                resultado = 0;

            if (numeros[10] != resultado)
                return false;

            return true;
        }

        public static bool ValidaCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma, resto;
            string digito, tempCnpj;
            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static bool ValidaCPFCNPJ(string cpfcnpj)
        {
            if (string.IsNullOrEmpty(cpfcnpj))
                return true;

            cpfcnpj = cpfcnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cpfcnpj.Length <= 11)
            {
                return ValidarCPF(cpfcnpj);
            }
            else if (cpfcnpj.Length == 14)
            {
                return ValidaCNPJ(cpfcnpj);
            }
            else
            {
                throw new ApplicationException("Quantidade de caracteres não corresponde a CPF ou CNPJ.");
            }
        }
    }
}
