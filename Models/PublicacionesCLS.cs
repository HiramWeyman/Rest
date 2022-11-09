using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class PublicacionesCLS
    {
        [Key]
        public int pub_id { get; set; }
        public string pub_titulo { get; set; }
        public string pub_subtitulo { get; set; }
        public string pub_texto { get; set; }
        public string pub_u_publica { get; set; }
        public System.DateTime pub_f_publica { get; set; }
        public string pub_cancela { get; set; }
        public int pub_id_categoria { get; set; }
        public string pub_ruta { get; set; }

    }
}