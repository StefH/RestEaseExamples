using RestEase;
using System.Net.Http;

namespace RestEaseXmlExample
{
    /// <seealso cref="ISomeApiClientFactory" />
    internal class SomeApiClientFactory : ISomeApiClientFactory
    {
        private readonly IHttpClientFactory _factory;
        private readonly IXmlRequestBodySerializer _serializer;

        public SomeApiClientFactory(IHttpClientFactory factory, IXmlRequestBodySerializer serializer)
        {
            _factory = factory;
            _serializer = serializer;
        }

        /// <summary>
        /// Creates the Rest Client.
        /// </summary>
        /// <typeparam name="T">Generic interface</typeparam>
        /// <returns>T</returns>
        public T CreateClient<T>() where T : ISomeApi
        {
            return new RestClient(_factory.CreateClient())
            {
                RequestBodySerializer = _serializer
            }.For<T>();
        }
    }
}