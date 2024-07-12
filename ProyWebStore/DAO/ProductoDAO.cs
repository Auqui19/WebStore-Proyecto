using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.DAO
{
    public class ProductoDAO
    {
        private string cadena_sql = "";
        public ProductoDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        //Obtener listado de los productos
        public List<Producto> GetProductos()
        {
            var lista = new List<Producto>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarProductos");
            while (dr.Read())
            {
                lista.Add(new Producto()
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.GetString(2),
                    IdMarca = dr.GetInt32(3),
                    IdCategoria = dr.GetInt32(4),
                    Precio = dr.GetDecimal(5),
                    Stock = dr.GetInt32(6),
                    ImgProducto = dr.GetString(7),
                    Activo = dr.GetBoolean(8)
                });
            }
            return lista;
        }

        public List<Producto> GetProductosActivos()
        {
            var lista = new List<Producto>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cadena_sql, "ListarProductosActivos");
            while (dr.Read())
            {
                lista.Add(new Producto()
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.GetString(2),
                    IdMarca = dr.GetInt32(3),
                    IdCategoria = dr.GetInt32(4),
                    Precio = dr.GetDecimal(5),
                    Stock = dr.GetInt32(6),
                    ImgProducto = dr.GetString(7),
                    Activo = dr.GetBoolean(8)
                });
            }
            return lista;
        }

        //Grabar - Actualizar producto
        public string GrabarProducto(Producto obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cadena_sql, "GestionarProducto", obj.IdProducto, obj.Nombre,obj.Descripcion, 
                    obj.IdMarca, obj.IdCategoria, obj.Precio, obj.Stock, obj.ImgProducto, obj.Activo);
                return $"El producto : {obj.Nombre} fue Registrado / Actualizado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Eliminar producto
        public string EliminarProducto(int id)
        {
            SqlHelper.ExecuteNonQuery(cadena_sql, "EliminarProducto", id);
            return $"El producto con ID: {id} fue eliminado correctamente";
        }
    }
}
