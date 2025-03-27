using System.Windows;
using BackFactory;
using Model.User;

namespace ShoeStoreFront
{
    public partial class PagoWindow : Window
    {

        public PagoWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            txtPrecioTotal.Text = Utils.MyCesta.PrecioFinalConIva.ToString("C2");
            txtNombre.Text = Utils.LoggedUser.CC.Nombre;
            txtNumero.Text = Utils.LoggedUser.CC.Numero;
            txtCVC.Text = Utils.LoggedUser.CC.CVC;
            txtFechaCaducidad.Text = Utils.LoggedUser.CC.Fecha_caducidad;
        }

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pago realizado con éxito.");

            this.Close();

            FacturaFactory.crearFactura(Utils.MyCesta, Utils.LoggedUser);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var cesta = new CestaWindow();
            cesta.Show();
            this.Close();
        }
    }
}
