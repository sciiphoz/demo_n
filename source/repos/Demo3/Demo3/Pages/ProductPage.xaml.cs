using Demo3.DBContext;
using Demo3.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo3.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            RefreshData();

            var filterOptions = new List<FilterProductOption>
            {
                new FilterProductOption
                {
                    DisplayName = "Все производители",
                    Value = null
                }
            };

            var options = DataBaseConnection.demoEntities.Manufacturer.Select(x => new {x.ID, x.Name}).Distinct().OrderBy(x => x.ID).ToList();

            foreach (var option in options)
            {
                filterOptions.Add(new FilterProductOption
                {
                    DisplayName = option.Name,
                    Value = option.ID
                });
            }

            FilterCB.ItemsSource = filterOptions;

            FilterCB.DisplayMemberPath = "DisplayName";
            FilterCB.SelectedValuePath = "Value";

            FilterCB.SelectedIndex = 0;
            SortCB.SelectedIndex = 0;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = string.Empty;
            SortCB.SelectedIndex = 0;
            FilterCB.SelectedIndex = 0;

            RefreshData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow();
            addWindow.ShowDialog();

            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Product;

            if (selectedItem == null) return;

            var mbox = MessageBox.Show("Вы точно хотите удалить данный продукт?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbox == MessageBoxResult.Yes) 
            {    
                DataBaseConnection.demoEntities.Product.Remove(selectedItem);
                DataBaseConnection.demoEntities.SaveChanges();
            }

            RefreshData();
        }

        private void MainListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Product;

            if (selectedItem == null) return;

            var addWindow = new EditProductWindow(selectedItem);
            addWindow.ShowDialog();

            RefreshData();
        }
        private void ApplyFilters()
        {
            var search = SearchTB.Text.Trim();
            int sort = SortCB.SelectedIndex;
            var filter = FilterCB.SelectedItem as FilterProductOption;

            IQueryable<Product> result = DataBaseConnection.demoEntities.Product;

            if (!string.IsNullOrEmpty(search)) 
                result = result.Where(x => x.Name.Contains(search) || x.Manufacturer.Name.Contains(search) || x.Supplier.Name.Contains(search) || x.Desc.Contains(search));

            if (filter != null && filter.Value.HasValue)
                result = result.Where(x => x.ID_Manufacturer == filter.Value.Value);

            switch (sort)
            {
                case 1:
                    result = result.OrderBy(x => x.Price);
                    break;
                case 2:
                    result = result.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }
            
            MainListView.ItemsSource = result.ToList();
        }

        private void RefreshData()
        {
            MainListView.ItemsSource = DataBaseConnection.demoEntities.Product.OrderBy(x => x.ID)
                .Include("Category")
                .Include("Manufacturer")
                .Include("Supplier")
                .Include("Measurement")
                .ToList();
        }
    }
}

public class FilterProductOption
{
    public string DisplayName { get; set; }
    public int? Value { get; set; }
}
