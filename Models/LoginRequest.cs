using System;
using Newtonsoft.Json;

namespace Front_Login.Models
{
    public class LoginRequest
    {
        //[JsonProperty("userUsuario")]
        //public string UserUsuario { get; set; }

        [JsonProperty("apellidos")]
        public string NombreUsuario { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("contrasena")]
        public string Contrasena { get; set; }

        [JsonProperty("email")]
        public string EmailUsuario { get; set; }


    }
}
