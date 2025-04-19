using BackFactory;
using Model.Cesta;
using Model.Product;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ShoeStoreFront.controls
{
    /// <summary>
    /// Interaction logic for CestaItemControl.xaml
    /// </summary>
    public partial class CestaItemControl : UserControl
    {
        public CestaItemControl()
        {
            InitializeComponent();
        }

        public event EventHandler<ObjectId> EliminarClicked;
        public event EventHandler PrecioChanged;

        public ItemCesta MyItemCesta
        {
            get { return (ItemCesta)GetValue(MyItemCestaProperty); }
            set { SetValue(MyItemCestaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyItemCesta.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyItemCestaProperty =
            DependencyProperty.Register("MyItemCesta", typeof(ItemCesta), typeof(CestaItemControl), new PropertyMetadata(StaticMyItemChanged));

        private static void StaticMyItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CestaItemControl;

            control.MyItemChanged();            
        }

        private void MyItemChanged()
        {
            decimal precioConDescuento = MyItemCesta.Precio - (MyItemCesta.Precio * (decimal)MyItemCesta.Descuento / 100);
            decimal totalConIVA = precioConDescuento * MyItemCesta.Cantidad * (1 + MyItemCesta.IVA.Porcentaje / 100);

            txtTotalConIVA.Text = totalConIVA.ToString("C2");
            txbPrecioFinal.Text = precioConDescuento.ToString("C2");

            if (MyItemCesta.Descuento> 0)
            {
                borderDesc.Visibility = Visibility.Visible;
                txbPrecio.TextDecorations = TextDecorations.Strikethrough;
                txbPrecio.Foreground = System.Windows.Media.Brushes.Gray;
                txbPrecioFinal.Visibility = Visibility.Visible;
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            EliminarClicked?.Invoke(this, MyItemCesta.TallaId);
        }

        private void btnIncrementar_Click(object sender, RoutedEventArgs e)
        {
            MyItemCesta.Cantidad += 1;
            RecargarPrecios();
        }

        private void btnDecrementar_Click(object sender, RoutedEventArgs e)
        {
            if (MyItemCesta.Cantidad > 1)
            {
                MyItemCesta.Cantidad -= 1;
            }
            else
            {
                EliminarClicked?.Invoke(this, MyItemCesta.TallaId);
            }

            RecargarPrecios();
        }

        private void RecargarPrecios()
        {
            Utils.MyCesta.RecalcularPrecioFinal();
            MyItemChanged();
            Utils.CestaManager.ActualizarCesta(Utils.MyCesta);
            PrecioChanged?.Invoke(this, null);
        }
    }
}
