using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Front_Login.Models
{
    public class Doctor
    {
        [JsonProperty("idDoctor")]
        public int IdDoctor { get; set; }

        [JsonProperty("nombreDoctor")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NomDoctor { get; set; }

        [JsonProperty("apellidoDoctor")]
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string ApellidosDoctor { get; set; }

        [JsonProperty("emailDoctor")]
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
        public string emailDoctor { get; set; }

        [JsonProperty("edadDoctor")]
        [Required(ErrorMessage = "La edad es obligatoria.")]
        public string edadDoctor { get; set; }

        [JsonProperty("estadoDoctor")]
        [Required(ErrorMessage = "Debe indicar el estado.")]
        public string estadoDoctor { get; set; }

        // 🔹 Relación con especialidad
        [JsonProperty("fkIdEspecialidad")]
        public Especialidad fkIdEspecialidad { get; set; }
    }
}
