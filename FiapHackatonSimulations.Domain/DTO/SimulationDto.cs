using Newtonsoft.Json;

namespace FiapHackatonSimulations.Domain.DTO;

public record SimulationDto(
    [property: JsonProperty("plot")] Guid Plot,
    [property: JsonProperty("soil_moisture")] decimal SoilMoisture,
    [property: JsonProperty("temperature")] decimal Temperature,
    [property: JsonProperty("precipitation_level")] decimal PrecipitationLevel
);
