using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Front_Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Front_Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Muestra el formulario de inicio de sesión
        public IActionResult Index()
        {
            return View();
        }

        // Muestra el formulario de registro (Login/Registro.cshtml)
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }
        ///////////////////////////////
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Modelo inválido. Revisa los campos.";
                return View("Index", model);
            }

            try
            {
                // 👇 Construir el JSON para enviar al backend
                var payload = new
                {
                    email = model.EmailUsuario,
                    contrasena = model.Contrasena
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 👇 POST hacia tu API de Spring Boot
                var response = await _httpClient.PostAsync("http://localhost:8080/api/clientes/login", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var cliente = JsonConvert.DeserializeObject<Usuario>(responseBody);
                    HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(cliente));
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["Error"] = "Credenciales incorrectas";
                }
                else
                {
                    TempData["Error"] = $"Error en el servidor: {responseBody}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Excepción: {ex.Message}";
            }

            return View("Index", model);
        }


        ///////////////////////////////

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] LoginRequest model)
        {
            try
            {
                var payload = new
                {
                    nombre = model.NombreUsuario,
                    telefono = model.Telefono,
                    email = model.EmailUsuario,
                    contrasena = model.Contrasena
                };

                var jsonContent = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:8080/api/clientes", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { ok = true, mensaje = "Usuario registrado" });
                }
                else
                {
                    return Json(new { ok = false, mensaje = responseBody });
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }

    }

}
