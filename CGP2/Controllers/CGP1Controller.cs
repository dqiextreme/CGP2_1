using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CGP2_1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft;

namespace CGP2_1.Controllers
{
    public class CGP1Controller : Controller
    {
        T3WEB_2012Entities1 _oModelos = new T3WEB_2012Entities1();
        private readonly T3WEB_2012Entities1 _oModelos2 = new T3WEB_2012Entities1();

        public static string _Str_Vendedor = "S042_58";
        public static string _Str_Company = "S042";
        public static string _Str_CompanyGroup = "4";
        public static string _Str_ZonaUsuario = "A-03";
        public static int _Int_Cliente = 349;
        public static string _Str_Cliente = "349";
        
        

        public void rec(string js, Pedido model)
        {
        
            List<Producto> lstProd = new List<Producto>();
            JToken.Parse(js).ToList().ForEach(x => lstProd.Add(new Producto{
                 ProductoID = x.Value<string>("cproducto").Trim(),
                 Cajas = 1
            }));
            model.Productos = lstProd;
            IndexDetalle(model);
        }

        //master validacion
        //public PartialViewResult IndexDetalle(Pedido model)
        public void IndexDetalle(Pedido model)
        {
            bool btnadd = true;
            bool btndel = false;
            //if (Session["strDescUsuario"] == null)
            //{
            //    return PartialView("SesionExpirada");
            //}

            //limpio el campo productoid para evitar espacios en blanco
            model.Productos.ToList().ForEach(x =>
            {
                if (x.ProductoID != null)
                {
                    x.ProductoID = x.ProductoID.Trim();
                }
            });

            var strGroupCompUsuaro = _Str_CompanyGroup; // Convert.ToString(Session["strGroupCompUsuaro"]);
            var strCompUsuaro = _Str_Company; // Session["strCompUsuaro"].ToString();
            var strZonaUsuario = _Str_ZonaUsuario; // Session["strZonaUsuario"].ToString();
            var intDireccDespIDCheckeada = model.Direcciones.First().DirecDespValue;
            //////var pTuplaList = new List<Tuple<int, int, string, bool>>();
            //////model.Direcciones.ToList().ForEach(
            //////    x => pTuplaList.Add(new Tuple<int, int, string, bool>(int.Parse(x.DirecDespID.ToString()),
            //////        int.Parse(x.DirecDespID.ToString()),
            //////        x.DirecDespDesc,
            //////        x.DirecDespID == intDireccDespIDCheckeada)));
            //////CargarDirecciones(model, pTuplaList);
            //--------------------------------------------------------------------------------------------------
            //var strNombreBtnEliminarPresionado = ""; // NombreBtnEliminarPresionado();
            //if (ModelState.IsValid && !string.IsNullOrEmpty(Request["btnAgregarProducto"]))
            if (btndel)
            {
                AgregarProducto(model);
            }
            //else if (!string.IsNullOrEmpty(Request[strNombreBtnEliminarPresionado]))
            else if (btndel)
            {
                //var productoLits = new List<Producto>();
                //productoLits.AddRange(model.Productos);
                EliminarTempDataProductos(model.Productos.Where(x => x.PedidoDetalleID == 0));//---------------
                model.Productos = model.Productos.Where(x => x.PedidoDetalleID != 0);
                if (model.Productos.Count() == 0)
                {
                    AgregarProducto(model);
                }
            }
            //--------------------------------------------------------------------------------------------------
            //else if ((ModelState.IsValid))
            else if (true)
            {
                var intPedidoValidado = 0;
                var productosExistentes = new List<ProductosFullInfo>();
                EliminarTempDataProductos(model.Productos);//---------------
                try
                {
                    ProcesarProductos(model, strGroupCompUsuaro, strCompUsuaro, ref intPedidoValidado, ref productosExistentes);
                }
                catch (Exception ex)
                {
                    //////problem
                    //var oModelo = new T3WEB_2012Entities();
                    //var oT3TLogError = new T3TLOGERROR
                    //{
                    //    UsuarioId = User.Identity.Name,
                    //    Fecha = DateTime.Now,
                    //    Error = "ProcesarProductos: " + ex.InnerException == null ? ex.Message : ex.InnerException.Message
                    //};
                    //oModelo.T3TLOGERROR.AddObject(oT3TLogError);
                    //oModelo.SaveChanges();
                }
                switch (intPedidoValidado)
                {
                    case 1:
                        //ModelState.AddModelError(
                        //                         !string.IsNullOrEmpty(Request["btnFinalizar"])
                        //                         ? "MensajeValidacionID" : "MensajeValidacionIDValidar",
                        model.Observaciones = "Debe ingresar el detalle del pedido.";
                        break;
                    case 2:
                        model.Observaciones = "Debe corregir el detalle del pedido.";
                        break;
                    case 3:
                        model.Observaciones = "No debe mezclar alimentos y otros.";
                        break;
                    case 5:
                        model.Observaciones = "El monto del pedido es menor al monto mínimo exigido para un cliente especial.";
                        break;
                    case 6:
                        model.Observaciones = "El pedido no debe superar 22 items.";
                        break;
                    default:
                        //here 
                        //creo que por aqui finalizo el pedido----------------
                        //if (!string.IsNullOrEmpty(Request["btnFinalizar"]))
                        if (true)
                        {
                            if (!string.IsNullOrEmpty(model.Observaciones) && model.Observaciones.Length > 255)
                            {
                                model.Observaciones = "Las observaciones no deben sobrepasar los 255 carácteres.";
                            }
                            else if (string.IsNullOrEmpty(model.FormaPagoID))
                            {
                                model.Observaciones = "Debe elegir la forma de pago.";
                            }
                            else if (ObtenerBloqueoDeRutas() && !EsValidoPosteoDiaActual(strGroupCompUsuaro, strCompUsuaro, model.ClienteID, strZonaUsuario))
                            {
                                model.Observaciones = "No se pueden postear pedidos para el cliente seleccionado ya que no se encuentra enrutado para hoy.";
                            }
                            else
                            {
                                try
                                {
                                    strZonaUsuario = Convert.ToString(Session["strZonaUsuario"]);
                                    FinalizarPedido(model, strCompUsuaro, strZonaUsuario, intDireccDespIDCheckeada, productosExistentes);
                                }
                                catch (Exception ex)
                                {
                                    model.Observaciones = "Ocurrió un error al procesar la solicitud. " + ex.Message;
                                    /////problem
                                    //ModelState.AddModelError("MensajeValidacionID",
                                    //"Ocurrió un error al procesar la solicitud. " + ex.Message);
                                    //var oModelo = new T3WEB_2012Entities();
                                    //var oT3TLogError = new T3TLOGERROR
                                    //{
                                    //    UsuarioId = User.Identity.Name,
                                    //    Fecha = DateTime.Now,
                                    //    Error = "FinalizarPedido: " + ex.InnerException == null ? ex.Message : ex.InnerException.Message
                                    //};
                                    //oModelo.T3TLOGERROR.AddObject(oT3TLogError);
                                    //oModelo.SaveChanges();
                                }
                            }
                        }
                        break;
                }
            }
            //return PartialView("_IndexDetalle", model);
        }


         

