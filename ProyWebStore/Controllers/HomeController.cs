using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Diagnostics;

namespace ProyWebStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductoDAO productoDAO;
        private readonly ClienteDAO clienteDAO;
        private readonly CategoriaDAO categoriaDAO;

        public HomeController(ILogger<HomeController> logger, ProductoDAO producto, 
                ClienteDAO cliente, CategoriaDAO categoria)
        {
            _logger = logger;
            productoDAO = producto;
            clienteDAO = cliente;
            categoriaDAO = categoria;
        }

        public async Task<IActionResult> Salir() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginUsuario", "LoginUsuario");
        }

        public IActionResult Index(string nombreUsuario)
        {
            ViewBag.NombreUsuario = nombreUsuario;
            int cantProductos = productoDAO.GetProductos().Count;
            int cantCategorias = categoriaDAO.GetCategorias().Count;
            int cantClientes = clienteDAO.GetClientes().Count;
            ViewData["CantProductos"] = cantProductos;
            ViewData["CantCategorias"] = cantCategorias;
            ViewData["CantClientes"] = cantClientes;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
