using System.Threading.Tasks;

namespace BroCollie.UGS
{
    public interface IRemoteConfig
    {
        Task FetchRemoteConfigAsync(userAttributes userAttributes = default,
            appAttributes appAttributes = default, filterAttributes filterAttributes = default);
        string GetString(string key);
        T GetDataFromJson<T>(string key);
    }
}
