using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CGP2_1.Models
{
    public class Pedido
    {

        public int PedidoID { get; set; }

        public bool Validado { get; set; }

        public int TipoPedidoID { get; set; }

        public string FormaPagoID { get; set; }
        public string Observaciones { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessage = "Debe introducir valores numericos")]
        [Required(ErrorMessage = " ")]
        public int ClienteID { get; set; }

        public string ClienteDesc { get; set; }

        public string TipoContribuyente { get; set; }

        public IEnumerable<DireccionDesp> Direcciones { get; set; }

        public IEnumerable<Producto> Productos { get; set; }
    }
}