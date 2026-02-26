using Bogus;
using FiapHackatonSimulations.Domain.DTO;

namespace FiapHackatonSimulations.Tests.Mocks.DTO;

public static class SimulationDtoMock
{
    public static SimulationDto CreateValid()
    {
        var faker = new Faker("pt_BR");

        return new SimulationDto(Guid.NewGuid(),
                                 faker.Random.Decimal(0, 100),
                                 faker.Random.Decimal(-10, 40),
                                 faker.Random.Decimal(0, 200));
    }
}
