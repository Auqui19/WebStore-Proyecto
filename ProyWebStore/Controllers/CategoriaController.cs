using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace ProyWebStore.Controllers
{
    [Authorize(Roles = "1")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaDAO daoCat;

        public CategoriaController(CategoriaDAO cat)
        {
            daoCat = cat;
        }
        
        public IActionResult IndexCategorias()
        {
            var listado = daoCat.GetCategorias();
            return View(listado);
        }

        public ActionResult CreateCategoria()
        {
            Categoria nuevo = new Categoria();
            return View(nuevo);
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategoria(Categoria obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = daoCat.GrabarCategoria(obj);
                    return RedirectToAction(nameof(IndexCategorias));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View(obj);
        }

        // GET: CategoriaController/Edit
        public ActionResult EditCategoria(int id)
        {
            var buscado = daoCat.GetCategorias().Find(cat => cat.IdCategoria == id);            
            return View(buscado);
        }

        // POST: CategoriaController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategoria(int id, Categoria obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = daoCat.GrabarCategoria(obj);
                    return RedirectToAction(nameof(IndexCategorias));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View(obj);
        }

        // GET: CategoriaController/Delete
        public ActionResult DeleteCategoria(int id)
        {
            var buscado = daoCat.GetCategorias().Find( c => c.IdCategoria == id);
            return View(buscado);
        }

        // POST: CategoriaController/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoria(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = daoCat.EliminarCategoria(id);
                return RedirectToAction(nameof(IndexCategorias));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View();
        }
    }
}
