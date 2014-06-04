using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FaceBookNuker
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
                if (!File.Exists(filename)) return false;
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
        public static void FeedMail()
        {
            try
            {
                Pop3Client pop3 = new Pop3Client();
                pop3.Connect("mx1.hostinger.vn", 110, false);
                pop3.Authenticate("mail@tinphuong.me", "ngvantin");
                int count = pop3.GetMessageCount();
                Dictionary<string, string> dicMail = new Dictionary<string, string>();
                for (int iIndex = 1; iIndex <= count; iIndex++)
                {
                    OpenPop.Mime.Message message = pop3.GetMessage(iIndex);
                    var strBody = message.MessagePart.BodyEncoding.GetString(message.MessagePart.Body);
                    string strLink = Regex.Match(strBody, @"c.php\?code=[^\s]+").Value;
                    if (!dicMail.ContainsKey(message.Headers.To[0].MailAddress.Address))
                    {
                        dicMail.Add(message.Headers.To[0].MailAddress.Address, strLink);
                    }
                }
                //pop3.DeleteAllMessages();
                //string strEmails = string.Empty;
                //foreach (var item in dicMail)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //    strEmails += (string.IsNullOrEmpty(strEmails) ? "" : "\r\n") + item.Key;
                //    client = new WebClientEx();
                //    string strResponse = client.DoGet("https://www.facebook.com");
                //    strResponse = client.DoGet("https://www.facebook.com/" + item.Value);
                //}
            }
            catch
            {
            }
        }
        #endregion
    }
    public static class TMessage
    {
        #region - DECLARE -
        private static string strCaptionMesageBox = "FaceBook Nuker";
        #endregion
        #region - PROPERTY -
        #endregion
        #region - CONTRUCTOR -
        #endregion
        #region - METHOD -
        public static void ShowException(Exception ex)
        {
            MessageBox.Show("Rất tiếc, đã có lỗi xảy ra !\n - Nội dung lỗi : \n\"" + ex.Message + "\"\n - Tracking : \n" + ex.StackTrace, strCaptionMesageBox, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        public static DialogResult ShowQuestion(string strMessage, MessageBoxButtons msgButton)
        {
            return MessageBox.Show(strMessage, strCaptionMesageBox, msgButton, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
        public static void ShowError(string strMessage)
        {
            MessageBox.Show(strMessage, strCaptionMesageBox, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        public static void ShowInfomation(string strMessage)
        {
            MessageBox.Show(strMessage, strCaptionMesageBox, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        #endregion
    }
}
