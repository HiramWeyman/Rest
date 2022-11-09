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
                return db.Menu.Where(x => x.menu_orden != 999 && x.menu_cancelar == "N").OrderBy(x => x.menu_orden).ToList();

            }
        }

        public HttpResponseMessage Get(String Bandera)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var menu = db.Menu.Where(x => x.menu_orden != 999).OrderBy(x => x.menu_orden).ToList();
                if (menu != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, menu);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Menu no encontrado.");
                }
            }
        }

        // GET: api/Menu/5
        //public IEnumerable<Menu> Get(int id)
        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var menu = db.Menu.FirstOrDefault(x => x.menu_id == id);
                if (menu != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, menu);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Menu no encontrado.");
                }

            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, MenuCLS menuCLS)
        {

            try
            {
                id = menuCLS.menu_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Menu menu = db.Menu.Where(p => p.menu_id.Equals(id)).First();
                    if (menu == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Menu no encontrado.");
                    }
                    else
                    {
                        menu.menu_descrip = menuCLS.menu_descrip;
                        menu.menu_orden = menuCLS.menu_orden;
                        menu.menu_cancelar = menuCLS.menu_cancelar;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
