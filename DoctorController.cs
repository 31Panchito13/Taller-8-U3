using Front_Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Front_Login.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrlDoctor = "doctor"; // 👈 debe ser igual al endpoint del backend (revisa si es /api/doctor)
        private readonly string apiUrlEspecialidad = "http://localhost:8082/api/especialidad";

        public DoctorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8082/api/");
        }

        // 🔹 Obtener lista de doctores
        private async Task<List<Doctor>> ObtenerDoctor()
        {
            var response = await _httpClient.GetAsync(apiUrlDoctor);
            if (!response.IsSuccessStatusCode) return new List<Doctor>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Doctor>>(json);
        }

        // 🔹 Obtener lista de especialidades (para el combo)
        private async Task<List<Especialidad>> ObtenerEspecialidades()
        {
            var response = await _httpClient.GetAsync(apiUrlEspecialidad);
            if (!response.IsSuccessStatusCode) return new List<Especialidad>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Especialidad>>(json);
        }

        // 🔹 Mostrar lista de doctores
        public async Task<IActionResult> Index()
        {
            var doctores = await ObtenerDoctor();
            if (doctores.Count == 0)
            {
                ViewBag.Error = "No se pudo obtener la lista de doctores desde la API.";
            }
            return View(doctores);
        }

        // 🔹 Mostrar formulario para crear nuevo doctor
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Especialidades = await ObtenerEspecialidades();
            return View();
        }

        // 🔹 Crear nuevo doctor
        [HttpPost]
        public async Task<IActionResult> Crear(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidades = await ObtenerEspecialidades();
                TempData["ErrorMessage"] = "Por favor, complete todos los campos.";
                return View(doctor);
            }

            var json = JsonConvert.SerializeObject(doctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 🔹 CORREGIDO: se envía al endpoint correcto
            var response = await _httpClient.PostAsync(apiUrlDoctor, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Doctor registrado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al registrar el doctor.";
            ViewBag.Especialidades = await ObtenerEspecialidades();
            return View(doctor);
        }

        // 🔹 Detalle del doctor
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            Doctor doctor = null;
            var response = await _httpClient.GetAsync($"{apiUrlDoctor}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                doctor = JsonConvert.DeserializeObject<Doctor>(json);
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo obtener el doctor.";
                return RedirectToAction("Index");
            }

            ViewBag.Doctores = await ObtenerDoctor();
            return View(doctor);
        }

        // 🔹 Eliminar doctor
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiUrlDoctor}/{id}");
                TempData["SuccessMessage"] = response.IsSuccessStatusCode
                    ? "Doctor eliminado correctamente."
                    : "Error al eliminar el doctor.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error de conexión: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
        // 🔹 Mostrar formulario para editar un doctor
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUrlDoctor}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "No se pudo obtener la información del doctor.";
                return RedirectToAction("Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            var doctor = JsonConvert.DeserializeObject<Doctor>(json);

            ViewBag.Especialidades = await ObtenerEspecialidades();
            return View(doctor);
        }

        // 🔹 Enviar cambios del doctor editado
        [HttpPost]
        public async Task<IActionResult> Editar(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidades = await ObtenerEspecialidades();
                TempData["ErrorMessage"] = "Por favor, complete todos los campos correctamente.";
                return View(doctor);
            }

            var json = JsonConvert.SerializeObject(doctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiUrlDoctor}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Doctor actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el doctor.";
            ViewBag.Especialidades = await ObtenerEspecialidades();
            return View(doctor);
        }

    }
}
