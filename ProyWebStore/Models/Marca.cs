namespace ProyWebStore.Models
{
    public class Marca
    {
        public int? IdMarca { get; set; }
        public string Descripcion { get; set; } = "";
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
    }
}
