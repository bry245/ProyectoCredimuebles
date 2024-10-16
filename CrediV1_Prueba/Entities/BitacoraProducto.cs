namespace CrediV1_Prueba.Entities
{
    public class BitacoraProducto
    {

        public int idBProducto {  get; set; }
        public long idProducto { get; set; }
        public long idUsuario { get; set; }
        public string nombreProducto {  get; set; }
        public string nombreUsuario { get; set; }

        public string accion {  get; set; }

        public DateTime fecha { get; set; }
    }
}
