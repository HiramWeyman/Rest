using Microsoft.SqlServer.Server;
//using Org.BouncyCastle.Bcpg.OpenPgp;
using Rest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.WebPages;


namespace Rest.Controllers
{
    public class PadronController : ApiController
    {
        [System.Web.Http.Route("api/Padron")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostPadron(string[] stringArray)
        {
            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Padron_advo padron = new Padron_advo();
                    string[] partes;
                    int pad_mat = 0;
                    string pad_nombre = "";
                    string pad_adscripcion = "";
                    string pad_categoria = "";
                    string pad_sueldo = "";
                    string pad_funcion = "";
                    string pad_situacion = "";
                    string pad_permanencia = "";
                    string pad_f_ingreso = "";
                    string pad_permisos = "";
                    string pad_f_antig = "";
                    string pad_n_insaluble = "";
                    string pad_adscrip_base = "";
                    string pad_catego_base = "";
                    string pad_funcion_base = "";
                    string pad_situacion_base = "";
                    string pad_num_contacto = "";
                    string pad_observaciones = "";
                    

                    try
                    {
               

                        for (int i = stringArray.GetLowerBound(0); i <= stringArray.GetUpperBound(0); ++i) {

                            //Console.Out.WriteLine(stringArray[i]);
                            if (stringArray[i] != "") {
                                partes = stringArray[i].Split(',');
                                Console.Out.WriteLine(partes[0]);
                                Console.WriteLine(partes[0]);
                                pad_mat = int.Parse(partes[0]);
                                pad_nombre = partes[1];
                                pad_adscripcion = partes[2];
                                pad_categoria = partes[3];
                                pad_sueldo = partes[4] + "," + partes[5];
                                pad_funcion = partes[6];
                                pad_situacion = partes[7];
                                pad_permanencia = partes[8];
                                pad_f_ingreso = partes[9];
                                pad_permisos = partes[10];
                                pad_f_antig = partes[11];
                                pad_n_insaluble = partes[12];
                                pad_adscrip_base = partes[13];
                                pad_catego_base = partes[14];
                                pad_funcion_base = partes[15];
                                pad_situacion_base = partes[16];
                                pad_num_contacto = partes[17];
                                pad_observaciones = partes[18];

                     


                                padron.pad_mat = pad_mat;
                                padron.pad_nombre = pad_nombre;
                                padron.pad_adscripcion = pad_adscripcion;
                                padron.pad_categoria = pad_categoria;
                                padron.pad_sueldo = pad_sueldo;
                                padron.pad_funcion = pad_funcion;
                                padron.pad_situacion = pad_situacion;
                                padron.pad_permanencia = pad_permanencia;
                                padron.pad_f_ingreso = pad_f_ingreso;
                                padron.pad_permisos = pad_permisos;
                                padron.pad_f_antig = pad_f_antig;
                                padron.pad_n_insaluble = pad_n_insaluble;
                                padron.pad_adscrip_base = pad_adscrip_base;
                                padron.pad_catego_base = pad_catego_base;
                                padron.pad_funcion_base = pad_funcion_base;
                                padron.pad_situacion_base = pad_situacion_base;
                                padron.pad_num_contacto = pad_num_contacto;
                                padron.pad_observaciones = pad_observaciones;
                         

                                db.Padron_advo.Add(padron);
                                db.SaveChanges();
                            }
                   

                        }


                    }
                    catch (DbEntityValidationException dbEx)
                    {

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }

                    }


                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, "Archivo registrado con exito");
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.Route("api/PadDelete")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeletePad(string[] stringArray)
        {

            try
            {
             
            

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                   
                  

                    try
                    {
                        //Padron_advo dbx = new Padron_advo();
                        string query = "delete from Padron_advo";
                        db.Database.ExecuteSqlCommand(query);


                    }
                    catch (DbEntityValidationException dbEx)
                    {

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }

                    }

                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, "Archivo borrado con exito");
                    return Mensaje;

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        [System.Web.Http.Route("api/PadDeleteAct")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeletePadAct(string[] stringArray)
        {

            try
            {



                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {




                    try
                    {
                        //Padron_advo dbx = new Padron_advo();
                        string query = "delete from Padron_advo where pad_situacion='A'";
                        db.Database.ExecuteSqlCommand(query);


                    }
                    catch (DbEntityValidationException dbEx)
                    {

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }

                    }

                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, "Archivo borrado con exito");
                    return Mensaje;

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        [System.Web.Http.Route("api/PadDeleteJub")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeletePadJub(string[] stringArray)
        {

            try
            {



                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {




                    try
                    {
                        //Padron_advo dbx = new Padron_advo();
                        string query = "delete from Padron_advo where pad_situacion='J'";
                        db.Database.ExecuteSqlCommand(query);


                    }
                    catch (DbEntityValidationException dbEx)
                    {

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                    validationError.PropertyName,
                                    validationError.ErrorMessage);
                            }
                        }

                    }

                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, "Archivo borrado con exito");
                    return Mensaje;

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }

}



