﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class ConcursoPlazasCLS
    {
        [Key]
        public int pad_id { get; set; }
        public int pad_plaza_id { get; set; }
        public string pad_plaza_descrip { get; set; }
        public long pad_mat { get; set; }
        public string pad_nombre { get; set; }
        public string pad_adscripcion { get; set; }
        public string pad_categoria { get; set; }
        public string pad_sueldo { get; set; }
        public string pad_funcion { get; set; }
        public string pad_situacion { get; set; }
        public string pad_permanencia { get; set; }
        public string pad_f_ingreso { get; set; }
        public string pad_permisos { get; set; }
        public string pad_f_antig { get; set; }
        public string pad_n_insaluble { get; set; }
        public string pad_adscrip_base { get; set; }
        public string pad_catego_base { get; set; }
        public string pad_funcion_base { get; set; }
        public string pad_situacion_base { get; set; }
        public string pad_num_contacto { get; set; }
        public string pad_observaciones { get; set; }
        public string pad_string_fec { get; set; }
        public string pad_user_cancela { get; set; }

        public string pad_user_restablece { get; set; }

        public string pad_fecha_restablece { get; set; }

        public string catp_descrip { get; set; }

        public string catp_categoria { get; set; }

        public string catp_funcion { get; set; }

        public string catp_adscripcion { get; set; }

    }
}