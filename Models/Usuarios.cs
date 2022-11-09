//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuarios
    {
        public Usuarios()
        {
            this.Lista_Asistencia = new HashSet<Lista_Asistencia>();
        }
    
        public int id { get; set; }
        public long matricula { get; set; }
        public string nombre_completo { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string trabajador_base_rec { get; set; }
        public string observaciones { get; set; }
        public int act_id { get; set; }
        public int role_id { get; set; }
        public Nullable<System.DateTime> fecho_ingreso { get; set; }
        public string password { get; set; }
        public string user_add { get; set; }
        public string user_mod { get; set; }
        public string user_login { get; set; }
        public Nullable<int> perfil_id { get; set; }
        public Nullable<System.DateTime> user_baja { get; set; }
    
        public virtual Actividades Actividades { get; set; }
        public virtual ICollection<Lista_Asistencia> Lista_Asistencia { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
