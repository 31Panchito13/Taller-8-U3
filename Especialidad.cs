using Newtonsoft.Json;

namespace Front_Login.Models
{
    public class Especialidad
    {
        [JsonProperty("idEspecialidad")]
        public int IdCategoria { get; set; }

        [JsonProperty("nombreEspecialidad")]
        public string NomCategoria { get; set; }

        [JsonProperty("estadoEspecialidad")]
        public string EstadoCategoria { get; set; }
    }
}
