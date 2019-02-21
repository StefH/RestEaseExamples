using RestEase;
using System.Net.Http;
using System.Text;

namespace RestEaseXmlExample
{
    public interface IXmlRequestBodySerializer : IRequestBodySerializer
    {
    }

    internal class XmlRequestBodySerializer : IXmlRequestBodySerializer
    {
        private readonly IXmlSerializer _serializer;

        public XmlRequestBodySerializer(IXmlSerializer serializer)
        {
            _serializer = serializer;
        }

        public HttpContent SerializeBody<T>(T body, RequestBodySerializerInfo info)
        {
            return SerializeBodyInternal(body);
        }

        public HttpContent SerializeBody<T>(T body)
        {
            return SerializeBodyInternal(body);
        }

        private HttpContent SerializeBodyInternal<T>(T body)
        {
            if (body == null)
            {
                return null;
            }

            string xml = _serializer.SerializeToString(body);

            return new StringContent(xml, Encoding.UTF8, "application/xml");
        }
    }
}
