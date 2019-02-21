namespace RestEaseXmlExample
{
    /// <summary>
    /// A factory to create an Auth0 Client.
    /// </summary>
    internal interface ISomeApiClientFactory
    {
        /// <summary>
        /// Creates an Auth0 Client.
        /// </summary>
        /// <typeparam name="T">Generic interface which implements IAuth0Api</typeparam>
        /// <returns>T</returns>
        T CreateClient<T>() where T : ISomeApi;
    }
}