        ///-----------------------------------------------------------------------------------------------

       //procesos llamados durante la validacion

        private enum TiposDePrecio
        {
            PMVPI = 1,
            PMVMC,
            PMVP,
            PPM
        }

        //other
        private int IdentificarTipoPedido(int pTipoPedidoID, string pCompUsuaro, IEnumerable<string> pProductosCarga)
        {
            if (pTipoPedidoID != 3)
                return pTipoPedidoID;
            var bAlimentos = _oModelos.VST_T3_PRODUCTOFULLINFO_AL.Where(
                x =>
                    x.ccompany == pCompUsuaro && x.cvendedor == User.Identity.Name &&
                    pProductosCarga.Contains(x.cproducto)).Any();
            if (!bAlimentos)
                return 2;
            var bOtros = _oModelos.VST_T3_PRODUCTOFULLINFO_OT.Where(
                x =>
                    x.ccompany == pCompUsuaro && x.cvendedor == User.Identity.Name &&
                    pProductosCarga.Contains(x.cproducto)).Any();
            return !bOtros ? 1 : 3;
        }

        //other 2
        private List<ProductosFullInfo> ProductosExistentes(int pTipoPedidoID, string pCompUsuaro, IEnumerable<string> pProductosCarga)
        {
            var productosExistentes = new List<ProductosFullInfo>();
            var productosCarga = pProductosCarga.ToList();
            switch (pTipoPedidoID)
            {
                case 1:
                    ProductosExistentesAl(ref productosExistentes, pCompUsuaro, productosCarga);
                    break;
                case 2:
                    ProductosExistentesOt(ref productosExistentes, pCompUsuaro, productosCarga);
                    break;
                default:
                    ProductosExistentesAl(ref productosExistentes, pCompUsuaro, productosCarga);
                    ProductosExistentesOt(ref productosExistentes, pCompUsuaro, productosCarga);
                    break;
            }
            return productosExistentes;
        }

        //other 3
        private void ProductosExistentesAl(ref List<ProductosFullInfo> pProductosExistentes, string pCompUsuaro, IEnumerable<string> pProductosCarga)
        {
            //Version Original antes de cambio de costo bruto
            //var oProductos = _oModelos.VST_T3_PRODUCTOFULLINFO_AL.Where(
            //    x =>
            //        x.ccompany == pCompUsuaro && x.cvendedor == User.Identity.Name &&
            //        pProductosCarga.Contains(x.cproducto)).ToList();


            var oProductosAgregar = new List<VST_T3_PRODUCTOFULLINFO_AL>();
            foreach (var strProducto in pProductosCarga)
            {
                var oProductos = _oModelos.VST_T3_PRODUCTOFULLINFO_AL.Where(
                                    x =>
                                        x.ccompany == pCompUsuaro &&
                                        x.cvendedor == User.Identity.Name &&
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
                TipoProducto = 1,
                CidProductod = x.cidproductod,
                Pmv = x.cprecioventamax
            }));
        }

