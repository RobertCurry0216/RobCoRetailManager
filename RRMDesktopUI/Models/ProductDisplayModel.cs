using System.ComponentModel;

namespace RRMDesktopUI.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public bool IsTaxable { get; set; }

        private int _QuantityInStock;

        public int QuantityInStock
        {
            get { return _QuantityInStock; }
            set 
            { 
                _QuantityInStock = value;
                CallPropertyChanged(nameof(QuantityInStock));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void CallPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}