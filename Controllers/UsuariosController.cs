using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Rest.Models;

namespace Rest.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class UsuariosController : ApiController
    {
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Usuarios.OrderBy(x => x.nombre_completo).ToList();

            }
        }

        [Route("api/Actividades")]
        [HttpGet]
        public IEnumerable<Actividade> GetActividades()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Actividades.OrderBy(x => x.actividad_desc).ToList();

            }
        }

        [Route("api/Roles")]
        [HttpGet]
        public IEnumerable<Role> GetRoles()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Roles.OrderBy(x=>x.rol_desc).ToList();

            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.Usuarios.FirstOrDefault(e => e.id == id);
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Id" + id.ToString() + " no encontrado");
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage Post(UsuariosCLS userCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Random rdn = new Random();
                    int a = rdn.Next(100, 900);
                    int b = rdn.Next(100, 900);
                    string c = a + "" + b;

                    Usuario usuarios = new Usuario();
                    usuarios.matricula = long.Parse(c);
                    usuarios.nombre_completo = userCLS.nombre_completo;
                    usuarios.direccion = userCLS.direccion;
                    usuarios.telefono = userCLS.telefono;
                    usuarios.celular = userCLS.celular;
                    usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                    usuarios.observaciones = userCLS.observaciones;
                    usuarios.act_id = userCLS.act_id;
                    usuarios.role_id = userCLS.role_id;


                    db.Usuarios.Add(usuarios);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        public HttpResponseMessage Put(int id,UsuariosCLS userCLS)
        {

            try
            {
                id = userCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Usuario usuario = db.Usuarios.Where(p => p.id.Equals(id)).First();
                    if (usuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        usuario.nombre_completo = userCLS.nombre_completo;
                        usuario.direccion = userCLS.direccion;
                        usuario.telefono = userCLS.telefono;
                        usuario.celular = userCLS.celular;
                        usuario.trabajador_base_rec = userCLS.trabajador_base_rec;
                        usuario.observaciones = userCLS.observaciones;
                        usuario.act_id = userCLS.act_id;
                        usuario.role_id = userCLS.role_id;
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

        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var user = db.Usuarios.FirstOrDefault(e => e.id == id);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Usuarios.Remove(user);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, user);
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
