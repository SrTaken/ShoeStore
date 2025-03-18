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

        }
    }
}

