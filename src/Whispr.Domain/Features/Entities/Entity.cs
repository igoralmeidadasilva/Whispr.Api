namespace Whispr.Domain.Features.Entities;

public abstract class Entity
{
    public virtual Guid Id { get; init; }
    
    protected Entity()
    {
        Id = Guid.CreateVersion7();
    }
}