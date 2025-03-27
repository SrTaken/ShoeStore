using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Model.Cesta
{
    public class Cesta : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("usuario")]
        public ObjectId UsuarioId { get; set; }

        [BsonElement("precio_final")]
        public decimal PrecioFinal { get; set; }

        [BsonElement("precio_final_iva")]
        public decimal PrecioFinalConIva { get; set; }

        [BsonElement("impuestos")]
        public decimal Impuestos { get; set; }

        [BsonElement("metodo_envio")]
        public ObjectId MetodoEnvio { get; set; }

        [BsonElement("productos")]
        public ObservableCollection<ItemCesta> Productos { get; set; } = new ObservableCollection<ItemCesta>();

        [BsonIgnore]
        public decimal precioMetodoEnvioBase { get; set; }
        [BsonIgnore]
        public decimal precioMetodoEnvioMinimo { get; set; }
        [BsonIgnore]
        public decimal MetodoEnvioIVA { get; set; }

        public void AñadirProducto(ItemCesta item)
        {
            Productos.Add(item);
            RecalcularPrecioFinal();
        }

        public void QuitarProducto(ItemCesta item)
        {
            Productos.Remove(item);
            RecalcularPrecioFinal();
        }
        public void RecalcularPrecioFinal()
        {
            decimal totalSinIVA = (Productos.Sum(p => p.Precio  * p.Cantidad)) ;
            decimal totalSinIvaConMetodoEnvio = totalSinIVA>=precioMetodoEnvioMinimo ? totalSinIVA:totalSinIVA+precioMetodoEnvioBase;
            PrecioFinal = totalSinIvaConMetodoEnvio;

            decimal iva = Productos.Sum(p => p.Precio * p.IVA.Porcentaje / 100 * p.Cantidad);
            Impuestos = iva;

            decimal totalConIVA= (Productos.Sum(p => (p.Precio * (1 + p.IVA.Porcentaje/100)) * p.Cantidad));
            decimal totalConIvaConMetodoEnvio = totalConIVA >= precioMetodoEnvioMinimo ? totalConIVA : totalConIVA + (precioMetodoEnvioBase * 1+ MetodoEnvioIVA/100);
            PrecioFinalConIva = totalConIvaConMetodoEnvio;
        }
    }
}
