using Newtonsoft.Json;

namespace Front_Login.Models
{
    public class Producto
    {
        [JsonProperty("idProducto")]
        public long? IdProducto { get; set; }

        [JsonProperty("nombreProducto")]
        public string NomProducto { get; set; }

        [JsonProperty("descripcionProducto")]
        public string DcProducto { get; set; }

        [JsonProperty("estadoProducto")]
        public string EsProducto { get; set; }

        [JsonProperty("precioProducto")]
        public double PreProducto { get; set; }

        [JsonProperty("stockProducto")]
        public int StockProducto { get; set; }

        [JsonProperty("categoria")]
        public Categoria Categoria { get; set; }
    }
}
//    public class Categoria
//    {
//        [JsonProperty("idCategoria")]
//        public int IdCategoria { get; set; }
//    }
//}
//{
//"nombreProducto": "Carrito  4x4",
//"descripcionProducto": "Cuantro ruedas",
//"stockProducto": 120,
//"precioProducto": 29.999,
//"estadoProducto": "Disponible",
//  "categoria": {
//        "idCategoria": 3
//  }
//}
