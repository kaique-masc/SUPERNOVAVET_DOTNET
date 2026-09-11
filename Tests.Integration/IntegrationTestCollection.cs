using Xunit;

namespace Tests.Integration;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<ApiFactory>
{
}