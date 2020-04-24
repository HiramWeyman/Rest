using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class MenuCLS
    {
        [Key]
        public int menu_id { get; set; }
        public string menu_descrip { get; set; }
        public int menu_orden { get; set; }
        public string menu_cancelar { get; set; }
        public string menu_routerLink { get; set; }

    }
}