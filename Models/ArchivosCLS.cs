using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class ArchivosCLS
    {
        [Key]
        public int archivo_id { get; set; }
        public string archivo_descrip { get; set; }
        public string archivo_ruta { get; set; }
        public string archivo_cancela { get; set; }
        public string archivo_seccion { get; set; }

    }
}