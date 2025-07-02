using System.Threading.Tasks;

namespace BroCollie.UGS
{
    public interface IAuthentication
    {
        Task InitializeUnityServicesAsync(string envName);
        Task SignInAsync();
    }
}