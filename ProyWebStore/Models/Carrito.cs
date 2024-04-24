namespace ProyWebStore.Models
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public int IdCliente { get; set; }
        public Cliente? Cliente { get; set; }
        public int IdProducto { get; set; }
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
