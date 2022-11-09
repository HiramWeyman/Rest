using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class UserBaseCLS
    {
        [Key]
        public string ub_user { get; set; }
        public string ub_nombre { get; set; }
        public string ub_password { get; set; }
        public string ub_curp { get; set; }
        public string ub_rfc { get; set; }
    }
}