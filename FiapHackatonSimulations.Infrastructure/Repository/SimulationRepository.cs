using FiapHackatonSimulations.Domain.Entity;
using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FiapHackatonSimulations.Infrastructure.Repository;

public class SimulationRepository(AppDbContext context) : Repository<SensorData>(context), ISimulationRepository
{
    public async Task<IEnumerable<SensorData>> GetByPlotIdAsync(Guid plotId) =>
        await _context.Set<SensorData>()
            .Where(x => x.Plot == plotId)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();
}
