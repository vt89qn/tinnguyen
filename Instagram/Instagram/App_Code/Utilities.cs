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
    #endregion
}

