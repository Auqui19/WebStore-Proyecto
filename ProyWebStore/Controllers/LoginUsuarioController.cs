using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyWebStore.DAO;
using ProyWebStore.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace ProyWebStore.Controllers
{
    public class LoginUsuarioController : Controller
    {
        private string cadena_sql = "";

        public LoginUsuarioController(IConfiguration cfg)
        {
            cadena_sql = cfg.GetConnectionString("cnx1");
        }

        public IActionResult LoginUsuario()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUsuario(Usuario u)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena_sql))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("ValidarUsuario", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@nomUsuario", u.NomUsuario);
                    command.Parameters.AddWithValue("@Clave", u.Clave);
                    command.Parameters.AddWithValue("@IdRol", u.IdRol);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["nomUsuario"] != null && u.NomUsuario != null && reader["Clave"] != null)
                        {
                            int IdRol = Convert.ToInt32(reader["IdRol"]);

                            List<Claim> c = new List<Claim>()
                            {
                                new Claim(ClaimTypes.NameIdentifier, u.NomUsuario)

                            };

                            c.Add(new Claim(ClaimTypes.Role, IdRol.ToString()));
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("IdRol")))
                                {
                                    int otroRol = Convert.ToInt32(reader["IdRol"]);
                                    c.Add(new Claim(ClaimTypes.Role, otroRol.ToString()));
                                }
                            }

                            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                            AuthenticationProperties p = new();
                            p.AllowRefresh = true;
                            p.IsPersistent = u.Activo;

                            if (!u.Activo)
                            {
                                p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                            }
                            else
                            {
                                p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
                            }

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                            if (IdRol == 1)
                            {
                                return RedirectToAction("Index", "Home", new { nombreUsuario = u.NomUsuario });
                            }
                            else if (IdRol == 2)
                            {
                                return RedirectToAction("Index", "Home", new { nombreUsuario = u.NomUsuario });
                            }
                            return RedirectToAction("Index", "Home", new { nombreUsuario = u.NomUsuario });
                        }
                        else
                        {
                            ViewBag.Error = "Credenciales incorrectas";
                        }
                    }
                    connection.Close();
                }
                return View();
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
