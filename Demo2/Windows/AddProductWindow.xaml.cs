using Demo2.DataBaseContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Demo2.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private string _newImage = null;
        public AddProductWindow()
        {
            InitializeComponent();

            MeasurementCB.ItemsSource = DataBaseConnection.demoEntities.Measurement.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            CategoryCB.ItemsSource = DataBaseConnection.demoEntities.Category.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ManufacturerCB.ItemsSource = DataBaseConnection.demoEntities.Manufacturer.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            SupplierCB.ItemsSource = DataBaseConnection.demoEntities.Supplier.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            MeasurementCB.SelectedIndex = 0;
            CategoryCB.SelectedIndex = 0;
            ManufacturerCB.SelectedIndex = 0;
            SupplierCB.SelectedIndex = 0;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newProduct = new Product
                {
                    Article = ArticleTB.Text,
                    Desc = DescTB.Text,
                    Discount = Convert.ToDecimal(DiscountTB.Text),
                    Name = NameTB.Text,
                    Price = Convert.ToDecimal(PriceTB.Text),
                    Quantity = Convert.ToInt32(QuantityTB.Text),
                    Image = _newImage != null ? SaveImage(_newImage) : "/Media/Products/picture.png",

                    ID_Measurement = MeasurementCB.SelectedIndex + 1,
                    ID_Category = CategoryCB.SelectedIndex + 1,
                    ID_Manufacturer = ManufacturerCB.SelectedIndex + 1,
                    ID_Supplier = SupplierCB.SelectedIndex + 1,
                };

                DataBaseConnection.demoEntities.Product.Add(newProduct);
                DataBaseConnection.demoEntities.SaveChanges();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "JPG|*.jpg|PNG|*.png";

            if (dialog.ShowDialog() == true)
            {
                _newImage = dialog.FileName;
                ProductImage.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private string SaveImage(string file)
        {
            string name = Guid.NewGuid().ToString() + ".jpg";
            string folder = AppDomain.CurrentDomain.BaseDirectory + "Media\\Products\\";

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fullPath = System.IO.Path.Combine(folder, name);
            File.Copy(file, fullPath, true);

            return "/Media/Products/" + name;
        }
    }
}
