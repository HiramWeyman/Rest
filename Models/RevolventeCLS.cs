using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class RevolventeCLS
    {
        [Key]
        public int pr_id { get; set; }
        public string pr_nombre { get; set; }
        public string pr_matricula { get; set; }
        public string pr_telefono { get; set; }
        public string pr_ingreso { get; set; }
        public string pr_modificacion { get; set; }
        public string pr_tipo { get; set; }
        public string pr_estatus { get; set; }
        public string pr_recibo { get; set; }
        public string pr_ine { get; set; }
        public System.DateTime pr_fecha { get; set; }
        public string pr_adscripcioon { get; set; }
        public string pre_tarjeta_cuenta { get; set; }
        public string pre_banco { get; set; }

    }
}