using System;
using System.Threading.Tasks;

namespace BroCollie.SaveLoad
{
    public interface ISaveLoader
    {
        event Action OnSave;
        event Action OnLoad;

        Task LoadDataAsync();
        Task SaveDataAsync();
    }
}