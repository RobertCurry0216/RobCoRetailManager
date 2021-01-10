using RRMDataManager.Library.DataAccess;
using RRMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace RRMDataManager.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        [HttpGet]
        public List<ProductModel> Get()
        {
            var data = new ProductData();

            return data.GetProducts();
        }
    }
}