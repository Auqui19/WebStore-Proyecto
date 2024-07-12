namespace ProyWebStore.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int CodUsuario { get; set; }
        public decimal VtaIgv { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaVenta { get; set; }
    }
}
