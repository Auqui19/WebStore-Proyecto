using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class MarcaDAO
    {
        private string cadena_sql = "";
        public MarcaDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Listado de Marcas
        public List<Marca> GetMarcas()
        {
            var lista = new List<Marca>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarMarcas");
            while (dr.Read())
            {
                lista.Add(new Marca()
                {
                    IdMarca = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }
        public List<Marca> GetMarcasActivas()
        {
            var lista = new List<Marca>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarMarcasActivas");
            while (dr.Read())
            {
                lista.Add(new Marca()
                {
                    IdMarca = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }

        //Grabar - Actualizar Marca
        public string GrabarMarca(Marca obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cadena_sql, "GestionarMarca", obj.IdMarca, obj.Descripcion, obj.Activo);
                return $"La marca: {obj.Descripcion} fue Registrada / Actualizada correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Eliminar Marca
        public string EliminarMarca(int id)
        {
            SqlHelper.ExecuteNonQuery(cadena_sql, "EliminarMarca", id);
            string cad = $"La marca con Id {id} fue eliminada correctamente";
            return cad;
        }
    }
}
