using RRMDesktopUI.Models;
using System.Threading.Tasks;

namespace RRMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}