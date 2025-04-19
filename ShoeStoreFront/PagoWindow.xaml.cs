using System.Collections;
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
            txtNumero.Text = new string('*', Utils.LoggedUser.CC.Numero.Length - 4) + Utils.LoggedUser.CC.Numero.Substring(Utils.LoggedUser.CC.Numero.Length - 4); 
            txtCVC.Text = new string('*', Utils.LoggedUser.CC.CVC.Length);
            txtFechaCaducidad.Text = Utils.LoggedUser.CC.Fecha_caducidad;
        }

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            ArrayList listaErrores = new ArrayList();
            //verificamos si todos los productos se puede comprar
            foreach(var producto in Utils.MyCesta.Productos)
            {
                try
                {
                    Utils.ProductoManager.VerificarStockSuficiente(producto.TallaId, producto.Cantidad);
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains("NotEnoughtStock"))
                    {
                        listaErrores.Add($"No hay stock suficiente para el producto {producto.Nombre} {producto.Variante} {producto.Talla}!");
                    }
                    else if (ex.Message.Contains("NoProdFound"))
                    {
                        listaErrores.Add($"No existe el producto {producto.Nombre} {producto.Variante} {producto.Talla}!");
                    }
                    else if (ex.Message.Contains("NoSizeFromProdFound"))
                    {
                        listaErrores.Add($"No existe la talla {producto.Talla} del producto {producto.Nombre} {producto.Variante}!");
                    }
                }
                
            }

            if (listaErrores.Count > 0)
            {
                //han habido errores
                string mensajeError = "Se han encontrado los siguientes problemas al intentar hacer el pago:\n\n" +
                         string.Join(Environment.NewLine, listaErrores.Cast<string>());

                MessageBox.Show(mensajeError, "Errores en la compra", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach(var producto in Utils.MyCesta.Productos)
                {
                    Utils.ProductoManager.RestarStockTalla(producto.TallaId, producto.Cantidad);
                }
                FacturaFactory.crearFactura(Utils.MyCesta, Utils.LoggedUser);

                MessageBox.Show("Pago realizado con éxito.");
                var prod = new ProductosWindow();
                prod.Show();
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var cesta = new CestaWindow();
            cesta.Show();
            this.Close();
        }
    }
}
