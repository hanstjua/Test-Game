using System;

public abstract class Entity
{
    public abstract object Id();
    public bool Equals(Entity obj)
    {
        return Id() == obj.Id();
    }
}