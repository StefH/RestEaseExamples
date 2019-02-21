namespace RestEaseXmlExample
{
    internal interface IXmlSerializer
    {
        string SerializeToString<T>(T value);

        byte[] Serialize<T>(T value);

        T Deserialize<T>(string xml);

        T Deserialize<T>(byte[] data);
    }
}