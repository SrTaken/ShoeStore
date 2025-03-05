using DB;
using Model.User;
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
    private UsuarioManager db;
    public MainWindow()
    {
        InitializeComponent();
        Utils.mongoDBConnection = new MongoDBConnection();

        db = new UsuarioManager(Utils.mongoDBConnection);
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(txbUsr.Text) || string.IsNullOrEmpty(txbPasw.Text)) return;

        Usuario usuario = db.Login(txbUsr.Text, txbPasw.Text);
        if(usuario != null)
        {
            MessageBox.Show("Bienvenido " + usuario.Nombre); 
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