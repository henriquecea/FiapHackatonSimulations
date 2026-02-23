using FiapHackatonSimulations.Domain.Entity;

namespace FiapHackatonSimulations.Domain.Interface.Repository;

public interface ISimulationRepository : IRepository<SensorData>
{
    Task<IEnumerable<SensorData>> GetByPlotIdAsync(Guid plotId);
}
