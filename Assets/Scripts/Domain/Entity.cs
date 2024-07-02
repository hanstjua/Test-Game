using System;

public abstract class Entity : IEquatable<object>
{
    public abstract object Id();

    public override bool Equals(object obj)
    {
        return obj is not null && Id() == ((Entity) obj).Id();
    }

    public override int GetHashCode()
    {
        return Id().GetHashCode();
    }

    public static bool operator ==(Entity e1, Entity e2) => e1 is null ? e2 is null : e1.Equals(e2);
    public static bool operator !=(Entity e1, Entity e2) => e1 is null ? e2 is not null : !e1.Equals(e2);   
}