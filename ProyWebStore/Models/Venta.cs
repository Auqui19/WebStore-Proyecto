namespace ProyWebStore.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public Cliente? Cliente { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoDecimal { get; set; }
        public string Contacto { get; set; } = "";
        public string IdDistrito { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string IdTransaccion { get; set; } = "";
        public DateTime FechaVenta { get; set; }
    }
}
