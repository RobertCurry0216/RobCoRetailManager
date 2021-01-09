using Caliburn.Micro;
using RRMDesktopUI.Library.Api;
using RRMDesktopUI.Library.Helpers;
using RRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint productEndPoint;
        IConfigHelper configHelper;
        ISaleEndPoint saleEndPoint;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint)
        {
            this.productEndPoint = productEndPoint;
            this.configHelper = configHelper;
            this.saleEndPoint = saleEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        #region Properties

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return selectedProduct; }
            set { 
                selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            return Cart.Sum(i => i.Product.RetailPrice * i.QuantityInCart);
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxRate = configHelper.GetTaxRate() / 100;
            return Cart
                .Where(i => i.Product.IsTaxable)
                .Sum(i => i.Product.RetailPrice * i.QuantityInCart * taxRate);
        }

        public string Total
        {
            get
            {
                var total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C")  ;
            }
        }

        #endregion

        #region button methods

        public bool CanAddToCart
        {
            get {
                var output = false;
                if (ItemQuantity > 0 && selectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }
                return output;
            }
        }

        public void AddToCart()
        {
            var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.ResetBindings();
            }
            else
            {
                var cartItem = new CartItemModel()
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity,
                };

                Cart.Add(cartItem);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;


                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task CheckOut()
        {
            var sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel() 
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart,
                });
            }

            await saleEndPoint.PostSale(sale);
        }

        #endregion
    }
}
