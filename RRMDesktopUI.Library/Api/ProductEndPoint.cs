using RRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Api
{
    public class ProductEndPoint : IProductEndPoint
    {
        private IAPIHelper apiHelper;

        public ProductEndPoint(IAPIHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            using (var response = await apiHelper.ApiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}