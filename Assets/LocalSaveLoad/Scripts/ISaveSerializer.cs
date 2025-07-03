namespace BroCollie.SaveLoad
{
    public interface ISaveSerializer
    {
        string Serialize(object data);
        void Deserialize(string serializedData, object target);
    }
}