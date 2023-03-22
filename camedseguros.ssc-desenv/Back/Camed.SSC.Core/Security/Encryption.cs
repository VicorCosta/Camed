using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Camed.SSC.Core.Security
{
    public class Encryption
    {
        private static byte[] chave = new byte[] { };
        private static byte[] iv = new byte[] { 12, 34, 56, 78, 90, 102, 114, 126 };
        private const string chaveCriptografia = "Amanhã é um novo dia.";

        /// <summary>
        /// Encrypt for the MD5
        /// </summary>
        /// <param name="Valor">Valor a ser criptografado</param>
        /// <returns>String Criptografada</returns>
        public static string EncryptMD5(string Valor)
        {
            string strResultado = "";

            byte[] bytMensagem = Encoding.ASCII.GetBytes(Valor);

            // Cria o Hash MD5 hash
            MD5CryptoServiceProvider oMD5Provider = new MD5CryptoServiceProvider();

            // Gera o Hash Code
            byte[] bytHashCode = oMD5Provider.ComputeHash(bytMensagem);

            for (int iItem = 0; iItem < bytHashCode.Length; iItem++)
            {
                strResultado += (char)(bytHashCode[iItem]);
            }

            return strResultado;
        }

        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Encrypt value
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string Encrypt(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = Encoding.UTF8.GetBytes(valor);
                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateEncryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Decrypt value
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string Decrypt(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[valor.Length];
                input = Convert.FromBase64String(valor.Replace(" ", "+"));
                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));
                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
