using System.Runtime.CompilerServices;
using RestEase;

// This is needed because the I... are marked as 'internal'.
[assembly: InternalsVisibleTo(RestClient.FactoryAssemblyName)]