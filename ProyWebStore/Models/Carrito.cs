namespace ProyWebStore.Models
{
    public class Carrito
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe
        {
            get
            {
                return Precio * Cantidad;
            }
        }
    }
}
