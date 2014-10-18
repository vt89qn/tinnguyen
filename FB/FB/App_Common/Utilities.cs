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
namespace FB.App_Common
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
            //System.Threading.Thread.Sleep(23);
            //strName += listHo[new Random().Next(listHo.Count - 1)].Trim();

            //strName[1] = strName[strName.Length - 2];
            //try
            //{
            //    strName = strName.Substring(0, 1) + listTen[new Random().Next(listTen.Count - 1)].Trim().Substring(1, 2) + strName.Substring(1, 2) + strName.Substring(3);

            //}
            //catch
            //{
            //    strName = listHo[new Random().Next(listHo.Count - 1)].Trim() + " " + listTen[new Random().Next(listTen.Count - 1)].Trim();
            //}
            //strName = strName.Replace(strName[2], strName[strName.Length - 4]);
            return strName;
        }
        #endregion
    }
}

