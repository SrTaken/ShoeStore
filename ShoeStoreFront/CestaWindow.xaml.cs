using System.Windows;
using System.Windows.Controls;
using BackFactory;
using DB;
using Model.Cesta;
using Model.Product;
using MongoDB.Bson;
using ShoeStoreFront.controls;

namespace ShoeStoreFront
{
    public partial class CestaWindow : Window
    {

        public CestaWindow()
        {
            InitializeComponent();
            cargarProductos();
            Utils.MyCesta.RecalcularPrecioFinal();
            lvCestaProductos.ItemsSource = Utils.MyCesta.Productos;



            var metodosEnvio = Utils.MetodoEnvioManager.GetMetodosEnvio();
            cbMetodoEnvio.ItemsSource = metodosEnvio;
            cbMetodoEnvio.SelectedItem = Utils.MetodoEnvioManager.GetMetodoEnvio(Utils.MyCesta.MetodoEnvio);

            Utils.MyCesta.precioMetodoEnvioMinimo = (int)((MetodoEnvio)cbMetodoEnvio.SelectedItem).PrecioMinimo;
            Utils.MyCesta.precioMetodoEnvioBase = (int)((MetodoEnvio)cbMetodoEnvio.SelectedItem).PrecioBase;
            Utils.MyCesta.MetodoEnvioIVA = Utils.IVAManager.GetIVA(((MetodoEnvio)cbMetodoEnvio.SelectedItem).Iva).Porcentaje;
            Utils.MyCesta.RecalcularPrecioFinal();

            txtTotal.Text = Utils.MyCesta.PrecioFinalConIva.ToString("C"); 


        }

        private static void cargarProductos()
        {
            foreach (var item in Utils.MyCesta.Productos)
            {
                var producto = Utils.ProductoManager.GetProductoByTallaId(item.TallaId);
                if (producto != null)
                {
                    var variante = producto.Variantes.FirstOrDefault(v => v.Tallas.Any(t => t.Id == item.TallaId));
                    if (variante != null)
                    {
                        item.Nombre = producto.Nombre;
                        item.Imagen = variante.Imagenes.First();
                        item.Precio = (decimal)variante.Precio;
                        item.Descuento = variante.Descuento;
                        item.IVA = Utils.IVAManager.GetIVA(producto.Iva);
                    }
                }
            }
        }
        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            
            if (lvCestaProductos.Items.Count == 0)
            {
                MessageBox.Show("No hay productos en la cesta.");
                return;
            }

            //TODO Falta implementar la funcionalidad de pago

            var pagoWindow = new PagoWindow();
            pagoWindow.Show();
            this.Close();


        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            var productosWindow = new ProductosWindow();
            productosWindow.Show();
            this.Close();
        }

        private void cbMetodoEnvio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Utils.MyCesta.MetodoEnvio = ((MetodoEnvio)cbMetodoEnvio.SelectedItem).Id;
            Utils.MyCesta.precioMetodoEnvioMinimo = (decimal)((MetodoEnvio)cbMetodoEnvio.SelectedItem).PrecioMinimo;
            Utils.MyCesta.precioMetodoEnvioBase = (decimal)((MetodoEnvio)cbMetodoEnvio.SelectedItem).PrecioBase;
            Utils.MyCesta.MetodoEnvioIVA = Utils.IVAManager.GetIVA(((MetodoEnvio)cbMetodoEnvio.SelectedItem).Iva).Porcentaje;

            Utils.MyCesta.RecalcularPrecioFinal(); 

            Utils.CestaManager.ActualizarCesta(Utils.MyCesta);
            txtTotal.Text = Utils.MyCesta.PrecioFinalConIva.ToString("C");
        }

        private void CestaItemControl_EliminarClicked(object sender, ObjectId e)
        {
            var item = Utils.MyCesta.Productos.FirstOrDefault(p => p.TallaId == e);
            if (item != null)
            {
                Utils.MyCesta.Productos.Remove(item);
                Utils.CestaManager.ActualizarCesta(Utils.MyCesta);
                Utils.MyCesta.RecalcularPrecioFinal();
                txtTotal.Text = Utils.MyCesta.PrecioFinalConIva.ToString("C");
            }
        }
    }
}
