using DB;
using Model.Product;
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
using System.Windows.Shapes;

namespace ShoeStoreFront
{
    /// <summary>
    /// Interaction logic for ProductosWindow.xaml
    /// </summary>
    public partial class ProductosWindow : Window
    {
        private List<Producto> _productos;
        private ProductoManager dbProducto;
        private CategoriaManager dbCategoria; 
        private List<Categoria> categorias;
        public ProductosWindow()
        {
            InitializeComponent();
            dbProducto = new ProductoManager(Utils.mongoDBConnection);
            dbCategoria = new CategoriaManager(Utils.mongoDBConnection);
            dtgProd.ItemsSource = dbProducto.GetAll();
            LoadCategorias();
        }
        private void LoadCategorias()
        {
            categorias = dbCategoria.GetParentCategorias();
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
            var childCategories = dbCategoria.GetChildCategorias(parentCategory);

            foreach (var child in childCategories)
            {
                var childItem = new TreeViewItem { Header = child.Nombre, Tag = child };
                AddChildCategories(childItem, allCategories);
                parentItem.Items.Add(childItem);
            }
        }
        private void treeViewCategorias_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeViewCategorias.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Categoria selectedCategory)
            {
                var productosFiltrados = dbProducto.GetProductosByCategoria(selectedCategory);
                dtgProd.ItemsSource = productosFiltrados;
            }
        }

    }
}
