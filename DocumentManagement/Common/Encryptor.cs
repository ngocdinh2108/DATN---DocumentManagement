using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DocumentManagement.Common
{
    public class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string EnCrypt(string strEnCrypt, string key = "MD5")
        {
            try
            {
                byte[] keyArr;
                byte[] EnCryptArr = UTF8Encoding.UTF8.GetBytes(strEnCrypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
                return Convert.ToBase64String(arrResult, 0, arrResult.Length);
            }
            catch (Exception ex) { }
            return "";
        }

        public static string DeCrypt(string strDecypt, string key = "MD5")
        {
            try
            {
                byte[] keyArr;
                byte[] DeCryptArr = Convert.FromBase64String(strDecypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateDecryptor();
                byte[] arrResult = transform.TransformFinalBlock(DeCryptArr, 0, DeCryptArr.Length);
                return UTF8Encoding.UTF8.GetString(arrResult);
            }
            catch (Exception ex) { }
            return "";
        }
    }
}