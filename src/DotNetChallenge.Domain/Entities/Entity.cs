namespace DotNetChallenge.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public bool Excluded { get; private set; }

    public void SetExcluded() => Excluded = true;

    public void RecoverExcluded() => Excluded = false;

    public Entity() { }
}