using RRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Api
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
    }
}