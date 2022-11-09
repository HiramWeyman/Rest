using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class CajaAhorroCLS
    {
        [Key]
        public int pre_id { get; set; }
        public string pre_nombre { get; set; }
        public string pre_matricula { get; set; }
        public string pre_adscripcioon { get; set; }
        public string pre_tarjeta_cuenta { get; set; }
        public string pre_banco { get; set; }
        public string pre_telefono { get; set; }
        public string pre_recibo { get; set; }
        public string pre_ine { get; set; }
        public string pre_cantidad { get; set; }
        public System.DateTime pre_fecha { get; set; }
        public string pre_tipo { get; set; }
        public string pre_estatus { get; set; }

    }
}