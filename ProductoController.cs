using Front_Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Front_Login.Controllers
{
    public class ProductoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrlProducto = "producto";
        private readonly string apiUrlCategoria = "categoria";

        public ProductoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8081/api/");
        }

        // ========================
        // GET: Producto/Index
        // ========================
        public async Task<IActionResult> Index()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                var response = await _httpClient.GetAsync(apiUrlProducto);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productos = JsonConvert.DeserializeObject<List<Producto>>(apiResponse);
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener la lista de productos desde la API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error de conexión: {ex.Message}";
            }

            return View(productos);
        }

        // ========================
        // GET: Producto/ReProducto
        // ========================
        public async Task<IActionResult> ReProducto()
        {
            await CargarCategorias();
            return View();
        }

        // ========================
        // POST: Producto/ReProducto
        // ========================
        [HttpPost]
        public async Task<IActionResult> ReProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                await CargarCategorias();
                return View(producto);
            }

            try
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrlProducto, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "✅ Producto registrado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = $"⚠️ Error al registrar el producto: {response.StatusCode} - {errorContent}";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"⚠️ Error inesperado: {ex.Message}";
            }

            await CargarCategorias();
            return View(producto);
        }

        // ========================
        // Método auxiliar para cargar categorías
        // ========================
        private async Task CargarCategorias()
        {
            try
            {
                var response = await _httpClient.GetAsync(apiUrlCategoria);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ViewBag.Categorias = JsonConvert.DeserializeObject<List<Categoria>>(json);
                }
                else
                {
                    ViewBag.Categorias = new List<Categoria>();
                }
            }
            catch
            {
                ViewBag.Categorias = new List<Categoria>();
            }
        }

        // ========================
        // GET: Producto/Eliminar/{id}
        // ========================
        [HttpGet]
        public async Task<IActionResult> Eliminar(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{apiUrlProducto}/{id}");
                TempData[response.IsSuccessStatusCode ? "Mensaje" : "Error"] =
                    response.IsSuccessStatusCode
                        ? "Producto eliminado correctamente."
                        : "Error al eliminar el producto.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error de conexión: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ========================
        // GET: Producto/Detalle/{id}
        // ========================
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(long id)
        {
            var producto = await GetProductoPorIdAsync(id);
            if (producto == null)
                return NotFound();

            await CargarCategorias();
            return View(producto);
        }

        // ========================
        // Método auxiliar: obtener producto por ID
        // ========================
        public async Task<Producto> GetProductoPorIdAsync(long id)
        {
            Producto producto = null;

            try
            {
                var response = await _httpClient.GetAsync($"{apiUrlProducto}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    producto = JsonConvert.DeserializeObject<Producto>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el Producto: {ex.Message}");
            }

            return producto;
        }

        // ========================
        // POST: Producto/actualizarProducto
        // ========================
        [HttpPost]
        public async Task<IActionResult> actualizarProducto(Producto producto)
        {
            Console.WriteLine($"🟢 Entró al método actualizarProducto con ID: {producto.IdProducto}");

            if (!ModelState.IsValid)
            {
                await CargarCategorias();
                return View("Detalle", producto);
            }

            try
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{apiUrlProducto}/{producto.IdProducto}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "✅ Producto actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = $"⚠️ Error al actualizar el producto: {response.StatusCode} - {errorContent}";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"⚠️ Error inesperado: {ex.Message}";
            }

            await CargarCategorias();
            return View("Detalle", producto);
        }
    }
}
