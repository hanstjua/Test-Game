using System;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
{
    public abstract T Value();
	public bool Equals(ValueObject<T> obj)
    {
        return ((T) Value()).Equals((T) obj.Value());
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as ValueObject<T>);
    }
    public override int GetHashCode()
    {
        return Value().GetHashCode();
    }
}