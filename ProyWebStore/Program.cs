using Microsoft.AspNetCore.Authentication.Cookies;
using ProyWebStore.DAO;

var builder = WebApplication.CreateBuilder(args);

// DAO para la inyeccion
builder.Services.AddScoped<CategoriaDAO>();
builder.Services.AddScoped<MarcaDAO>();
builder.Services.AddScoped<ProductoDAO>();
builder.Services.AddScoped<ClienteDAO>();
builder.Services.AddScoped<UsuarioDAO>();
builder.Services.AddScoped<VentasDAO>();
builder.Services.AddScoped<RolesDAO>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(s => s.IdleTimeout = TimeSpan.FromHours(1));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie( opcion =>
{
    opcion.LoginPath = "/LoginUsuario/LoginUsuario";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "ususario",
    pattern: "{controller=LoginUsuario}/{action=LoginUsuario}");

app.Run();
