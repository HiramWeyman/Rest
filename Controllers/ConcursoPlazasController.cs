using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;
using System.Globalization;

namespace Rest.Controllers
{
    public class ConcursoPlazasController : ApiController
    {
        [HttpGet]
        public IEnumerable<ConcursoPlazasCLS> Get()
        {
            List<ConcursoPlazasCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                //string query = "  SELECT pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_cancelado,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion,(TRY_CONVERT(date, pad_f_antig, 103)) as FECHA_DATE FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id  order by FECHA_DATE ";
                string query = "select* from concurso_antig order by pad_fec_date,pad_sueldo";
                 //string query = "  SELECT pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_cancelado,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id  order by SUBSTRING(pad_f_antig,7, 4), pad_sueldo ";
                 //string query = "  SELECT pad_id,SUBSTRING(pad_f_antig,7, 4) as anio FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id  order by SUBSTRING(pad_f_antig,7, 4), pad_sueldo ";
                 listaEmpleado = db.Database.SqlQuery<ConcursoPlazasCLS>(query).ToList();
                //listaEmpleado = (from advos in db.Concurso_Plazas
                //                 join plaza in db.Cat_Plazas
                //                 on advos.pad_plaza_id equals plaza.catp_id
                //                 orderby advos.pad_f_antig

                                 //select new ConcursoPlazasCLS
                                 //{
                                 //    pad_id = advos.pad_id,
                                 //    pad_plaza_id = advos.pad_plaza_id,
                                 //    pad_plaza_descrip = plaza.catp_descrip,
                                 //    pad_mat = advos.pad_mat,
                                 //    pad_nombre = advos.pad_nombre,
                                 //    pad_adscripcion = advos.pad_adscripcion,
                                 //    pad_categoria = advos.pad_categoria,
                                 //    pad_funcion = advos.pad_funcion,
                                 //    pad_situacion = advos.pad_situacion,
                                 //    pad_permanencia = advos.pad_permanencia,
                                 //    pad_f_ingreso = advos.pad_f_ingreso,
                                 //    pad_permisos = advos.pad_permisos,
                                 //    pad_f_antig = advos.pad_f_antig,
                                 //    pad_n_insaluble = advos.pad_n_insaluble,
                                 //    pad_adscrip_base = advos.pad_adscrip_base,
                                 //    pad_catego_base = advos.pad_catego_base,
                                 //    pad_funcion_base = advos.pad_funcion_base,
                                 //    pad_situacion_base = advos.pad_situacion_base,
                                 //    pad_num_contacto = advos.pad_num_contacto,
                                 //    pad_observaciones = advos.pad_observaciones

                                 //    /*
                                 //    id = usr.id,
                                 //    matricula = (long)usr.matricula,
                                 //    nombre_completo = usr.nombre_completo,
                                 //    direccion = usr.direccion,
                                 //    fecho_ingreso = usr.fecho_ingreso,
                                 //    telefono = usr.telefono,
                                 //    celular = usr.celular,
                                 //    trabajador_base_rec = usr.trabajador_base_rec,
                                 //    observaciones = usr.observaciones,
                                 //    act_id = (int)usr.act_id,
                                 //    role_id = (int)usr.role_id,
                                 //    perfil_desc = per.perfil_desc,
                                 //    actividad_desc = act.actividad_desc
                                 //    */
                                 //}).ToList();


                return listaEmpleado;

            }
        }

