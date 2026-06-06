using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DSA_Midterms
{
    public partial class MainWindow : Window
    {
        // Source list for available products
        private readonly List<Product> _productList = new List<Product>();

        // Source list for cart products
        private readonly List<Product> _cartList = new List<Product>();

        // Observable collections bound to the two DataGrids
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();
        public ObservableCollection<Product> CartItems { get; } = new ObservableCollection<Product>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RefreshProductGrid();
            RefreshCartGrid();
        }


        private void RefreshProductGrid()
        {
            Products.Clear();
            foreach (var p in _productList)
                Products.Add(p);
        }

        private void RefreshCartGrid()
        {
            CartItems.Clear();
            foreach (var p in _cartList)
                CartItems.Add(p);
        }


        private Product ReadInputs()
        {
            if (!int.TryParse(txtID.Text.Trim(), out int id))
            {
                MessageBox.Show("Product ID must be a whole number.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            if (!decimal.TryParse(txtSalary.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Price must be a number.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return new Product
            {
                ProductID = id,
                ProductName = txtName.Text.Trim(),
                Description = txtTitle.Text.Trim(),
                Price = price
            };
        }

        private void FillInputs(Product p)
        {
            txtID.Text = p.ProductID.ToString();
            txtName.Text = p.ProductName ?? "";
            txtTitle.Text = p.Description ?? "";
            txtSalary.Text = p.Price.ToString("0.##");
        }

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtTitle.Text = "";
            txtSalary.Text = "";
        }


        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Product p)
                FillInputs(p);
        }

        // Add a new product to the Available Items list
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var product = ReadInputs();
            if (product == null) return;

            if (_productList.Exists(x => x.ProductID == product.ProductID))
            {
                MessageBox.Show("A product with ID " + product.ProductID + " already exists.", "Duplicate ID",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _productList.Add(product);
            RefreshProductGrid();
            ClearInputs();
            MessageBox.Show("Product '" + product.ProductName + "' added successfully.", "Add",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Remove selected product from the Available Items list
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(myDataGrid.SelectedItem is Product selected))
            {
                MessageBox.Show("Select a row to remove.", "Remove",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _productList.RemoveAll(x => x.ProductID == selected.ProductID);
            RefreshProductGrid();
            ClearInputs();
            MessageBox.Show("Product '" + selected.ProductName + "' removed successfully.", "Remove",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Add the selected Available Item to the Shopping Cart
        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(myDataGrid.SelectedItem is Product selected))
            {
                MessageBox.Show("Select a product from Available Items to add to cart.", "Add to Cart",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _cartList.Add(new Product
            {
                ProductID = selected.ProductID,
                ProductName = selected.ProductName,
                Description = selected.Description,
                Price = selected.Price
            });

            RefreshCartGrid();
            MessageBox.Show("'" + selected.ProductName + "' added to cart.", "Add to Cart",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Clear all records from both grids
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.UnselectAll();
            ClearInputs();
        }


        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }
    }
}

