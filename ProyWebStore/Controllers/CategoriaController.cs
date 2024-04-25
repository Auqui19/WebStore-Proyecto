using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyWebStore.Models;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace ProyWebStore.Controllers
{
    public class CategoriaController : Controller
    {
        private string cad_cone = "";

        public CategoriaController(IConfiguration confi)
        {
            cad_cone = confi.GetConnectionString("conexiondb");
        }

        #region CRUD DE CATEGORIA
        public List<Categoria> GetCategorias() 
        {
            var lista = new List<Categoria>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cad_cone, "sp_ListarCategorias");
            while (dr.Read()) 
            {
                lista.Add(new Categoria ()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }

        public string GrabarCategoria(Categoria obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cad_cone, "sp_AgregarCategoria",
                                  obj.IdCategoria, obj.Descripcion, obj.Activo);
                return $"La categoria: {obj.Descripcion} " + "fue Registrado / Actualizado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EliminarCategoria(int cod_cat)
        {
            SqlHelper.ExecuteNonQuery(cad_cone, "sp_EliminarCategoria", cod_cat);
            string cad = $"La categoria: {cod_cat} fue eliminado correctamente";
            return cad;
        }

        #endregion 

        public IActionResult IndexCategorias()
        {
            var listado = GetCategorias();
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
                    TempData["mensaje"] = GrabarCategoria(obj);
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
            var buscado = GetCategorias().Find(cat => cat.IdCategoria == id);            
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
                    TempData["mensaje"] = GrabarCategoria(obj);
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
            var buscado = GetCategorias().Find(cat => cat.IdCategoria == id);
            return View(buscado);
        }

        // POST: CategoriaController/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoria(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = EliminarCategoria(id);
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
