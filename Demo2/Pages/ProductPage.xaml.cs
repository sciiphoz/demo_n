using Demo2.DataBaseContext;
using Demo2.Windows;
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

            RefreshData();

            var filterOptions = new List<FilterProductOption>
            {
                new FilterProductOption { DisplayName = "По умолчанию", Value = null }
            };

            var options = DataBaseConnection.demoEntities.Manufacturer.Select(x => new { x.ID, x.Name }).Distinct().OrderBy(x => x.ID).ToList();

            foreach (var option in options)
            {
                filterOptions.Add(new FilterProductOption
                {
                    DisplayName = option.Name,
                    Value = option.ID,
                });
            }

            FilterCB.ItemsSource = filterOptions;

            FilterCB.DisplayMemberPath = "DisplayName";
            FilterCB.SelectedValuePath = "Value";

            FilterCB.SelectedIndex = 0;
            SortCB.SelectedIndex = 0;

            if (CurrentUser.currentUser.ID_Role == 0 || CurrentUser.currentUser.ID_Role == 3)
            {
                SearchTB.Visibility = Visibility.Collapsed;
                FilterCB.Visibility = Visibility.Collapsed;
                SortCB.Visibility = Visibility.Collapsed;
                ResetButton.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;

                MainListView.MouseDoubleClick -= MainListView_MouseDoubleClick;
            }

            if (CurrentUser.currentUser.ID_Role == 2)
            {
                AddButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;

                MainListView.MouseDoubleClick -= MainListView_MouseDoubleClick;
            }
        }

        private void MainListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Product;

            if (selectedItem == null) return;

            var parent = Window.GetWindow(this);
            var editProductWindow = new EditProductWindow(selectedItem);
            editProductWindow.ShowDialog();

            RefreshData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this);
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();

            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Product;

            if (selectedItem == null) return;

            DataBaseConnection.demoEntities.Product.Remove(selectedItem);
            DataBaseConnection.demoEntities.SaveChanges();

            RefreshData();
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
            FilterCB.SelectedIndex = 0;
            SortCB.SelectedIndex = 0;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string search = SearchTB.Text.Trim() ?? string.Empty;
            int sort = SortCB.SelectedIndex;

            var selectedFilter = FilterCB.SelectedItem as FilterProductOption;

            IQueryable<Product> result = DataBaseConnection.demoEntities.Product;

            if (!string.IsNullOrEmpty(search)) 
                result = result.Where(x => x.Name.Contains(search) 
                || x.Desc.Contains(search) 
                || x.Manufacturer.Name.Contains(search) 
                || x.Supplier.Name.Contains(search));

            if (selectedFilter != null && selectedFilter.Value.HasValue)
                result = result.Where(x => x.ID_Manufacturer == selectedFilter.Value.Value);

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
        private void ProductImage_Loaded(object sender, RoutedEventArgs e)
        {
            var img = sender as Image;
            var product = img.DataContext as Product;
            if (product == null) return;

            string path = product.Image;

            if (string.IsNullOrEmpty(path) || path == "/Media/Products/picture.png")
            {
                img.Source = new BitmapImage(new Uri("C:\\Users\\sciiphoz\\source\\repos\\Demo2\\Demo2\\bin\\Debug\\Media\\Products\\picture.png", UriKind.Absolute));
                return;
            }

            string folder = AppDomain.CurrentDomain.BaseDirectory + "Media\\Products\\";
            string file = System.IO.Path.GetFileName(path);
            string full = System.IO.Path.Combine(folder, file);

            if (System.IO.File.Exists(full))
                img.Source = new BitmapImage(new Uri(full));
            else
                img.Source = new BitmapImage(new Uri("C:\\Users\\sciiphoz\\source\\repos\\Demo2\\Demo2\\bin\\Debug\\Media\\Products\\picture.png", UriKind.Absolute));
        }
        private void RefreshData()
        {
            MainListView.ItemsSource = DataBaseConnection.demoEntities.Product
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
