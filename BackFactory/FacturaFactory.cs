using Model.Cesta;
using Model.Factura;
using Model.User;

namespace BackFactory
{
    public class FacturaFactory
    {
        public static void crearFactura(Cesta cesta, Usuario usuario)
        {
            Factura factura = new Factura();
            factura.Empresa = Utils.EmpresaManager.GetEmpresa(); //Empresa solo hay 1
            factura.Fecha = System.DateTime.Now;
            factura.PrecioFinal = cesta.PrecioFinalConIva;
            factura.PrecioBase = cesta.PrecioFinal;
            factura.Impuestos = cesta.Impuestos;
            factura.Cliente = new ClienteFactura();
            factura.Cliente.ClienteId = usuario.Id;
            factura.MetodoEnvio = Utils.MetodoEnvioManager.GetMetodoEnvio(cesta.MetodoEnvio);

            foreach (var item in cesta.Productos)
            {
                LineaFactura linea = new LineaFactura();
                linea.ProductoId = item.TallaId;
                linea.Cantidad = item.Cantidad;
                linea.PrecioUnitario = item.Precio;
                linea.Nombre = item.Nombre;
                linea.Descuento = item.Descuento;
                linea.Iva = item.IVA;
                factura.Lineas.Add(linea);
            }


            factura.Cliente.Nombre = usuario.Nombre;
            factura.Cliente.Telefono = usuario.Telefono;
            factura.Cliente.ClienteId = usuario.Id;
            factura.Cliente.CCPago = usuario.CC;
            factura.Cliente.Direccion = usuario.Direcciones;

            //Ultimos pasos
            Utils.FacturaManager.InsertFactura(factura);
            Utils.CestaManager.EliminarCesta(cesta);
        }
    }
}