        [Route("api/plazasCon/{id}")]
        public IEnumerable<concurso_antig> GetPlazas(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                List<concurso_antig> listaEmpleado = null;
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Concurso_Plazas.Where(x => x.pad_plaza_id == id).OrderBy(x => x.pad_f_antig).ToList();
                string query = " select * from concurso_antig where pad_plaza_id=" + id + " order by pad_fec_date,pad_sueldo ";

                //string query = "  SELECT  pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_string_fec,pad_cancelado,pad_user_cancela,pad_user_restablece,pad_fecha_cancelacion,pad_fecha_restablece,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id and pad_cancelado IS NULL and pad_plaza_id=" + id+ "  order by SUBSTRING(pad_f_antig,7, 4), pad_sueldo ";
                listaEmpleado = db.Database.SqlQuery<concurso_antig>(query).ToList();
                return listaEmpleado;
            }
        }

        [Route("api/SolCancel/{id}")]
        [HttpGet]
        public IEnumerable<concurso_antig> GetCancel(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                List<concurso_antig> listaEmpleado = null;
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Concurso_Plazas.Where(x => x.pad_plaza_id == id).OrderBy(x => x.pad_f_antig).ToList();

                string query = " select* from concurso_antig where pad_cancelado = 'S' and pad_plaza_id = " + id + " order by pad_fec_date,pad_sueldo";
               // string query = "  SELECT  pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_string_fec,pad_cancelado,pad_user_cancela,pad_user_restablece,pad_fecha_cancelacion,pad_fecha_restablece,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id and pad_cancelado='S' and pad_plaza_id=" + id + "  order by SUBSTRING(pad_f_antig,7, 4), pad_sueldo ";
                listaEmpleado = db.Database.SqlQuery<concurso_antig>(query).ToList();
                return listaEmpleado;
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var concurso = db.Concurso_Plazas.FirstOrDefault(x => x.pad_id == id);
                if (concurso != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, concurso);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Registro no encontrado.");
                }

            }
        }

        
        //public HttpResponseMessage GetSolicitudesReimpresion(int id)
        //{
        //    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
        //    {
        //        db.Configuration.LazyLoadingEnabled = false;
        //        var concurso = db.Concurso_Plazas.FirstOrDefault(x => x.pad_mat == id);
        //        if (concurso != null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, concurso);
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Registro no encontrado.");
        //        }

        //    }
        //}
        [Route("api/GetSolicitudesReimpresion/{id}")]
        public IEnumerable<Concurso_antigCLS> GetSolicitudesReimpresion(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                List<Concurso_antigCLS> listaEmpleado = null;
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Concurso_Plazas.Where(x => x.pad_plaza_id == id).OrderBy(x => x.pad_f_antig).ToList();
                //string query = "  select * from concurso_antig where pad_plaza_id=" + id + " order by pad_fec_date,pad_sueldo  ";
                string query = " SELECT "+
                               " [pad_id]"+
                              " ,[pad_plaza_id]"+
                              " ,[pad_mat]"+
                              ",[pad_nombre]"+
                              ",[pad_adscripcion]"+
                              ",[pad_categoria]"+
                              ",[pad_sueldo]"+
                              ",[pad_funcion]"+
                              ",[pad_situacion]"+
                              ",[pad_permanencia]"+
                              ",[pad_f_ingreso]"+
                              ",[pad_permisos]"+
                              ",[pad_f_antig]"+
                              ",[pad_n_insaluble]"+
                              ",[pad_adscrip_base]"+
                              ",[pad_catego_base]"+
                              ",[pad_funcion_base]"+
                              ",[pad_situacion_base]"+
                              ",[pad_num_contacto]"+
                              ",[pad_observaciones]"+
                              ",[pad_fec_date]"+
                              ",[pad_id_estatus]"+
                              ",[pla_id]"+
                              ",[pla_desc_corta]"+
                              ",[pla_desc_extendida]"+
                              ",[catp_descrip]"+
                              " FROM[steujedo_sindicato].[steujedo_sindicato].[concurso_antig],[steujedo_sindicato].[steujedo_sindicato].[Cat_Plazas]"+
                              " where[pad_plaza_id] =[catp_id] and[pad_mat] = " + id;
                listaEmpleado = db.Database.SqlQuery<Concurso_antigCLS>(query).ToList();
                return listaEmpleado;
            }
        }

        public HttpResponseMessage Post(ConcursoPlazasCLS concursoplazasCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Concurso_Plazas concurso_plazas = new Concurso_Plazas();
                    concurso_plazas.pad_plaza_id = concursoplazasCLS.pad_plaza_id;
                    concurso_plazas.pad_mat = concursoplazasCLS.pad_mat;
                    concurso_plazas.pad_nombre = concursoplazasCLS.pad_nombre;
                    concurso_plazas.pad_adscripcion = concursoplazasCLS.pad_adscripcion;
                    concurso_plazas.pad_categoria = concursoplazasCLS.pad_categoria;
                    concurso_plazas.pad_sueldo = concursoplazasCLS.pad_sueldo;
                    concurso_plazas.pad_funcion = concursoplazasCLS.pad_funcion;
                    concurso_plazas.pad_situacion = concursoplazasCLS.pad_situacion;
                    concurso_plazas.pad_permanencia = concursoplazasCLS.pad_permanencia;
                    concurso_plazas.pad_f_ingreso = concursoplazasCLS.pad_f_ingreso;
                    concurso_plazas.pad_f_antig = concursoplazasCLS.pad_f_antig;
                    concurso_plazas.pad_n_insaluble = concursoplazasCLS.pad_n_insaluble;
                    concurso_plazas.pad_adscrip_base = concursoplazasCLS.pad_adscrip_base;
                    concurso_plazas.pad_catego_base = concursoplazasCLS.pad_catego_base;
                    concurso_plazas.pad_funcion_base = concursoplazasCLS.pad_funcion_base;
                    concurso_plazas.pad_situacion_base = concursoplazasCLS.pad_situacion_base;
                    concurso_plazas.pad_num_contacto = concursoplazasCLS.pad_num_contacto;
                    concurso_plazas.pad_observaciones = concursoplazasCLS.pad_observaciones;
                    //var dateTime = DateTime.Now;
                    //dateTime.ToString("dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                    concurso_plazas.pad_string_fec = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");

                    db.Concurso_Plazas.Add(concurso_plazas);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, concurso_plazas);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/CancelarSol/{id}")]
        [HttpPut]
        public HttpResponseMessage CancelaSol(int id, ConcursoPlazasCLS concursoPlazasCLS)
        {

            try
            {
                id = concursoPlazasCLS.pad_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Concurso_Plazas Concursousuario = db.Concurso_Plazas.Where(p => p.pad_id.Equals(id)).First();
                    if (Concursousuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        Concursousuario.pad_cancelado = "S";
                        var dateTime = DateTime.Now;
                        dateTime.ToString("dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                        Concursousuario.pad_fecha_cancelacion = dateTime.ToString();
                        Concursousuario.pad_user_cancela = concursoPlazasCLS.pad_user_cancela;
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



        [Route("api/RestablecerSol/{id}")]
        [HttpPut]
        public HttpResponseMessage RestablecerSol(int id, ConcursoPlazasCLS concursoPlazasCLS)
        {

            try
            {
                id = concursoPlazasCLS.pad_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Concurso_Plazas Concursousuario = db.Concurso_Plazas.Where(p => p.pad_id.Equals(id)).First();
                    if (Concursousuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        Concursousuario.pad_cancelado = null;
                        var dateTime = DateTime.Now;
                        dateTime.ToString("dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                        Concursousuario.pad_fecha_restablece = dateTime.ToString();
                        Concursousuario.pad_user_restablece = concursoPlazasCLS.pad_user_restablece;
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


        [Route("api/ActEstatusSol/{id}")]
        [HttpPut]
        public HttpResponseMessage ActEstatusSol(int id, int parametro)
        {

            try
            {
                
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Concurso_Plazas Concursousuario = db.Concurso_Plazas.Where(p => p.pad_id.Equals(id)).First();
                    if (Concursousuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Solicitud con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        Concursousuario.pad_cancelado = null;
                        var dateTime = DateTime.Now;
                        dateTime.ToString("dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                        Concursousuario.pad_id_estatus = parametro;
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


        [Route("api/ActComentarios/{id}")]
        [HttpPut]
        public HttpResponseMessage ActualizaComentarios(int id, string comentario)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Concurso_Plazas Concursousuario = db.Concurso_Plazas.Where(p => p.pad_id.Equals(id)).First();
                    if (Concursousuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Solicitud con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        //Concursousuario.pad_cancelado = null;
                        //var dateTime = DateTime.Now;
                        //dateTime.ToString("dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                        Concursousuario.pad_comentarios = comentario;
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
