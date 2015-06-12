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
using PokerTexas.App_UserControl;
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
        public static string EncodeString(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return string.Empty;
                return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            }
            catch
            {
                return input;
            }
        }

        public static string DecodeString(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return string.Empty;
                return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input));
            }
            catch
            {
                return input;
            }
        }

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
            if (blob != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(blob);
                return bf.Deserialize(ms);
            }
            return null;
        }

        public static string ConvertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string ConvertToUsignNew(string s)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặDéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶDÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = s.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(s[index]);
                s = s.Replace(s[index], ReplText[index2]);
            }
            return s;
        }

        public static string getSigPoker(object paramObject, string paramString, string skey)
        {
            string localObject1 = skey;
            if (((paramObject is bool)) || (paramObject == null) || ((paramObject is byte[])))
            {
                return localObject1;
            }
            if (((paramObject is string)) || ((paramObject is int)))
            {
                localObject1 = localObject1 + "T" + paramString + Regex.Replace(paramObject.ToString(), "[^0-9a-zA-Z]", string.Empty);
                return localObject1;
            }
            if (paramObject is IDictionary<string, object>)
            {
                foreach (KeyValuePair<string, object> item in paramObject as IDictionary<string, object>)
                {
                    localObject1 = localObject1 + item.Key + "=" + getSigPoker(item.Value, paramString, skey);
                }
            }
            return localObject1;
        }

        public static string getSignFB(NameValueCollection param, string secret)
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

        public static string GetMaleName()
        {
            #region - Name -
            string stockName = @"An Cơ	An Khang	Ân Lai	An Nam
An Nguyên	An Ninh	An Tâm	Ân Thiện
An Tường	Anh Dức	Anh Dũng	Anh Duy
Anh Hoàng	Anh Khải	Anh Khoa	Anh Khôi
Anh Minh	Anh Quân	Anh Quốc	Anh Sơn
Anh Tài	Anh Thái	Anh Tú	Anh Tuấn
Anh Tùng	Anh Việt	Anh Vũ	Bá Cường
Bá Kỳ	Bá Lộc	Bá Long	Bá Phước
Bá Thành	Bá Thiện	Bá Thịnh	Bá Thúc
Bá Trúc	Bá Tùng	Bách Du	Bách Nhân
Bằng Sơn	Bảo An	Bảo Bảo	Bảo Chấn
Bảo Dịnh	Bảo Duy	Bảo Giang	Bảo Hiển
Bảo Hoa	Bảo Hoàng	Bảo Huy	Bảo Huynh
Bảo Huỳnh	Bảo Khánh	Bảo Lâm	Bảo Long
Bảo Pháp	Bảo Quốc	Bảo Sơn	Bảo Thạch
Bảo Thái	Bảo Tín	Bảo Toàn	Bích Nhã
Bình An	Bình Dân	Bình Dạt	Bình Dịnh
Bình Dương	Bình Hòa	Bình Minh	Bình Nguyên
Bình Quân	Bình Thuận	Bình Yên	Bửu Chưởng
Bửu Diệp	Bữu Toại	Cảnh Tuấn	Cao Kỳ
Cao Minh	Cao Nghiệp	Cao Nguyên	Cao Nhân
Cao Phong	Cao Sĩ	Cao Sơn	Cao Sỹ
Cao Thọ	Cao Tiến	Cát Tường	Cát Uy
Chấn Hùng	Chấn Hưng	Chấn Phong	Chánh Việt
Chế Phương	Chí Anh	Chí Bảo	Chí Công
Chí Dũng	Chí Giang	Chí Hiếu	Chí Khang
Chí Khiêm	Chí Kiên	Chí Nam	Chí Sơn
Chí Thanh	Chí Thành	Chiến Thắng	Chiêu Minh
Chiêu Phong	Chiêu Quân	Chính Tâm	Chính Thuận
Chính Trực	Chuẩn Khoa	Chung Thủy	Công Án
Công Ân	Công Bằng	Công Giang	Công Hải
Công Hào	Công Hậu	Công Hiếu	Công Hoán
Công Lập	Công Lộc	Công Luận	Công Luật
Công Lý	Công Phụng	Công Sinh	Công Sơn
Công Thành	Công Tráng	Công Tuấn	Cường Dũng
Cương Nghị	Cương Quyết	Cường Thịnh	Dắc Cường
Dắc Di	Dắc Lộ	Dắc Lực	Dắc Thái
Dắc Thành	Dắc Trọng	Dại Dương	Dại Hành
Dại Ngọc	Dại Thống	Dân Hiệp	Dân Khánh
Dan Quế	Dan Tâm	Dăng An	Dăng Dạt
Dăng Khánh	Dăng Khoa	Dăng Khương	Dăng Minh
Dăng Quang	Danh Nhân	Danh Sơn	Danh Thành
Danh Văn	Dạt Dũng	Dạt Hòa	Dình Chiểu
Dình Chương	Dình Cường	Dình Diệu	Dình Dôn
Dình Dương	Dình Hảo	Dình Hợp	Dình Kim
Dinh Lộc	Dình Lộc	Dình Luận	Dịnh Lực
Dình Nam	Dình Ngân	Dình Nguyên	Dình Nhân
Dình Phú	Dình Phúc	Dình Quảng	Dình Sang
Dịnh Siêu	Dình Thắng	Dình Thiện	Dình Toàn
Dình Trung	Dình Tuấn	Doàn Tụ	Dồng Bằng
Dông Dương	Dông Hải	Dồng Khánh	Dông Nguyên
Dông Phong	Dông Phương	Dông Quân	Dông Sơn
Dức Ân	Dức Anh	Dức Bằng	Dức Bảo
Dức Bình	Dức Chính	Dức Duy	Dức Giang
Dức Hải	Dức Hạnh	Dức Hòa	Dức Hòa
Dức Huy	Dức Khải	Dức Khang	Dức Khiêm
Dức Kiên	Dức Long	Dức Mạnh	Dức Minh
Dức Nhân	Dức Phi	Dức Phong	Dức Phú
Dức Quang	Dức Quảng	Dức Quyền	Dức Siêu
Dức Sinh	Dức Tài	Dức Tâm	Dức Thắng
Dức Thành	Dức Thọ	Dức Toàn	Dức Toản
Dức Trí	Dức Trung	Dức Tuấn	Dức Tuệ
Dức Tường	Dũng Trí	Dũng Việt	Dương Anh
Dương Khánh	Duy An	Duy Bảo	Duy Cẩn
Duy Cường	Duy Hải	Duy Hiền	Duy Hiếu
Duy Hoàng	Duy Hùng	Duy Khang	Duy Khánh
Duy Khiêm	Duy Kính	Duy Luận	Duy Mạnh
Duy Minh	Duy Ngôn	Duy Nhượng	Duy Quang
Duy Tâm	Duy Tân	Duy Thạch	Duy Thắng
Duy Thanh	Duy Thành	Duy Thông	Duy Tiếp
Duy Tuyền	Gia Ân	Gia Anh	Gia Bạch
Gia Bảo	Gia Bình	Gia Cần	Gia Cẩn
Gia Cảnh	Gia Dạo	Gia Dức	Gia Hiệp
Gia Hòa	Gia Hoàng	Gia Huấn	Gia Hùng
Gia Hưng	Gia Huy	Gia Khánh	Gia Khiêm
Gia Kiên	Gia Kiệt	Gia Lập	Gia Minh
Gia Nghị	Gia Phong	Gia Phúc	Gia Phước
Gia Thiện	Gia Thịnh	Gia Uy	Gia Vinh
Giang Lam	Giang Nam	Giang Sơn	Giang Thiên
Hà Hải	Hải Bằng	Hải Bình	Hải Dăng
Hải Dương	Hải Giang	Hải Hà	Hải Long
Hải Lý	Hải Nam	Hải Nguyên	Hải Phong
Hải Quân	Hải Sơn	Hải Thụy	Hán Lâm
Hạnh Tường	Hào Nghiệp	Hạo Nhiên	Hiền Minh
Hiệp Dinh	Hiệp Hà	Hiệp Hào	Hiệp Hiền
Hiệp Hòa	Hiệp Vũ	Hiếu Dụng	Hiếu Học
Hiểu Lam	Hiếu Liêm	Hiếu Nghĩa	Hiếu Phong
Hiếu Thông	Hồ Bắc	Hồ Nam	Hòa Bình
Hòa Giang	Hòa Hiệp	Hòa Hợp	Hòa Lạc
Hòa Thái	Hoài Bắc	Hoài Nam	Hoài Phong
Hoài Thanh	Hoài Tín	Hoài Trung	Hoài Việt
Hoài Vỹ	Hoàn Kiếm	Hoàn Vũ	Hoàng Ân
Hoàng Duệ	Hoàng Dũng	Hoàng Giang	Hoàng Hải
Hoàng Hiệp	Hoàng Khải	Hoàng Khang	Hoàng Khôi
Hoàng Lâm	Hoàng Linh	Hoàng Long	Hoàng Minh
Hoàng Mỹ	Hoàng Nam	Hoàng Ngôn	Hoàng Phát
Hoàng Quân	Hoàng Thái	Hoàng Việt	Hoàng Xuân
Hồng Dăng	Hồng Dức	Hồng Giang	Hồng Lân
Hồng Liêm	Hồng Lĩnh	Hồng Minh	Hồng Nhật
Hồng Nhuận	Hồng Phát	Hồng Quang	Hồng Quý
Hồng Sơn	Hồng Thịnh	Hồng Thụy	Hồng Việt
Hồng Vinh	Huân Võ	Hùng Anh	Hùng Cường
Hưng Dạo	Hùng Dũng	Hùng Ngọc	Hùng Phong
Hùng Sơn	Hùng Thịnh	Hùng Tường	Hướng Bình
Hướng Dương	Hướng Thiện	Hướng Tiền	Hữu Bào
Hữu Bảo	Hữu Bình	Hữu Canh	Hữu Cảnh
Hữu Châu	Hữu Chiến	Hữu Cương	Hữu Cường
Hữu Dạt	Hữu Dịnh	Hữu Hạnh	Hữu Hiệp
Hữu Hoàng	Hữu Hùng	Hữu Khang	Hữu Khanh
Hữu Khoát	Hữu Khôi	Hữu Long	Hữu Lương
Hữu Minh	Hữu Nam	Hữu Nghị	Hữu Nghĩa
Hữu Phước	Hữu Tài	Hữu Tâm	Hữu Tân
Hữu Thắng	Hữu Thiện	Hữu Thọ	Hữu Thống
Hữu Thực	Hữu Toàn	Hữu Trác	Hữu Trí
Hữu Trung	Hữu Từ	Hữu Tường	Hữu Vĩnh
Hữu Vượng	Huy Anh	Huy Chiểu	Huy Hà
Huy Hoàng	Huy Kha	Huy Khánh	Huy Khiêm
Huy Lĩnh	Huy Phong	Huy Quang	Huy Thành
Huy Thông	Huy Trân	Huy Tuấn	Huy Tường
Huy Việt	Huy Vũ	Khắc Anh	Khắc Công
Khắc Dũng	Khắc Duy	Khắc Kỷ	Khắc Minh
Khắc Ninh	Khắc Thành	Khắc Triệu	Khắc Trọng
Khắc Tuấn	Khắc Việt	Khắc Vũ	Khải Ca
Khải Hòa	Khai Minh	Khải Tâm	Khải Tuấn
Khang Kiện	Khánh An	Khánh Bình	Khánh Dan
Khánh Duy	Khánh Giang	Khánh Hải	Khánh Hòa
Khánh Hoàn	Khánh Hoàng	Khánh Hội	Khánh Huy
Khánh Minh	Khánh Nam	Khánh Văn	Khoa Trưởng
Khôi Nguyên	Khởi Phong	Khôi Vĩ	Khương Duy
Khuyến Học	Kiên Bình	Kiến Bình	Kiên Cường
Kiến Dức	Kiên Giang	Kiên Lâm	Kiên Trung
Kiến Văn	Kiệt Võ	Kim Dan	Kim Hoàng
Kim Long	Kim Phú	Kim Sơn	Kim Thịnh
Kim Thông	Kim Toàn	Kim Vượng	Kỳ Võ
Lạc Nhân	Lạc Phúc	Lâm Dồng	Lâm Dũng
Lam Giang	Lam Phương	Lâm Trường	Lâm Tường
Lâm Viên	Lâm Vũ	Lập Nghiệp	Lập Thành
Liên Kiệt	Long Giang	Long Quân	Long Vịnh
Lương Quyền	Lương Tài	Lương Thiện	Lương Tuyền
Mạnh Cương	Mạnh Cường	Mạnh Dình	Mạnh Dũng
Mạnh Hùng	Mạnh Nghiêm	Mạnh Quỳnh	Mạnh Tấn
Mạnh Thắng	Mạnh Thiện	Mạnh Trình	Mạnh Trường
Mạnh Tuấn	Mạnh Tường	Minh Ân	Minh Anh
Minh Cảnh	Minh Dân	Minh Dan	Minh Danh
Minh Dạt	Minh Dức	Minh Dũng	Minh Giang
Minh Hải	Minh Hào	Minh Hiên	Minh Hiếu
Minh Hòa	Minh Hoàng	Minh Huấn	Minh Hùng
Minh Hưng	Minh Huy	Minh Hỷ	Minh Khang
Minh Khánh	Minh Khiếu	Minh Khôi	Minh Kiệt
Minh Kỳ	Minh Lý	Minh Mẫn	Minh Nghĩa
Minh Nhân	Minh Nhật	Minh Nhu	Minh Quân
Minh Quang	Minh Quốc	Minh Sơn	Minh Tân
Minh Thạc	Minh Thái	Minh Thắng	Minh Thiện
Minh Thông	Minh Thuận	Minh Tiến	Minh Toàn
Minh Trí	Minh Triết	Minh Triệu	Minh Trung
Minh Tú	Minh Tuấn	Minh Vu	Minh Vũ
Minh Vương	Mộng Giác	Mộng Hoàn	Mộng Lâm
Mộng Long	Nam An	Nam Dương	Nam Hải
Nam Hưng	Nam Lộc	Nam Nhật	Nam Ninh
Nam Phi	Nam Phương	Nam Sơn	Nam Thanh
Nam Thông	Nam Tú	Nam Việt	Nghị Lực
Nghị Quyền	Nghĩa Dũng	Nghĩa Hòa	Ngọc Ẩn
Ngọc Cảnh	Ngọc Cường	Ngọc Danh	Ngọc Doàn
Ngọc Dũng	Ngọc Hải	Ngọc Hiển	Ngọc Huy
Ngọc Khang	Ngọc Khôi	Ngọc Khương	Ngọc Lai
Ngọc Lân	Ngọc Minh	Ngọc Ngạn	Ngọc Quang
Ngọc Sơn	Ngọc Thạch	Ngọc Thiện	Ngọc Thọ
Ngọc Thuận	Ngọc Tiển	Ngọc Trụ	Ngọc Tuấn
Nguyên Bảo	Nguyên Bổng	Nguyên Dan	Nguyên Giang
Nguyên Giáp	Nguyễn Hải An	Nguyên Hạnh	Nguyên Khang
Nguyên Khôi	Nguyên Lộc	Nguyên Nhân	Nguyên Phong
Nguyên Sử	Nguyên Văn	Nhân Nguyên	Nhân Sâm
Nhân Từ	Nhân Văn	Nhật Bảo Long	Nhật Dũng
Nhật Duy	Nhật Hòa	Nhật Hoàng	Nhật Hồng
Nhật Hùng	Nhật Huy	Nhật Khương	Nhật Minh
Nhật Nam	Nhật Quân	Nhật Quang	Nhật Quốc
Nhật Tấn	Nhật Thịnh	Nhất Tiến	Nhật Tiến
Như Khang	Niệm Nhiên	Phi Cường	Phi Diệp
Phi Hải	Phi Hoàng	Phi Hùng	Phi Long
Phi Nhạn	Phong Châu	Phong Dinh	Phong Dộ
Phú Ân	Phú Bình	Phú Hải	Phú Hiệp
Phú Hùng	Phú Hưng	Phú Thịnh	Phú Thọ
Phú Thời	Phúc Cường	Phúc Diền	Phúc Duy
Phúc Hòa	Phúc Hưng	Phúc Khang	Phúc Lâm
Phục Lễ	Phúc Nguyên	Phúc Sinh	Phúc Tâm
Phúc Thịnh	Phụng Việt	Phước An	Phước Lộc
Phước Nguyên	Phước Nhân	Phước Sơn	Phước Thiện
Phượng Long	Phương Nam	Phương Phi	Phương Thể
Phương Trạch	Phương Triều	Quân Dương	Quang Anh
Quang Bửu	Quảng Dại	Quang Danh	Quang Dạt
Quảng Dạt	Quang Dức	Quang Dũng	Quang Dương
Quang Hà	Quang Hải	Quang Hòa	Quang Hùng
Quang Hưng	Quang Hữu	Quang Huy	Quang Khải
Quang Khanh	Quang Lâm	Quang Lân	Quang Linh
Quang Lộc	Quang Minh	Quang Nhân	Quang Nhật
Quang Ninh	Quang Sáng	Quang Tài	Quang Thạch
Quang Thái	Quang Thắng	Quang Thiên	Quang Thịnh
Quảng Thông	Quang Thuận	Quang Triều	Quang Triệu
Quang Trọng	Quang Trung	Quang Trường	Quang Tú
Quang Tuấn	Quang Vinh	Quang Vũ	Quang Xuân
Quốc Anh	Quốc Bảo	Quốc Bình	Quốc Dại
Quốc Diền	Quốc Hải	Quốc Hạnh	Quốc Hiền
Quốc Hiển	Quốc Hòa	Quốc Hoài	Quốc Hoàng
Quốc Hùng	Quốc Hưng	Quốc Huy	Quốc Khánh
Quốc Mạnh	Quốc Minh	Quốc Mỹ	Quốc Phong
Quốc Phương	Quốc Quân	Quốc Quang	Quốc Quý
Quốc Thắng	Quốc Thành	Quốc Thiện	Quốc Thịnh
Quốc Thông	Quốc Tiến	Quốc Toản	Quốc Trụ
Quốc Trung	Quốc Trường	Quốc Tuấn	Quốc Văn
Quốc Việt	Quốc Vinh	Quốc Vũ	Quý Khánh
Quý Vĩnh	Quyết Thắng	Sĩ Hoàng	Sơn Dương
Sơn Giang	Sơn Hà	Sơn Hải	Sơn Lâm
Sơn Quân	Sơn Quyền	Sơn Trang	Sơn Tùng
Song Lam	Sỹ Dan	Sỹ Hoàng	Sỹ Phú
Sỹ Thực	Tạ Hiền	Tài Dức	Tài Nguyên
Tâm Thiện	Tân Bình	Tân Dịnh	Tấn Dũng
Tấn Khang	Tấn Lợi	Tân Long	Tấn Nam
Tấn Phát	Tân Phước	Tấn Sinh	Tấn Tài
Tân Thành	Tấn Thành	Tấn Trình	Tấn Trương
Tất Bình	Tất Hiếu	Tất Hòa	Thạch Sơn
Thạch Tùng	Thái Bình	Thái Dức	Thái Dương
Thái Duy	Thái Hòa	Thái Minh	Thái Nguyên
Thái San	Thái Sang	Thái Sơn	Thái Tân
Thái Tổ	Thắng Cảnh	Thắng Lợi	Thăng Long
Thành An	Thành Ân	Thành Châu	Thành Công
Thành Danh	Thanh Dạo	Thành Dạt	Thành Dệ
Thanh Doàn	Thành Doanh	Thanh Hải	Thanh Hào
Thanh Hậu	Thành Hòa	Thanh Huy	Thành Khiêm
Thanh Kiên	Thanh Liêm	Thành Lợi	Thanh Long
Thành Long	Thanh Minh	Thành Nguyên	Thành Nhân
Thanh Phi	Thanh Phong	Thành Phương	Thanh Quang
Thành Sang	Thanh Sơn	Thanh Thế	Thanh Thiên
Thành Thiện	Thanh Thuận	Thành Tín	Thanh Tịnh
Thanh Toàn	Thanh Toản	Thanh Trung	Thành Trung
Thanh Tú	Thanh Tuấn	Thanh Tùng	Thanh Việt
Thanh Vinh	Thành Vinh	Thanh Vũ	Thành Ý
Thất Cương	Thất Dũng	Thất Thọ	Thế An
Thế Anh	Thế Bình	Thế Dân	Thế Doanh
Thế Dũng	Thế Duyệt	Thế Huấn	Thế Hùng
Thế Lâm	Thế Lực	Thế Minh	Thế Năng
Thế Phúc	Thế Phương	Thế Quyền	Thế Sơn
Thế Trung	Thế Tường	Thế Vinh	Thiên An
Thiên Ân	Thiện Ân	Thiên Bửu	Thiên Dức
Thiện Dức	Thiện Dũng	Thiện Giang	Thiên Hưng
Thiện Khiêm	Thiên Lạc	Thiện Luân	Thiên Lương
Thiện Lương	Thiên Mạnh	Thiện Minh	Thiện Ngôn
Thiên Phú	Thiện Phước	Thiện Sinh	Thiện Tâm
Thiện Thanh	Thiện Tính	Thiên Trí	Thiếu Anh
Thiệu Bảo	Thiếu Cường	Thịnh Cường	Thời Nhiệm
Thông Dạt	Thông Minh	Thống Nhất	Thông Tuệ
Thụ Nhân	Thu Sinh	Thuận Anh	Thuận Hòa
Thuận Phong	Thuận Phương	Thuận Thành	Thuận Toàn
Thượng Cường	Thượng Khang	Thường Kiệt	Thượng Liệt
Thượng Năng	Thượng Nghị	Thượng Thuật	Thường Xuân
Thụy Du	Thụy Long	Thụy Miên	Thụy Vũ
Tích Dức	Tích Thiện	Tiến Dức	Tiến Dũng
Tiền Giang	Tiến Hiệp	Tiến Hoạt	Tiến Võ
Tiểu Bảo	Toàn Thắng	Tôn Lễ	Trí Dũng
Trí Hào	Trí Hùng	Trí Hữu	Trí Liên
Trí Minh	Trí Thắng	Trí Tịnh	Triển Sinh
Triệu Thái	Triều Thành	Trọng Chính	Trọng Dũng
Trọng Duy	Trọng Hà	Trọng Hiếu	Trọng Hùng
Trọng Khánh	Trọng Kiên	Trọng Nghĩa	Trọng Nhân
Trọng Tấn	Trọng Trí	Trọng Tường	Trọng Việt
Trọng Vinh	Trúc Cương	Trúc Sinh	Trung Anh
Trung Chính	Trung Chuyên	Trung Dức	Trung Dũng
Trung Hải	Trung Hiếu	Trung Kiên	Trung Lực
Trung Nghĩa	Trung Nguyên	Trung Nhân	Trung Thành
Trung Thực	Trung Việt	Trường An	Trường Chinh
Trường Giang	Trường Hiệp	Trường Kỳ	Trường Liên
Trường Long	Trường Nam	Trường Nhân	Trường Phát
Trường Phu	Trường Phúc	Trường Sa	Trường Sinh
Trường Sơn	Trường Thành	Trường Vinh	Trường Vũ
Từ Dông	Tuấn Anh	Tuấn Châu	Tuấn Chương
Tuấn Dức	Tuấn Dũng	Tuấn Hải	Tuấn Hoàng
Tuấn Hùng	Tuấn Khải	Tuấn Khanh	Tuấn Khoan
Tuấn Kiệt	Tuấn Linh	Tuấn Long	Tuấn Minh
Tuấn Ngọc	Tuấn Sĩ	Tuấn Sỹ	Tuấn Tài
Tuấn Thành	Tuấn Trung	Tuấn Tú	Tuấn Việt
Tùng Anh	Tùng Châu	Tùng Lâm	Tùng Linh
Tùng Minh	Tùng Quang	Tường Anh	Tường Lâm
Tường Lân	Tường Lĩnh	Tường Minh	Tường Nguyên
Tường Phát	Tường Vinh	Tuyền Lâm	Uy Phong
Uy Vũ	Vạn Hạnh	Vạn Lý	Văn Minh
Vân Sơn	Vạn Thắng	Vạn Thông	Văn Tuyển
Viễn Cảnh	Viễn Dông	Viễn Phương	Viễn Thông
Việt An	Việt Anh	Việt Chính	Việt Cương
Việt Cường	Việt Dũng	Việt Dương	Việt Duy
Việt Hải	Việt Hoàng	Việt Hồng	Việt Hùng
Việt Huy	Việt Khải	Việt Khang	Việt Khoa
Việt Khôi	Việt Long	Việt Ngọc	Viết Nhân
Việt Nhân	Việt Phong	Việt Phương	Việt Quốc
Việt Quyết	Viết Sơn	Việt Sơn	Viết Tân
Việt Thái	Việt Thắng	Việt Thanh	Việt Thông
Việt Thương	Việt Tiến	Việt Võ	Vĩnh Ân
Vinh Diệu	Vĩnh Hải	Vĩnh Hưng	Vĩnh Long
Vĩnh Luân	Vinh Quốc	Vĩnh Thọ	Vĩnh Thụy
Vĩnh Toàn	Vũ Anh	Vũ Minh	Vương Gia
Vương Triều	Vương Triệu	Vương Việt	Xuân An
Xuân Bình	Xuân Cao	Xuân Cung	Xuân Hàm
Xuân Hãn	Xuân Hiếu	Xuân Hòa	Xuân Huy
Xuân Khoa	Xuân Kiên	Xuân Lạc	Xuân Lộc
Xuân Minh	Xuân Nam	Xuân Ninh	Xuân Phúc
Xuân Quân	Xuân Quý	Xuân Sơn	Xuân Thái
Xuân Thiện	Xuân Thuyết	Xuân Trung	Xuân Trường
Xuân Tường	Xuân Vũ	Yên Bằng	Yên Bình
Yên Sơn";
            #endregion
            stockName = stockName.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim();
            List<string> listHo = new List<string>();
            List<string> listTen = new List<string>();
            var names = stockName.Split(' ');
            for (int iIndex = 0; iIndex < names.Length - 1; iIndex += 2)
            {
                listHo.Add(names[iIndex]);
                listTen.Add(names[iIndex + 1]);
            }
            string strName = listHo[new Random().Next(listHo.Count - 1)].Trim();
            System.Threading.Thread.Sleep(23);
            strName += " " + listTen[new Random().Next(listTen.Count - 1)].Trim();
            System.Threading.Thread.Sleep(23);
            strName += " " + listTen[new Random().Next(listTen.Count - 1)].Trim();
            System.Threading.Thread.Sleep(23);
            return strName;
        }

        /// <summary>
        /// 1 : name="key" value="value"
        /// 2 : "name":"key"
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="iType"></param>
        /// <returns></returns>
        public static string GetRegexString(string input, string key, int iType)
        {
            string value = string.Empty;
            if (iType == 1)
            {
                value = Regex.Match(input, "name=([\\\\]+|)(\"|')" + key + "([\\\\]+|)(\"|') value=([\\\\]+|)(\"|')(?<val>[^'\"\\\\]+)([\\\\]+|)(\"|')", RegexOptions.IgnoreCase).Groups["val"].Value;
            }
            else if (iType == 2)
            {
                value = Regex.Match(input, "([\\\\]+|)(\"|')" + key + "([\\\\]+|)(\"|')([\\s]+|):([\\s]+|)([\\\\]+|)(\"|')(?<val>[^'\"\\\\]+)([\\\\]+|)(\"|')", RegexOptions.IgnoreCase).Groups["val"].Value;
            }
            return value;
        }

        public static string GetMyIpAddress()
        {
            return new WebClientEx().DoGet("http://115.79.60.134:8082/api/ip.aspx");
        }

        public static string GenNewIpAddress()
        {
            string strIP = "183";
            List<string> list = new List<string>() { "80", "81", "80", "81" };
            strIP += "." + list[new Random().Next(0, list.Count)];
            System.Threading.Thread.Sleep(5);
            strIP += "." + new Random().Next(1, 127).ToString();
            System.Threading.Thread.Sleep(5);
            strIP += "." + new Random().Next(1, 254).ToString();
            return strIP;
        }
        public static string GetProfileInfo(int iWhat)
        {
            List<string> listTHCS = new List<string>("603093866374134,1539658186245753,1517916378437586,1497335753816715,573722549312405,315302845183833,267096210044423,139159149499453,366101536811976,153451078160075,771970322826212,194472424019835,792605847430818,162308497269803,258859317564022,423654174394279,580918805309372,383562235028763,392944317507701,1422657364657875,544562415632450,310223825743171,1460851127467239,626604480705559,423910557654572,189854467856601,282683795076681,203623393097273,502910936408728,359884317413364,644771992215589,452878324728754,467550606615806,187601901395333,234098750050963,380149595406853,495255667222606,352351161508102,237192396324147,276679605781629,793472227371239,327744670652889,603692052992082,416129971751844,101358840055005,423649271019714,130863697073600,257594737687895,193582614121861,529228130494018,1534218336801265,244314822367023,285017308235337,146733452101160,340719816007395,261862347250147,320723894698921,440564952729072,350045655079693,260777034071024,119311248239433,432609566795442,474331099249054,176072059245231,473420852713943,352919051458313,160568370352,248441695217341,326388857510616,580291195423405,604569502897567,201332466696545,227785704002351,536928636386470,268882296491103,428672743917371,238761452911222,216486295119921,274597595894814,211681238981543,296195117074874,193470934060766,250565861716754,261084333939602,310852169010489,304171286320565,174893442602254,254141824653441,327294747343002,309341239195049,189472407864097,168647656599141,236341043072111,108856532491590,288298497869913,127118997383072,183527675121261,216627265087134,250837581695289,205984196135419,676072512480394,382307225137847,126232067435725,188828514494081,127120367363047,224580670904758,564005016950879,262854027162418,200711479973793,108560145915477,105128759634062,257455774291558,581100985246760,352343971512654,264533083652692,403617243070617,170111789714101,549410191803156,450975441716048,424057061046920".Split(','));
            List<string> listTHPT = new List<string>("218018481556567,240593366062498,100113626751600,284056381708027,268665179841322,254322197974300,250111688372328,132381146836898,190239781040562,154675647911204,463658733718503,1413351198915941,458830860837242,637327659629939,1415173212054135,146347295556421,258859317564022,110816368990151,110371062387079,385952241442114,172061232985358,313828988694982,287121468085918,316877605068708,486262318137304,319319658158304,161329813934001,594774493938179,624244104273180,340062012750075,425625664154378,525404297482061,430958240288880,126427310818918,469977869686628,195557080482131,190940827747370,453186128056886,227634257437340,473420852713943,628064410583385,449652881747372,161997920565950,379026568835058,380954691955884,342352749211434,261929907254145,123795184305204,110036349163284,124483674257217,259469310740962,125563950788050,162615997102766,161111920617670,127236214033364,289272937758319,141531919258797,235531393161504,113279982078880,203365426361491,472498506107819,102030846616799,335514733177863,298533303570187,142804325868118,183302138467954,116951015181115,550695791611829,114370388650172,138565986211171,167392106684180,357652457597024,367949409957395,177131292310287,193470934060766,392395474181131,257230630976613,288804537887256,438886076141196,207831409372186,489760247702616,208502565903065,552345891500006,179006918797041,268530526511656,199008363471907,362953850458644,320290984651442,438689902875157,257846034383774,157580987651007,185614638198498,175744175784400,257224514376297,192430957468192,306826709418480,179014018860518,431997590196061,179328435458635,124746540875209,109249865896032,218748291495230,104714596288621,180399152001556,176880992329379,274994526023540,161338507293405,222982484398874,106820632754012,124540770961265,217667844920857,100003168404322,100003079355277,110199085758010,210527139111598,143888349034944,297589670405857,254294371296639,316551931803326,284218854927029".Split(','));
            List<string> listCDDH = new List<string>("118007305015576,508066399312237,522429477847920,1421167994798483,517282911659214,331418876881865,229308117222370,1388866211342690,324668094212658,114767985394630,346064672160773,113783135451080,130055243809944,447527448647026,171184239744253,349409215074630,419554594766295,331019846957632,1482251065335190,357272160993585,177999075706645,269135149788440,605952612780088,349473538460875,103179549795105,351407224886406,328598913900328,348796275168772,1450503945216823,260820113958315,234813823242671,448863421821970,207360139383221,443589159106355,277125125664238,438608309519905,427334600677198,421353644556109,141742939228948,362513380493102,591007297616128,229513503814568,524231494296061,216991401788629,147838015364431,180189825500847,646376362067943,395995377201504,268365023273279,472973516058263,195810517171726,144522158999444,1519704671598170,268381876609386,329049553905687,347579675319552,241394962648023,326344200709020,467113040013113,262188430611290,265246673533144,154251751365624,154872911253837,237577083048811,525532407483848,334111739971797,1412217735662790,624879004204215,532723113424776,340575559351369,269583649820274,228880000544238,469430816463157,301907309896738,603730069659434,394893643910084,611465368905704,450186921660014,231760360263138,171193816401861,282825575167774,271340859601269,111009892344215,201815319906246,231921363533563,257560877619005,315229588583968,329020213817431,225368530924351,206781472670647,238910706160539,118149731648005,185059021585929,204938359613111,149598298550089,356810511052436,362478217206683,419108241473132,257479867678956,221994387903059,100003341011316,135968609892586,194415173912792,127288630689543,165591340201548,220349618046467,475497799201431,110457932309418,821713587841568,282017565154770,550688854952476,393725897391834,853718534647052,610056362404810,186335644834463,303162493110595,260894457349805,538304112862225,486822084675866,398893673561131,250119161761272,133493293416491,162681807152692,158701267550483,564746326878852,190541120995814,171984966209913,1471760539735113,217536528314535,581741848514771,285902338198842,167814166757125,312353575448031,351086971620324,314576941949951,331998843551882,108955925919191,290451617707776,361297973930747,263760170392450,234362266671413,245687912111994,196343770394034,311576025590671,332389523540452,205786212802552,591302017560660,208035252634446,127651704041020,342728285837217,230149177026224,338528722872481,254259284586065,103108753125687,186117838137228,256798027802922,139497852841140,106929529345661,1435003580078354,439390799474945,368816783150451,429167437123938,264253516929934,160089624104655,506467589446419,1413460415556130,374904825916643,174023552741246,294386103981544,102444586583901,289469077749235,1387856394770164,447371708681230,419124014864061,150359951725775,506016542823052,450277245002702,218080238254209,449043521805132,113059198781188,433752176723033,217655444944902,142659875761887,302269166541541,235604763153949,118502361566576,532004123481067,314423688606102,152934634793285,248192125352322,101030043284949,533399000008958,143571295707110,392346330843981,100773183339510,412497792140652,300068356724576,353173208589,179931195476812,300630419956617,158248054330848,181668126144,172878396058117,222913021084020,380043135376303,125292960909946,374384632646429,195859740503151,268387536637911,141178912634035,306277426062124,100007479408382,122523137923491,231713690209881,106680876030539,235687863136651,156966804349448,135710773194524,112270908853537,124305777580504".Split(','));
            List<string> listCCTY = new List<string>("199335430228364,1449882825259646,404364316299372,364266503653396,284441914994985,150432771833857,273585562717263,186526491412477,491718167601112,160889070755690,455136817892832,278691522172298,394673770605082,193736914113469,303815509698891,527939503885072,589469711073338,439576109404073,172634796203930,392841364163208,380352878715697,178710288859092,396969850363495,324293407660845,149685271884873,411805008910319,220837428048496,393325970743478,229059833870833,604549982992424,265697196972715,608264482550784,1508953502658167,243592189055788,590839780932535,328128390614827,169473469803136,1431668743715490,410931079014774,804915459547988,302347599941122,1355523107920366,1524047434479238,176576215846923,132880673583705,611621508906484,432441956850935,299122373470263,516287901797023,297887053691759,341353432659545,821302657904029,1419126244967888,458183870922920,828361847176158,246327162213574,287355684764752,296237853911209,456162307852351,246058735465989,114486425230945,339291512808574,313839781999286,595128230497694,228574320666013,350598698394883,588286127928525,420350851410929,437220633088417,1436698343210538,107069715994723,203102629411,1374453576167497,175824552550885,442921555753826,649829715124781,742312892455086,298656160270365,373402429430776,140755475944662,401351536626572,111558409008965,476253729122161,218452775030979,735899406434068,1378775712343103,106855639353150,503780313014468,336276036541065,491164747657734,343220645802963,167254893411299,148701341990612,366760276762280,325622440933354,223370504492468,366206566847559,295601970639781,225237857603960,1651344311757493,178998485518224,235547613319022,576797812412550,1444652822427029,161884933978575,174007909416087,282103828647892,664179060284345,525699184140672,648765911886631,175118999981,1374568959470237,608201065858192,212817775547341,317089548345594,617058111643894,1417536901856386,518834961507053,301971153281646,578525178833483".Split(','));
            System.Threading.Thread.Sleep(11);
            if (iWhat == 1)
            {
                return listTHCS[new Random().Next(1, listTHCS.Count - 1)];
            }
            else if (iWhat == 2)
            {
                return listTHPT[new Random().Next(1, listTHPT.Count - 1)];
            }
            else if (iWhat == 3)
            {
                return listCDDH[new Random().Next(1, listCDDH.Count - 1)];
            }
            else
            {
                return listCCTY[new Random().Next(1, listCCTY.Count - 1)];
            }
        }
        #endregion
    }
}

