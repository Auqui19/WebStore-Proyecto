using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class RolesDAO
    {
        private string cadena_sql = "";
        public RolesDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Listado de Marcas
        public List<Roles> GetRoles()
        {
            var lista = new List<Roles>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarRoles");
            while (dr.Read())
            {
                lista.Add(new Roles()
                {
                    IdRol = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                });
            }
            return lista;
        }
    }
}
