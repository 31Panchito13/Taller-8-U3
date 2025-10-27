using Front_Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Front_Login.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrlCategoria = "categoria";

        public CategoriaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8081/api/");
        }

        // Obtener lista de categorías
        private async Task<List<Categoria>> ObtenerCategorias()
        {
            var response = await _httpClient.GetAsync(apiUrlCategoria);
            if (!response.IsSuccessStatusCode) return new List<Categoria>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Categoria>>(json);
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await ObtenerCategorias();
            if (categorias.Count == 0)
            {
                ViewBag.Error = "No se pudo obtener la lista de categorías desde la API.";
            }
            return View(categorias);
        }

        // Detalle de categoría
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            Categoria categoria = null;
            var response = await _httpClient.GetAsync($"{apiUrlCategoria}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                categoria = JsonConvert.DeserializeObject<Categoria>(json);
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo obtener la categoría";
                return RedirectToAction("Index");
            }

            // Lista de categorías para combo u otra vista
            ViewBag.Categorias = await ObtenerCategorias();

            return View(categoria);
        }

        // Eliminar categoría
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiUrlCategoria}/{id}");
                TempData["Mensaje"] = response.IsSuccessStatusCode
                    ? "Categoría eliminada correctamente."
                    : "Error al eliminar la categoría.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error de conexión: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
