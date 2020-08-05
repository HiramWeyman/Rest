using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class CatPlazasCLS
    {
        [Key]
        public int catp_id { get; set; }
        public string catp_descrip { get; set; }
        public string catp_status { get; set; }
        public string catp_u_captura { get; set; }
        public System.DateTime catp_f_captura { get; set; }
        public string catp_categoria { get; set; }
        public string catp_funcion { get; set; }
        public string catp_adscripcion { get; set; }
    }
}