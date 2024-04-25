using System.ComponentModel.DataAnnotations.Schema;

namespace ProyWebStore.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string RutaImagen { get; set; } = String.Empty;
        public string NombreImagen { get; set; } = String.Empty;
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        [NotMapped]
        public IFormFile? Archivo { get; set; }
    }
}
