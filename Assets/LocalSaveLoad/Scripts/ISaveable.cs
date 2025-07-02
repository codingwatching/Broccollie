namespace BroCollie.SaveLoad
{
    public interface ISaveable
    {
        void SaveState(object state);
        void LoadState(object state);
    }
}
