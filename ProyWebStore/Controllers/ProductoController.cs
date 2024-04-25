using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyWebStore.Models;
using System.Data.SqlClient;

namespace ProyWebStore.Controllers
{
    public class ProductoController : Controller
    {
        private string cad_cone = "";
        
        public ProductoController(IConfiguration configuration)
        {
            cad_cone = configuration.GetConnectionString("conexiondb");
        }

        #region CRUD PRODUCTO
        public List<Producto> GetProductos()
        {
            var lista = new List<Producto>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cad_cone, "sp_ListarProductos");
            while (dr.Read())
            {
                lista.Add(new Producto()
                {
                    IdProducto = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.GetString(2),
                    IdMarca = dr.GetInt32(3),
                    IdCategoria = dr.GetInt32(4),
                    Precio = dr.GetDecimal(5),
                    Stock = dr.GetInt32(6),
                    RutaImagen = dr.GetString(7),
                    NombreImagen = dr.GetString(8),
                    Activo = dr.GetBoolean(9)
                });
            }
            return lista;
        }

        public string GrabarProducto(Producto obj, string rutaImagen, string nombreImagen)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(cad_cone, "sp_AgregarProducto", obj.IdProducto, obj.Nombre,
                    obj.Descripcion, obj.IdMarca, obj.IdCategoria, obj.Precio, obj.Stock, obj.RutaImagen,
                    obj.NombreImagen, obj.Activo);
                return $"El producto: {obj.Nombre} fue Registrado / Actualizado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EliminarProducto(int id)
        {
            SqlHelper.ExecuteNonQuery(cad_cone, "sp_EliminarProducto", id);
            return $"El producto con ID: {id} fue eliminado correctamente";
        }
        #endregion

        #region Listado y Marca
        public List<Categoria> GetCategorias()
        {
            var lista = new List<Categoria>();
            SqlDataReader dr = SqlHelper.ExecuteReader(cad_cone, "sp_ListarCategorias");
            while (dr.Read())
            {
                lista.Add(new Categoria()
                {
                    IdCategoria = dr.GetInt32(0),
                    Descripcion = dr.GetString(1),
                    
                });
            }
            return lista;
        }

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
                    
                });
            }
            return lista;
        }

        #endregion


        public ActionResult IndexProductos()
        {
            var listado = GetProductos();
            return View(listado);
        }
        public ActionResult DetailsProducto(int id)
        {
            var buscado = GetProductos().Find(art => art.IdProducto == id);
            return View(buscado);
        }

        public ActionResult CreateProducto()
        {
            Producto nuevo = new Producto();
            ViewBag.categoria = new SelectList(GetCategorias(), "IdCategoria", "Descripcion");
            ViewBag.marca = new SelectList(GetMarcas(), "IdMarca", "Descripcion");
            return View(nuevo);
        }

        [HttpPost]
        public ActionResult CreateProducto(Producto obj, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (archivo != null && archivo.Length > 0)
                    {
                        // Aquí puedes manejar el archivo, guardar en el sistema de archivos o en una base de datos.
                        // Por simplicidad, vamos a asumir que guardamos la ruta del archivo en la propiedad RutaImagen del producto.
                        obj.RutaImagen = GuardarArchivo(archivo);
                        obj.NombreImagen = archivo.FileName;
                    }

                    // Luego, insertas el producto en la base de datos
                    GrabarProducto(obj, obj.RutaImagen, obj.NombreImagen);

                    TempData["mensaje"] = $"El producto '{obj.Nombre}' fue registrado correctamente";
                    return RedirectToAction(nameof(IndexProductos));
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Error: " + ex.Message;
                }
            }

            // Si hay errores de validación, vuelves a mostrar el formulario con los datos ingresados
            ViewBag.categoria = new SelectList(GetCategorias(), "IdCategoria", "Descripcion", obj.IdCategoria);
            ViewBag.marca = new SelectList(GetMarcas(), "IdMarca", "Descripcion", obj.IdMarca);
            return View(obj);
        }
        public ActionResult EditProducto(int id)
        {
            var buscado = GetProductos().Find(p => p.IdProducto == id);
            ViewBag.categoria = new SelectList(GetCategorias(), "IdCategoria", "Descripcion");
            ViewBag.marca = new SelectList(GetMarcas(), "IdMarca", "Descripcion");
            return View(buscado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducto(int id, Producto obj, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (archivo != null && archivo.Length > 0)
                    {
                        // Manejar el archivo como en el método CreateProducto
                        obj.RutaImagen = GuardarArchivo(archivo);
                        obj.NombreImagen = archivo.FileName;
                    }

                    // Actualizas el producto en la base de datos
                    GrabarProducto(obj, obj.RutaImagen, obj.NombreImagen);

                    TempData["mensaje"] = $"El producto '{obj.Nombre}' fue actualizado correctamente";
                    return RedirectToAction(nameof(IndexProductos));
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Error: " + ex.Message;
                }
            }

            // Si hay errores de validación, vuelves a mostrar el formulario con los datos ingresados
            ViewBag.categoria = new SelectList(GetCategorias(), "IdCategoria", "Descripcion", obj.IdCategoria);
            ViewBag.marca = new SelectList(GetMarcas(), "IdMarca", "Descripcion", obj.IdMarca);
            return View(obj);
        }

        public ActionResult DeleteProducto(int id)
        {
            var buscado = GetProductos().Find(p => p.IdProducto == id);
            return View(buscado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProducto(int id, IFormCollection collection)
        {
            try
            {
                TempData["mensaje"] = EliminarProducto(id);
                return RedirectToAction(nameof(IndexProductos));
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error: " + ex.Message;
            }
            return View();
        }

        private string GuardarArchivo(IFormFile archivo)
        {
            if (archivo != null && archivo.Length > 0)
            {
                // Directorio donde se guardará el archivo, puedes cambiarlo según tu estructura de carpetas
                string directorio = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                // Verificar si el directorio existe, si no, crearlo
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                // Nombre del archivo (puedes generar un nombre único si es necesario)
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);

                // Ruta completa del archivo
                string rutaArchivo = Path.Combine(directorio, nombreArchivo);

                // Guardar el archivo en el sistema de archivos
                using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    archivo.CopyTo(fileStream);
                }

                // Devolver la ruta relativa del archivo (para almacenarla en la base de datos)
                return Path.GetFullPath("Images", nombreArchivo);
            }
            else
            {
                return ""; // Devolver una cadena vacía si no se proporciona ningún archivo
            }
        }
    }
}
