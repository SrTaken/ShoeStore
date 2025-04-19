using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BackFactory;
using DB;
using Microsoft.Web.WebView2.Core;
using Model.Cesta;
using Model.Product;
using ShoeStoreFront.controls;

namespace ShoeStoreFront
{
    public partial class ProductoDetailsWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Producto SelectedProducto { get; set; }
        public string SelectedImage { get; set ; }
        public VarianteProducto SelectedVariante { get; set; }
        public TallaVarianteProducto SelectedTalla { get; set; }

        public ProductoDetailsWindow(Producto producto)
        {
            InitializeComponent();
            SelectedProducto = producto;
            lsvVariantes.SelectedIndex = 0;
            DataContext = this;
            InitializeWebView();
            this.Title = SelectedProducto.Nombre;
        }

        private async void InitializeWebView()
        {
            await wvDesc.EnsureCoreWebView2Async(null);
            wvDesc.CoreWebView2.NavigateToString(SelectedProducto.Descripcion);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                SelectedImage = image.Source.ToString();
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var existingItem = Utils.MyCesta.Productos.FirstOrDefault(item => item.TallaId == SelectedTalla.Id);
            if (existingItem != null)
            {
                existingItem.Cantidad += (int)spinnerCantidad.Value;
                //Utils.MyCesta.RecalcularPrecioFinal();
            }
            else
            {
                Utils.MyCesta.AñadirProducto(new ItemCesta
                {
                    TallaId = SelectedTalla.Id,
                    Cantidad = (int)spinnerCantidad.Value,
                    IVA = Utils.IVAManager.GetIVA(SelectedProducto.Iva),
                    Talla = SelectedTalla.Talla
                });
            }
                
            Utils.CestaManager.ActualizarCesta(Utils.MyCesta);

            //SelectedTalla.Stock -= (int)spinnerCantidad.Value;
            //Utils.ProductoManager.ActualizarStockTalla(SelectedProducto.Id, SelectedTalla.Id, SelectedTalla.Stock);

            lsvTallas_SelectionChanged(null, null);
            MessageBox.Show($"{SelectedProducto.Nombre} añadido a la cesta.");
            spinnerCantidad.Value = 1;
            var header = new HeaderControl();
            header.UpdateCestaItems();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var productosWindow = new ProductosWindow();
            productosWindow.Show();
            this.Close();
        }

        private void lsvVariantes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarianteProducto variante = lsvVariantes.SelectedItem as VarianteProducto;
            SelectedVariante = variante;
            SelectedImage = SelectedVariante.Imagenes[0];

            txbDesc.Text = (SelectedVariante.Descuento / 100).ToString("P0");
            if (SelectedVariante.Descuento > 0)
            {
                txbPrecio.TextDecorations = TextDecorations.Strikethrough;
                txbPrecio.Foreground = System.Windows.Media.Brushes.Gray;
                txbPrecioFinal.Text = (SelectedVariante.Precio - (SelectedVariante.Precio * (SelectedVariante.Descuento / 100))).ToString("C2");
                borderDesc.Visibility = Visibility.Visible;
            }
            else 
            { 
                borderDesc.Visibility = Visibility.Collapsed;
                txbPrecio.TextDecorations = null;
                txbPrecioFinal.Text = null;
                txbPrecio.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            }
            
        }

        private void lsvTallas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTalla = lsvTallas.SelectedItem as TallaVarianteProducto;
            if(SelectedTalla != null)
            {
                btnAddQT.Visibility = Visibility.Visible;
                spinnerCantidad.Maximum = SelectedTalla.Stock;
                btnAddCarro.IsEnabled = SelectedTalla.Stock > 0 ? true : false;
            }
            else
            {
                btnAddCarro.IsEnabled = false;
                btnAddQT.Visibility = Visibility.Collapsed;
            }
                
        }

        private void btnAddQT_Click(object sender, RoutedEventArgs e)
        {
            SelectedTalla.Stock += 1;
            Utils.ProductoManager.ActualizarStockTalla(SelectedProducto.Id  ,SelectedTalla.Id, SelectedTalla.Stock);
            lsvTallas_SelectionChanged(null, null);
        }
    }
}