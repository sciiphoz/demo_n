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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();

            RefreshData();

            var filterOptions = new List<FilterOrderOption>
            {
                new FilterOrderOption { DisplayName = "По умолчанию", Value = null }
            };

            var options = DataBaseConnection.demoEntities.Status.Select(x => new { x.ID, x.Name }).Distinct().OrderBy(x => x.ID).ToList();

            foreach (var option in options)
            {
                filterOptions.Add(new FilterOrderOption
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
        }

        private void MainListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Order;

            if (selectedItem == null) return;

            var parent = Window.GetWindow(this);
            var editOrderWindow = new EditOrderWindow(selectedItem);
            editOrderWindow.ShowDialog();

            RefreshData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this);
            var addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog();

            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainListView.SelectedItem as Order;

            if (selectedItem == null) return;

            DataBaseConnection.demoEntities.Order.Remove(selectedItem);
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

            var selectedFilter = FilterCB.SelectedItem as FilterOrderOption;

            IQueryable<Order> result = DataBaseConnection.demoEntities.Order;

            if (!string.IsNullOrEmpty(search))
                result = result.Where(x => x.Address.Name.Contains(search));

            if (selectedFilter != null && selectedFilter.Value.HasValue)
                result = result.Where(x => x.ID_Status == selectedFilter.Value.Value);

            switch (sort)
            {
                case 1:
                    result = result.OrderBy(x => x.ShipmentDate);
                    break;
                case 2:
                    result = result.OrderByDescending(x => x.ShipmentDate);
                    break;
                default:
                    break;
            }

            MainListView.ItemsSource = result.ToList();
        }

        private void RefreshData()
        {
            MainListView.ItemsSource = DataBaseConnection.demoEntities.Order.Include("Address").Include("Status").Include("User").ToList();
        }
    }
}
public class FilterOrderOption
{
    public string DisplayName { get; set; }
    public int? Value { get; set; }
}