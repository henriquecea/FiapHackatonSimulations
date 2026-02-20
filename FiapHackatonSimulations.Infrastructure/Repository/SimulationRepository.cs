using FiapHackatonSimulations.Domain.Entity;
using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Infrastructure.Data;

namespace FiapHackatonSimulations.Infrastructure.Repository;

public class SimulationRepository(AppDbContext context) : Repository<SensorData>(context), ISimulationRepository
{

}
