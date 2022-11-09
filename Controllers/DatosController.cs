using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rest.Models;

namespace Rest.Controllers
{
    public class DatosController : ApiController
    {

        [HttpGet]

        public IEnumerable<Usuarios> Get(string user)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Usuarios.Where(x=> x.user_login==user).ToList();
            }
        }

    }
}
