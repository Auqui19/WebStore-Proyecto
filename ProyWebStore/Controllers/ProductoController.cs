using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.Controllers
{
    [Authorize(Roles = "1")]
    public class ProductoController : Controller
    {
        private readonly ProductoDAO productoDAO;
        private readonly MarcaDAO marcaDAO;
        private readonly CategoriaDAO categoriaDAO;
        private readonly IWebHostEnvironment _env;

        public ProductoController(ProductoDAO producto, MarcaDAO marca, CategoriaDAO categoria, IWebHostEnvironment env)
        {
            productoDAO = producto;
            marcaDAO = marca;
            categoriaDAO = categoria;
            _env = env;
        }

        public ActionResult IndexProductos()
        {
            var listado = productoDAO.GetProductos();
            return View(listado);
        }
        public ActionResult DetailsProducto(int id)
        {
            var buscado = productoDAO.GetProductos().Find(art => art.IdProducto == id);
            return View(buscado);
        }

        public ActionResult CreateProducto()
        {
            Producto nuevo = new Producto();
            ViewBag.categoria = new SelectList(categoriaDAO.GetCategoriasActivas(), "IdCategoria", "Descripcion");
            ViewBag.marca = new SelectList(marcaDAO.GetMarcasActivas(), "IdMarca", "Descripcion");
            return View(nuevo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProducto(Producto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = productoDAO.GrabarProducto(obj);
                    return RedirectToAction(nameof(IndexProductos));
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Error: " + ex.Message;
            }
            
            ViewBag.categoria = new SelectList(categoriaDAO.GetCategoriasActivas(), "IdCategoria", "Descripcion", obj.IdCategoria);
            ViewBag.marca = new SelectList(marcaDAO.GetMarcasActivas(), "IdMarca", "Descripcion", obj.IdMarca);
            return View(obj);
        }
        
        public ActionResult EditProducto(int id)
        {
            var buscado = productoDAO.GetProductos().Find(p => p.IdProducto == id);
            ViewBag.categoria = new SelectList(categoriaDAO.GetCategoriasActivas(), "IdCategoria", "Descripcion");
            ViewBag.marca = new SelectList(marcaDAO.GetMarcasActivas(), "IdMarca", "Descripcion");
            return View(buscado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducto(int id, Producto obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        TempData["mensaje"] = productoDAO.GrabarProducto(obj);
                        return RedirectToAction(nameof(IndexProductos));
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Error: " + ex.Message;
                }
            }
            ViewBag.categoria = new SelectList(categoriaDAO.GetCategoriasActivas(), "IdCategoria", "Descripcion", obj.IdCategoria);
            ViewBag.marca = new SelectList(marcaDAO.GetMarcasActivas(), "IdMarca", "Descripcion", obj.IdMarca);
            return View(obj);
        }

        public ActionResult DeleteProducto(int id)
        {
            var buscado = productoDAO.GetProductos().Find(p => p.IdProducto == id);
            return View(buscado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProducto(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = productoDAO.EliminarProducto(id);
                return RedirectToAction(nameof(IndexProductos));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View();
        } 
    }
}
