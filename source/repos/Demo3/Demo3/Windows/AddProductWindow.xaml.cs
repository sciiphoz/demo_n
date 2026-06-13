using Demo3.DBContext;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace Demo3.Windows
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

            ProductImage.Source = new BitmapImage(new Uri("/Media/Products/picture.png", UriKind.Relative));

            MeasurementCB.ItemsSource = DataBaseConnection.demoEntities.Measurement.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            CategoryCB.ItemsSource = DataBaseConnection.demoEntities.Category.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            ManufacturerCB.ItemsSource = DataBaseConnection.demoEntities.Manufacturer.OrderBy(x => x.ID).Select(x => x.Name).ToList();
            SupplierCB.ItemsSource = DataBaseConnection.demoEntities.Supplier.OrderBy(x => x.ID).Select(x => x.Name).ToList();

            MeasurementCB.SelectedIndex = 0;
            CategoryCB.SelectedIndex = 0;
            ManufacturerCB.SelectedIndex = 0;
            SupplierCB.SelectedIndex = 0;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PNG|*.png|JPG|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                _newImage = dialog.FileName;
                ProductImage.Source = new BitmapImage(new Uri(_newImage));
            }
        }

        private string SaveImage(string file)
        {
            string name = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file);

            string projectBase = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = new System.IO.DirectoryInfo(projectBase).Parent.Parent.FullName; 
            string projectFolder = System.IO.Path.Combine(projectDir, "Media", "Products");
            File.Copy(file, System.IO.Path.Combine(projectFolder, name), true);

            string debugFolder = System.IO.Path.Combine(projectBase, "Media", "Products");
            System.IO.Directory.CreateDirectory(debugFolder);
            File.Copy(file, System.IO.Path.Combine(debugFolder, name), true);

            return "Media/Products/" + name;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
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

                    Image = _newImage != null ? SaveImage(_newImage) : null,

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
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
