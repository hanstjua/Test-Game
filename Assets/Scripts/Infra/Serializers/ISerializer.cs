public interface ISerializer<out T>
{
    public byte[] Serialize(object obj);
    public T Deserialize(byte[] payload);
}