using RestEase;
using System.Threading.Tasks;

namespace RestEaseXmlExample
{
    internal interface ISomeApi
    {
        [Path("endpoint", UrlEncode = false)]
        string Endpoint { get; set; }

        [Post("{endpoint}")]
        Task<string> PostSettingsAsync<T>([Body] T value);
    }
}
