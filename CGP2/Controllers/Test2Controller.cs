using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CGP2_1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft;
using CGP2_1.Clase;


namespace CGP2_1.Controllers
{
    public class Test2Controller : Controller
    {
        T3WEB_2012Entities1 _oModelos = new T3WEB_2012Entities1();
        public ActionResult Index()
        {
            //test();
            return View();
        }

        public ActionResult _Product_Client_List()
        {
            
            List<VST_T3_JOEC_PROD> res = _oModelos.VST_T3_JOEC_PROD.Where(x => x.ccompany == "S01").ToList();
            ViewBag.list1 = res;
            return PartialView();
        }

        public ActionResult _Product_Client_List2()
        {
            List<VST_T3_JOEC_PROD> res = _oModelos.VST_T3_JOEC_PROD.Where(x => x.ccompany == "S01").ToList();
            ViewBag.list1 = res;
            return PartialView();
        }
        

        public ActionResult _Product_Client_List3()
        {
            List<VST_T3_JOEC_PROD> res = _oModelos.VST_T3_JOEC_PROD.Where(x => x.ccompany == "S01").ToList();
            ViewBag.list1 = res;
            return PartialView();
        }

        Prod_Desc_Json pdj = new Prod_Desc_Json();
        public ActionResult _Product_Client_Modal()
        {


            List<VST_T3_PRODUCTO_DESCARGA> res = _oModelos.VST_T3_PRODUCTO_DESCARGA.Where(x => x.ccompany == "S01" && x.cvendedor == "S01_108").ToList();

            ViewBag.List = res;
            return PartialView();
        }

        public static List<VST_T3_PRODUCTO_DESCARGA> res1 = new List<VST_T3_PRODUCTO_DESCARGA>(); 
        public void load1()
        {
            res1 = _oModelos.VST_T3_PRODUCTO_DESCARGA.Where(x => x.ccompany == "S01" && x.cvendedor == "S01_108").ToList();
        }

        public JsonResult _Product_t1() 
        {
            var mod1 = res1.GroupBy(x => x.cproducto).Select(y => new { id = y.Key, values = y.ToList() }).ToList();
            
            List<Prod_Desc_Json.prod_desc3> res_json = new List<Prod_Desc_Json.prod_desc3>();
            mod1.ForEach(x =>
            {
                var a = x.id;
                var b = x.values.ToList();
                res_json.Add(new Prod_Desc_Json.prod_desc3 { ProductoC = a.ToString(), res = b });

            });
            //res_json.Add(res.GroupBy(x => x.cproducto).Select(y => new Prod_Desc_Json.prod_desc2 { ProductoC = y.Key.ToString(), res = y.ToList() }).ToList());
            var ad1 = Json(res_json);
            var ad2 = JObject.FromObject(ad1);
            var ad3 = ad2["Data"].ToString();
            return ad1;
        }


        public void testjson(string js)
        {
            List<VST_T3_PRODUCTO_DESCARGA> ls2 = new List<VST_T3_PRODUCTO_DESCARGA>();
            //var a3 = JToken.Parse(js).ToList();
            JToken.Parse(js).ToList().ForEach(x =>
            {
                ls2.Add(new VST_T3_PRODUCTO_DESCARGA
                {
                    ccompany = x.Value<string>("ccompany").Trim(),
                    cvendedor = x.Value<string>("cvendedor").Trim(),
                    cproducto = x.Value<string>("cproducto").Trim(),
                    cnamefc = x.Value<string>("cnamefc").Trim(),
                    ccostobruto_u1 = Convert.ToDecimal(x.Value<string>("ccostobruto_u1").Replace('.', ',')),
                    cprecioventamax = Convert.ToDecimal(x.Value<string>("cprecioventamax").Replace('.', ',')),
                    c_nomb_comer = x.Value<string>("c_nomb_comer").Trim(),
                    cname = x.Value<string>("cname").Trim()
                });
            });
            var final = ls2.Last();
            
        }

        public void test()
        {
            //var a = _oModelos.VST_T3_JOEC_PROD.Where(x => x.cproducto == "010114080").Take(5).ToList();
            //var b = JsonConvert.SerializeObject(a);
            //var c = JToken.FromObject(a).ToString();
            //var d = JArray.FromObject(a)[0].ToString();
            //var d1 = JsonConvert.DeserializeObject(d);

            var a = _oModelos.VST_T3_JOEC_PROD.Take(5).ToList();

            var lst = a.Select(x => new
            {
                id = x.cproducto,
                values = 
                new {
                     cproducto = x.cproducto,
                     c_nomb_comer = x.c_nomb_comer,
                     ccompany = x.ccompany,
                     ccostobruto_u1 = x.ccostobruto_u1,
                     cname = x.cname,
                     cnamefc = x.cnamefc,
                     cprecioventamax = x.cprecioventamax
                }
            }).ToList();
                
            //    lstt.Take(5).ToList().Select(x => new{
            //    id = x.cproducto, new CGP2_1.Models.VST_T3_JOEC_PROD 
            //    {
            //        cproducto = x.cp

            //    }
            //});
            
                //@Html.Raw(JsonConvert.SerializeObject
        }



        ///----------------------------------------------metodos directos de CGP
        ///
        public void Prod_Val(string js)
        {
            var productosExistentes = new List<ProductosFullInfo>();
            
            List<string> productosCarga = new List<string>();
            JToken.Parse(js).ToList().ForEach(x => productosCarga.Add(x.Value<string>("cproducto").Trim()));
            
            var compUsuario = "S01";
            
            ProductosExistentesOt(ref productosExistentes, compUsuario, productosCarga);
            
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
