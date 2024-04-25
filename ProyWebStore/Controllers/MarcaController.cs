using Microsoft.AspNetCore.Mvc;
using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.Controllers
{
    public class MarcaController : Controller
    {
        private string cad_cone = "";

        public MarcaController(IConfiguration confi)
        {
            cad_cone = confi.GetConnectionString("conexiondb");
        }

        #region CRUD DE MARCA
        public List<Marca> GetMarcas()
        {
            var lista = new List<Marca>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cad_cone, "sp_ListarMarcas");
            while (dr.Read())
            {
                lista.Add(new Marca()
                {
                    IdMarca = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    Activo = dr.GetBoolean(2),
                    FechaRegistro = dr.GetDateTime(3),
                });
            }
            return lista;
        }

        public string GrabarMarca(Marca obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cad_cone, "sp_AgregarMarca",
                                  obj.IdMarca, obj.Descripcion, obj.Activo);
                return $"La marca: {obj.Descripcion} fue Registrada / Actualizada correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EliminarMarca(int idMarca)
        {
            SqlHelper.ExecuteNonQuery(cad_cone, "sp_EliminarMarca", idMarca);
            string cad = $"La marca con Id {idMarca} fue eliminada correctamente";
            return cad;
        }
        #endregion 

        public IActionResult IndexMarcas()
        {
            var listado = GetMarcas();
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
                    TempData["mensaje"] = GrabarMarca(obj);
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
            var buscada = GetMarcas().Find(marca => marca.IdMarca == id);
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
                    TempData["mensaje"] = GrabarMarca(obj);
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
            var buscada = GetMarcas().Find(marca => marca.IdMarca == id);
            return View(buscada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMarca(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = EliminarMarca(id);
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
