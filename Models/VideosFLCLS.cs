using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rest.Models
{
    public class VideosFLCLS
    {
        [Key]
        public int vid_id { get; set; }
        public string vid_titulo { get; set; }
        public int vid_ancho { get; set; }
        public int vid_largo { get; set; }
        public string vid_src { get; set; }
        public string vid_u_publica { get; set; }
        public System.DateTime vid_f_publica { get; set; }
    }
}