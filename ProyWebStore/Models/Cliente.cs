namespace ProyWebStore.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string nombres { get; set; } = "";
        public string apellido { get; set; } = "";
        public int docIdentidad { get; set; }
        public bool activo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
