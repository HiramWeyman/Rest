using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class AudiotecaCLS
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string status { get; set; }
        public string Usuario { get; set; }
    }
}