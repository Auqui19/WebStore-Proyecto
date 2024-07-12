using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyWebStore.DAO;
using ProyWebStore.Models;

namespace ProyWebStore.Controllers
{
    [Authorize(Roles = "1, 2")]
    public class CarritoController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductoDAO productoDAO;
        private readonly ClienteDAO clienteDAO;
        private readonly VentasDAO ventasDAO;
        List<Carrito> lista_car = new List<Carrito>();

        public CarritoController(ProductoDAO producto, ClienteDAO cliente, VentasDAO ventas, IHttpContextAccessor httpContextAccessor)
        {
            productoDAO = producto;
            clienteDAO = cliente;
            ventasDAO = ventas;
            _httpContextAccessor = httpContextAccessor;
        }

        public ActionResult ListarProductos()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Session == null)
            {
                return RedirectToAction("Error");
            }
            if (_httpContextAccessor.HttpContext.Session.GetString("carrito") == null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("carrito", JsonConvert.SerializeObject(lista_car));
            }
            var listado = productoDAO.GetProductosActivos();
            return View(listado);
        }

        public ActionResult AgregarProducto(int id)
        {
            var producto = productoDAO.GetProductos().Find(a => a.IdProducto == id);
            return View(producto);
        }


        [HttpPost]
        public ActionResult AgregarProducto(int id, int cantidad)
        {
            // Verificar si la sesión "Carrito" existe y no es nula
            if (_httpContextAccessor.HttpContext.Session.GetString("Carrito") == null)
            {
                // Si es nula, inicializar una nueva lista y asignarla a la sesión "Carrito"
                _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(new List<Carrito>()));
            }

            // Recuperar el carrito de compra
            var lista_car = JsonConvert.DeserializeObject<List<Carrito>>(_httpContextAccessor.HttpContext.Session.GetString("Carrito"));

            // Obtener los datos del artículo a agregar al carrito
            var producto = productoDAO.GetProductos().Find(a => a.IdProducto == id);


            // Buscar el artículo en el carrito
            Carrito encontrar = lista_car.Find(a => a.Codigo == producto.IdProducto);

            // Si no se encuentra el artículo en el carrito, agregarlo
            if (encontrar == null)
            {
                lista_car.Add(new Carrito()
                {
                    Codigo = producto.IdProducto ?? 0,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1 // Cantidad inicial
                });
                ViewBag.Mensaje = $"El artículo {producto.Nombre} fue agregado correctamente";
            }
            else // Si se encuentra el artículo en el carrito, aumentar la cantidad
            {
                encontrar.Cantidad++;
                ViewBag.Mensaje = $"El artículo {encontrar.Nombre} aumentó su cantidad a: {encontrar.Cantidad}";
            }

            // Actualizar la variable de sesión "Carrito" con la lista modificada
            _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(lista_car));

            return View(producto);
        }

        public ActionResult MostrarCarrito()
        {
            string carritoJson = _httpContextAccessor.HttpContext.Session.GetString("Carrito");

            if (carritoJson == null)
            {
                // Redirigir a la acción "ListarProductos" si la cadena JSON del carrito es null
                return RedirectToAction("ListarProductos");
            }

            var lista_car = JsonConvert.DeserializeObject<List<Carrito>>(carritoJson);

            ViewBag.contador = lista_car.Count;
            ViewBag.suma_importe = lista_car.Sum(a => a.Importe);

            return View(lista_car);
        }

        public ActionResult PagarCarrito()
        {
            string carritoJson = _httpContextAccessor.HttpContext.Session.GetString("Carrito");

            if (string.IsNullOrEmpty(carritoJson))
            {
                return RedirectToAction("ListarProductos");
            }

            var lista_car = JsonConvert.DeserializeObject<List<Carrito>>(carritoJson);

            // Calcular el total y el contador
            decimal total = lista_car.Sum(p => p.Importe);
            int contador = lista_car.Count;

            var clientes = clienteDAO.GetClientes();

            ViewBag.clientes = new SelectList(clientes, "idCliente", "nombres");

            ViewBag.total = total;
            ViewBag.contador = contador;

            return View(lista_car);
        }

        [HttpPost]
        public ActionResult PagarCarrito(int IdCliente)
        {
            List<Carrito> lista_car = null; // Declaramos la variable lista_car fuera del bloque try
            try
            {
                lista_car = JsonConvert.DeserializeObject<List<Carrito>>(_httpContextAccessor.HttpContext.Session.GetString("Carrito"));
                if (lista_car == null || lista_car.Count == 0)
                    return RedirectToAction("ListarProductos");
                string nomcli = clienteDAO.GetClientes().Find(c => c.idCliente.Equals(IdCliente))?.nombres;

                if (string.IsNullOrEmpty(nomcli))
                    throw new Exception("El cliente seleccionado no es válido.");

                decimal venta_total = lista_car.Sum(p => p.Importe);
                int numero = ventasDAO.GrabarVenta(IdCliente, venta_total);

                foreach (var item in lista_car)
                {
                    ventasDAO.GrabarVentaDetalle(numero, item);
                }

                _httpContextAccessor.HttpContext.Session.Remove("Carrito");
            

                ViewBag.Mensaje = $"Felicidades, {nomcli}. La venta número {numero} por un total de {venta_total} fue realizada correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                // Aquí puedes decidir cómo manejar la excepción, como redirigir a una página de error o mostrar un mensaje al usuario.
            }

            // Continuamos con la ejecución del código después del bloque try

            if (lista_car != null)
            {
                ViewBag.total = lista_car.Sum(p => p.Importe);
                ViewBag.contador = lista_car.Count;
            }
            else
            {
                ViewBag.total = 0;
                ViewBag.contador = 0;
            }

            ViewBag.clientes = new SelectList(clienteDAO.GetClientes(), "idCliente", "nombres");
            return View();
        }

        public ActionResult EliminarItemCarrito(int id)
        {
            var lista_car = JsonConvert.DeserializeObject<List<Carrito>>(_httpContextAccessor.HttpContext.Session.GetString("Carrito"));

            if (lista_car == null)
                return RedirectToAction("ListarProductos");

            Carrito eliminar = lista_car.Find(c => c.Codigo == id);
            if (eliminar != null)
            {
                lista_car.Remove(eliminar);
                _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(lista_car));
                TempData["Mensaje"] = $"Se eliminó el Articulo: {eliminar.Nombre}";
            }

            return RedirectToAction("MostrarCarrito");
        }

        public ActionResult AumentarItemCarrito(int id)
        {
            var lista_car = JsonConvert.DeserializeObject<List<Carrito>>(_httpContextAccessor.HttpContext.Session.GetString("Carrito"));

            if (lista_car == null)
                return RedirectToAction("ListarProductos");

            Carrito buscado = lista_car.Find(c => c.Codigo == id);
            if (buscado != null)
            {
                buscado.Cantidad++;
                _httpContextAccessor.HttpContext.Session.SetString("Carrito", JsonConvert.SerializeObject(lista_car));
                TempData["Mensaje"] = $"Se aumentó la cantidad del Articulo: {buscado.Nombre}";
            }

            return RedirectToAction("MostrarCarrito");
        }
    }
}
