namespace FiapHackatonSimulations.Domain.Entity;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; } = DateTime.UtcNow;

    public DateTime? LastModitificationDate { get; set; }

    public bool IsDeleted { get; set; }
}
