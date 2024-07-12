using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.Controllers
{
    [Authorize(Roles = "1")]
    public class MarcaController : Controller
    {
        private readonly MarcaDAO marcaDAO;

        public MarcaController(MarcaDAO marca)
        {
            marcaDAO = marca;
        }

        public IActionResult IndexMarcas()
        {
            var listado = marcaDAO.GetMarcas();
            return View(listado);
        }

        public ActionResult CreateMarca()
        {
            Marca nueva = new Marca();
            return View(nueva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMarca(Marca obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = marcaDAO.GrabarMarca(obj);
                    return RedirectToAction(nameof(IndexMarcas));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View(obj);
        }

        public ActionResult EditMarca(int id)
        {
            var buscada = marcaDAO.GetMarcas().Find(marca => marca.IdMarca == id);
            return View(buscada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMarca(int id, Marca obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = marcaDAO.GrabarMarca(obj);
                    return RedirectToAction(nameof(IndexMarcas));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View(obj);
        }

        public ActionResult DeleteMarca(int id)
        {
            var buscada = marcaDAO.GetMarcas().Find(marca => marca.IdMarca == id);
            return View(buscada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMarca(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = marcaDAO.EliminarMarca(id);
                return RedirectToAction(nameof(IndexMarcas));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View();
        }
        
    }
}
