using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class IntegrantesCLS
    {
        [Key]
        public int int_id { get; set; }
        public string int_nombre { get; set; }
        public string int_puesto { get; set; }
        public string int_ruta_imagen { get; set; }
        public string int_cancelado { get; set; }

    }
}