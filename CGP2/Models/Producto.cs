using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGP2_1.Models
{
    public class Producto
    {
        public int PedidoDetalleID { get; set; }

        public string ProductoID { get; set; }
        public bool ProductoIDError { get; set; }

        public string ProductoDesc { get; set; }

        public string Pmv { get; set; }
        public bool PmvError { get; set; }

        public int? Cajas { get; set; }
        public bool CajaError { get; set; }

        public int? Unidad { get; set; }
        public bool UnidadError { get; set; }

        public string Desc1 { get; set; }

        public double? Desc2 { get; set; }
        public bool Desc2Error { get; set; }

        public double? Desc3 { get; set; }
        public bool Desc3Error { get; set; }

        public string MensajeError { get; set; }

        public bool Existe { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? Impuesto { get; set; }

        public decimal? Total { get; set; }

        public decimal? PrecioUni { get; set; }

        public decimal? PrecioVentaU1 { get; set; }

        public decimal? PrecioVentaU2 { get; set; }

        public int? TipoProducto { get; set; }

        public bool PrecioUnidadMinimaError { get; set; }
    }
}