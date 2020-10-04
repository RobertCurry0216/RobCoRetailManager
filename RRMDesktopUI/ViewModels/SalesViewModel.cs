using Caliburn.Micro;
using RRMDesktopUI.Library.Api;
using RRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint productEndPoint;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            this.productEndPoint = productEndPoint;
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

        private BindingList<ProductModel> _cart;

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { 
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }


        public string SubTotal
        {
            get { 
               return  "0"; 
            }
        }

        public string Tax
        {
            get
            {
                return "0";
            }
        }

        public string Total
        {
            get
            {
                return "0";
            }
        }


        #endregion

        #region button methods


        public bool CanAddToCart
        {
            get {
                bool output = false;


                return output;
            }
        }

        public void AddToCart()
        {

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

        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;


                return output;
            }
        }


        public void CheckOut()
        {

        }

        #endregion
    }
}
