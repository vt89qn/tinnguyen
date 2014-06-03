using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;

public class Serializer
{
    public Serializer()
    {
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
}