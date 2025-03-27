using BackFactory;
using DB;
using Model.Product;
using MongoDB.Bson;
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

namespace ShoeStoreFront
{
    /// <summary>
    /// Interaction logic for ProductosWindow.xaml
    /// </summary>
    public partial class ProductosWindow : Window
    {
        private List<Producto> _productos;
        private List<Categoria> categorias;
        private Categoria selectedCategory;
        private List<string> selectedMarcas = new List<string>();

        public ProductosWindow()
        {
            InitializeComponent();
            LoadProductos();
            LoadCategorias();
            LoadMarcas();
        }

        private void LoadProductos()
        {
            _productos = Utils.ProductoManager.GetAll();
            lvProd.ItemsSource = _productos;
        }

        private void LoadCategorias()
        {
            categorias = Utils.CategoriaManager.GetParentCategorias();
            foreach (var categoria in categorias)
            {
                var treeViewItem = new TreeViewItem { Header = categoria.Nombre, Tag = categoria };
                AddChildCategories(treeViewItem, categorias);
                treeViewCategorias.Items.Add(treeViewItem);
            }
        }

        private void AddChildCategories(TreeViewItem parentItem, List<Categoria> allCategories)
        {
            var parentCategory = (Categoria)parentItem.Tag;
            var childCategories = Utils.CategoriaManager.GetChildCategorias(parentCategory);

            foreach (var child in childCategories)
            {
                var childItem = new TreeViewItem { Header = child.Nombre, Tag = child };
                AddChildCategories(childItem, allCategories);
                parentItem.Items.Add(childItem);
            }
        }

        private void LoadMarcas()
        {
            var marcas = Utils.ProductoManager.GetAllMarcas();
            foreach (var marca in marcas)
            {
                var checkBox = new CheckBox { Content = marca };
                checkBox.Checked += Marca_Checked;
                checkBox.Unchecked += Marca_Unchecked;
                stackPanelMarcas.Children.Add(checkBox);
            }
        }

        private void Marca_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                selectedMarcas.Add(checkBox.Content.ToString());
                ApplyFilters();
            }
        }

        private void Marca_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                selectedMarcas.Remove(checkBox.Content.ToString());
                ApplyFilters();
            }
        }

        private void treeViewCategorias_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeViewCategorias.SelectedItem is TreeViewItem selectedItem)
            {
                selectedCategory = selectedItem.Tag as Categoria;
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            var nombre = txtNombre.Text;
            var minPrecio = sliderMinPrecio.Value;
            var maxPrecio = sliderMaxPrecio.Value;
            var talla = txtTalla.Text;

            var filteredProducts = Utils.ProductoManager.GetFilteredProducts(selectedCategory, nombre, minPrecio, maxPrecio, talla, selectedMarcas);
            lvProd.ItemsSource = filteredProducts;
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void sliderMinPrecio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplyFilters();
        }

        private void sliderMaxPrecio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(sliderMaxPrecio.Value != 1000)
                ApplyFilters();
        }

        private void txtTalla_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ClearFilters();
        }

        private void ClearFilters()
        {
            txtNombre.Text = string.Empty;
            sliderMinPrecio.Value = sliderMinPrecio.Minimum;
            sliderMaxPrecio.Value = sliderMaxPrecio.Maximum;
            txtTalla.Text = string.Empty;
            selectedMarcas.Clear();
            foreach (CheckBox checkBox in stackPanelMarcas.Children)
            {
                checkBox.IsChecked = false;
            }
            ApplyFilters();
        }

        private void lvProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvProd.SelectedItem is Producto selectedProduct)
            {

                var productWindow = new ProductoDetailsWindow(selectedProduct);
                productWindow.Show();
                this.Close();
            }
        }
    }
}

