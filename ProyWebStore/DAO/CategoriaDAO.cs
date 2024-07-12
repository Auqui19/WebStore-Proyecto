using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class CategoriaDAO
    {
        private string cadena_sql = "";
        public CategoriaDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Listar Categorias
        public List<Categoria> GetCategorias()
        {
            var lista = new List<Categoria>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarCategorias");
            while (dr.Read())
            {
                lista.Add(new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }

        public List<Categoria> GetCategoriasActivas()
        {
            var lista = new List<Categoria>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarCategoriasActivas");
            while (dr.Read())
            {
                lista.Add(new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }

        //Grabar - Actualizar Categoria
        public string GrabarCategoria(Categoria obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cadena_sql, "GestionarCategoria", obj.IdCategoria, obj.Descripcion, obj.Activo);
                return $"La categoria: {obj.Descripcion} " + "fue Registrado / Actualizado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EliminarCategoria(int id)
        {
            SqlHelper.ExecuteNonQuery(cadena_sql, "EliminarCategoria", id);
            string cad = $"La categoria: {id} fue desactivado correctamente";
            return cad;
        }
    }
}
