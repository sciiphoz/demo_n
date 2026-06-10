using Demo2.DataBaseContext;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            MainListBox.ItemsSource = DataBaseConnection.demoEntities.Product.Include("Category").Include("Manufacturer").Include("Supplier").Include("Measurement").ToList();   
            
        }

        private void MainItemControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
