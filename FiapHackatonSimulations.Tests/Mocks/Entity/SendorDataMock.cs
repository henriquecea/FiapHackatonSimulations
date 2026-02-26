using Bogus;
using FiapHackatonSimulations.Domain.Entity;

namespace FiapHackatonSimulations.Tests.Mocks.Entity;

public static class SendorDataMock
{
    public static SensorData CreateValid()
    {
        var faker = new Faker("pt_BR");

        return new SensorData
        {
            Id = Guid.NewGuid(),
            Plot = Guid.NewGuid(),
            SoilMoisture = faker.Random.Decimal(0, 100),
            Temperature = faker.Random.Decimal(-10, 40),
            PrecipitationLevel = faker.Random.Decimal(0, 200),
            CreationTime = DateTime.UtcNow,
            IsDeleted = false,
            LastModitificationDate = null
        };
    }
}
