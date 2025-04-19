using Model.Cesta;
using Model.Factura;
using Model.User;
using System.Collections.ObjectModel;

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
            factura.NumeroFactura = Utils.FacturaManager.GetUltimoNumeroFactura() + 1; //Ultimo numero de factura + 1

            foreach (var item in cesta.Productos)
            {
                LineaFactura linea = new LineaFactura();
                linea.ProductoId = item.TallaId;
                linea.Cantidad = item.Cantidad;
                linea.PrecioUnitario = item.Precio;
                linea.PrecioFinal = ((item.Precio - (item.Precio * (decimal)item.Descuento / 100)) * (1 + item.IVA.Porcentaje / 100)) *item.Cantidad;
                linea.Nombre = item.Nombre;
                linea.Variante = item.Variante;
                linea.Talla = item.Talla;
                linea.Descuento = item.Descuento;
                linea.Iva = item.IVA;
                factura.Lineas.Add(linea);
            }


            factura.Cliente.Nombre = usuario.Nombre;
            factura.Cliente.Telefono = usuario.Telefono;
            factura.Cliente.ClienteId = usuario.Id;
            factura.Cliente.CCPago = usuario.CC;
            factura.Cliente.Direccion = usuario.Direcciones;
            factura.Cliente.Nie = usuario.Nie;
            factura.Cliente.Mail = usuario.Mail;

            //Ultimos pasos
            Utils.FacturaManager.InsertFactura(factura);
            Utils.CestaManager.EliminarCesta(cesta);
            //Limiamos la cesta
            Utils.MyCesta = new Cesta
            {
                UsuarioId = Utils.LoggedUser.Id,
                PrecioFinal = 0,
                MetodoEnvio = Utils.MetodoEnvioManager.GetMetodosEnvio().FirstOrDefault().Id,
                Productos = new ObservableCollection<ItemCesta>()
            };

            FacturationMailing.enviarMail(usuario.Mail, factura.NumeroFactura); 
        }
    }
}
