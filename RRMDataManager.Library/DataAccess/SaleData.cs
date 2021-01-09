using RRMDataManager.Library.Internal.DataAccess;
using RRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: make this solid/dry
            //start filling in models to be saved to the database
            var details = new List<SaleDetailDBModel>();
            var products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could net be found in the database");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                }

                details.Add(detail);
            }

            //create the sale model
            var sale = new SaleDBModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId,
            };

            sale.Total = sale.SubTotal + sale.Tax;

            //save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "RRMData");

            //get the id from the sale model
            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new {CashierId = sale.CashierId, SaleDate = sale.SaleDate }, "RRMData").FirstOrDefault();


            //finish filling it the sale model
            foreach (var item in details)
            {
                item.SaleId = sale.Id;

                //save the sale detail models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "RRMData");
            }

        }
    }
}
