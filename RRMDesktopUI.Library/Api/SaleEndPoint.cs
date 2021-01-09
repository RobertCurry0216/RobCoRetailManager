using RRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Api
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private IAPIHelper apiHelper;

        public SaleEndPoint(IAPIHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (var response = await apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }

    }
}
