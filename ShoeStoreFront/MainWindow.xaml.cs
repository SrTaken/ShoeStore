using DB;
using Model.Cesta;
using Model.User;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeStoreFront;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Utils.mongoDBConnection = new MongoDBConnection();

        Utils.UsuarioManager = new UsuarioManager(Utils.mongoDBConnection);
        Utils.CestaManager = new CestaManager(Utils.mongoDBConnection);
        Utils.ProductoManager = new ProductoManager(Utils.mongoDBConnection);
        Utils.CategoriaManager = new CategoriaManager(Utils.mongoDBConnection);
        Utils.MetodoEnvioManager = new MetodoEnvioManager(Utils.mongoDBConnection);
        Utils.IVAManager = new IVAManager(Utils.mongoDBConnection);

    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(txbUsr.Text) || string.IsNullOrEmpty(txbPasw.Text)) return;

        Utils.LoggedUser = Utils.UsuarioManager.Login(txbUsr.Text, txbPasw.Text);
        if(Utils.LoggedUser != null)
        {
            CestaManager cestaManager = new CestaManager(Utils.mongoDBConnection);
            Utils.MyCesta = cestaManager.GetCestaByUsuarioId(Utils.LoggedUser.Id);

            if (Utils.MyCesta == null)
            {
                Utils.MyCesta = new Cesta
                {
                    UsuarioId = Utils.LoggedUser.Id,
                    PrecioFinal = 0,
                    MetodoEnvio = Utils.MetodoEnvioManager.GetMetodosEnvio().FirstOrDefault().Id,
                    Productos = new ObservableCollection<ItemCesta>()
                };
                cestaManager.CrearCesta(Utils.MyCesta);
            }

            MessageBox.Show("Bienvenido " + Utils.LoggedUser.Nombre); 
            ProductosWindow productosWindow = new ProductosWindow();
            productosWindow.Show();
            this.Close();
        }
        else
        {
            MessageBox.Show("Usuario o contraseña incorrectos");
        }
    }
}