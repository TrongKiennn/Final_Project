//using DemoListBinding1610.Models;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using static DemoListBinding1610.MainViewModel;

//namespace DemoListBinding1610.ViewModels
//{
//    public class OrderPageViewModel : INotifyPropertyChanged
//    {
//        private string _keyword;
//        private bool _sortById;
//        private ObservableCollection<Product> _filteredProducts;

//        public ObservableCollection<Product> Products { get; set; }
//        public ObservableCollection<ImageItem> ImageItems { get; set; }

//        public string Keyword
//        {
//            get => _keyword;
//            set
//            {
//                _keyword = value;
//                OnPropertyChanged(nameof(Keyword));
//            }
//        }

//        public bool SortById
//        {
//            get => _sortById;
//            set
//            {
//                _sortById = value;
//                OnPropertyChanged(nameof(SortById));
//                FilterProductsByKeyword();
//            }
//        }

//        public ObservableCollection<Product> FilteredProducts
//        {
//            get => _filteredProducts;
//            set
//            {
//                _filteredProducts = value;
//                OnPropertyChanged(nameof(FilteredProducts));
//            }
//        }

//        public OrderPageViewModel()
//        {
//            // Initialize sample data
//            Products = new ObservableCollection<Product>
//            {
//                new Product { Name = "Latte", Price = "$5.00", ImageSource = "ms-appx:///Assets/latte.png" },
//                new Product { Name = "Cappuccino", Price = "$4.00", ImageSource = "ms-appx:///Assets/cappuccino.png" },
//                new Product { Name = "Espresso", Price = "$3.00", ImageSource = "ms-appx:///Assets/espresso.png" }
//            };

//            ImageItems = new ObservableCollection<ImageItem>
//            {
//                new ImageItem { ImageSource = "ms-appx:///Assets/latte.png", BackgroundColor = "LightBlue" },
//                new ImageItem { ImageSource = "ms-appx:///Assets/cappuccino.png", BackgroundColor = "LightCoral" }
//            };

//            FilteredProducts = new ObservableCollection<Product>(Products);
//        }

//        public void FilterProductsByKeyword()
//        {
//            var filtered = Products
//                .Where(p => string.IsNullOrEmpty(Keyword) || p.Name.Contains(Keyword))
//                .ToList();

//            if (SortById)
//                filtered = filtered.OrderBy(p => p.Name).ToList();

//            FilteredProducts = new ObservableCollection<Product>(filtered);
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
