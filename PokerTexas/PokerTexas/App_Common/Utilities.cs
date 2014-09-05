using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
namespace PokerTexas.App_Common
{
    public class Utilities
    {
        #region - DECALRE -
        #endregion
        #region - PROPERTIES -
        #endregion
        #region - CONTRUCTOR -
        #endregion
        #region - METHOD -
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public static void SerializeObject(string filename, object objectToSerialize)
        {
            try
            {
                Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();
                stream.Dispose();
            }
            catch { }
        }

        public static object DeSerializeObject(string filename)
        {
            try
            {
                object objectToSerialize;
                if (!File.Exists(filename)) return null;
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = bFormatter.Deserialize(stream);
                stream.Close();
                stream.Dispose();
                return objectToSerialize;
            }
            catch { }
            return null;
        }

        public static byte[] ConvertObjectToBlob(object objectToConvert)
        {
            if (objectToConvert == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, objectToConvert);
            return ms.ToArray();

        }

        public static object ConvertBlobToObject(byte[] blob)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(blob);
            return bf.Deserialize(ms);
        }

        public static string ConvertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string getSigPoker(object paramObject, string paramString)
        {
            string localObject1 = "V";
            if (((paramObject is bool)) || (paramObject == null) || ((paramObject is byte[])))
            {
                return "V";
            }
            if (((paramObject is string)) || ((paramObject is int)))
            {
                localObject1 = localObject1 + "T" + paramString + Regex.Replace(paramObject.ToString(), "[^0-9a-zA-Z]", string.Empty);
                return localObject1;
            }
            if (paramObject is SortedDictionary<string, object>)
            {
                foreach (KeyValuePair<string, object> item in paramObject as SortedDictionary<string, object>)
                {
                    localObject1 = localObject1 + item.Key + "=" + getSigPoker(item.Value, paramString);
                }
            }
            return localObject1;
        }

        public static string getSignFB(NameValueCollection param,string secret)
        {
            SortedDictionary<string, string> dicParam = new SortedDictionary<string, string>();
            foreach (string key in param)
            {
                var value = param[key];
                dicParam.Add(key, param[key]);
            }

            StringBuilder t = new StringBuilder(256);
            string tt = null;
            t.Append(tt);

            foreach (KeyValuePair<string, string> item in dicParam)
            {
                t.Append(item.Key);
                t.Append("=");
                t.Append(item.Value);
            }
            t.Append(secret);
            //secrett.Append("62f8ce9f74b12f84c123cc23437a4a32");
            return FBHash(t.ToString());
        }

        public static string FBHash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] a = {
                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 
                    97, 98, 99, 100, 101, 102
            };
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder(data.Length * 2);

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                int j = 0xff & data[i];
                sBuilder.Append((char)a[j >> 4]);
                sBuilder.Append((char)a[j & 0xf]);

            }
            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public static string GetCurrentSecond()
        {
            return ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString();
        }
        #endregion
    }
}

