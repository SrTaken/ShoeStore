using System.Windows;
using System.Windows.Controls;
using Model.Product;

namespace ShoeStoreFront.controls
{
    public partial class ProductItemControl : UserControl
    {
        public ProductItemControl()
        {
            InitializeComponent();
        }

        public Producto MyProducto
        {
            get { return (Producto)GetValue(MyProductoProperty); }
            set { SetValue(MyProductoProperty, value); }
        }
        public static readonly DependencyProperty MyProductoProperty =
            DependencyProperty.Register("MyProducto", typeof(Producto), typeof(ProductItemControl), new PropertyMetadata(onProductoChangedStatic));

        private static void onProductoChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductItemControl;

            control.onProductoChanged(d, e);
        }

        private void onProductoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (MyProducto.Variantes[0].Descuento > 0)
            {
                borderDesc.Visibility = Visibility.Visible;
                txbPrecio.TextDecorations = TextDecorations.Strikethrough;
                txbPrecio.Foreground = System.Windows.Media.Brushes.Gray;
                txbPrecioFinal.Text = (MyProducto.Variantes[0].Precio - (MyProducto.Variantes[0].Precio * MyProducto.Variantes[0].Descuento/100)).ToString("C2");
            }

            bool lowStock = false;
            bool noStock = false;

            foreach (var talla in MyProducto.Variantes[0].Tallas)
            {
                if (talla.Stock < 10 && talla.Stock > 0)
                {
                    lowStock = true;
                }
                else if (talla.Stock == 0)
                {
                    noStock = true;
                    break;
                }
            }

            brdLowStock.Visibility = lowStock&&!noStock ? Visibility.Visible : Visibility.Collapsed;
            brdNoStock.Visibility = noStock ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

