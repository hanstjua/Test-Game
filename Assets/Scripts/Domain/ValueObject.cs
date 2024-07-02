using System;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
{
    public abstract T Value();
	public bool Equals(ValueObject<T> obj)
    {
        return obj is not null && Value().Equals(obj.Value());
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as ValueObject<T>);
    }
    public override int GetHashCode()
    {
        return Value().GetHashCode();
    }

    public static bool operator ==(ValueObject<T> obj1, ValueObject<T> obj2) => obj1 is null ? obj2 is null : obj1.Equals(obj2);
    public static bool operator !=(ValueObject<T> obj1, ValueObject<T> obj2) => obj1 is null ? obj2 is not null : !obj1.Equals(obj2);
}