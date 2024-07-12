using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class UsuarioDAO
    {
        private string cadena_sql = "";
        public UsuarioDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Listado de Usuario
        public List<Usuario> GetUsuarios()
        {
            var lista = new List<Usuario>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarUsuarios");
            while (dr.Read())
            {
                lista.Add(new Usuario()
                {
                    IdUsuario = dr.GetInt32(0),
                    Nombres = dr.GetString(1),
                    Apellidos = dr.GetString(2),
                    DocIdentidad = dr.GetInt32(3),
                    Telefono = dr.GetString(4),
                    Correo = dr.GetString(5),
                    NomUsuario = dr.GetString(6),
                    Clave = dr.GetString(7),
                    Activo = dr.GetBoolean(8),
                    IdRol = dr.GetInt32(9),
                    FechaRegistro = dr.GetDateTime(10)
                });
            }
            return lista;
        }

        //Grabar - Actualizar Ususario
        public string GrabarUsuario(Usuario obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cadena_sql, "GestionarUsuario",obj.IdUsuario, obj.Nombres, obj.Apellidos, 
                    obj.DocIdentidad, obj.Telefono, obj.Correo, obj.NomUsuario, obj.Clave, obj.Activo, obj.IdRol);
                return $"El usuario : {obj.Nombres} fue Registrado / Actualizado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Eliminar Usuario
        public string EliminarUsuario(int id)
        {
            SqlHelper.ExecuteNonQuery(cadena_sql, "EliminarUsuario", id);
            string cad = $"La usuario con Id {id} fue eliminada correctamente";
            return cad;
        }
     
    }
}
