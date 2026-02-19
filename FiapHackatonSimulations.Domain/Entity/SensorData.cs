using Newtonsoft.Json;

namespace FiapHackatonSimulations.Domain.Entity;

public class SensorData : BaseEntity
{
    [JsonProperty("plot")]
    public required Guid Plot { get; set; }

    [JsonProperty("soil_moisture")]
    public decimal SoilMoisture { get; set; }

    [JsonProperty("temperature")]
    public decimal Temperature { get; set; }

    [JsonProperty("precipitation_level")]
    public decimal PrecipitationLevel { get; set; }
}
