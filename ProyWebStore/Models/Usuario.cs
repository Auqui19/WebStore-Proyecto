namespace ProyWebStore.Models
{
    public class Usuario
    {
        public int? IdUsuario { get; set; }
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public int DocIdentidad { get; set; }
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string NomUsuario { get; set; } = "";
        public string Clave { get; set; } = "";
        public bool Activo { get; set; } = true;
        public int IdRol { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
