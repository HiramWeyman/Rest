using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class GaleriaCLS
    {
        [Key]
        public int gal_id { get; set; }
        public string gal_titulo { get; set; }
        public string gal_ruta { get; set; }
        public string gal_u_publica { get; set; }
        public System.DateTime gal_f_publica { get; set; }
        public string gal_cancela { get; set; }

    }
}