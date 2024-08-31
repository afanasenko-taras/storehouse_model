using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace AbstractModel
{
    public class Helper
    {
        public static byte[] SerializeXML<T>(T stamp)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, stamp);
            return stream.ToArray();
        }

        public static T DeserializeXML<T>(byte[] binaryData)
        {
            var formatter = new XmlSerializer(typeof(T));
            var ms = new MemoryStream(binaryData);
            return (T)formatter.Deserialize(ms);
        }

        public static void FileSerialize<T>(T obj, string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static T FileDeserialize<T>(string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            IFormatter formatter = new BinaryFormatter();
            T obj = (T)formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

    }
}
