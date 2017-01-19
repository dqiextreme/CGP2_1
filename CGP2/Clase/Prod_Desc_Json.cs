using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CGP2_1.Models;

namespace CGP2_1.Clase
{
    public class Prod_Desc_Json
    {
        public class prod_desc3
        {
            public string ProductoC { get; set; }
            public List<VST_T3_PRODUCTO_DESCARGA> res { get; set; }
        }

        public class prod_desc2
        {
            public string ProductoC { get; set; }
            public VST_T3_PRODUCTO_DESCARGA res { get; set; }
        }
    }
}