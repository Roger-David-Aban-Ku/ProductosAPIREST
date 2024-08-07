namespace ProductosAPIREST.Models
{
    public class Producto
    {
        public int id{ get; set; }
        public required string nombre{ get; set; }
        public required string descripcion{ get; set; }
        public required decimal precio {  get; set; }
        public required int cantidad {  get; set; }
    }
}
