using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Claims;

namespace ProyWebStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO usuarioDAO;
        private readonly RolesDAO rolesDAO;
        public UsuarioController(UsuarioDAO usuario, RolesDAO roles)
        {
            usuarioDAO = usuario;
            rolesDAO = roles;
        }

        // GET: UsuarioController
        public ActionResult IndexUsuarios()
        {
            var listado = usuarioDAO.GetUsuarios();
            return View(listado);
        }

        // GET: UsuarioController/Details/5
        public ActionResult DetailsUsuario(int id)
        {
            var buscado = usuarioDAO.GetUsuarios().Find( u => u.IdUsuario == id);
            return View(buscado);
        }

        // GET: UsuarioController/Create
        public ActionResult CreateUsuario()
        {
            Usuario nuevo = new Usuario();
            ViewBag.ROL = new SelectList(rolesDAO.GetRoles(), "IdRol", "Nombre");
            return View(nuevo);
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsuario(Usuario obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = usuarioDAO.GrabarUsuario(obj);
                    return RedirectToAction(nameof(IndexUsuarios));
                }
            }
            catch(Exception ex)
            {
                ViewBag.mensaje = "Error : " + ex.Message;
            }
            ViewBag.ROL = new SelectList(rolesDAO.GetRoles(), "IdRol", "Nombre");
            return View(obj);
        }

        // GET: UsuarioController/Edit/5
        public ActionResult EditUsuario(int id)
        {
            var buscado = usuarioDAO.GetUsuarios().Find(u => u.IdUsuario == id);
            ViewBag.ROL = new SelectList(rolesDAO.GetRoles(), "IdRol", "Nombre");
            return View(buscado);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsuario(int id, Usuario obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["mensaje"] = usuarioDAO.GrabarUsuario(obj);
                    return RedirectToAction(nameof(IndexUsuarios));
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error : " + ex.Message;
            }
            ViewBag.ROL = new SelectList(rolesDAO.GetRoles(), "IdRol", "Nombre");
            return View(obj);
        }

        // GET: UsuarioController/Delete/5
        public ActionResult DeleteUsuario(int id)
        {
            var buscado = usuarioDAO.GetUsuarios().Find(u => u.IdUsuario == id);
            return View(buscado);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUsuario(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = usuarioDAO.EliminarUsuario(id);
                return RedirectToAction(nameof(IndexUsuarios));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View();
        } 
    }
}
