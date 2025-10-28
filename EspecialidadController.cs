using Front_Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Front_Login.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly HttpClient _httpClient;

        public EspecialidadController()
        {
            // 🔹 Dirección base CORRECTA de tu API Java
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8082/");
        }

        // 🔹 Mostrar todas las especialidades
        public async Task<IActionResult> Index()
        {
            // ⚠️ IMPORTANTE: usar la ruta completa aquí
            var response = await _httpClient.GetAsync("api/especialidad");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // ✅ Deserializar correctamente
                var lista = JsonConvert.DeserializeObject<List<Especialidad>>(json);

                // 🟢 Verificación adicional: si lista tiene datos
                if (lista != null && lista.Any())
                    return View(lista);
                else
                {
                    ViewBag.Error = "La API respondió correctamente, pero no hay datos.";
                    return View(new List<Especialidad>());
                }
            }

            ViewBag.Error = "Error al conectar con la API.";
            return View(new List<Especialidad>());
        }

        // 🔹 Editar (GET)
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"api/especialidad/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var especialidad = JsonConvert.DeserializeObject<Especialidad>(json);
                return View(especialidad);
            }

            TempData["ErrorMessage"] = "No se encontró la especialidad.";
            return RedirectToAction("Index");
        }

        // 🔹 Editar (POST)
        [HttpPost]
        public async Task<IActionResult> Editar(Especialidad especialidad)
        {
            var json = JsonConvert.SerializeObject(especialidad);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // tu API Java usa POST para insertar/actualizar
            var response = await _httpClient.PostAsync("api/especialidad", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Especialidad actualizada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar la especialidad.";
            return View(especialidad);
        }

        // 🔹 Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/especialidad/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Especialidad eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar la especialidad.";
            }

            return RedirectToAction("Index");
        }


        // 🔹 Mostrar formulario para registrar nueva especialidad
        [HttpGet]
        public IActionResult Crear()
        {
            return View(); // Muestra la vista Crear.cshtml
        }

        // 🔹 Registrar nueva especialidad
        [HttpPost]
        public async Task<IActionResult> Crear(Especialidad especialidad)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos.";
                return View(especialidad);
            }

            var json = JsonConvert.SerializeObject(especialidad);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Tu API Java usa POST para insertar una nueva especialidad
            var response = await _httpClient.PostAsync("api/especialidad", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Especialidad registrada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al registrar la especialidad.";
            return View(especialidad);
        }

    }
}
