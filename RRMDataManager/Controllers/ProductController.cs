using RRMDataManager.Library.DataAccess;
using RRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
