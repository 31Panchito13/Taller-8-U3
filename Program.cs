using Front_Login.Controllers;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 🔧 CONFIGURACIÓN DE SERVICIOS
// ----------------------------------------------------

// Controladores y vistas
builder.Services.AddControllersWithViews();

// ⚙️ Servicios de sesión
builder.Services.AddDistributedMemoryCache(); // Requerido para almacenar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Evita acceso desde JS
    options.Cookie.IsEssential = true; // Necesario para que siempre se guarde la cookie
});

// ⚙️ Acceso a HttpContext (para sesiones)
builder.Services.AddHttpContextAccessor();

// ⚙️ Cliente HTTP (para consumir la API)
builder.Services.AddHttpClient(); // Esto basta, no necesitas registrar LoginController aquí

// ----------------------------------------------------
// 🔧 CONSTRUCCIÓN DE LA APP
// ----------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------
// 🔧 CONFIGURACIÓN DEL PIPELINE HTTP
// ----------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Archivos estáticos (CSS, JS, imágenes, etc.)
app.UseStaticFiles();

// Routing
app.UseRouting();

// 🔐 Autorización (si usas [Authorize])
app.UseAuthorization();

// 🧠 Sesiones (debe ir DESPUÉS de Routing y ANTES de MapControllerRoute)
app.UseSession();

// ----------------------------------------------------
// 🔧 RUTAS
// ----------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // 👈 Arranca en Login por defecto

// ----------------------------------------------------
// 🚀 EJECUTAR APP
// ----------------------------------------------------
app.Run();
