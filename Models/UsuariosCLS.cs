using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Rest.Models
{
    public class UsuariosCLS
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Matricula")]
        public long matricula { get; set; }

        [Display(Name = "Nombre Completo")]
        public string nombre_completo { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Display(Name = "F_ingreso")]
        public DateTime? fecho_ingreso { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Celular")]
        public string celular { get; set; }
        [Display(Name = "Trabajador base")]
        public string trabajador_base_rec { get; set; }

        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }
        public int perfil_id { get; set; }
        public int act_id { get; set; }
        public int role_id { get; set; }
        public string perfil_desc { get; set; }
        public string actividad_desc { get; set; }
        public string user_login { get; set; }
        public string password { get; set; }
        public string usr_mod { get; set; }
        public string entrada { get; set; }
    }
}