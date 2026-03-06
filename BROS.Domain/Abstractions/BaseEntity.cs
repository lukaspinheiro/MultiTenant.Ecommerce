namespace BROS.Domain.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; protected set;  }
    public DateTime CriadoEm {  get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }
}
