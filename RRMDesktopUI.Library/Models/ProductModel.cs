﻿namespace RRMDesktopUI.Library.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsTaxable { get; set; }
    }
}