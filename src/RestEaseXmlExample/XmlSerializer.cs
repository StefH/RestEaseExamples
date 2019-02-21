using System.Collections.Concurrent;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SystemXmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace RestEaseXmlExample
{
    /// <summary>
    /// see also https://long2know.com/2017/09/net-xmlserializer-memory-leak/
    /// </summary>
    /// <seealso cref="IXmlSerializer" />
    internal class XmlSerializer : IXmlSerializer
    {
        private static readonly XmlSerializerNamespaces EmptyNamespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty, string.Empty) });
        private static readonly XmlWriterSettings DefaultWriterSettings = new XmlWriterSettings { OmitXmlDeclaration = false, Indent = false };

        private static readonly ConcurrentDictionary<string, SystemXmlSerializer> Serializers = new ConcurrentDictionary<string, SystemXmlSerializer>();

        public T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            using (var stringReader = new StringReader(xml))
            {
                var xmlSerializer = GetSerializer<T>();
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }

            using (var ms = new MemoryStream(data))
            {
                var xmlSerializer = GetSerializer<T>();
                return (T)xmlSerializer.Deserialize(ms);
            }
        }

        public string SerializeToString<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            using (var stringWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, DefaultWriterSettings))
                {
                    var serializer = GetSerializer<T>();
                    serializer.Serialize(xmlWriter, value, EmptyNamespaces);
                }

                return stringWriter.ToString();
            }
        }

        public byte[] Serialize<T>(T value)
        {
            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, DefaultWriterSettings))
            {
                var serializer = GetSerializer<T>();
                serializer.Serialize(writer, value, EmptyNamespaces);

                return ms.ToArray();
            }
        }

        private static SystemXmlSerializer GetSerializer<T>(string rootName = null, XNamespace xs = null)
        {
            string key = $"{typeof(T)}|{rootName}|{xs}";

            if (!Serializers.TryGetValue(key, out SystemXmlSerializer serializer))
            {
                if (!string.IsNullOrWhiteSpace(rootName) && xs == null)
                {
                    serializer = new SystemXmlSerializer(typeof(T), null, null, new XmlRootAttribute(rootName), string.Empty);
                }
                else if (string.IsNullOrWhiteSpace(rootName) && xs == null)
                {
                    serializer = new SystemXmlSerializer(typeof(T));
                }
                else
                {
                    serializer = new SystemXmlSerializer(typeof(T), null, null, new XmlRootAttribute(rootName), xs?.ToString());
                }

                Serializers.TryAdd(key, serializer);
            }

            return serializer;
        }
    }
}