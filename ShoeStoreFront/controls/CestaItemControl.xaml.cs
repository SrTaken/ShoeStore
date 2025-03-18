using Model.Cesta;
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

            control.MyItemChanged( e);

            
        }

        private void MyItemChanged(DependencyPropertyChangedEventArgs e)
        {
            decimal totalConIVA = MyItemCesta.Precio * MyItemCesta.Cantidad * (1 + MyItemCesta.IVA / 100);

            txtTotalConIVA.Text = totalConIVA+"";
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            EliminarClicked?.Invoke(this, MyItemCesta.TallaId);
        }
    }
}
