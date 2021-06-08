using System.Threading.Tasks;
using System.Net.Http;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IIntegrationRepository
    {
        Task<HttpResponseMessage> SendPost(object obj, string token, string url);        
        Task<HttpResponseMessage> GetUrlApi(string url, string token);
    }
}
