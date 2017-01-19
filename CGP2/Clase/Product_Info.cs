using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP2_1.Clase
{
    public class Product_Info
    {
        public string ccompany { get; set; }
        public string cvendedor { get; set; }
        public string cproducto { get; set; }
        public string cnamefc { get; set; }
        public string cgrupo { get; set; }
        public string csku { get; set; }
        public Nullable<decimal> ccostobruto_u1 { get; set; }
        public Nullable<decimal> ccostobruto_u2 { get; set; }
        public Nullable<decimal> ccostoneto_u1 { get; set; }
        public Nullable<decimal> ccostoneto_u2 { get; set; }
        public Nullable<byte> cventaund2 { get; set; }
        public Nullable<double> descuento1 { get; set; }
        public Nullable<double> descuento2 { get; set; }
        public Nullable<double> descuento3 { get; set; }
        public Nullable<double> cantmaxunid { get; set; }
        public decimal porcimp { get; set; }
        public string csubgrupo { get; set; }
        public string cproveedor { get; set; }
        public Nullable<int> cidproductod { get; set; }
        public Nullable<decimal> cprecioventamax { get; set; }
    }
}