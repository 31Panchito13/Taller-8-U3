using Front_Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_Login.Controllers
{
    
    public class UsuariosController : Controller
    {

        // Acción para mostrar un usuario por ID
        private readonly HttpClient _httpClient;
        private readonly string url_api = "http://localhost:8080/api/clientes/";

        public UsuariosController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/api/clientes");

        }

        public async Task<IActionResult> Index()
        {
            var usuario = new Usuario();
            var usuarios = await GetUsuariosAsync();
            return View(usuarios);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            //string url_api = $"http://localhost:8080/api/clientes";
            List<Usuario> usuarios = new List<Usuario>();
   
            try
            {
                HttpResponseMessage reponse =
                    await _httpClient.GetAsync("");

                if (reponse.IsSuccessStatusCode)
                {
                    string json = await reponse.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

                }
            }
            catch (Exception ex)
            {

            }

            return usuarios;
        }



        public async Task<Usuario> GetUsuarioPorIdAsync(long id)
        {
            Usuario usuario = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url_api+id);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(json);
                }
            }
            catch (Exception ex)
            {
                // Puedes registrar el error si deseas
                Console.WriteLine($"Error al obtener el usuario: {ex.Message}");
            }

            return usuario;
        }

        // Acción para mostrar un usuario por ID
        [Route("Usuarios/Detalle/{id}")]
        public async Task<IActionResult> Detalle(long id)
        {
            var usuario = await GetUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View("Detalles", usuario);
        }


        public async Task<IActionResult> Guardar(long id)
        {
            if (id != null)
            {
                //update

            }


            var usuario = await GetUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View("Detalles", usuario);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(long id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url_api + id);
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return StatusCode((int)response.StatusCode, "Error al eliminar el usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuario usuario)
        {
            try
            {
                string json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(url_api + usuario.IdUsuario, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Usuario actualizado correctamente");
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error del servidor: {error}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Guardar(LoginRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["Error"] = "Por favor completa todos los campos correctamente.";
        //        return View("Index", model);
        //    }

        //    try
        //    {
        //        var payload = new
        //        {
        //            userUsuario = model.UserUsuario,
        //            telefono = model.Telefono,
        //            email= model.EmailUsuario,
        //            contrasena = model.Contrasena
        //        };

        //        var json = JsonConvert.SerializeObject(payload);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync(url_api, content);
        //        var responseBody = await response.Content.ReadAsStringAsync();

        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["Success"] = "Usuario guardado correctamente.";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["Error"] = $"Error al guardar: {response.StatusCode}. Respuesta: {responseBody}";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Error al conectar con la API: {ex.Message}";
        //    }

        //    return View("Index", model);
        //}

    }
}