        //other 4
        private void ProductosExistentesOt(ref List<ProductosFullInfo> pProductosExistentes, string pCompUsuaro, IEnumerable<string> pProductosCarga)
        {
            //Version Original antes de cambio de costo bruto
            //var oProductos = _oModelos.VST_T3_PRODUCTOFULLINFO_OT.Where(
            //                    x =>
            //                        x.ccompany == pCompUsuaro && 
            //                        x.cvendedor == User.Identity.Name &&
            //                        pProductosCarga.Contains(x.cproducto)
            //                       ).ToList();

            var oProductosAgregar = new List<VST_T3_PRODUCTOFULLINFO_OT>();
            foreach (var strProducto in pProductosCarga)
            {
                var oProductos = _oModelos.VST_T3_PRODUCTOFULLINFO_OT.Where(
                                    x =>
                                        x.ccompany == _Str_Company &&
                                        x.cvendedor == _Str_Vendedor &&
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

        //other 5
        private int CodigoPedido()
        {
            var strCompUsuaro = Session["strCompUsuaro"].ToString();
            var oT3Tcotpedfacmcons = new T3TCOTPEDFACMCONS
            {
                ccompany = strCompUsuaro,
                cvendedor = User.Identity.Name
            };
            _oModelos.T3TCOTPEDFACMCONS.Add(oT3Tcotpedfacmcons);
            _oModelos.SaveChanges();
            //---------------------------------------------------------
            return (int)oT3Tcotpedfacmcons.cpedido;
        }


        //-----------val inside
        private bool EsValidoPosteoDiaActual(string pstrGroupCompUsuario, string pCompUsuario, int pClienteId, string pZonaUsuario)
        {
            //Obtengo el dia actual
            var shortDiaSemana = (int)DateTime.Now.DayOfWeek;
            //Obtengo la fecha actual
            var oFechaActual = DateTime.Now.Date;
            //Cargo la Excepciones del cliente
            var oExcepciones = _oModelos.T3TEXECRUTA.FirstOrDefault(o => o.ccompany == pCompUsuario
                                                                      && o.ccliente == pClienteId
                                                                      && o.c_zona == pZonaUsuario
                                                                      && o.cfechaeje_ov == oFechaActual
                                                                      && o.cdelete == 0
                                                                      && o.c_otrodiapedido == 1);
            //Verifico si hay excepciones
            if (oExcepciones != null)
                return true;

            //Cargo las rutas del cliente
            var oRutas =
                _oModelos.VST_T3_RUTASVISITADIAACTUAL.FirstOrDefault(x => x.ccompany == pCompUsuario
                                                                       && x.cvendedor == User.Identity.Name
                                                                       && x.ccliente == pClienteId
                                                                       && x.cgroupcomp == pstrGroupCompUsuario
                                                                       && x.cdiasemana == shortDiaSemana);
            //Verifico si hay rutas
            if (oRutas != null)
                return true;

            //Si llegamos aqui no hay ni excepCiones ni rutas
            return false;
        }

        private void InformacionCliente(Pedido model)
        {
            var strGroupCompUsuaro = Convert.ToString(Session["strGroupCompUsuaro"]);
            var strZonaUsuario = Session["strZonaUsuario"].ToString();
            var oCliente =
                _oModelos.VST_T3_CLIENTEFULLINFO.Where(
                    x =>
                        x.cvendedor == User.Identity.Name && x.ccliente == model.ClienteID &&
                        x.cgroupcomp == strGroupCompUsuaro);
            if (oCliente.Count() > 0)
            {
                if (oCliente.First().ccanal.Trim() == "5" || oCliente.First().ccanal.Trim() == "6")
                {
                    ModelState.AddModelError("ClienteID",
                        "No se pueden ingresar clientes de tipo empleado ni de intercompañías.");
                }
                else if (ObtenerBloqueoDeRutas() && !EsValidoPosteoDiaActual(oCliente.First().cgroupcomp, oCliente.First().ccompany, model.ClienteID, strZonaUsuario))
                {
                    ModelState.AddModelError("ClienteID",
                        "No se pueden postear pedidos para el cliente seleccionado ya que no se encuentra enrutado para hoy.");
                }
                else
                {
                    model.ClienteDesc = oCliente.First().c_nomb_comer;
                    model.TipoContribuyente = oCliente.First().c_tip_contribuy;
                    var pTuplaList = new List<Tuple<int, int, string, bool>>();
                    oCliente.ToList().ForEach(
                        x => pTuplaList.Add(new Tuple<int, int, string, bool>(int.Parse(x.c_direcc_despa.ToString()),
                            int.Parse(x.c_direcc_despa.ToString()),
                            x.c_direcc_descrip, pTuplaList.Count() == 0)));
                    CargarDirecciones(model, pTuplaList);
                    model.Validado = pTuplaList.Count() > 0;
                }
            }
            else
            {
                ModelState.AddModelError("ClienteID",
                    "El código del cliente no existe o no esta asignado al código de vendedor");
            }
        }


        private void FinalizarPedido(Pedido model, string pCompUsuaro, string pZonaUsuario, int pDireccDespaID, List<ProductosFullInfo> pProductosExistentes)
        {
            InformacionCliente(model);
            //Validamos de nuevo el cliente
            if (!ModelState.IsValid)
            {
                throw new Exception("El código del cliente no existe o no esta asignado al código de vendedor");
            }
            //---------------------------------------------------------
            var decPedidoGenerado = model.PedidoID;
            //---------------------------------------------------------
            //var oT3Tcotpedfacm =
            //    _oModelos.T3TCOTPEDFACM.Where(
            //        x =>
            //        x.ccompany == pCompUsuaro && x.ccotizacion == decPedidoGenerado && x.cpedido == decPedidoGenerado).
            //        FirstOrDefault();
            //if (oT3Tcotpedfacm != null)
            //{
            //    oT3Tcotpedfacm.ctipoalimento = (byte?)(model.TipoPedidoID == 1 ? 1 : 0);
            //    oT3Tcotpedfacm.c_direcc_despa = pDireccDespaID;
            //    oT3Tcotpedfacm.cfpago = model.FormaPagoID;
            //    oT3Tcotpedfacm.cobservaciones = model.Observaciones == null ? "" : model.Observaciones.ToUpper();
            //    //oT3Tcotpedfacm.c_montotot_si = (double?)model.Productos.Select(x => x.SubTotal).Sum();
            //    //oT3Tcotpedfacm.c_impuesto = (double?)model.Productos.Select(x => x.Impuesto).Sum();
            //    _oModelos.T3TCOTPEDFACM.Detach(oT3Tcotpedfacm);
            //    _oModelos.T3TCOTPEDFACM.Attach(oT3Tcotpedfacm);
            //    _oModelos.ObjectStateManager.ChangeObjectState(oT3Tcotpedfacm, System.Data.EntityState.Modified);
            //    _oModelos.SaveChanges();

            //    var oT3Tcotpedfacd =
            //        _oModelos.T3TCOTPEDFACD.Where(
            //            x =>
            //                x.cpedido == decPedidoGenerado && x.ccotizacion == decPedidoGenerado &&
            //                x.ccompany == pCompUsuaro).ToList();
            //    oT3Tcotpedfacd.ForEach(x => _oModelos.DeleteObject(x));
            //    _oModelos.SaveChanges();
            //}
            //else
            //{
            //    oT3Tcotpedfacm = new T3TCOTPEDFACM
            //    {
            //        ccompany = pCompUsuaro,
            //        ccotizacion = decPedidoGenerado,
            //        cpedido = decPedidoGenerado,
            //        cfactura = 0,
            //        cproceso = "P",
            //        ccliente = model.ClienteID,
            //        cvendedor = User.Identity.Name,
            //        c_zona = pZonaUsuario,
            //        c_fecha_cotiza = DateTime.Now,
            //        c_fecha_pedido = DateTime.Now,
            //        cobservaciones = model.Observaciones == null ? "" : model.Observaciones.ToUpper(),
            //        ctipoalimento = (byte?)(model.TipoPedidoID == 1 ? 1 : 0),
            //        c_direcc_despa = pDireccDespaID,
            //        cfpago = model.FormaPagoID,
            //        //c_montotot_si = (double?)model.Productos.Select(x => x.SubTotal).Sum(),
            //        //c_impuesto = (double?)model.Productos.Select(x => x.Impuesto).Sum(),
            //        cstatus = 0,
            //        cdateadd = DateTime.Now,
            //        cuseradd = User.Identity.Name,
            //        cdelete = 0,
            //        c_nuevo_web = 0,
            //        cpedidomobil = 1,
            //        replicado = 0
            //    };
            //    _oModelos.T3TCOTPEDFACM.AddObject(oT3Tcotpedfacm);
            //    _oModelos.SaveChanges();
            //}
            ////---------------------------------------------------------
            //pProductosExistentes.ForEach(x =>
            //{
            //    var productoCarga =
            //        model.Productos.Where(
            //            prod => prod.ProductoID != null && prod.ProductoID.Trim() == x.Producto.Trim()).First();
            //    var oT3Tcotpedfacd = new T3TCOTPEDFACD
            //    {
            //        ccompany = pCompUsuaro,
            //        ccotizacion =
            //            decPedidoGenerado,
            //        cpedido = decPedidoGenerado,
            //        cfactura = 0,
            //        cproducto = x.Producto.Trim(),
            //        cdesc1 =
            //            Convert.ToDouble(
            //                productoCarga.Desc1),
            //        cdesc2 =
            //            productoCarga.Desc2.GetValueOrDefault(0),
            //        cdesc3 =
            //            productoCarga.Desc3.GetValueOrDefault(0),
            //        cempaques =
            //            productoCarga.Cajas.GetValueOrDefault(0),
            //        cunidades =
            //            productoCarga.Unidad.GetValueOrDefault(0),
            //        csubgrupo = x.SubGrupo,
            //        csku = x.Sku,
            //        cgrupo = x.Grupo,
            //        cproveedor = x.Proveedor,
            //        c_montotot_si =
            //            Convert.ToDouble(
            //                productoCarga.SubTotal),
            //        c_impuesto =
            //            Convert.ToDouble(
            //                productoCarga.Impuesto),
            //        ccostobruto_u1 =
            //            Convert.ToDouble(x.CostobrutoU1),
            //        ccostobruto_u2 =
            //            Convert.ToDouble(x.CostobrutoU2),
            //        ccostoneto_u1 =
            //            Convert.ToDouble(productoCarga.PrecioVentaU1),
            //        ccostoneto_u2 =
            //            Convert.ToDouble(productoCarga.PrecioVentaU2),
            //        cpreciounitario =
            //            productoCarga.PrecioUni,
            //        cprecioventamax =
            //                         string.IsNullOrEmpty(
            //                             productoCarga.Pmv)
            //                         ? 0 : Convert.ToDecimal(
            //                             productoCarga.Pmv),
            //        cdateadd = DateTime.Now,
            //        cuseradd = User.Identity.Name,
            //        cdelete = 0,
            //        ctipoalimento = (byte?)(model.TipoPedidoID == 1 ? 1 : 0),
            //        replicado = 0
            //    };
            //    _oModelos.T3TCOTPEDFACD.AddObject(oT3Tcotpedfacd);
            //    _oModelos.SaveChanges();
            //});
            //var strDiaSemana = Convert.ToString((int)DateTime.Now.DayOfWeek);
            //var oactividiaria =
            //    _oModelos.VST_T3_ACTIVIDIARIA.Where(
            //        x =>
            //            x.ccompany == pCompUsuaro && x.cvendedor == User.Identity.Name && x.ccliente == model.ClienteID &&
            //            x.c_zona == pZonaUsuario && x.cdiasemana == strDiaSemana).FirstOrDefault();
            //if (oactividiaria != null)
            //{
            //    var oT3Tactividiaria =
            //        _oModelos.T3TACTIVIDIARIA.Where(
            //            x =>
            //            x.ccompany == pCompUsuaro && x.cvendedor == User.Identity.Name && x.ccliente == model.ClienteID &&
            //            x.c_zona == pZonaUsuario && x.cdiasemana == strDiaSemana).OrderByDescending(x => x.cfecha).FirstOrDefault();
            //    oT3Tactividiaria.c_pedido = 1;
            //    oT3Tactividiaria.c_texistencia = 0;
            //    _oModelos.T3TACTIVIDIARIA.Detach(oT3Tactividiaria);
            //    _oModelos.T3TACTIVIDIARIA.Attach(oT3Tactividiaria);
            //    _oModelos.ObjectStateManager.ChangeObjectState(oT3Tactividiaria, System.Data.EntityState.Modified);
            //    _oModelos.SaveChanges();
            //}
            //else
            //{
            //    var oT3Tactividiaria = new T3TACTIVIDIARIA
            //    {
            //        ccompany = pCompUsuaro,
            //        cvendedor = User.Identity.Name,
            //        ccliente = model.ClienteID,
            //        c_zona = pZonaUsuario,
            //        cfecha = DateTime.Now,
            //        cdiasemana = strDiaSemana,
            //        c_pedido = 1,
            //        cdateadd = DateTime.Now,
            //        cuseradd = User.Identity.Name,
            //        cdelete = 0
            //    };
            //    _oModelos.T3TACTIVIDIARIA.AddObject(oT3Tactividiaria);
            //    _oModelos.SaveChanges();
            //}
            //var oT3TCOTPEDFACD = _oModelos.T3TCOTPEDFACD.Where(x => x.cpedido == decPedidoGenerado && x.ccompany == pCompUsuaro).ToList();
            //oT3Tcotpedfacm = _oModelos.T3TCOTPEDFACM.Where(x => x.cpedido == decPedidoGenerado && x.ccompany == pCompUsuaro).Single();
            //oT3Tcotpedfacm.c_montotot_si = oT3TCOTPEDFACD.Sum(x => x.c_montotot_si);
            //oT3Tcotpedfacm.c_impuesto = oT3TCOTPEDFACD.Sum(x => x.c_impuesto);
            //oT3Tcotpedfacm.cstatus = 1;
            //_oModelos.T3TCOTPEDFACM.Detach(oT3Tcotpedfacm);
            //_oModelos.T3TCOTPEDFACM.Attach(oT3Tcotpedfacm);
            //_oModelos.ObjectStateManager.ChangeObjectState(oT3Tcotpedfacm, System.Data.EntityState.Modified);
            //_oModelos.SaveChanges();
            //ViewBag.PedidoGenerado = decPedidoGenerado;
        }

        private bool ObtenerBloqueoDeRutas()
        {
            //version de un solo campo ya obsoleta desde que nació! nunca se usó! expiró!
            //var oBloqueoRutas = _oModelos.T3TCONFIGCONSSA.Select(x => x.cbloqueruta).FirstOrDefault(); 
            var strCompUsuario = Session["strCompUsuaro"].ToString();
            var oFechaActual = DateTime.Now.Date;
            var oBloqueoRutas = _oModelos.T3TCALENDCONT.FirstOrDefault(x => x.ccompany == strCompUsuario
                                                                         && x.cdiafecha_cal == oFechaActual
                                                                         && x.cdelete == 0);
            if (oBloqueoRutas != null)
            {
                if (oBloqueoRutas.cbloqrutapedido != null)
                {
                    return oBloqueoRutas.cbloqrutapedido == 1;
                }
                return false;
            }
            return false;
        }

        //-----------val inside

        //1.1
        private static void CargarDirecciones(Pedido model, IEnumerable<Tuple<int, int, string, bool>> pTuplaList)
        {
            var direccionDespList = new List<DireccionDesp>();
            pTuplaList.ToList().ForEach(
                x => direccionDespList.Add(new DireccionDesp
                {
                    DirecDespID = x.Item1,
                    DirecDespValue = x.Item2,
                    DirecDespDesc = x.Item3,
                    Checked = x.Item4
                }));
            model.Direcciones = direccionDespList;
        }

        //1.2
        private string NombreBtnEliminarPresionado()
        {
            foreach (var item in Request.Form.AllKeys)
            {
                if (Request.Form[item].Contains("Eliminar"))
                {
                    return item;
                }
            }
            return "";
        }

        //1.3
        private static void AgregarProducto(Pedido model)
        {
            var productoList = new List<Producto>();
            if (model.Productos == null)
            {
                for (var intI = 1; intI <= 8; intI++)
                {
                    productoList.Add(new Producto { Cajas = null, Unidad = null, Desc2 = null, Desc3 = null });
                }
                model.Productos = productoList;
            }
            else
            {
                for (var intI = 1; intI <= 5; intI++)
                {
                    productoList.Add(new Producto { Cajas = null, Unidad = null, Desc2 = null, Desc3 = null });
                }
                model.Productos = model.Productos.Concat(productoList);
            }
            var productos = model.Productos;
            productos.Where(x => x.PedidoDetalleID == 0).ToList().ForEach(
                x => x.PedidoDetalleID = model.Productos.Max(z => z.PedidoDetalleID) + 1);
        }

        //1.4
        private void EliminarTempDataProductos(IEnumerable<Producto> pProductos)
        {
            pProductos.ToList().ForEach(x =>
            {
                if (TempData["PmvList" + x.ProductoID] != null)
                {
                    TempData.Remove("PmvList" + x.ProductoID);
                }
            });
        }

        //1.5
        public void ProcesarProductos(Pedido model, string pGroupCompUsuaro, string pCompUsuaro, ref int pEstatusDetalle, ref List<ProductosFullInfo> pProductosExistentes)
        {
            if (model.Productos == null)
                return;
            var productosCarga =
                model.Productos.Where(x => !string.IsNullOrWhiteSpace(x.ProductoID)).Select(x => x.ProductoID).
                ToList();
            if (model.TipoPedidoID == 0)
            {
                model.Productos.ToList().ForEach(x =>
                {
                    x.ProductoDesc = "";
                    x.ProductoIDError = false;
                    x.PmvError = false;
                    x.CajaError = false;
                    x.UnidadError = false;
                    x.Desc2Error = false;
                    x.Desc3Error = false;
                    x.MensajeError = "";
                    x.TipoProducto = 0;
                });
                return;
            }
            var intEstatusDetalleTemp = 1;
            model.TipoPedidoID = IdentificarTipoPedido(model.TipoPedidoID, pCompUsuaro, productosCarga);
            if (model.TipoPedidoID == 3)
                intEstatusDetalleTemp = 3;

            //replace -> var productosExistentes = ProductosExistentes(model.TipoPedidoID, pCompUsuaro, productosCarga);
            var productosExistentes = CargaCController.ProdExist.Where(x => productosCarga.Contains(x.Producto)).ToList();
            //Eliminar atendidos por directo
            //var strClienteId = model.ClienteID.ToString();
            ///////problem
            //productosExistentes.RemoveAll(
            //    x => _oModelos.T3TATENDIRECTO.Any(
            //        c =>
            //        c.cgroupcomp == pGroupCompUsuaro && c.cproveedor == x.Proveedor &&
            //        c.ccliente == strClienteId && c.cdelete == 0));
            ////-------------------------------


            model.Productos
                 .ToList()
                 .ForEach(x =>
                 {
                     x.ProductoDesc = "";
                     x.ProductoIDError = false;
                     x.PmvError = false;
                     x.CajaError = false;
                     x.UnidadError = false;
                     x.Desc2Error = false;
                     x.Desc3Error = false;
                     x.TipoProducto = 0;
                     decimal decPrecioUnitarioConDescuentos = 0;  //Sprint 16

                     if (string.IsNullOrWhiteSpace(x.ProductoID))
                     {
                         x.MensajeError = "";
                     }
                     //else if (!productosExistentes.Select(z => z.Producto.Trim()).Contains(
                     //    x.ProductoID.Trim()))
                     //{
                     //    x.MensajeError =
                     //        "El producto no existe, no está disponible para esta región o el cliente es atendido por directo por el proveedor.";
                     //    x.ProductoIDError = true;
                     //}
                     //else if (productosCarga.Where(z => z == x.ProductoID).Count() > 1)
                     //{
                     //    x.MensajeError = "El producto ha sido ingresado mas de una vez.";
                     //    x.ProductoIDError = true;
                     //}
                     else
                     {
                         x.MensajeError = "";
                         var productoExis =
                             productosExistentes.Where(prod => prod.Producto == x.ProductoID).First();
                         //------------
                         x.ProductoDesc = productoExis.Descripcion.Trim();
                         x.TipoProducto = productoExis.TipoProducto;
                         //-----------------------------------
                         //Cargamos la Lista de Pmvs
                         var productoPmvs = _oModelos.VST_T3_PRODUCTOPMV
                                                     .Where(
                             z => z.ccompany == pCompUsuaro && z.cproducto.Trim() == x.ProductoID)
                                                     .ToList();
                         //Si solo hay un Pmv, lo asignamos por defecto
                         if (productoPmvs.Count() == 1)
                             x.Pmv = productoPmvs.Single().cprecioventamax.ToString();

                         //Pasamos la lista a la página
                         TempData["PmvList" + x.ProductoID] = new SelectList(productoPmvs, "cprecioventamax", "cprecioventamax", x.Pmv);

                         //Verifica si esta seleccionado algun PMV
                         if (((SelectList)TempData["PmvList" + x.ProductoID]).Count() > 0 && string.IsNullOrEmpty(x.Pmv))
                         {
                             x.MensajeError += "Campo \"PMV\": Debe elegir el PMV del producto./";
                             x.PmvError = true;
                         }
                         else if (((SelectList)TempData["PmvList" + x.ProductoID]).Count() == 0)
                         {
                             x.MensajeError += "Campo \"PMV\": El producto no tiene existencia./";
                             x.PmvError = true;
                             if (intEstatusDetalleTemp != 3)
                                 intEstatusDetalleTemp = 2;
                             return;
                         }
                         //-----------------------------------
                         if (x.Cajas.GetValueOrDefault() == 0 && x.Unidad.GetValueOrDefault() == 0 &&
                             productoExis.VentaUndidad == 1)
                         {
                             x.MensajeError +=
                                 "Campos \"Cajas y Unidad\": Debe ingresar cantidades de cajas o unidades./";
                             x.CajaError = true;
                             x.UnidadError = true;
                         }
                         else if (x.Cajas.GetValueOrDefault() == 0 && productoExis.VentaUndidad == 0)
                         {
                             x.MensajeError +=
                                 "Campo \"Cajas\": Debe ingresar cantidades de cajas./";
                             x.CajaError = true;
                         }
                         //-----------------------------------
                         if (x.Unidad > 0)
                         {
                             if (productoExis.VentaUndidad == 0)
                             {
                                 x.MensajeError +=
                                     "Campo \"Unidad\": El producto no se puede vender por unidades./";
                                 x.UnidadError = true;
                             }
                             else if (x.Unidad >= productoExis.CantMaxUnid)
                             {
                                 x.MensajeError +=
                                                  "Campo \"Unidad\": La máxima cantidad de unidades para el producto debe ser " +
                                                  Convert.ToInt32(productoExis.CantMaxUnid - 1) + "./";
                                 x.UnidadError = true;
                             }
                         }
                         //------------
                         decimal decPmv = 0;
                         decimal.TryParse(x.Pmv, out decPmv);
                         productosExistentes.RemoveAll(prod => prod.Producto == x.ProductoID && prod.Pmv != decPmv);

                         //Vemos si hay productos repetidos
                         var _Int_ = productosExistentes.Count(prod => prod.Producto == x.ProductoID);

                         //Si devuelve 2 registros, eliminamos el registro con el costo bruto mas bajo
                         if (_Int_ > 1)
                         {
                             //Obtenemos el costobruto a eliminar
                             var _Dbl_CostoBruto = productosExistentes.Where(prod => prod.Producto == x.ProductoID).OrderBy(elemento => elemento.CostobrutoU1).First().CostobrutoU1;
                             //Eliminamos el elemento
                             productosExistentes.RemoveAll(prod => prod.Producto == x.ProductoID && prod.CostobrutoU1 == _Dbl_CostoBruto);
                         }

                         var oDescuento = productosExistentes.Where(prod => prod.Producto == x.ProductoID).SingleOrDefault();
                         //------------
                         if (x.Pmv != null && oDescuento != null)
                         {
                             x.Desc1 = oDescuento.Descuento1.GetValueOrDefault().ToString("#,##0.00");
                             if (x.Desc2 > Convert.ToDouble(oDescuento.Descuento2.GetValueOrDefault()))
                             {
                                 x.MensajeError += "Campo \"Desc2\": El descuento ingresado (" + x.Desc2 +
                                                   ") es mayor que el descuento máximo permitido (" +
                                                   oDescuento.Descuento2.GetValueOrDefault().ToString(
                                                       "#,##0.00") +
                                                   ")/";
                                 x.Desc2Error = true;
                             }
                             if (x.Desc3 > Convert.ToDouble(oDescuento.Descuento3.GetValueOrDefault()))
                             {
                                 x.MensajeError += "Campo \"Desc3\": El descuento ingresado (" + x.Desc3 +
                                                   ") es mayor que el descuento máximo permitido (" +
                                                   oDescuento.Descuento3.GetValueOrDefault().ToString(
                                                       "#,##0.00") +
                                                   ")/";
                                 x.Desc3Error = true;
                             }
                         }
                         //-----------------------------------
                         if (string.IsNullOrEmpty(x.MensajeError))
                         {
                             decimal? decSubTotal = 0, decImpuesto = 0, decTotal = 0, decPrecioUni = 0, decPrecioVentaU1 = 0, decPrecioVentaU2 = 0;
                             CalcularPrecio(pGroupCompUsuaro, pCompUsuaro, model.ClienteID, x.ProductoID,
                                 x.Cajas.GetValueOrDefault(), x.Unidad.GetValueOrDefault(),
                                 Convert.ToDecimal(productoExis.CantMaxUnid.GetValueOrDefault()),
                                 productoExis.PorcImp.GetValueOrDefault(),
                                 Convert.ToDecimal(x.Desc1),
                                 Convert.ToDecimal(x.Desc2.GetValueOrDefault()),
                                 Convert.ToDecimal(x.Desc3.GetValueOrDefault()),
                                 ref decSubTotal, ref decImpuesto, ref decTotal, ref decPrecioUni, ref decPrecioVentaU1, ref decPrecioVentaU2,
                                 Convert.ToDecimal(x.Pmv),
                                 ref decPrecioUnitarioConDescuentos);
                             x.SubTotal = Math.Round(Convert.ToDecimal(decSubTotal), 2);
                             x.Impuesto = Math.Round(Convert.ToDecimal(decImpuesto), 2);
                             x.Total = Math.Round(Convert.ToDecimal(decTotal), 2);
                             x.PrecioUni = Math.Round(Convert.ToDecimal(decPrecioUni), 2);
                             x.PrecioVentaU1 = Math.Round(Convert.ToDecimal(decPrecioVentaU1), 2);
                             x.PrecioVentaU2 = Math.Round(Convert.ToDecimal(decPrecioVentaU2), 2);
                         }
                     }
                     //----------------------------------- Validacion Sprint 16 -----------------------------------
                     var cRegpmv = ProductoRequierePmv(x.ProductoID);
                     if (cRegpmv > 0) //Solo si el producto esta configurado como PMV o PPM
                     {
                         switch (cRegpmv)
                         {
                             case 1: //PMV
                                 //Obtengo el PMVMC
                                 var dcPmvmc = ObtenerPrecioHistoricoPmv(x.ProductoID, TiposDePrecio.PMVMC);
                                 //Si No existe el registro
                                 if (dcPmvmc == -1)
                                 {
                                     x.MensajeError += "Campo \"PMV\": El producto no tiene asignado un Precio Máximo de Venta por favor comuníquese con su gerente de área.";
                                     x.PrecioUnidadMinimaError = true;
                                 }
                                 //Valido el precio
                                 if (decPrecioUnitarioConDescuentos > dcPmvmc)
                                 {
                                     x.MensajeError += "Campo \"PMV\": Hay un problema con el PMV del producto por favor comuníquese con su gerente de área. El precio máximo de venta es superior al regulado";
                                     x.PrecioUnidadMinimaError = true;
                                 }
                                 break;
                             case 2: //PPM
                                 //Obtengo el PPM
                                 var dcPpm = ObtenerPrecioHistoricoPmv(x.ProductoID, TiposDePrecio.PPM);
                                 //Si No existe el registro
                                 if (dcPpm == -1)
                                 {
                                     x.MensajeError += "Campo \"PMV\": El producto no tiene asignado un Precio Máximo de Venta por favor comuníquese con su gerente de área.";
                                     x.PrecioUnidadMinimaError = true;
                                 }
                                 break;
                         }
                     }
                     //----------------------------------- Validacion Sprint 16 -----------------------------------

                     if (!string.IsNullOrEmpty(x.MensajeError) && intEstatusDetalleTemp != 3)
                     {
                         intEstatusDetalleTemp = 2;
                     }
                 });
            pProductosExistentes = productosExistentes;
            pEstatusDetalle = productosExistentes.Count > 0 && intEstatusDetalleTemp == 1 ? 4 : intEstatusDetalleTemp;

            //INICIO - Validación comentada por Ignacio el 02/05/2013 debido a que aun no se sabe si se va a implantar
            if (pEstatusDetalle == 4)//Si el pedido es válido
            {
                var decImpuesto = model.Productos.Where(x => string.IsNullOrWhiteSpace(x.MensajeError)).Sum(x => x.Impuesto);
                var DecTotal = model.Productos.Where(x => string.IsNullOrWhiteSpace(x.MensajeError)).Sum(x => x.Total);
                if (decImpuesto > 0 && !MontoPedidoValido(model.TipoContribuyente, DecTotal.GetValueOrDefault()))
                { pEstatusDetalle = 5; return; }
                if (productosExistentes.Count > 22)
                { pEstatusDetalle = 6; return; }

            }
            //FIN - Validación comentada por Ignacio el 02/05/2013 debido a que aun no se sabe si se va a implantar


            //Validacion de Rutas


        }

        //1.5.1
        private void CalcularPrecio(string pGroupCompUsuaro, string pCompUsuaro, int pClienteID, string pProductoID, int? pCajas, int? pUnidades, decimal? pUnidadesPorCaja, decimal? pPorcImp, decimal? pDesc1, decimal? pDesc2, decimal? pDesc3, ref decimal? pSubTotal, ref decimal? pImpuesto, ref decimal? pTotal, ref decimal? pPrecioUni, ref decimal? pPrecioVentaU1, ref decimal? pPrecioVentaU2, decimal pPrecioSeleccionado, ref decimal pPrecioUnitarioConDescuentos)
        {
            decimal? decPrecioVentaU1 = 0;
            var oPrecioListaComp =
                _oModelos.VST_T3_PRECIOLISTA_COMP.Where(
                    x =>
                        x.cgroupcomp == pGroupCompUsuaro &&
                        x.ccompany == pCompUsuaro &&
                        x.ccliente == pClienteID &&
                        x.cproducto == pProductoID &&
                        x.cprecioventamax == pPrecioSeleccionado
                        ).FirstOrDefault();
            if (oPrecioListaComp != null)
            {
                decPrecioVentaU1 = oPrecioListaComp.PrecioVentaU1;
            }
            else
            {
                var oPrecioListaProd =
                    _oModelos.VST_T3_PRECIOLISTA_PROD.Where(
                        x =>
                            x.cgroupcomp == pGroupCompUsuaro &&
                             x.ccompany == pCompUsuaro &&
                            x.ccliente == pClienteID &&
                            x.cproducto == pProductoID &&
                            x.cprecioventamax == pPrecioSeleccionado
                            ).FirstOrDefault();
                if (oPrecioListaProd != null)
                {
                    decPrecioVentaU1 = oPrecioListaProd.PrecioVentaU1;
                }
            }
            var decPrecioVentaU2 = Math.Round(Convert.ToDecimal(decPrecioVentaU1 / pUnidadesPorCaja), 2);
            pSubTotal = (pCajas * decPrecioVentaU1) + (pUnidades * decPrecioVentaU2);
            if (pDesc1 > 0)
            {
                pSubTotal = pSubTotal - ((pSubTotal * pDesc1) / 100);
            }
            if (pDesc2 > 0)
            {
                pSubTotal = pSubTotal - ((pSubTotal * pDesc2) / 100);
            }
            if (pDesc3 > 0)
            {
                pSubTotal = pSubTotal - ((pSubTotal * pDesc3) / 100);
            }
            pImpuesto = (pSubTotal * pPorcImp) / 100;
            pTotal = pSubTotal + pImpuesto;
            pPrecioUni = decPrecioVentaU2;
            pPrecioVentaU1 = decPrecioVentaU1;
            pPrecioVentaU2 = decPrecioVentaU2;

            //Obtenemos la unidad minima
            var decUnidadMinima = Convert.ToDecimal(_oModelos.T3TPRODUCTO.SingleOrDefault(x => x.cproducto == pProductoID).ccontenidoma1);
            //Tomamos el precio
            decimal? decPrecioConDescuentos = Math.Round(Convert.ToDecimal(decPrecioVentaU1 / decUnidadMinima), 2);
            //Aplicamos los decuentos
            if (pDesc1 > 0)
            {
                decPrecioConDescuentos = decPrecioConDescuentos - ((decPrecioConDescuentos * pDesc1) / 100);
            }
            if (pDesc2 > 0)
            {
                decPrecioConDescuentos = decPrecioConDescuentos - ((decPrecioConDescuentos * pDesc2) / 100);
            }
            if (pDesc3 > 0)
            {
                decPrecioConDescuentos = decPrecioConDescuentos - ((decPrecioConDescuentos * pDesc3) / 100);
            }
            //Devolvemos
            pPrecioUnitarioConDescuentos = Convert.ToDecimal(decPrecioConDescuentos); //Sprint 16
        }

        //1.5.2
        private int ProductoRequierePmv(string pcproducto)
        {
            var oPmv = _oModelos.T3TPRODUCTO.SingleOrDefault(x => x.cproducto == pcproducto && x.cregpmv != 0);
            return oPmv == null ? 0 : Convert.ToInt32(oPmv.cregpmv);
        }

        //1.5.3
        private decimal ObtenerPrecioHistoricoPmv(string pcproducto, TiposDePrecio pTipoPrecio)
        {
            var oFechaActual = DateTime.Now.Date;
            decimal? dcResultado = 0;
            var oPrecio = _oModelos.T3THISTORICOPMV.FirstOrDefault(x => x.cproducto == pcproducto
                                                                  && x.cpreciomanejado == (int)pTipoPrecio
                                                                  && oFechaActual >= x.cfechainicio
                                                                  && oFechaActual <= (x.cfechafinal ?? oFechaActual));
            //Si no hay precio
            if (oPrecio == null)
                return -1;

            //En función al tipo de Pmv Usado
            switch (pTipoPrecio)
            {
                case TiposDePrecio.PMVP:
                    dcResultado = oPrecio.cpmvp;
                    break;
                case TiposDePrecio.PMVPI:
                    dcResultado = oPrecio.cpmvpi;
                    break;
                case TiposDePrecio.PMVMC:
                    dcResultado = oPrecio.cpmvmc;
                    break;
                case TiposDePrecio.PPM:
                    dcResultado = oPrecio.cppm;
                    break;
            }

            //Devuelvo
            return Convert.ToDecimal(dcResultado);

        }

        //1.6
        private bool MontoPedidoValido(string pTipoContribuyente, decimal pMontoPedido)
        {
            if (pTipoContribuyente.Trim() == "2")
            {
                var decCantidadUtMin =
                    _oModelos.T3TCONFIGCONSSA.Select(x => x.ccantidadutmin).FirstOrDefault();
                var decMontoUt =
                    _oModelos.T3TUNITRIBUT.Select(x => x.cvalor).FirstOrDefault();
                if (pMontoPedido < (decMontoUt * decCantidadUtMin))
                {
                    return false;
                }
            }
            return true;
        }
        
        
        
         
    }
}
