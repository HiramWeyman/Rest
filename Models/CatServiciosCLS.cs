using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class CatServiciosCLS
    {
        [Key]
        public int cats_id { get; set; }
        public string cats_descrip { get; set; }
        public string cats_status { get; set; }
        public string cats_u_captura { get; set; }
        public System.DateTime cats_f_captura { get; set; }
    }
}