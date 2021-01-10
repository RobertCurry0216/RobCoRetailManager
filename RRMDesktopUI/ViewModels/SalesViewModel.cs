using AutoMapper;
using Caliburn.Micro;
using RRMDesktopUI.Library.Api;
using RRMDesktopUI.Library.Helpers;
using RRMDesktopUI.Library.Models;
using RRMDesktopUI.Models;
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
        IMapper mapper;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint, IMapper mapper)
        {
            this.productEndPoint = productEndPoint;
            this.configHelper = configHelper;
            this.saleEndPoint = saleEndPoint;
            this.mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await productEndPoint.GetAll();
            var mappedProducts = mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(mappedProducts);
        }

        #region Properties

        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return selectedProduct; }
            set { 
                selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private CartItemDisplayModel selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return selectedCartItem; }
            set
            {
                selectedCartItem = value;
                NotifyOfPropertyChange(() => selectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
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
            }
            else
            {
                var cartItem = new CartItemDisplayModel()
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
                if (selectedCartItem != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock++;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart--;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

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
