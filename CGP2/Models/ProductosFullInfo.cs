namespace CGP2_1.Models
{
    public class ProductosFullInfo
    {
        public string Company { get; set; }
        public string Vendedor { get; set; }
        public string Producto { get; set; }
        public string Descripcion { get; set; }
        public string Proveedor { get; set; }
        public string SubGrupo { get; set; }
        public string Grupo { get; set; }
        public string Sku { get; set; }
        public decimal? CostobrutoU1 { get; set; }
        public decimal? CostobrutoU2 { get; set; }
        public byte? VentaUndidad { get; set; }
        public double? CantMaxUnid { get; set; }
        public double? Descuento1 { get; set; }
        public double? Descuento2 { get; set; }
        public double? Descuento3 { get; set; }
        public decimal? PorcImp { get; set; }
        public int? TipoProducto { get; set; }
        public int? CidProductod { get; set; }
        public decimal? Pmv { get; set; }
    }
}