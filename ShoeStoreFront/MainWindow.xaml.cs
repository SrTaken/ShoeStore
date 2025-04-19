using BackFactory;
using DB;
using Microsoft.VisualBasic;
using Model;
using Model.Cesta;
using Model.User;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
    //Metodo para que c2 pueda tener el idioma actual
    public static void InitializeCurrentLanguageForWPF()
    {
        if (!languageLoaded)
        {
            // Create a made-up IETF language tag "more specific" than the culture we are based on.
            // This allows all standard logic regarding IETF language tag hierarchy to still make sense and we are
            // compatible with the fact that we may have overridden language specific defaults with Windows OS settings.
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var language = XmlLanguage.GetLanguage(culture.IetfLanguageTag + "-current");
            var type = typeof(XmlLanguage);
            const BindingFlags kField = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            type.GetField("_equivalentCulture", kField).SetValue(language, culture);
            type.GetField("_compatibleCulture", kField).SetValue(language, culture);
            if (culture.IsNeutralCulture)
                culture = System.Globalization.CultureInfo.CreateSpecificCulture(culture.Name);
            type.GetField("_specificCulture", kField).SetValue(language, culture);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(language));

            languageLoaded = true;
        }
    }
    private static bool languageLoaded = false;
    public MainWindow()
    {
        InitializeCurrentLanguageForWPF();
        InitializeComponent();

        Utils.mongoDBConnection = new MongoDBConnection();
        Utils.UsuarioManager = new UsuarioManager(Utils.mongoDBConnection);
        Utils.CestaManager = new CestaManager(Utils.mongoDBConnection);
        Utils.ProductoManager = new ProductoManager(Utils.mongoDBConnection);
        Utils.CategoriaManager = new CategoriaManager(Utils.mongoDBConnection);
        Utils.MetodoEnvioManager = new MetodoEnvioManager(Utils.mongoDBConnection);
        Utils.IVAManager = new IVAManager(Utils.mongoDBConnection);
        Utils.EmpresaManager = new EmpresaManager(Utils.mongoDBConnection);
        Utils.FacturaManager = new FacturaManager(Utils.mongoDBConnection);

    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(txbUsr.Text) || string.IsNullOrEmpty(txbPasw.Password)) return;

        string hashedPassword = Cifrar(txbPasw.Password);
        Utils.LoggedUser = Utils.UsuarioManager.Login(txbUsr.Text, "crypted-"+hashedPassword);
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

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private string Cifrar(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}