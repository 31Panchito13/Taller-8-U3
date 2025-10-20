using System;
using Newtonsoft.Json;
namespace Front_Login.Models
{
    public class Usuario
    {
        [JsonProperty("id")]
        public int IdUsuario { get; set; }
        [JsonProperty("nombre")]
        public string NombreUsuario { get; set; }
        [JsonProperty("apellidos")]
        public string ApellidosUsuario { get; set; }
        [JsonProperty("email")]
        public string EmailUsuario { get; set; }
        [JsonProperty("user")]
        public string UserUsuario { get; set; }
        [JsonProperty("passUsuario")]
        public string PassUsuario { get; set; }
        [JsonProperty("estadoUsuario")]
        public string Estado { get; set; }
        [JsonProperty("fkIdRol")]
        public Roles FkIdRol { get; set; }
        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("contrasena")]
        public string Contrasena { get; set; }

    }
}

