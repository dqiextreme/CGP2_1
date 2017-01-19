using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using CGP2_1.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using CGP2_1.Clase;

namespace CGP2_1.Controllers
{
    public class Test3Controller : Controller
    {
        T3WEB_2012Entities1 _oModelos = new T3WEB_2012Entities1();
        CGP1Controller cgpcon = new CGP1Controller();
        CGP_Class cgp_mas = new CGP_Class();
        
        

        public void real_test()
        {
            Pedido ped = new Pedido();
            ped.ClienteID = 1234;
        }



        public JsonResult prodtest(string js)
        {
            List<string> lst_res = new List<string>();

            List<string> productosCarga = new List<string>();
            JToken.Parse(js).ToList().ForEach(x => productosCarga.Add(x.Value<string>("cproducto").Trim()));
            
            Pedido model = cgp_mas.master_model(productosCarga);

            ////if minimizado
            //var tesres = 1 > 0 && 2 == 3 ? 1 : 2;

            cgpcon.rec(js, model);
            
            
            return Json(Prod_Val(js));
        }

        public List<string> Prod_Val(string js)
        {
            var productosExistentes = new List<ProductosFullInfo>();

            List<string> productosCarga = new List<string>();
            JToken.Parse(js).ToList().ForEach(x => productosCarga.Add(x.Value<string>("cproducto").Trim()));

            var compUsuario = "S01";
            ProductosExistentesOt(ref productosExistentes, compUsuario, productosCarga);
            
            var result2 = productosCarga.Where(x => !productosExistentes.Any(y => y.Producto.ToString().Trim() == x.ToString().Trim())).ToList();


            return productosCarga.Where(x => !productosExistentes.Any(y => y.Producto.ToString().Trim() == x.ToString().Trim())).ToList();
        }

        private void ProductosExistentesOt(ref List<ProductosFullInfo> pProductosExistentes, string pCompUsuaro, IEnumerable<string> pProductosCarga)
        {
            var oProductosAgregar = new List<VST_T3_PRODUCTOFULLINFO_OT>();
            foreach (var strProducto in pProductosCarga)
            {
                var oProductos = _oModelos.VST_T3_PRODUCTOFULLINFO_OT.Where(
                                    x =>
                                        //x.ccompany == "S01" &&
                                        x.cvendedor == "S01_108" &&
                                        x.cproducto == strProducto
                                       ).ToList();
                if (oProductos != null)
                    oProductosAgregar.AddRange(oProductos);
            }


            pProductosExistentes.AddRange(oProductosAgregar.Select(x => new ProductosFullInfo
            {
                Company = x.ccompany,
                Vendedor = x.cvendedor,
                Producto = x.cproducto,
                Descripcion = x.cnamefc,
                SubGrupo = x.csubgrupo,
                Proveedor = x.cproveedor,
                Grupo = x.cgrupo,
                Sku = x.csku,
                CostobrutoU1 = x.ccostobruto_u1,
                CostobrutoU2 = x.ccostobruto_u2,
                VentaUndidad = x.cventaund2,
                CantMaxUnid = x.cantmaxunid,
                Descuento1 = x.descuento1,
                Descuento2 = x.descuento2,
                Descuento3 = x.descuento3,
                PorcImp = x.porcimp,
                TipoProducto = 2,
                CidProductod = x.cidproductod,
                Pmv = x.cprecioventamax
            }));
        }

    }
}
