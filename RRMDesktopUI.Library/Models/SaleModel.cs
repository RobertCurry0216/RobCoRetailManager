using System.Collections.Generic;

namespace RRMDesktopUI.Library.Models
{
    public class SaleModel
    {
        public List<SaleDetailModel> SaleDetails { get; set; } = new List<SaleDetailModel>();
    }
}