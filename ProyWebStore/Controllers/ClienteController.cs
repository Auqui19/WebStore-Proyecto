using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Text;

namespace ProyWebStore.Controllers
{
    [Authorize(Roles = "1,2")]
    public class ClienteController : Controller
    {
        List<Cliente> listaClientes = new List<Cliente>();

        public async Task<List<Cliente>> traerClientes()
        {
            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync("http://localhost:5169/api/ClienteAPI/GetClientes");
                string cadena = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Cliente>>(cadena)!;
            }
        }
        public async Task<ActionResult> IndexClientes()
        {
            listaClientes = await traerClientes();
            return View(listaClientes);
        }

        public async Task<ActionResult> DetailsClientes(int id)
        {
            listaClientes = await traerClientes();
            Cliente buscado = listaClientes.Find(c => c.idCliente == id)!;
            return View(buscado);
        }

        public async Task<string> enviarCliente(int opcion, Cliente objcli)
        {
            string resultado = string.Empty;
            using (var httpcliente = new HttpClient())
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(objcli), Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();

                if (opcion == 1) // Post = Grabar
                    respuesta = await httpcliente.PostAsync("http://localhost:5169/api/ClienteAPI/PostClientes", contenido);
                else // Put = Actualizar
                    respuesta = await httpcliente.PutAsync("http://localhost:5169/api/ClienteAPI/PutClientes", contenido);

                resultado = await respuesta.Content.ReadAsStringAsync();
            }
            return resultado;
        }

        public ActionResult CreateCliente()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCliente(Cliente obj)
        {
            if (ModelState.IsValid)
            {
                TempData["mensaje"] = await enviarCliente(1, obj);
                return RedirectToAction("IndexClientes");
            }
            ViewBag.mensaje = "Error al Grabar el Cliente";
            return View(obj);
        }

        public async Task<ActionResult> EditClientes(int id)
        {
            listaClientes = await traerClientes();
            Cliente buscado = listaClientes.Find(c => c.idCliente == id)!;
            return View(buscado);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditClientes(int id, Cliente obj)
        {
            if (ModelState.IsValid == true)
            {
                TempData["mensaje"] = await enviarCliente(2, obj);
                return RedirectToAction("IndexClientes");
            }
            ViewBag.mensaje = "Error al Actualizar el Cliente";
            return View(obj);
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> DeleteClientes(int id)
        {
            listaClientes = await traerClientes();
            Cliente buscado = listaClientes.Find(c => c.idCliente == id)!;
            return View(buscado);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClientes(int id, IFormCollection collection)
        { 
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.DeleteAsync("http://localhost:5169/api/ClienteAPI/DeleteClientes/" + id);
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                TempData["mensaje"] = respuestaAPI;
            }
            return RedirectToAction("IndexClientes");
        }

    }

}
