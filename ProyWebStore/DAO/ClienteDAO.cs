using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class ClienteDAO
    {
        private string cadena_sql = "";
        public ClienteDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Listar Cliente
        public List<Cliente> GetClientes()
        {
            var lista = new List<Cliente>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarClientesActivos");
            while (dr.Read())
            {
                lista.Add(new Cliente()
                {
                   idCliente = dr.GetInt32(0),
                   nombres = dr.GetString(1)
                });
            }
            return lista;
        }

    }
}
