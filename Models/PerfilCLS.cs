using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class PerfilCLS
    {
        public int id { get; set; }
        public string perfil_desc { get; set; }
        public string user_add { get; set; }
        public string user_mod { get; set; }
    }
}