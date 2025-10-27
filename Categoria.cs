using Newtonsoft.Json;

namespace Front_Login.Models
{
    public class Categoria
    {


        [JsonProperty("idCategoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("nombreCategoria")]
        public string NomCategoria { get; set; }

        [JsonProperty("estado")]
        public string EstadoCategoria { get; set; }

    }
}
