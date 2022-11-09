using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Rest.Models;
using System.IO;
using System.Globalization;

namespace Rest.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class UsuariosController : ApiController
    {
        [HttpGet]
        public IEnumerable<UsuariosCLS> Get()
        {
            List<UsuariosCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleado = (from usr in db.Usuarios
                                 join per in db.Perfil
                                 on usr.perfil_id equals per.id 
                                 join act in db.Actividades
                                 on usr.act_id equals act.id
                                 where usr.role_id==2
                                 && usr.user_baja.Equals(null)
                                 orderby usr.nombre_completo
                                
                                 select new UsuariosCLS
                                 {
                                     id = usr.id,
                                     matricula =(long)usr.matricula,
                                     nombre_completo =usr.nombre_completo,
                                     direccion = usr.direccion,
                                     fecho_ingreso = usr.fecho_ingreso,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec =usr.trabajador_base_rec,
                                     observaciones =usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc=per.perfil_desc,
                                     actividad_desc = act.actividad_desc
                                 }).ToList();
                return listaEmpleado;

            }
        }

        [Route("api/Integrante")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetIntegrante()
        {
            List<UsuariosCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleado = (from usr in db.Usuarios
                                 join per in db.Perfil
                                 on usr.perfil_id equals per.id
                                 join act in db.Actividades
                                 on usr.act_id equals act.id
                                 where usr.role_id != 2
                                 && usr.user_baja.Equals(null)
                                 orderby usr.nombre_completo

                                 select new UsuariosCLS
                                 {
                                     id = usr.id,
                                     matricula = (long)usr.matricula,
                                     nombre_completo = usr.nombre_completo,
                                     direccion = usr.direccion,
                                     fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec = usr.trabajador_base_rec,
                                     observaciones = usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc = per.perfil_desc,
                                     actividad_desc = act.actividad_desc
                                 }).ToList();
                return listaEmpleado;

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.Usuarios.FirstOrDefault(e => e.id == id && e.user_baja.Equals(null));
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

        [Route("api/usuariosNombre/{nombre}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersNombre(string nombre)
        {
            List<UsuariosCLS> listaEmpleadoNombre = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoNombre = (from usr in db.Usuarios
                                       join per in db.Perfil
                                       on usr.perfil_id equals per.id
                                       join act in db.Actividades
                                       on usr.act_id equals act.id
                                       where usr.nombre_completo.Contains(nombre)
                                       && usr.role_id == 2
                                       && usr.user_baja.Equals(null)
                                       orderby usr.nombre_completo

                                       select new UsuariosCLS
                                       {
                                           id = usr.id,
                                           matricula = (long)usr.matricula,
                                           nombre_completo = usr.nombre_completo,
                                           direccion = usr.direccion,
                                           fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                           telefono = usr.telefono,
                                           celular = usr.celular,
                                           trabajador_base_rec = usr.trabajador_base_rec,
                                           observaciones = usr.observaciones,
                                           act_id = (int)usr.act_id,
                                           role_id = (int)usr.role_id,
                                           perfil_desc = per.perfil_desc,
                                           actividad_desc = act.actividad_desc
                                       }).ToList();
                return listaEmpleadoNombre;

            }
        }


        [Route("api/usuariosNombreInt/{nombre}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersNombreInt(string nombre)
        {
            List<UsuariosCLS> listaEmpleadoNombre = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoNombre = (from usr in db.Usuarios
                                       join per in db.Perfil
                                       on usr.perfil_id equals per.id
                                       join act in db.Actividades
                                       on usr.act_id equals act.id
                                       where usr.nombre_completo.Contains(nombre)
                                       && usr.role_id != 2
                                       && usr.user_baja.Equals(null)
                                       orderby usr.nombre_completo

                                       select new UsuariosCLS
                                       {
                                           id = usr.id,
                                           matricula = (long)usr.matricula,
                                           nombre_completo = usr.nombre_completo,
                                           direccion = usr.direccion,
                                           fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                           telefono = usr.telefono,
                                           celular = usr.celular,
                                           trabajador_base_rec = usr.trabajador_base_rec,
                                           observaciones = usr.observaciones,
                                           act_id = (int)usr.act_id,
                                           role_id = (int)usr.role_id,
                                           perfil_desc = per.perfil_desc,
                                           actividad_desc = act.actividad_desc
                                       }).ToList();
                return listaEmpleadoNombre;

            }
        }

        [HttpPost]
        public HttpResponseMessage Post(string usr,UsuariosCLS userCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    //Console.WriteLine(usr);
                    //Random rdn = new Random();
                    //int a = rdn.Next(100, 900);
                    //int b = rdn.Next(100, 900);
                    //string c = a + "" + b;
                    DateTime? date;
                    Usuarios usuarios = new Usuarios();
                    //if (userCLS.fecho_ingreso != null)
                    //{
                    //    string x = userCLS.fecho_ingreso.ToString();
                    //    Console.WriteLine(x);
                    //    date = Convert.ToDateTime(x);
                    //    Console.WriteLine(date);

                    //    usuarios.matricula = userCLS.matricula;
                    //    usuarios.nombre_completo = userCLS.nombre_completo;
                    //    usuarios.direccion = userCLS.direccion;
                    //    usuarios.fecho_ingreso = date;
                    //    usuarios.telefono = userCLS.telefono;
                    //    usuarios.celular = userCLS.celular;
                    //    usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                    //    usuarios.observaciones = userCLS.observaciones;
                    //    usuarios.perfil_id = userCLS.perfil_id;
                    //    usuarios.act_id = userCLS.act_id;
                    //    usuarios.role_id = userCLS.role_id;
                    //    usuarios.user_login = userCLS.user_login;
                    //    usuarios.user_add = usr;

                    //    db.Usuarios.Add(usuarios);
                    //    db.SaveChanges();
                    //    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                    //    return Mensaje;
                    //}
                    if (userCLS.password != null && userCLS.password != "")
                    {
                        usuarios.matricula = userCLS.matricula;
                        usuarios.nombre_completo = userCLS.nombre_completo;
                        usuarios.direccion = userCLS.direccion;
                        usuarios.telefono = userCLS.telefono;
                        usuarios.celular = userCLS.celular;
                        usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                        usuarios.observaciones = userCLS.observaciones;
                        usuarios.perfil_id = userCLS.perfil_id;
                        usuarios.act_id = userCLS.act_id;
                        usuarios.role_id = userCLS.role_id;
                        usuarios.user_login = userCLS.user_login;
                        usuarios.user_add = usr;
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(userCLS.password);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                        usuarios.password = contraCifrada;
                        db.Usuarios.Add(usuarios);
                        db.SaveChanges();
                        var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                        return Mensaje;
                    }
                    else if (userCLS.fecho_ingreso != null && userCLS.password != null && userCLS.password != "")
                    {
                        string x = userCLS.fecho_ingreso.ToString();
                        Console.WriteLine(x);
                        date = Convert.ToDateTime(x);
                        Console.WriteLine(date);

                        usuarios.matricula = userCLS.matricula;
                        usuarios.nombre_completo = userCLS.nombre_completo;
                        usuarios.direccion = userCLS.direccion;
                        usuarios.fecho_ingreso = date;
                        usuarios.telefono = userCLS.telefono;
                        usuarios.celular = userCLS.celular;
                        usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                        usuarios.observaciones = userCLS.observaciones;
                        usuarios.perfil_id = userCLS.perfil_id;
                        usuarios.act_id = userCLS.act_id;
                        usuarios.role_id = userCLS.role_id;
                        usuarios.user_login = userCLS.user_login;
                        usuarios.user_add = usr;
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(userCLS.password);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                        usuarios.password = contraCifrada;
                        db.Usuarios.Add(usuarios);
                        db.SaveChanges();
                        var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                        return Mensaje;
                    }
                    else {
                        usuarios.matricula = userCLS.matricula;
                        usuarios.nombre_completo = userCLS.nombre_completo;
                        usuarios.direccion = userCLS.direccion;
                        usuarios.telefono = userCLS.telefono;
                        usuarios.celular = userCLS.celular;
                        usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                        usuarios.observaciones = userCLS.observaciones;
                        usuarios.perfil_id = userCLS.perfil_id;
                        usuarios.act_id = userCLS.act_id;
                        usuarios.role_id = userCLS.role_id;
                        usuarios.user_login = userCLS.user_login;
                        usuarios.user_add = usr;

                        db.Usuarios.Add(usuarios);
                        db.SaveChanges();
                        var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                        return Mensaje;
                    }
                 
               
             
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
                    Usuarios usuarios = new Usuarios();
                    usuarios = db.Usuarios.Where(p => p.id.Equals(id)).First();
                    if (usuarios == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        Console.WriteLine(userCLS.password);
                        if (userCLS.password != null && userCLS.password != "")
                        {

                            usuarios.nombre_completo = userCLS.nombre_completo;
                            usuarios.direccion = userCLS.direccion;
                            usuarios.fecho_ingreso = userCLS.fecho_ingreso;
                            usuarios.telefono = userCLS.telefono;
                            usuarios.celular = userCLS.celular;
                            usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                            usuarios.observaciones = userCLS.observaciones;
                            usuarios.perfil_id = userCLS.perfil_id;
                            usuarios.act_id = userCLS.act_id;
                            usuarios.role_id = userCLS.role_id;
                            usuarios.user_login = userCLS.user_login;
                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(userCLS.password);
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                            usuarios.password = contraCifrada;
                            usuarios.user_mod = userCLS.usr_mod;
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else {

                            usuarios.nombre_completo = userCLS.nombre_completo;
                            usuarios.direccion = userCLS.direccion;
                            usuarios.fecho_ingreso = userCLS.fecho_ingreso;
                            usuarios.telefono = userCLS.telefono;
                            usuarios.celular = userCLS.celular;
                            usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                            usuarios.observaciones = userCLS.observaciones;
                            usuarios.perfil_id = userCLS.perfil_id;
                            usuarios.act_id = userCLS.act_id;
                            usuarios.role_id = userCLS.role_id;
                            usuarios.user_login = userCLS.user_login;
                            usuarios.user_mod = userCLS.usr_mod;
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                       

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/userDelete/{id}")]
        [HttpPut]
        public HttpResponseMessage deleteUser(int id, UsuariosCLS userCLS)
        {

            try
            {
                id = userCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Usuarios usuario = db.Usuarios.Where(p => p.id.Equals(id)).First();
                    if (usuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        usuario.user_baja = DateTime.Now;
                        usuario.user_mod = userCLS.usr_mod;
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



        [Route("api/Actividades")]
        [HttpGet]
        public IEnumerable<Actividades> GetActividades()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Actividades.Where(x => x.actividad_desc!=null).OrderBy(x => x.actividad_desc).ToList();

            }
        }

        [Route("api/Actividades/{id}")]
        [HttpGet]
        public HttpResponseMessage GetActividades(int id) {

            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var act = db.Actividades.FirstOrDefault(e => e.id == id);
                if (act != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, act);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con Id" + id.ToString() + " no encontrado");
                }

            }
        }

        [Route("api/usuariosActividad/{id}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersActividad(int id)
        {
            List<UsuariosCLS> listaEmpleadoAct = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoAct = (from usr in db.Usuarios
                                       join per in db.Perfil
                                       on usr.perfil_id equals per.id
                                       join act in db.Actividades
                                       on usr.act_id equals act.id
                                       where usr.act_id == id
                                       && usr.role_id==2
                                       && usr.user_baja.Equals(null)
                                    orderby usr.nombre_completo

                                       select new UsuariosCLS
                                       {
                                           id = usr.id,
                                           matricula = (long)usr.matricula,
                                           nombre_completo = usr.nombre_completo,
                                           direccion = usr.direccion,
                                           fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                           telefono = usr.telefono,
                                           celular = usr.celular,
                                           trabajador_base_rec = usr.trabajador_base_rec,
                                           observaciones = usr.observaciones,
                                           act_id = (int)usr.act_id,
                                           role_id = (int)usr.role_id,
                                           perfil_desc = per.perfil_desc,
                                           actividad_desc = act.actividad_desc
                                       }).ToList();
                return listaEmpleadoAct;

            }
        }

        [Route("api/usuariosActividadInt/{id}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersActividadInt(int id)
        {
            List<UsuariosCLS> listaEmpleadoAct = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoAct = (from usr in db.Usuarios
                                    join per in db.Perfil
                                    on usr.perfil_id equals per.id
                                    join act in db.Actividades
                                    on usr.act_id equals act.id
                                    where usr.act_id == id
                                    && usr.role_id != 2
                                    && usr.user_baja.Equals(null)
                                    orderby usr.nombre_completo

                                    select new UsuariosCLS
                                    {
                                        id = usr.id,
                                        matricula = (long)usr.matricula,
                                        nombre_completo = usr.nombre_completo,
                                        direccion = usr.direccion,
                                        fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                        telefono = usr.telefono,
                                        celular = usr.celular,
                                        trabajador_base_rec = usr.trabajador_base_rec,
                                        observaciones = usr.observaciones,
                                        act_id = (int)usr.act_id,
                                        role_id = (int)usr.role_id,
                                        perfil_desc = per.perfil_desc,
                                        actividad_desc = act.actividad_desc
                                    }).ToList();
                return listaEmpleadoAct;

            }
        }

        [Route("api/Actividades")]
        [HttpPost]
        public HttpResponseMessage PostAct(string usr,ActividadCLS actCLS) {
            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                   
                    Actividades actividad = new Actividades();
                    actividad.actividad_desc = actCLS.actividad_desc;
                    actividad.user_add = usr;
                    db.Actividades.Add(actividad);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, actCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("api/Actividades/{id}")]
        [HttpPut]
        public HttpResponseMessage PutAct(int id, ActividadCLS actCLS) {
            try
            {
                id = actCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Actividades act = db.Actividades.Where(p => p.id.Equals(id)).First();
                    if (act == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con ID " + id.ToString() + " no encontrada");
                    }
                    else
                    {
                        act.actividad_desc = actCLS.actividad_desc;
                        act.user_mod = actCLS.user_mod;
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

        [Route("api/Actividades/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteAct(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var act = db.Actividades.FirstOrDefault(e => e.id == id);
                    if (act == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con Id" + id.ToString() + " no encontrada");

                    }
                    else
                    {
                        db.Actividades.Remove(act);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, act);
                    }


                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/Perfil")]
        [HttpGet]
        public IEnumerable<Perfil> GetPerfil()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Perfil.Where(x => x.perfil_desc != null).OrderBy(x => x.perfil_desc).ToList();

            }
        }

        [Route("api/Perfil/{id}")]
        [HttpGet]
        public HttpResponseMessage GetPerfil(int id) {

            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var per = db.Perfil.FirstOrDefault(e => e.id == id);
                if (per != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, per);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con Id" + id.ToString() + " no encontrado");
                }

            }
        }

        [Route("api/usuariosPerfil/{id}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersPerfil(int id)
        {
            List<UsuariosCLS> listaEmpleadoPerfil = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoPerfil = (from usr in db.Usuarios
                                 join per in db.Perfil
                                 on usr.perfil_id equals per.id
                                 join act in db.Actividades
                                 on usr.act_id equals act.id
                                 where usr.perfil_id==id
                                 && usr.role_id==2
                                 && usr.user_baja.Equals(null)
                                 orderby usr.nombre_completo

                                 select new UsuariosCLS
                                 {
                                     id = usr.id,
                                     matricula = (long)usr.matricula,
                                     nombre_completo = usr.nombre_completo,
                                     direccion = usr.direccion,
                                     fecho_ingreso = (DateTime)usr.fecho_ingreso,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec = usr.trabajador_base_rec,
                                     observaciones = usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc = per.perfil_desc,
                                     actividad_desc = act.actividad_desc
                                 }).ToList();
                return listaEmpleadoPerfil;

            }
        }


        [Route("api/usuariosPerfilInt/{id}")]
        [HttpGet]
        public IEnumerable<UsuariosCLS> GetUsersPerfilInt(int id)
        {
            List<UsuariosCLS> listaEmpleadoPerfil = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleadoPerfil = (from usr in db.Usuarios
                                       join per in db.Perfil
                                       on usr.perfil_id equals per.id
                                       join act in db.Actividades
                                       on usr.act_id equals act.id
                                       where usr.perfil_id == id
                                       && usr.role_id != 2
                                       && usr.user_baja.Equals(null)
                                       orderby usr.nombre_completo

                                       select new UsuariosCLS
                                       {
                                           id = usr.id,
                                           matricula = (long)usr.matricula,
                                           nombre_completo = usr.nombre_completo,
                                           direccion = usr.direccion,
                                           telefono = usr.telefono,
                                           celular = usr.celular,
                                           trabajador_base_rec = usr.trabajador_base_rec,
                                           observaciones = usr.observaciones,
                                           act_id = (int)usr.act_id,
                                           role_id = (int)usr.role_id,
                                           perfil_desc = per.perfil_desc,
                                           actividad_desc = act.actividad_desc
                                       }).ToList();
                return listaEmpleadoPerfil;

            }
        }
        [Route("api/Perfil")]
        [HttpPost]
        public HttpResponseMessage PostPer(string usr, PerfilCLS perCLS)
        {
            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Perfil per = new Perfil();
                    per.perfil_desc = perCLS.perfil_desc;
                    per.user_add = usr;
                    db.Perfil.Add(per);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, perCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("api/Perfil/{id}")]
        [HttpPut]
        public HttpResponseMessage PutPer(int id, PerfilCLS perCLS)
        {
            try
            {
                id = perCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Perfil per = db.Perfil.Where(p => p.id.Equals(id)).First();
                    if (per == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        per.perfil_desc = perCLS.perfil_desc;
                        perCLS.user_mod = perCLS.user_mod;
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

        [Route("api/Perfil/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeletePer(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var per = db.Perfil.FirstOrDefault(e => e.id == id);
                    if (per == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Perfil.Remove(per);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, per);
                    }


                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [Route("api/Roles")]
        [HttpGet]
        public IEnumerable<Roles> GetRoles()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Roles.OrderBy(x => x.rol_desc).ToList();

            }
        }

        //[Route("api/pruebas")]
        //[HttpGet]
        //public IEnumerable<UsuariosCLS> Pruebas()
        //{
        //    List<UsuariosCLS> listaEmpleado = null;

        //    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
        //    {
        //        Usuario usuarios = new Usuario();
        //        db.Configuration.LazyLoadingEnabled = false;
        //        listaEmpleado = (from usr in db.Usuarios
        //                         where usr.role_id == 2
        //                         orderby usr.nombre_completo

        //                         select new UsuariosCLS
        //                         {
        //                             id = usr.id,
        //                             matricula = (long)usr.matricula,
        //                             nombre_completo = usr.nombre_completo.Substring(1),
        //                             direccion = usr.direccion.Substring(1),
        //                             telefono = usr.telefono.Substring(1),
        //                             act_id = (int)usr.act_id,
        //                             role_id = (int)usr.role_id,
                             
        //                         }).ToList();
        //        foreach (UsuariosCLS i in listaEmpleado)
        //        {
        //            Console.WriteLine(i.nombre_completo);
        //            usuarios = db.Usuarios.Where(p => p.id.Equals(i.id)).First();
        //            if (usuarios == null)
        //            {
        //                Console.WriteLine("No actualizo nada");
        //            }
        //            else
        //            {
        //                usuarios.nombre_completo = i.nombre_completo;
        //                usuarios.direccion = i.direccion;
        //                usuarios.telefono = i.telefono;
        //                usuarios.act_id = i.act_id;
        //                usuarios.role_id =i.role_id;
        //                db.SaveChanges();


        //            }
        //        }
        //        //for (int i = 0; i < listaEmpleado.Count; i++) {

        //        //    usuarios = db.Usuarios.Where(p => p.id.Equals(listaEmpleado[i].id)).First();
        //        //    if (usuarios == null)
        //        //    {
        //        //        Console.WriteLine("No actualizo nada");
        //        //    }
        //        //    else
        //        //    {
        //        //        usuarios.nombre_completo = listaEmpleado[i].nombre_completo;
        //        //        usuarios.direccion = listaEmpleado[i].direccion;
        //        //        usuarios.telefono = listaEmpleado[i].telefono;
        //        //        usuarios.act_id = listaEmpleado[i].act_id;
        //        //        usuarios.role_id = listaEmpleado[i].role_id;
        //        //        db.SaveChanges();


        //        //    }
        //        //}
        //        return listaEmpleado;

        //    }
        //}

    }
}
