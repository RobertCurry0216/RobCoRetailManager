using RRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}