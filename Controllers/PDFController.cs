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
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Font = iTextSharp.text.Font;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using System.Configuration;
//using Grpc.Core;

namespace Rest.Controllers
{
    public class PDFController : ApiController
    {
        [Route("api/Reportelista")]
        [HttpGet]
        public HttpResponseMessage CreateLieferschein()
        {

            List<UsuariosCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleado = (from usr in db.Usuarios
                                 join per in db.Perfils
                                 on usr.perfil_id equals per.id
                                 join act in db.Actividades
                                 on usr.act_id equals act.id
                                 where usr.role_id == 2
                                 && usr.user_baja.Equals(null)
                                 orderby usr.nombre_completo

                                 select new UsuariosCLS
                                 {
                                     id = usr.id,
                                     matricula = (long)usr.matricula,
                                     nombre_completo = usr.nombre_completo,
                                     direccion = usr.direccion,
                                     fecho_ingreso = usr.fecho_ingreso,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec = usr.trabajador_base_rec,
                                     observaciones = usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc = per.perfil_desc,
                                     actividad_desc = act.actividad_desc
                                 }).ToList();

                String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
                iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
                iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                byte[] buffer;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter.GetInstance(doc, stream);
                    doc.Open();

                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                    //image1.ScalePercent(50f);
                    image1.ScaleAbsoluteWidth(70);
                    image1.ScaleAbsoluteHeight(60);
                    image1.Alignment = Element.ALIGN_CENTER;
                    doc.Add(image1);
                    Paragraph espacio = new Paragraph(" ");
                    doc.Add(espacio);

                    Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                    title1.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title1);
                    doc.Add(espacio);

                    Paragraph title = new Paragraph("Listado de Empleados Eventuales", font1);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(espacio);

                    ////Creando la tabla
                    PdfPTable tabla = new PdfPTable(9);
                    tabla.WidthPercentage = 100f;
                    ////Asignando los anchos de las columnas
                    float[] valores = new float[9] { 14, 35, 20, 20, 20,20,20,20,30 };
                    tabla.SetWidths(valores);

                    ////Creando celdas agregando contenido
                    PdfPCell celda1 = new PdfPCell(new Phrase("Matricula", font));
                    celda1.BackgroundColor = new BaseColor(240, 240, 240);
                    celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda1);

