using BackFactory;
using Model.User;
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
    /// Interaction logic for HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
            MyUsuario = Utils.LoggedUser; 
            
        }



        public Usuario MyUsuario
        {
            get { return (Usuario)GetValue(MyUsuarioProperty); }
            set { SetValue(MyUsuarioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUsuario.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUsuarioProperty =
            DependencyProperty.Register("MyUsuario", typeof(Usuario), typeof(HeaderControl), new PropertyMetadata(null));



        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Utils.LoggedUser = null;
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void btnCesta_Click(object sender, RoutedEventArgs e)
        {
            var cestaWindow = new CestaWindow();
            Window.GetWindow(this)?.Close();
            cestaWindow.Show();
        }

        private void ucHeader_Loaded(object sender, RoutedEventArgs e)
        {
            txbVentana.Text = Window.GetWindow(this)?.Title;

            txbCestaItems.Text = Utils.MyCesta.Productos.Count.ToString() ?? "0";  
        }

        public void UpdateCestaItems()
        {
            txbCestaItems.Text = Utils.MyCesta.Productos.Count.ToString() ?? "0";
        }
    }
}
