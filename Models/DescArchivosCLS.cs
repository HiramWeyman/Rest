using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class DescArchivosCLS
    {
        [Key]
        public int da_id { get; set; }
        public string da_descripcion { get; set; }
        public string da_nombre { get; set; }
        public System.DateTime da_fecha { get; set; }
        public string da_matricula { get; set; }
    }
}