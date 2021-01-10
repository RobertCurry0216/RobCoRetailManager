using System.ComponentModel;

namespace RRMDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }

        private int _QuantityInCart;

        public int QuantityInCart
        {
            get { return _QuantityInCart; }
            set
            {
                _QuantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get { return $"{QuantityInCart} :   {Product.Name}"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CallPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}