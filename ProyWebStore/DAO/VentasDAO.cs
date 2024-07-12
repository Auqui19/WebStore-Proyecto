using ProyWebStore.Models;

namespace ProyWebStore.DAO
{
    public class VentasDAO
    {
        private string cadena_sql = "";
        public VentasDAO(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        public int GrabarVenta(int idCliente, decimal montoTotal)
        {
            int numVenta = Convert.ToInt32(SqlHelper.ExecuteScalar(cadena_sql, "GrabarVentaProducto", idCliente, montoTotal));
            return numVenta;
        }

        public void GrabarVentaDetalle(int numVenta, Carrito fila)
        {
            SqlHelper.ExecuteReader(cadena_sql, "GrabarVentaDetalle", numVenta, fila.Codigo, fila.Cantidad, fila.Precio);
        }
    }
}
