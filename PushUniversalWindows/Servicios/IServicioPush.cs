using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;

namespace PushUniversalWindows.Servicios
{
    public interface IServicioPush
    {
        Task<Registration> Registrar();
    }
}