                    PdfPCell celda2 = new PdfPCell(new Phrase("Nombre Completo", font));
                    celda2.BackgroundColor = new BaseColor(240, 240, 240);
                    celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda2);

                    PdfPCell celda3 = new PdfPCell(new Phrase("Perfil", font));
                    celda3.BackgroundColor = new BaseColor(240, 240, 240);
                    celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda3);

                    PdfPCell celda4 = new PdfPCell(new Phrase("Actividad", font));
                    celda4.BackgroundColor = new BaseColor(240, 240, 240);
                    celda4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda4);

                    PdfPCell celda5 = new PdfPCell(new Phrase("f.ingreso", font));
                    celda5.BackgroundColor = new BaseColor(240, 240, 240);
                    celda5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda5);

                    PdfPCell celda6 = new PdfPCell(new Phrase("Direccion", font));
                    celda6.BackgroundColor = new BaseColor(240, 240, 240);
                    celda6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda6);

                    PdfPCell celda7 = new PdfPCell(new Phrase("Telefono", font));
                    celda7.BackgroundColor = new BaseColor(240, 240, 240);
                    celda7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda7);

                    PdfPCell celda8 = new PdfPCell(new Phrase("Celular", font));
                    celda8.BackgroundColor = new BaseColor(240, 240, 240);
                    celda8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda8);

                    PdfPCell celda9 = new PdfPCell(new Phrase("Observaciones", font));
                    celda9.BackgroundColor = new BaseColor(240, 240, 240);
                    celda9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda9);


                    ////Poniendo datos en la la tabla
                    //List<encuesta_usuariosCLS> listaUser = (List<encuesta_usuariosCLS>)Session["ListaUser"];
                    int nroregistros = listaEmpleado.Count();
                    for (int i = 0; i < nroregistros; i++)
                    {
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                        if (listaEmpleado[i].perfil_desc != null)
                        {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font)));
                        }
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].fecho_ingreso.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].direccion.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].telefono.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        if (listaEmpleado[i].celular != null)
                        {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].celular.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        }
                        if (listaEmpleado[i].observaciones!=null) {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].observaciones.ToString(), font)));
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font)));
                        }
                        
                        //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].empleado_tipopersonal.ToString(), font)));
                        //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].usua_presento, font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;


                    }
                    ////Agregando la tabla al documento
                    doc.Add(tabla);
                    doc.Close();
                    buffer = stream.ToArray();
                    var contentLength = buffer.Length;

                    var statuscode = HttpStatusCode.OK;
                    response = Request.CreateResponse(statuscode);
                    response.Content = new StreamContent(new MemoryStream(buffer));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentLength = contentLength;
                    ContentDispositionHeaderValue contentDisposition = null;
                    if (ContentDispositionHeaderValue.TryParse("inline; filename=Reporte.pdf", out contentDisposition))
                    {
                        response.Content.Headers.ContentDisposition = contentDisposition;
                    }
                    else
                    {
                        //var statuscode = HttpStatusCode.NotFound;
                        // var message = String.Format("Unable to find file. file \"{0}\" may not exist.");
                        // var responseData = responseDataFactory.CreateWithOnlyMetadata(statuscode, message);
                        response = Request.CreateResponse((HttpStatusCode.NotFound));
                    }
                }


                return response;


            }


        }



        [Route("api/ReporteSol")]
        [HttpGet]
        public HttpResponseMessage ReporteSol(
             String plaza_descrip,
             String categoria,
             String funcion,
             String adscripcion,
             String pad_nombre,
             String pad_adscripcion,
             String pad_categoria,
             String pad_funcion,
             String pad_situacion,
             String pad_permanencia,
             String pad_f_ingreso,
             String pad_permisos,
             String pad_f_antig,
             String pad_n_insaluble,
             String pad_adscrip_base,
             String pad_catego_base,
             String pad_funcion_base,
             String pad_situacion_base,
             String pad_num_contacto,
             String pad_observaciones
         )
        {

            Console.WriteLine(plaza_descrip);
            String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
            iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
            iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 40, 40, 42, 35);
            byte[] buffer;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                //image1.ScalePercent(50f);
                image1.ScaleAbsoluteWidth(70);
                image1.ScaleAbsoluteHeight(60);
                image1.Alignment = Element.ALIGN_CENTER;
                doc.Add(image1);
                Paragraph espacio = new Paragraph(" ");
                doc.Add(espacio);

                Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                title1.Alignment = Element.ALIGN_CENTER;
                doc.Add(title1);
                doc.Add(espacio);

                Paragraph title = new Paragraph("Solicitud de Plaza", font1);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(espacio);

                Paragraph title3 = new Paragraph("         Plaza Solicitada", font1);
                title3.Alignment = Element.ALIGN_LEFT;
                doc.Add(title3);
                doc.Add(espacio);

                ////Creando la tabla
                PdfPTable tabla = new PdfPTable(2);
                tabla.WidthPercentage = 80f;
                ////Asignando los anchos de las columnas
                float[] valores = new float[2] { 30, 40 };
                tabla.SetWidths(valores);

                ////Creando celdas agregando contenido
                PdfPCell celda1 = new PdfPCell(new Phrase("Plaza:", font));
                celda1.BackgroundColor = new BaseColor(240, 240, 240);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase(plaza_descrip, font));
                celda2.BackgroundColor = new BaseColor(240, 240, 240);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Categoria:", font));
                celda3.BackgroundColor = new BaseColor(240, 240, 240);
                celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda3);

                PdfPCell celda4 = new PdfPCell(new Phrase(categoria, font));
                celda4.BackgroundColor = new BaseColor(240, 240, 240);
                celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda4);

                PdfPCell celda5 = new PdfPCell(new Phrase("Función:", font));
                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda5);

                PdfPCell celda6 = new PdfPCell(new Phrase(funcion, font));
                celda6.BackgroundColor = new BaseColor(240, 240, 240);
                celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda6);

                PdfPCell celda7 = new PdfPCell(new Phrase("Adscripción:", font));
                celda7.BackgroundColor = new BaseColor(240, 240, 240);
                celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda7);

                PdfPCell celda8 = new PdfPCell(new Phrase(adscripcion, font));
                celda8.BackgroundColor = new BaseColor(240, 240, 240);
                celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla.AddCell(celda8);
                ////Agregando la tabla al documento
                doc.Add(tabla);
                doc.Add(espacio);

                ////tabla solicitud
                Paragraph title4 = new Paragraph("         Solicitud de plaza", font1);
                title4.Alignment = Element.ALIGN_LEFT;
                doc.Add(title4);
                doc.Add(espacio);

                ////Creando la tabla
                PdfPTable tabla2 = new PdfPTable(2);
                tabla2.WidthPercentage = 80f;
                ////Asignando los anchos de las columnas
                //float[] valores = new float[2] { 30, 40 };
                tabla2.SetWidths(valores);
             
                       ////Creando celdas agregando contenido
                PdfPCell c1 = new PdfPCell(new Phrase("Nombre:", font));
                c1.BackgroundColor = new BaseColor(240, 240, 240);
                c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c1);

                PdfPCell c2 = new PdfPCell(new Phrase(pad_nombre, font));
                c2.BackgroundColor = new BaseColor(240, 240, 240);
                c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c2);

                PdfPCell c3 = new PdfPCell(new Phrase("Adscripción:", font));
                c3.BackgroundColor = new BaseColor(240, 240, 240);
                c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c3);

                PdfPCell c4 = new PdfPCell(new Phrase(pad_adscripcion, font));
                c4.BackgroundColor = new BaseColor(240, 240, 240);
                c4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c4);

                PdfPCell c5 = new PdfPCell(new Phrase("Categoria:", font));
                c5.BackgroundColor = new BaseColor(240, 240, 240);
                c5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c5);

                PdfPCell c6 = new PdfPCell(new Phrase(pad_categoria, font));
                c6.BackgroundColor = new BaseColor(240, 240, 240);
                c6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c6);

                PdfPCell c7 = new PdfPCell(new Phrase("Función:", font));
                c7.BackgroundColor = new BaseColor(240, 240, 240);
                c7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c7);

                PdfPCell c8 = new PdfPCell(new Phrase(pad_funcion, font));
                c8.BackgroundColor = new BaseColor(240, 240, 240);
                c8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c8);

                PdfPCell c9 = new PdfPCell(new Phrase("Situación:", font));
                c9.BackgroundColor = new BaseColor(240, 240, 240);
                c9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c9);

                PdfPCell c10 = new PdfPCell(new Phrase(pad_situacion, font));
                c10.BackgroundColor = new BaseColor(240, 240, 240);
                c10.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c10);

                PdfPCell c11 = new PdfPCell(new Phrase("Permanencia:", font));
                c11.BackgroundColor = new BaseColor(240, 240, 240);
                c11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c11);

                PdfPCell c12 = new PdfPCell(new Phrase(pad_permanencia, font));
                c12.BackgroundColor = new BaseColor(240, 240, 240);
                c12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c12);

                PdfPCell c13 = new PdfPCell(new Phrase("Fecha de ingreso:", font));
                c13.BackgroundColor = new BaseColor(240, 240, 240);
                c13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c13);

                PdfPCell c14 = new PdfPCell(new Phrase(pad_f_ingreso, font));
                c14.BackgroundColor = new BaseColor(240, 240, 240);
                c14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c14);

                PdfPCell c15 = new PdfPCell(new Phrase("Permisos:", font));
                c15.BackgroundColor = new BaseColor(240, 240, 240);
                c15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c15);

                PdfPCell c16 = new PdfPCell(new Phrase(pad_permisos, font));
                c16.BackgroundColor = new BaseColor(240, 240, 240);
                c16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c16);

                PdfPCell c17 = new PdfPCell(new Phrase("Antigüedad:", font));
                c17.BackgroundColor = new BaseColor(240, 240, 240);
                c17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c17);

                PdfPCell c18 = new PdfPCell(new Phrase(pad_f_antig, font));
                c18.BackgroundColor = new BaseColor(240, 240, 240);
                c18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c18);

                PdfPCell c19 = new PdfPCell(new Phrase("Insaluble:", font));
                c19.BackgroundColor = new BaseColor(240, 240, 240);
                c19.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c19);

                PdfPCell c20 = new PdfPCell(new Phrase(pad_n_insaluble, font));
                c20.BackgroundColor = new BaseColor(240, 240, 240);
                c20.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c20);

                PdfPCell c21 = new PdfPCell(new Phrase("Adscripción Base:", font));
                c21.BackgroundColor = new BaseColor(240, 240, 240);
                c21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c21);

                PdfPCell c22 = new PdfPCell(new Phrase(pad_adscrip_base, font));
                c22.BackgroundColor = new BaseColor(240, 240, 240);
                c22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c22);

                PdfPCell c23 = new PdfPCell(new Phrase("Categoria Base:", font));
                c23.BackgroundColor = new BaseColor(240, 240, 240);
                c23.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c23);

                PdfPCell c24 = new PdfPCell(new Phrase(pad_catego_base, font));
                c24.BackgroundColor = new BaseColor(240, 240, 240);
                c24.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c24);

                PdfPCell c25 = new PdfPCell(new Phrase("Función Base:", font));
                c25.BackgroundColor = new BaseColor(240, 240, 240);
                c25.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c25);

                PdfPCell c26 = new PdfPCell(new Phrase(pad_funcion_base, font));
                c26.BackgroundColor = new BaseColor(240, 240, 240);
                c26.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c26);

                PdfPCell c27 = new PdfPCell(new Phrase("Situación Base:", font));
                c27.BackgroundColor = new BaseColor(240, 240, 240);
                c27.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c27);

                PdfPCell c28 = new PdfPCell(new Phrase(pad_situacion_base, font));
                c28.BackgroundColor = new BaseColor(240, 240, 240);
                c28.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c28);

                PdfPCell c29 = new PdfPCell(new Phrase("Numero de contacto:", font));
                c29.BackgroundColor = new BaseColor(240, 240, 240);
                c29.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c29);

                PdfPCell c30 = new PdfPCell(new Phrase(pad_num_contacto, font));
                c30.BackgroundColor = new BaseColor(240, 240, 240);
                c30.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c30);

                PdfPCell c31 = new PdfPCell(new Phrase("Observaciones:", font));
                c31.BackgroundColor = new BaseColor(240, 240, 240);
                c31.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c31);

                PdfPCell c32 = new PdfPCell(new Phrase(pad_observaciones, font));
                c32.BackgroundColor = new BaseColor(240, 240, 240);
                c32.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tabla2.AddCell(c32);

                //Agregando tabla al documento
                doc.Add(tabla2);
                doc.Add(espacio);

                Paragraph firma = new Paragraph("Firma:", font2);
                firma.Alignment = Element.ALIGN_CENTER;
                doc.Add(firma);
                doc.Add(espacio);
                doc.Add(espacio);

                Paragraph linea = new Paragraph(" _______________________________________________________________", font2);
                linea.Alignment = Element.ALIGN_CENTER;
                doc.Add(linea);
                Paragraph nom = new Paragraph(pad_nombre, font2);
                nom.Alignment = Element.ALIGN_CENTER;
                doc.Add(nom);
                doc.Close();
                buffer = stream.ToArray();
                var contentLength = buffer.Length;

                var statuscode = HttpStatusCode.OK;
                response = Request.CreateResponse(statuscode);
                response.Content = new StreamContent(new MemoryStream(buffer));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentLength = contentLength;
                ContentDispositionHeaderValue contentDisposition = null;
                if (ContentDispositionHeaderValue.TryParse("inline; filename=Reporte.pdf", out contentDisposition))
                {
                    response.Content.Headers.ContentDisposition = contentDisposition;
                }
                else
                {
                    //var statuscode = HttpStatusCode.NotFound;
                    // var message = String.Format("Unable to find file. file \"{0}\" may not exist.");
                    // var responseData = responseDataFactory.CreateWithOnlyMetadata(statuscode, message);
                    response = Request.CreateResponse((HttpStatusCode.NotFound));
                }
            }


            return response;



        }
    }
}
