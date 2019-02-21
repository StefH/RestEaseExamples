using System.IO;
using System.Text;

namespace RestEaseXmlExample
{
    internal sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
