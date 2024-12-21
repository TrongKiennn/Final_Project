using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.Collections.ObjectModel;
using Windows.UI;

namespace POS_App.ViewModel
{
    public class Material
    {
        public string ImagePath { get; set; }    // Đường dẫn ảnh
        public string Name { get; set; }        // Tên nguyên liệu
        public string Quantity { get; set; }    // Số lượng
        public string Unit { get; set; }        // Đơn vị tính
        public SolidColorBrush StatusColor { get; set; } // Màu trạng thái
    }

    public class MaterialManagementViewModel
    {
        public ObservableCollection<Material> Materials { get; set; }

        public MaterialManagementViewModel()
        {
            // Tạo dữ liệu mẫu
            Materials = new ObservableCollection<Material>
            {
                new Material
                {
                    ImagePath = "Assets/Sugar.png",
                    Name = "Đường",
                    Quantity = "5",
                    Unit = "kg",
                    StatusColor = new SolidColorBrush(Colors.Green)
                },
                new Material
                {
                    ImagePath = "Assets/Coffee.png",
                    Name = "Cà phê",
                    Quantity = "2",
                    Unit = "kg",
                    StatusColor = new SolidColorBrush(Colors.Yellow)
                },
                new Material
                {
                    ImagePath = "Assets/Milk.png",
                    Name = "Sữa đặc",
                    Quantity = "1",
                    Unit = "hộp",
                    StatusColor = new SolidColorBrush(Colors.Red)
                }
            };
        }
    }
}
