using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rest.Controllers
{
    public class MenuController : ApiController
    {
        [HttpGet]
        public IEnumerable<Menu> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Menus.Where(x => x.menu_orden != 999 && x.menu_cancelar == "N").OrderBy(x => x.menu_orden).ToList();

            }
        }

        // GET: api/Menu/5
        public IEnumerable<Menu> Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Menus.Where(x => x.menu_id == id).ToList();

            }
        }
    }
}
