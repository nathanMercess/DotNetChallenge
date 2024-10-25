﻿namespace DotNetChallenge.Domain.Entities;

public abstract class Entity
{
    protected Guid Id { get; }

    protected bool Excluded { get; private set; }

    protected void SetExcluded() => Excluded = true;

    protected void RecoverExcluded() => Excluded = false;
}