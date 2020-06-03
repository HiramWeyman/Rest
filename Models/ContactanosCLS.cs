using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class ContactanosCLS
    {
        [Key]
        public int con_id { get; set; }
        public string con_concepto { get; set; }
        public string con_texto { get; set; }
        public string con_link { get; set; }
        public string con_visible { get; set; }

    }
}