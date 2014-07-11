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

    public static string GenerationNewName(bool bMale)
    {
        using (WebClientEx client = new WebClientEx())
        {
            client.DoGet("http://www.fakenamegenerator.com/gen-" + (bMale ? "" : "fe") + "male-en-us.php");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                string strName = Regex.Match(client.ResponseText, "<div class=\"address\">[\\r\\n\\s]+<h3>(?<1>.+)</h3>").Groups[1].Value;
                strName = Regex.Replace(strName, @" [a-z]\. ", " ", RegexOptions.IgnoreCase);
                return strName;
            }
        }
        return string.Empty;
    }
    public static string ConvertToUnSign3(string s)
    {
        Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        string temp = s.Normalize(NormalizationForm.FormD);
        return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }
    #endregion
}

