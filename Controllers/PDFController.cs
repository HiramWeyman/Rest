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
using System.Collections;
using System.Globalization;
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
                                 join per in db.Perfil
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
        public HttpResponseMessage ReporteSol(int id
             //String plaza_descrip,
             //String categoria,
             //String funcion,
             //String adscripcion,
             //String pad_nombre,
             //String pad_adscripcion,
             //String pad_categoria,
             //String pad_funcion,
             //String pad_situacion,
             //String pad_permanencia,
             //String pad_f_ingreso,
             //String pad_permisos,
             //String pad_f_antig,
             //String pad_n_insaluble,
             //String pad_adscrip_base,
             //String pad_catego_base,
             //String pad_funcion_base,
             //String pad_situacion_base,
             //String pad_num_contacto,
             //String pad_observaciones
         )
        {

            List<ConcursoPlazasCLS> listaEmpleado = null;
           
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                string query = "  select  * from  [steujedo_sindicato].[steujedo_sindicato].[Concurso_Plazas],[steujedo_sindicato].[steujedo_sindicato].Cat_Plazas where pad_plaza_id= catp_id and pad_id=" + id;
                listaEmpleado = db.Database.SqlQuery<ConcursoPlazasCLS>(query).ToList();


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
                    int noregistros = listaEmpleado.Count();

                    for (int i = 0; i < noregistros; i++) {
                        ////Creando celdas agregando contenido
                        PdfPCell celda1 = new PdfPCell(new Phrase("Plaza:", font));
                        celda1.BackgroundColor = new BaseColor(240, 240, 240);
                        celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda1);

                        PdfPCell celda2 = new PdfPCell(new Phrase(listaEmpleado[i].catp_descrip, font));
                        celda2.BackgroundColor = new BaseColor(240, 240, 240);
                        celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda2);

                        PdfPCell celda3 = new PdfPCell(new Phrase("Categoria:", font));
                        celda3.BackgroundColor = new BaseColor(240, 240, 240);
                        celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda3);

                        PdfPCell celda4 = new PdfPCell(new Phrase(listaEmpleado[i].catp_categoria, font));
                        celda4.BackgroundColor = new BaseColor(240, 240, 240);
                        celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda4);

                        PdfPCell celda5 = new PdfPCell(new Phrase("Función:", font));
                        celda5.BackgroundColor = new BaseColor(240, 240, 240);
                        celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda5);

                        PdfPCell celda6 = new PdfPCell(new Phrase(listaEmpleado[i].catp_funcion, font));
                        celda6.BackgroundColor = new BaseColor(240, 240, 240);
                        celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda6);

                        PdfPCell celda7 = new PdfPCell(new Phrase("Adscripción:", font));
                        celda7.BackgroundColor = new BaseColor(240, 240, 240);
                        celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla.AddCell(celda7);

                        PdfPCell celda8 = new PdfPCell(new Phrase(listaEmpleado[i].catp_adscripcion, font));
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

                        PdfPCell c2 = new PdfPCell(new Phrase(listaEmpleado[i].pad_nombre, font));
                        c2.BackgroundColor = new BaseColor(240, 240, 240);
                        c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c2);

                        PdfPCell c3 = new PdfPCell(new Phrase("Adscripción:", font));
                        c3.BackgroundColor = new BaseColor(240, 240, 240);
                        c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c3);

                        PdfPCell c4 = new PdfPCell(new Phrase(listaEmpleado[i].pad_adscripcion, font));
                        c4.BackgroundColor = new BaseColor(240, 240, 240);
                        c4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c4);

                        PdfPCell c5 = new PdfPCell(new Phrase("Categoria:", font));
                        c5.BackgroundColor = new BaseColor(240, 240, 240);
                        c5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c5);

                        PdfPCell c6 = new PdfPCell(new Phrase(listaEmpleado[i].pad_categoria, font));
                        c6.BackgroundColor = new BaseColor(240, 240, 240);
                        c6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c6);

                        PdfPCell c7 = new PdfPCell(new Phrase("Función:", font));
                        c7.BackgroundColor = new BaseColor(240, 240, 240);
                        c7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c7);

                        PdfPCell c8 = new PdfPCell(new Phrase(listaEmpleado[i].pad_funcion, font));
                        c8.BackgroundColor = new BaseColor(240, 240, 240);
                        c8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c8);

                        PdfPCell c9 = new PdfPCell(new Phrase("Situación:", font));
                        c9.BackgroundColor = new BaseColor(240, 240, 240);
                        c9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c9);

                        PdfPCell c10 = new PdfPCell(new Phrase(listaEmpleado[i].pad_situacion, font));
                        c10.BackgroundColor = new BaseColor(240, 240, 240);
                        c10.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c10);

                        PdfPCell c11 = new PdfPCell(new Phrase("Permanencia:", font));
                        c11.BackgroundColor = new BaseColor(240, 240, 240);
                        c11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c11);

                        PdfPCell c12 = new PdfPCell(new Phrase(listaEmpleado[i].pad_permanencia, font));
                        c12.BackgroundColor = new BaseColor(240, 240, 240);
                        c12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c12);

                        PdfPCell c13 = new PdfPCell(new Phrase("Fecha de ingreso:", font));
                        c13.BackgroundColor = new BaseColor(240, 240, 240);
                        c13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c13);

                        PdfPCell c14 = new PdfPCell(new Phrase(listaEmpleado[i].pad_f_ingreso, font));
                        c14.BackgroundColor = new BaseColor(240, 240, 240);
                        c14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c14);

                        PdfPCell c15 = new PdfPCell(new Phrase("Permisos:", font));
                        c15.BackgroundColor = new BaseColor(240, 240, 240);
                        c15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c15);

                        PdfPCell c16 = new PdfPCell(new Phrase(listaEmpleado[i].pad_permisos, font));
                        c16.BackgroundColor = new BaseColor(240, 240, 240);
                        c16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c16);

                        PdfPCell c17 = new PdfPCell(new Phrase("Antigüedad:", font));
                        c17.BackgroundColor = new BaseColor(240, 240, 240);
                        c17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c17);

                        PdfPCell c18 = new PdfPCell(new Phrase(listaEmpleado[i].pad_f_antig, font));
                        c18.BackgroundColor = new BaseColor(240, 240, 240);
                        c18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c18);

                        PdfPCell c19 = new PdfPCell(new Phrase("Insaluble:", font));
                        c19.BackgroundColor = new BaseColor(240, 240, 240);
                        c19.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c19);

                        PdfPCell c20 = new PdfPCell(new Phrase(listaEmpleado[i].pad_n_insaluble, font));
                        c20.BackgroundColor = new BaseColor(240, 240, 240);
                        c20.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c20);

                        PdfPCell c21 = new PdfPCell(new Phrase("Adscripción Base:", font));
                        c21.BackgroundColor = new BaseColor(240, 240, 240);
                        c21.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c21);

                        PdfPCell c22 = new PdfPCell(new Phrase(listaEmpleado[i].pad_adscrip_base, font));
                        c22.BackgroundColor = new BaseColor(240, 240, 240);
                        c22.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c22);

                        PdfPCell c23 = new PdfPCell(new Phrase("Categoria Base:", font));
                        c23.BackgroundColor = new BaseColor(240, 240, 240);
                        c23.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c23);

                        PdfPCell c24 = new PdfPCell(new Phrase(listaEmpleado[i].pad_catego_base, font));
                        c24.BackgroundColor = new BaseColor(240, 240, 240);
                        c24.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c24);

                        PdfPCell c25 = new PdfPCell(new Phrase("Función Base:", font));
                        c25.BackgroundColor = new BaseColor(240, 240, 240);
                        c25.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c25);

                        PdfPCell c26 = new PdfPCell(new Phrase(listaEmpleado[i].pad_funcion_base, font));
                        c26.BackgroundColor = new BaseColor(240, 240, 240);
                        c26.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c26);

                        PdfPCell c27 = new PdfPCell(new Phrase("Situación Base:", font));
                        c27.BackgroundColor = new BaseColor(240, 240, 240);
                        c27.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c27);

                        PdfPCell c28 = new PdfPCell(new Phrase(listaEmpleado[i].pad_situacion_base, font));
                        c28.BackgroundColor = new BaseColor(240, 240, 240);
                        c28.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c28);

                        PdfPCell c29 = new PdfPCell(new Phrase("Numero de contacto:", font));
                        c29.BackgroundColor = new BaseColor(240, 240, 240);
                        c29.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c29);

                        PdfPCell c30 = new PdfPCell(new Phrase(listaEmpleado[i].pad_num_contacto, font));
                        c30.BackgroundColor = new BaseColor(240, 240, 240);
                        c30.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c30);

                        PdfPCell c31 = new PdfPCell(new Phrase("Observaciones:", font));
                        c31.BackgroundColor = new BaseColor(240, 240, 240);
                        c31.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tabla2.AddCell(c31);

                        PdfPCell c32 = new PdfPCell(new Phrase(listaEmpleado[i].pad_observaciones, font));
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

                        Paragraph nom = new Paragraph(listaEmpleado[i].pad_nombre, font2);
                        nom.Alignment = Element.ALIGN_CENTER;
                        doc.Add(nom);
                        doc.Add(espacio);
                        doc.Add(espacio);

                        //string hora = listaEmpleado[i].pad_string_fec.Substring(14);
                        //string fecha = listaEmpleado[i].pad_string_fec.Substring(0, 10);
                        if (listaEmpleado[i].pad_string_fec != null) {

                            //string format = "dd/MM/yyyy HH:mm:ss";
                            //DateTime dt = DateTime.ParseExact(pad_string_fec, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                           // string dt = Convert.ToDateTime(listaEmpleado[i].pad_string_fec).ToString(format);

                            //string format = "dd/MM/yyyy HH:mm:ss";
                            //DateTime dt = DateTime.ParseExact(listaEmpleado[i].pad_string_fec, format, CultureInfo.InvariantCulture);
                            //Console.WriteLine(s.ToString(format);
                            Paragraph fec = new Paragraph("Fecha: "+ listaEmpleado[i].pad_string_fec, font2);
                            linea.Alignment = Element.ALIGN_LEFT;
                            doc.Add(fec);
                            //Paragraph fecha = new Paragraph(listaEmpleado[i].pad_string_fec.ToLongDateString(), font2);
                            ////nom.Alignment = Element.ALIGN_CENTER;
                            //doc.Add(fecha);
                        }
                     


                    }
                    
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

                    return response;
                }

            }


        }


        [Route("api/Asistenciaeventuales")]
        [HttpGet]
        public HttpResponseMessage ReporteAsistencia( String fecha1,String fecha2,string trabajador, int tipo)
        {
            String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
            iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
            iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 40, 40, 42, 35);
            byte[] buffer;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);

            string mes_ = "";
            string dia = "";
            string mes = "";
            string anio = "";



            List<UsuariosCLS> listaEmpleado = null;
            if (tipo!=0) {
                switch (tipo) {
                    case 1:
                        string[] val_fec = fecha1.Split('-');
                        mes_ = "";
                        dia = val_fec[2];
                        mes = val_fec[1];
                        anio = val_fec[0];

                        switch (mes)
                        {
                            case "01":
                                mes_ = "Enero";
                                break;
                            case "02":
                                mes_ = "Febrero";
                                break;
                            case "03":
                                mes_ = "Marzo";
                                break;
                            case "04":
                                mes_ = "Abril";
                                break;
                            case "05":
                                mes_ = "Mayo";
                                break;
                            case "06":
                                mes_ = "Junio";
                                break;
                            case "07":
                                mes_ = "Julio";
                                break;
                            case "08":
                                mes_ = "Agosto";
                                break;
                            case "09":
                                mes_ = "Septiembre";
                                break;
                            case "10":
                                mes_ = "Octubre";
                                break;
                            case "11":
                                mes_ = "Noviembre";
                                break;
                            case "12":
                                mes_ = "Diciembre";
                                break;

                        }

                        using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            //convert(char, fecha, 103) as fecha

                            string query = "   select   matricula,nombre_completo, perfil_desc,actividad_desc,entrada from Usuarios,Lista_Asistencia,Perfil,Actividades   where Usuarios.id = Lista_Asistencia.usuario_id  and Usuarios.perfil_id = Perfil.id  and Usuarios.act_id = Actividades.id and Lista_Asistencia.fecha ='"+fecha1+"' order by entrada";
                            listaEmpleado = db.Database.SqlQuery<UsuariosCLS>(query).ToList();
                       

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

                                Paragraph title = new Paragraph("Lista de Asistencia de Trabajadores Eventuales al día " + dia+" de "+mes_+" de "+anio, font1);
                                title.Alignment = Element.ALIGN_CENTER;
                                doc.Add(title);
                                doc.Add(espacio);

                

                                ////Creando la tabla
                                PdfPTable tabla = new PdfPTable(5);
                                tabla.WidthPercentage = 80f;
                                ////Asignando los anchos de las columnas
                                float[] valores = new float[5] { 13, 42 ,24,24,31};
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

                                PdfPCell celda5 = new PdfPCell(new Phrase("Entrada", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                int nroregistros = listaEmpleado.Count();
                                for (int i = 0; i < nroregistros; i++)
                                {
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                                    if (listaEmpleado[i].perfil_desc != null)
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                                    }
                                    else
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase("", font)));
                                    }
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                }

                                doc.Add(tabla);
                                doc.Add(espacio);

                              
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

                        }
                        break;

                    case 2:

                        string[] val_fec2 = fecha1.Split('-');
                        string mes_1_ = "";
                        string dia_1 = val_fec2[2];
                        string mes_1 = val_fec2[1];
                        string anio_1 = val_fec2[0];

                        string[] val_fec3 = fecha2.Split('-');
                        string mes_2_ = "";
                        string dia_2 = val_fec3[2];
                        string mes_2 = val_fec3[1];
                        string anio_2 = val_fec3[0];

                        switch (mes_1)
                        {
                            case "01":
                                mes_1_ = "Enero";
                                break;
                            case "02":
                                mes_1_ = "Febrero";
                                break;
                            case "03":
                                mes_1_ = "Marzo";
                                break;
                            case "04":
                                mes_1_ = "Abril";
                                break;
                            case "05":
                                mes_1_ = "Mayo";
                                break;
                            case "06":
                                mes_1_ = "Junio";
                                break;
                            case "07":
                                mes_1_ = "Julio";
                                break;
                            case "08":
                                mes_1_ = "Agosto";
                                break;
                            case "09":
                                mes_1_ = "Septiembre";
                                break;
                            case "10":
                                mes_1_ = "Octubre";
                                break;
                            case "11":
                                mes_1_ = "Noviembre";
                                break;
                            case "12":
                                mes_1_ = "Diciembre";
                                break;

                        }

                        switch (mes_2)
                        {
                            case "01":
                                mes_2_ = "Enero";
                                break;
                            case "02":
                                mes_2_ = "Febrero";
                                break;
                            case "03":
                                mes_2_ = "Marzo";
                                break;
                            case "04":
                                mes_2_ = "Abril";
                                break;
                            case "05":
                                mes_2_ = "Mayo";
                                break;
                            case "06":
                                mes_2_ = "Junio";
                                break;
                            case "07":
                                mes_2_ = "Julio";
                                break;
                            case "08":
                                mes_2_ = "Agosto";
                                break;
                            case "09":
                                mes_2_ = "Septiembre";
                                break;
                            case "10":
                                mes_2_ = "Octubre";
                                break;
                            case "11":
                                mes_2_ = "Noviembre";
                                break;
                            case "12":
                                mes_2_ = "Diciembre";
                                break;

                        }

                        using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            //convert(char, fecha, 103) as fecha

                            string query = "   select   matricula,nombre_completo, perfil_desc,actividad_desc,entrada from Usuarios,Lista_Asistencia,Perfil,Actividades   where Usuarios.id = Lista_Asistencia.usuario_id  and Usuarios.perfil_id = Perfil.id  and Usuarios.act_id = Actividades.id and Lista_Asistencia.fecha between '" + fecha1 + "' and '"+fecha2+ "' order by nombre_completo ";
                            listaEmpleado = db.Database.SqlQuery<UsuariosCLS>(query).ToList();


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

                                Paragraph title = new Paragraph("Lista de Asistencia de Trabajadores Eventuales del día " + dia_1 + " de " + mes_1_ + " de " + anio_1 + " al día " + dia_2 + " de " + mes_2_ + " de " + anio_2, font1);
                                title.Alignment = Element.ALIGN_CENTER;
                                doc.Add(title);
                                doc.Add(espacio);



                                ////Creando la tabla
                                PdfPTable tabla = new PdfPTable(5);
                                tabla.WidthPercentage = 80f;
                                ////Asignando los anchos de las columnas
                                float[] valores = new float[5] { 13, 42, 24, 24, 31 };
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

                                PdfPCell celda5 = new PdfPCell(new Phrase("Entrada", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                int nroregistros = listaEmpleado.Count();
                                for (int i = 0; i < nroregistros; i++)
                                {
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                                    if (listaEmpleado[i].perfil_desc != null)
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                                    }
                                    else
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase("", font)));
                                    }
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                }

                                doc.Add(tabla);
                                doc.Add(espacio);


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

                        }
                        break;

                    case 3:
                        string[] val_fe = fecha1.Split('-');
                        mes_ = "";
                        dia = val_fe[2];
                        mes = val_fe[1];
                        anio = val_fe[0];
                        int mat = Int32.Parse(trabajador);
                        switch (mes)
                        {
                            case "01":
                                mes_ = "Enero";
                                break;
                            case "02":
                                mes_ = "Febrero";
                                break;
                            case "03":
                                mes_ = "Marzo";
                                break;
                            case "04":
                                mes_ = "Abril";
                                break;
                            case "05":
                                mes_ = "Mayo";
                                break;
                            case "06":
                                mes_ = "Junio";
                                break;
                            case "07":
                                mes_ = "Julio";
                                break;
                            case "08":
                                mes_ = "Agosto";
                                break;
                            case "09":
                                mes_ = "Septiembre";
                                break;
                            case "10":
                                mes_ = "Octubre";
                                break;
                            case "11":
                                mes_ = "Noviembre";
                                break;
                            case "12":
                                mes_ = "Diciembre";
                                break;

                        }

                        using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            //convert(char, fecha, 103) as fecha

                            string query = "   select   matricula,nombre_completo, perfil_desc,actividad_desc,entrada from Usuarios,Lista_Asistencia,Perfil,Actividades   where Usuarios.id = Lista_Asistencia.usuario_id  and Usuarios.perfil_id = Perfil.id  and Usuarios.act_id = Actividades.id and Lista_Asistencia.fecha ='" + fecha1 + "' and Usuarios.matricula='"+mat+"' order by entrada";
                            listaEmpleado = db.Database.SqlQuery<UsuariosCLS>(query).ToList();


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

                                Paragraph title = new Paragraph("Lista de Asistencia de Trabajadores Eventuales al día " + dia + " de " + mes_ + " de " + anio, font1);
                                title.Alignment = Element.ALIGN_CENTER;
                                doc.Add(title);
                                doc.Add(espacio);



                                ////Creando la tabla
                                PdfPTable tabla = new PdfPTable(5);
                                tabla.WidthPercentage = 80f;
                                ////Asignando los anchos de las columnas
                                float[] valores = new float[5] { 13, 42, 24, 24, 31 };
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

                                PdfPCell celda5 = new PdfPCell(new Phrase("Entrada", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                int nroregistros = listaEmpleado.Count();
                                for (int i = 0; i < nroregistros; i++)
                                {
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                                    if (listaEmpleado[i].perfil_desc != null)
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                                    }
                                    else
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase("", font)));
                                    }
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                }

                                doc.Add(tabla);
                                doc.Add(espacio);


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

                        }
                        break;

                    case 4:

                        string[] val_fe4 = fecha1.Split('-');
                      //  string mes4_ = "";
                        string dia4_ = val_fe4[2];
                        string mesx_ = val_fe4[1];
                        string anio4_ = val_fe4[0];

                        string[] val_fe4_2 = fecha2.Split('-');
                        string mesy = "";
                        string dia_2_ = val_fe4_2[2];
                        string mes_2x = val_fe4_2[1];
                        string anio_2_= val_fe4_2[0];

                        int mat2 = Int32.Parse(trabajador);
                        switch (mesx_)
                        {
                            case "01":
                                mesx_ = "Enero";
                                break;
                            case "02":
                                mesx_ = "Febrero";
                                break;
                            case "03":
                                mesx_ = "Marzo";
                                break;
                            case "04":
                                mesx_ = "Abril";
                                break;
                            case "05":
                                mesx_ = "Mayo";
                                break;
                            case "06":
                                mesx_ = "Junio";
                                break;
                            case "07":
                                mesx_ = "Julio";
                                break;
                            case "08":
                                mesx_ = "Agosto";
                                break;
                            case "09":
                                mesx_ = "Septiembre";
                                break;
                            case "10":
                                mesx_ = "Octubre";
                                break;
                            case "11":
                                mesx_ = "Noviembre";
                                break;
                            case "12":
                                mesx_ = "Diciembre";
                                break;

                        }

                        switch (mes_2x)
                        {
                            case "01":
                                mesy = "Enero";
                                break;
                            case "02":
                                mesy = "Febrero";
                                break;
                            case "03":
                                mesy = "Marzo";
                                break;
                            case "04":
                                mesy = "Abril";
                                break;
                            case "05":
                                mesy = "Mayo";
                                break;
                            case "06":
                                mesy = "Junio";
                                break;
                            case "07":
                                mesy = "Julio";
                                break;
                            case "08":
                                mesy = "Agosto";
                                break;
                            case "09":
                                mesy = "Septiembre";
                                break;
                            case "10":
                                mesy = "Octubre";
                                break;
                            case "11":
                                mesy = "Noviembre";
                                break;
                            case "12":
                                mesy = "Diciembre";
                                break;

                        }

                        using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            //convert(char, fecha, 103) as fecha

                            string query = "   select   matricula,nombre_completo, perfil_desc,actividad_desc,entrada from Usuarios,Lista_Asistencia,Perfil,Actividades   where Usuarios.id = Lista_Asistencia.usuario_id  and Usuarios.perfil_id = Perfil.id  and Usuarios.act_id = Actividades.id and Lista_Asistencia.fecha between '" + fecha1 + "' and '" + fecha2 + "' and Usuarios.matricula='"+mat2+"' order by nombre_completo ";
                            listaEmpleado = db.Database.SqlQuery<UsuariosCLS>(query).ToList();


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

                                Paragraph title = new Paragraph("Lista de Asistencia de Trabajadores Eventuales del día " + dia4_ + " de " + mesx_ + " de " + anio4_ + " al día " + dia_2_ + " de " + mesy + " de " + anio_2_, font1);
                                title.Alignment = Element.ALIGN_CENTER;
                                doc.Add(title);
                                doc.Add(espacio);



                                ////Creando la tabla
                                PdfPTable tabla = new PdfPTable(5);
                                tabla.WidthPercentage = 80f;
                                ////Asignando los anchos de las columnas
                                float[] valores = new float[5] { 13, 42, 24, 24, 31 };
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

                                PdfPCell celda5 = new PdfPCell(new Phrase("Entrada", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                int nroregistros = listaEmpleado.Count();
                                for (int i = 0; i < nroregistros; i++)
                                {
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                                    if (listaEmpleado[i].perfil_desc != null)
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                                    }
                                    else
                                    {
                                        tabla.AddCell(new PdfPCell(new Phrase("", font)));
                                    }
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                                    tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                }

                                doc.Add(tabla);
                                doc.Add(espacio);


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

                        }
                        break;
                }
            }
           
            return response;

        }

        [Route("api/ReporPrestamos")]
        [HttpGet]
        public HttpResponseMessage ReporPrestamos(int id, int tipo)
        {
            String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
            iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
            iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 40, 40, 42, 35);
            byte[] buffer;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);

     

            List<CajaAhorroCLS> listaEmpleado = null;
            List<RevolventeCLS> revolvente = null;
            switch (tipo) {

                case 1:
                    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        //convert(char, fecha, 103) as fecha

                        string query = "  SELECT pre_id,pre_nombre,pre_matricula,pre_adscripcioon,pre_tarjeta_cuenta,pre_banco,pre_telefono,pre_cantidad,pre_fecha,pre_tipo,pre_estatus  FROM steujedo_sindicato.steujedo_sindicato.Caja_Ahorro where pre_id=" + id;
                        listaEmpleado = db.Database.SqlQuery<CajaAhorroCLS>(query).ToList();


                        using (MemoryStream stream = new MemoryStream())
                        {
                            String nombre = "";
                            Double cant = 0.00;
                            String fecha = "";
                            PdfWriter.GetInstance(doc, stream);
                            doc.Open();

                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                            //image1.ScalePercent(50f);
                            image1.ScaleAbsoluteWidth(70);
                            image1.ScaleAbsoluteHeight(60);
                            image1.Alignment = Element.ALIGN_LEFT;
                            doc.Add(image1);
                            Paragraph espacio = new Paragraph(" ");
                            doc.Add(espacio);

                            Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                            title1.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title1);
                            doc.Add(espacio);

                            Paragraph title = new Paragraph("CAJA DE AHORRO", font1);
                            title.Alignment = Element.ALIGN_CENTER;
                            Paragraph title2 = new Paragraph("SOLICITUD DE PRESTAMO", font1);
                            title2.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title);
                            doc.Add(title2);
                            doc.Add(espacio);



                            ////Creando la tabla
                            PdfPTable tabla = new PdfPTable(2);
                            tabla.WidthPercentage = 80f;
                            ////Asignando los anchos de las columnas
                            float[] valores = new float[2] { 13, 40 };
                            tabla.SetWidths(valores);

                            ////Creando celdas agregando contenido
                            ///


                            int nroregistros = listaEmpleado.Count();
                            for (int i = 0; i < nroregistros; i++)
                            {
                                nombre = listaEmpleado[i].pre_nombre;
                                cant = Double.Parse(listaEmpleado[i].pre_cantidad.ToString());

                                PdfPCell celda17 = new PdfPCell(new Phrase("NUMERO DE SOLICITUD", font));
                                celda17.BackgroundColor = new BaseColor(240, 240, 240);
                                celda17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda17);

                                PdfPCell celda18 = new PdfPCell(new Phrase(listaEmpleado[i].pre_id.ToString(), font));
                                celda18.BackgroundColor = new BaseColor(240, 240, 240);
                                celda18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda18);

                                PdfPCell celda15 = new PdfPCell(new Phrase("ESTATUS", font));
                                celda15.BackgroundColor = new BaseColor(240, 240, 240);
                                celda15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda15);

                                PdfPCell celda16 = new PdfPCell(new Phrase(listaEmpleado[i].pre_estatus.ToString(), font));
                                celda16.BackgroundColor = new BaseColor(240, 240, 240);
                                celda16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda16);

                                PdfPCell celda1 = new PdfPCell(new Phrase("NOMBRE", font));
                                celda1.BackgroundColor = new BaseColor(240, 240, 240);
                                celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda1);

                                PdfPCell celda2 = new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font));
                                celda2.BackgroundColor = new BaseColor(240, 240, 240);
                                celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda2);



                                PdfPCell celda3 = new PdfPCell(new Phrase("MATRICULA", font));
                                celda3.BackgroundColor = new BaseColor(240, 240, 240);
                                celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda3);

                                PdfPCell celda4 = new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font));
                                celda4.BackgroundColor = new BaseColor(240, 240, 240);
                                celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda4);

                                PdfPCell celda5 = new PdfPCell(new Phrase("ADSCRIPCIÓN", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                PdfPCell celda6 = new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font));
                                celda6.BackgroundColor = new BaseColor(240, 240, 240);
                                celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda6);

                                PdfPCell celda7 = new PdfPCell(new Phrase("NO. CUENTA/TARJETA", font));
                                celda7.BackgroundColor = new BaseColor(240, 240, 240);
                                celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda7);

                                PdfPCell celda8 = new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font));
                                celda8.BackgroundColor = new BaseColor(240, 240, 240);
                                celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda8);

                                PdfPCell celda9 = new PdfPCell(new Phrase("BANCO", font));
                                celda9.BackgroundColor = new BaseColor(240, 240, 240);
                                celda9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda9);

                                PdfPCell celda10 = new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font));
                                celda10.BackgroundColor = new BaseColor(240, 240, 240);
                                celda10.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda10);

                                PdfPCell celda11 = new PdfPCell(new Phrase("TELEFONO", font));
                                celda11.BackgroundColor = new BaseColor(240, 240, 240);
                                celda11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda11);

                                PdfPCell celda12 = new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font));
                                celda12.BackgroundColor = new BaseColor(240, 240, 240);
                                celda12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda12);

                                PdfPCell celda13 = new PdfPCell(new Phrase("CANTIDAD", font));
                                celda13.BackgroundColor = new BaseColor(240, 240, 240);
                                celda13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda13);

                                PdfPCell celda14 = new PdfPCell(new Phrase("$" + cant, font));
                                celda14.BackgroundColor = new BaseColor(240, 240, 240);
                                celda14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda14);
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase("$"+cant, font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                fecha = listaEmpleado[i].pre_fecha.ToString("dd/MM/yyyy");
                                Console.WriteLine(fecha);
                            }

                            doc.Add(tabla);
                            doc.Add(espacio);
                            string[] fecprestamo = fecha.Split('/');
                            //  string mes4_ = "";
                            string diap = fecprestamo[0];
                            string mesp = fecprestamo[1];
                            string aniop = fecprestamo[2];
                            string valmes = "";
                            switch (mesp)
                            {
                                case "01":
                                    valmes = "ENERO";
                                    break;
                                case "02":
                                    valmes = "FEBRERO";
                                    break;
                                case "03":
                                    valmes = "MARZO";
                                    break;
                                case "04":
                                    valmes = "ABRIL";
                                    break;
                                case "05":
                                    valmes = "MAYO";
                                    break;
                                case "06":
                                    valmes = "JUNIO";
                                    break;
                                case "07":
                                    valmes = "JULIO";
                                    break;
                                case "08":
                                    valmes = "AGOSTO";
                                    break;
                                case "09":
                                    valmes = "SEPTIEMBRE";
                                    break;
                                case "10":
                                    valmes = "OCTUBRE";
                                    break;
                                case "11":
                                    valmes = "NOVIEMBRE";
                                    break;
                                case "12":
                                    valmes = "DICIEMBRE";
                                    break;

                            }

                            Paragraph fec = new Paragraph("DURANGO,DGO A "+ diap + " DE "+ valmes + " DE "+ aniop, font2);
                            fec.Alignment = Element.ALIGN_CENTER;
                            doc.Add(fec);
                            doc.Add(espacio);

                            Paragraph firma = new Paragraph("Firma:", font2);
                            firma.Alignment = Element.ALIGN_CENTER;
                            doc.Add(firma);
                            doc.Add(espacio);
                            doc.Add(espacio);

                            Paragraph linea = new Paragraph(" _______________________________________________________________", font2);
                            linea.Alignment = Element.ALIGN_CENTER;
                            doc.Add(linea);
                            Paragraph nom = new Paragraph(nombre, font2);
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

                    }

                    break;

                case 2:
                    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        //convert(char, fecha, 103) as fecha

                        string query = "  SELECT pre_id,pre_nombre,pre_matricula,pre_adscripcioon,pre_tarjeta_cuenta,pre_banco,pre_telefono,pre_cantidad,pre_fecha,pre_tipo,pre_estatus  FROM steujedo_sindicato.steujedo_sindicato.Caja_Ahorro where pre_id=" + id;
                        listaEmpleado = db.Database.SqlQuery<CajaAhorroCLS>(query).ToList();


                        using (MemoryStream stream = new MemoryStream())
                        {
                            String nombre = "";
                            Double cant = 0.00;
                            String fecha = "";
                            PdfWriter.GetInstance(doc, stream);
                            doc.Open();

                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                            //image1.ScalePercent(50f);
                            image1.ScaleAbsoluteWidth(70);
                            image1.ScaleAbsoluteHeight(60);
                            image1.Alignment = Element.ALIGN_LEFT;
                            doc.Add(image1);
                            Paragraph espacio = new Paragraph(" ");
                            doc.Add(espacio);

                            Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                            title1.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title1);
                            doc.Add(espacio);

                            Paragraph title = new Paragraph("CAJA DE AHORRO", font1);
                            title.Alignment = Element.ALIGN_CENTER;
                            Paragraph title2 = new Paragraph("SOLICITUD DE RETIRO DE AHORRO", font1);
                            title2.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title);
                            doc.Add(title2);
                            doc.Add(espacio);



                            ////Creando la tabla
                            PdfPTable tabla = new PdfPTable(2);
                            tabla.WidthPercentage = 80f;
                            ////Asignando los anchos de las columnas
                            float[] valores = new float[2] { 13, 40 };
                            tabla.SetWidths(valores);

                            ////Creando celdas agregando contenido
                            ///


                            int nroregistros = listaEmpleado.Count();
                            for (int i = 0; i < nroregistros; i++)
                            {
                                PdfPCell celda17 = new PdfPCell(new Phrase("NUMERO DE SOLICITUD", font));
                                celda17.BackgroundColor = new BaseColor(240, 240, 240);
                                celda17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda17);

                                PdfPCell celda18 = new PdfPCell(new Phrase(listaEmpleado[i].pre_id.ToString(), font));
                                celda18.BackgroundColor = new BaseColor(240, 240, 240);
                                celda18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda18);

                                nombre = listaEmpleado[i].pre_nombre;
                                cant = Double.Parse(listaEmpleado[i].pre_cantidad.ToString());
                                PdfPCell celda15 = new PdfPCell(new Phrase("ESTATUS", font));
                                celda15.BackgroundColor = new BaseColor(240, 240, 240);
                                celda15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda15);

                                PdfPCell celda16 = new PdfPCell(new Phrase(listaEmpleado[i].pre_estatus.ToString(), font));
                                celda16.BackgroundColor = new BaseColor(240, 240, 240);
                                celda16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda16);

                                PdfPCell celda1 = new PdfPCell(new Phrase("NOMBRE", font));
                                celda1.BackgroundColor = new BaseColor(240, 240, 240);
                                celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda1);

                                PdfPCell celda2 = new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font));
                                celda2.BackgroundColor = new BaseColor(240, 240, 240);
                                celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda2);



                                PdfPCell celda3 = new PdfPCell(new Phrase("MATRICULA", font));
                                celda3.BackgroundColor = new BaseColor(240, 240, 240);
                                celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda3);

                                PdfPCell celda4 = new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font));
                                celda4.BackgroundColor = new BaseColor(240, 240, 240);
                                celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda4);

                                PdfPCell celda5 = new PdfPCell(new Phrase("ADSCRIPCIÓN", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                PdfPCell celda6 = new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font));
                                celda6.BackgroundColor = new BaseColor(240, 240, 240);
                                celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda6);

                                PdfPCell celda7 = new PdfPCell(new Phrase("NO. CUENTA/TARJETA", font));
                                celda7.BackgroundColor = new BaseColor(240, 240, 240);
                                celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda7);

                                PdfPCell celda8 = new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font));
                                celda8.BackgroundColor = new BaseColor(240, 240, 240);
                                celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda8);

                                PdfPCell celda9 = new PdfPCell(new Phrase("BANCO", font));
                                celda9.BackgroundColor = new BaseColor(240, 240, 240);
                                celda9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda9);

                                PdfPCell celda10 = new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font));
                                celda10.BackgroundColor = new BaseColor(240, 240, 240);
                                celda10.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda10);

                                PdfPCell celda11 = new PdfPCell(new Phrase("TELEFONO", font));
                                celda11.BackgroundColor = new BaseColor(240, 240, 240);
                                celda11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda11);

                                PdfPCell celda12 = new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font));
                                celda12.BackgroundColor = new BaseColor(240, 240, 240);
                                celda12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda12);

                                PdfPCell celda13 = new PdfPCell(new Phrase("CANTIDAD", font));
                                celda13.BackgroundColor = new BaseColor(240, 240, 240);
                                celda13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda13);

                                PdfPCell celda14 = new PdfPCell(new Phrase("$" + cant, font));
                                celda14.BackgroundColor = new BaseColor(240, 240, 240);
                                celda14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda14);
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase("$"+cant, font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                fecha = listaEmpleado[i].pre_fecha.ToString("dd/MM/yyyy");
                                Console.WriteLine(fecha);
                            }

                            doc.Add(tabla);
                            doc.Add(espacio);
                            string[] fecprestamo = fecha.Split('/');
                            //  string mes4_ = "";
                            string diap = fecprestamo[0];
                            string mesp = fecprestamo[1];
                            string aniop = fecprestamo[2];
                            string valmes = "";
                            switch (mesp)
                            {
                                case "01":
                                    valmes = "ENERO";
                                    break;
                                case "02":
                                    valmes = "FEBRERO";
                                    break;
                                case "03":
                                    valmes = "MARZO";
                                    break;
                                case "04":
                                    valmes = "ABRIL";
                                    break;
                                case "05":
                                    valmes = "MAYO";
                                    break;
                                case "06":
                                    valmes = "JUNIO";
                                    break;
                                case "07":
                                    valmes = "JULIO";
                                    break;
                                case "08":
                                    valmes = "AGOSTO";
                                    break;
                                case "09":
                                    valmes = "SEPTIEMBRE";
                                    break;
                                case "10":
                                    valmes = "OCTUBRE";
                                    break;
                                case "11":
                                    valmes = "NOVIEMBRE";
                                    break;
                                case "12":
                                    valmes = "DICIEMBRE";
                                    break;

                            }

                            Paragraph fec = new Paragraph("DURANGO,DGO A " + diap + " DE " + valmes + " DE " + aniop, font2);
                            fec.Alignment = Element.ALIGN_CENTER;
                            doc.Add(fec);
                            doc.Add(espacio);

                            Paragraph firma = new Paragraph("Firma:", font2);
                            firma.Alignment = Element.ALIGN_CENTER;
                            doc.Add(firma);
                            doc.Add(espacio);
                            doc.Add(espacio);

                            Paragraph linea = new Paragraph(" _______________________________________________________________", font2);
                            linea.Alignment = Element.ALIGN_CENTER;
                            doc.Add(linea);
                            Paragraph nom = new Paragraph(nombre, font2);
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

                    }

                    break;

                case 3:
                    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        //convert(char, fecha, 103) as fecha

                        string query = "  SELECT [pr_id],[pr_nombre],[pr_matricula],[pr_telefono],[pr_ingreso],[pr_modificacion],[pr_tipo],[pr_estatus],[pr_recibo],[pr_ine],[pr_fecha] FROM [steujedo_sindicato].[steujedo_sindicato].[Pre_Revolvente] where [pr_id]=" + id;
                        revolvente = db.Database.SqlQuery<RevolventeCLS>(query).ToList();


                        using (MemoryStream stream = new MemoryStream())
                        {
                            String nombre = "";
                            Double cant = 0.00;
                            //String fecha = "";
                            PdfWriter.GetInstance(doc, stream);
                            doc.Open();

                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                            //image1.ScalePercent(50f);
                            image1.ScaleAbsoluteWidth(70);
                            image1.ScaleAbsoluteHeight(60);
                            image1.Alignment = Element.ALIGN_LEFT;
                            doc.Add(image1);
                            Paragraph espacio = new Paragraph(" ");
                            doc.Add(espacio);

                            Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                            title1.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title1);
                            doc.Add(espacio);

                            Paragraph title = new Paragraph("(S.T.E.U.J.E.D.)", font1);
                            title.Alignment = Element.ALIGN_CENTER;
                            Paragraph title2 = new Paragraph("CAJA DE AHORRO", font1);
                            title2.Alignment = Element.ALIGN_CENTER;
                            Paragraph title3= new Paragraph("SOLICITUD DE INGRESO", font1);
                            title3.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title);
                            doc.Add(title2);
                            doc.Add(title3);
                            doc.Add(espacio);



                            ////Creando la tabla
                            PdfPTable tabla = new PdfPTable(2);
                            tabla.WidthPercentage = 80f;
                            ////Asignando los anchos de las columnas
                            float[] valores = new float[2] { 13, 40 };
                            tabla.SetWidths(valores);

                            ////Creando celdas agregando contenido
                            ///


                            int nroregistros = revolvente.Count();
                            for (int i = 0; i < nroregistros; i++)
                            {
                               
                                nombre = revolvente[i].pr_nombre;
                                if (revolvente[i].pr_ingreso != null)
                                {
                                    cant = Double.Parse(revolvente[i].pr_ingreso.Replace("$","").ToString());
                                }
                                else if (revolvente[i].pr_modificacion != null) {

                                    cant = Double.Parse(revolvente[i].pr_modificacion.Replace("$", "").ToString());
                                }
                                else
                                {
                                    cant = 0.00;
                                }

                                PdfPCell celda17 = new PdfPCell(new Phrase("NUMERO DE SOLICITUD", font));
                                celda17.BackgroundColor = new BaseColor(240, 240, 240);
                                celda17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda17);

                                PdfPCell celda18 = new PdfPCell(new Phrase(revolvente[i].pr_id.ToString(), font));
                                celda18.BackgroundColor = new BaseColor(240, 240, 240);
                                celda18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda18);

                                PdfPCell celda15 = new PdfPCell(new Phrase("ESTATUS", font));
                                celda15.BackgroundColor = new BaseColor(240, 240, 240);
                                celda15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda15);

                                PdfPCell celda16 = new PdfPCell(new Phrase(revolvente[i].pr_estatus.ToString(), font));
                                celda16.BackgroundColor = new BaseColor(240, 240, 240);
                                celda16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda16);

                                string format = "dd/MM/yyyy HH:mm:ss";
                                string dt = Convert.ToDateTime(revolvente[i].pr_fecha).ToString(format);
                                PdfPCell celdafec = new PdfPCell(new Phrase("FECHA:", font));
                                celdafec.BackgroundColor = new BaseColor(240, 240, 240);
                                celdafec.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celdafec);

                                PdfPCell celdafecdate= new PdfPCell(new Phrase(dt, font));
                                celdafecdate.BackgroundColor = new BaseColor(240, 240, 240);
                                celdafecdate.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celdafecdate);

                                PdfPCell celda1 = new PdfPCell(new Phrase("SOLICITUD DE:", font));
                                celda1.BackgroundColor = new BaseColor(240, 240, 240);
                                celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda1);

                                PdfPCell celda2 = new PdfPCell(new Phrase(revolvente[i].pr_tipo.ToString(), font));
                                celda2.BackgroundColor = new BaseColor(240, 240, 240);
                                celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda2);



                                PdfPCell celda3 = new PdfPCell(new Phrase("MATRICULA", font));
                                celda3.BackgroundColor = new BaseColor(240, 240, 240);
                                celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda3);

                                PdfPCell celda4 = new PdfPCell(new Phrase(revolvente[i].pr_matricula.ToString(), font));
                                celda4.BackgroundColor = new BaseColor(240, 240, 240);
                                celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda4);

                                PdfPCell celda5 = new PdfPCell(new Phrase("NOMBRE:", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                PdfPCell celda6 = new PdfPCell(new Phrase(nombre, font));
                                celda6.BackgroundColor = new BaseColor(240, 240, 240);
                                celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda6);

                                PdfPCell celda7 = new PdfPCell(new Phrase("TELEFONO", font));
                                celda7.BackgroundColor = new BaseColor(240, 240, 240);
                                celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda7);

                                PdfPCell celda8 = new PdfPCell(new Phrase(revolvente[i].pr_telefono.ToString(), font));
                                celda8.BackgroundColor = new BaseColor(240, 240, 240);
                                celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda8);

                        
                            }

                            doc.Add(tabla);
                            doc.Add(espacio);

                            PdfPTable tabla2 = new PdfPTable(2);
                            tabla2.WidthPercentage = 80f;

                            float[] val = new float[2] {30,13 };
                            tabla2.SetWidths(val);

                            for (int i=0;i<nroregistros;i++) {

                                if (revolvente[i].pr_modificacion == null)
                                {

                                    PdfPCell celda13 = new PdfPCell(new Phrase("DESEO INGRESAR A LA CAJA DE AHORRO CON:", font));
                                    celda13.BackgroundColor = new BaseColor(240, 240, 240);
                                    celda13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    tabla2.AddCell(celda13);

                                    PdfPCell celda14 = new PdfPCell(new Phrase("$" + cant, font));
                                    celda14.BackgroundColor = new BaseColor(240, 240, 240);
                                    celda14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    tabla2.AddCell(celda14);
                                }
                                else {
                                    PdfPCell celda13 = new PdfPCell(new Phrase("MODIFICAR LA CANTIDAD DE MI AHORRO A::", font));
                                    celda13.BackgroundColor = new BaseColor(240, 240, 240);
                                    celda13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    tabla2.AddCell(celda13);

                                    PdfPCell celda14 = new PdfPCell(new Phrase("$" + cant, font));
                                    celda14.BackgroundColor = new BaseColor(240, 240, 240);
                                    celda14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    tabla2.AddCell(celda14);
                                }
                        
                            }
                            doc.Add(tabla2);
                            doc.Add(espacio);
                            Paragraph firma = new Paragraph("AUTORIZA:", font2);
                            firma.Alignment = Element.ALIGN_CENTER;
                            doc.Add(firma);
                            doc.Add(espacio);

                            Paragraph nom = new Paragraph(nombre, font2);
                            nom.Alignment = Element.ALIGN_CENTER;
                            doc.Add(nom);
                                                        Paragraph linea = new Paragraph(" _______________________________________________________________", font2);
                            linea.Alignment = Element.ALIGN_CENTER;
                            doc.Add(linea);
                            //Paragraph nom = new Paragraph(nombre, font2);
                            //nom.Alignment = Element.ALIGN_CENTER;
                            //doc.Add(nom);

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

                    }
                    break;

                case 4:
                    //rEVOLVENTE
                    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        //convert(char, fecha, 103) as fecha

                        string query = "  SELECT pre_id,pre_nombre,pre_matricula,pre_adscripcioon,pre_tarjeta_cuenta,pre_banco,pre_telefono,pre_cantidad,pre_fecha,pre_tipo,pre_estatus  FROM steujedo_sindicato.steujedo_sindicato.Caja_Ahorro where pre_tipo='REVOLVENTE' and pre_id=" + id;
                        listaEmpleado = db.Database.SqlQuery<CajaAhorroCLS>(query).ToList();


                        using (MemoryStream stream = new MemoryStream())
                        {
                            String nombre = "";
                            Double cant = 0.00;
                            String fecha = "";
                            PdfWriter.GetInstance(doc, stream);
                            doc.Open();

                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                            //image1.ScalePercent(50f);
                            image1.ScaleAbsoluteWidth(70);
                            image1.ScaleAbsoluteHeight(60);
                            image1.Alignment = Element.ALIGN_LEFT;
                            doc.Add(image1);
                            Paragraph espacio = new Paragraph(" ");
                            doc.Add(espacio);

                            Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                            title1.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title1);
                            doc.Add(espacio);

                            Paragraph title = new Paragraph("SOLICITUD DE PRESTAMO", font1);
                            title.Alignment = Element.ALIGN_CENTER;
                            Paragraph title2 = new Paragraph("FONDO REVOLVENTE", font1);
                            title2.Alignment = Element.ALIGN_CENTER;
                            doc.Add(title);
                            doc.Add(title2);
                            doc.Add(espacio);



                            ////Creando la tabla
                            PdfPTable tabla = new PdfPTable(2);
                            tabla.WidthPercentage = 80f;
                            ////Asignando los anchos de las columnas
                            float[] valores = new float[2] { 13, 40 };
                            tabla.SetWidths(valores);

                            ////Creando celdas agregando contenido
                            ///


                            int nroregistros = listaEmpleado.Count();
                            for (int i = 0; i < nroregistros; i++)
                            {
                                nombre = listaEmpleado[i].pre_nombre;
                                cant = Double.Parse(listaEmpleado[i].pre_cantidad.ToString());

                                PdfPCell celda17 = new PdfPCell(new Phrase("NUMERO DE SOLICITUD", font));
                                celda17.BackgroundColor = new BaseColor(240, 240, 240);
                                celda17.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda17);

                                PdfPCell celda18 = new PdfPCell(new Phrase(listaEmpleado[i].pre_id.ToString(), font));
                                celda18.BackgroundColor = new BaseColor(240, 240, 240);
                                celda18.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda18);

                                PdfPCell celda15 = new PdfPCell(new Phrase("ESTATUS", font));
                                celda15.BackgroundColor = new BaseColor(240, 240, 240);
                                celda15.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda15);

                                PdfPCell celda16 = new PdfPCell(new Phrase(listaEmpleado[i].pre_estatus.ToString(), font));
                                celda16.BackgroundColor = new BaseColor(240, 240, 240);
                                celda16.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda16);

                                PdfPCell celda1 = new PdfPCell(new Phrase("NOMBRE", font));
                                celda1.BackgroundColor = new BaseColor(240, 240, 240);
                                celda1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda1);

                                PdfPCell celda2 = new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font));
                                celda2.BackgroundColor = new BaseColor(240, 240, 240);
                                celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda2);



                                PdfPCell celda3 = new PdfPCell(new Phrase("MATRICULA", font));
                                celda3.BackgroundColor = new BaseColor(240, 240, 240);
                                celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda3);

                                PdfPCell celda4 = new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font));
                                celda4.BackgroundColor = new BaseColor(240, 240, 240);
                                celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda4);

                                PdfPCell celda5 = new PdfPCell(new Phrase("ADSCRIPCIÓN", font));
                                celda5.BackgroundColor = new BaseColor(240, 240, 240);
                                celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda5);

                                PdfPCell celda6 = new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font));
                                celda6.BackgroundColor = new BaseColor(240, 240, 240);
                                celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda6);

                                PdfPCell celda7 = new PdfPCell(new Phrase("NO. CUENTA/TARJETA", font));
                                celda7.BackgroundColor = new BaseColor(240, 240, 240);
                                celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda7);

                                PdfPCell celda8 = new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font));
                                celda8.BackgroundColor = new BaseColor(240, 240, 240);
                                celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda8);

                                PdfPCell celda9 = new PdfPCell(new Phrase("BANCO", font));
                                celda9.BackgroundColor = new BaseColor(240, 240, 240);
                                celda9.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda9);

                                PdfPCell celda10 = new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font));
                                celda10.BackgroundColor = new BaseColor(240, 240, 240);
                                celda10.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda10);

                                PdfPCell celda11 = new PdfPCell(new Phrase("TELEFONO", font));
                                celda11.BackgroundColor = new BaseColor(240, 240, 240);
                                celda11.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda11);

                                PdfPCell celda12 = new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font));
                                celda12.BackgroundColor = new BaseColor(240, 240, 240);
                                celda12.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda12);

                                PdfPCell celda13 = new PdfPCell(new Phrase("CANTIDAD", font));
                                celda13.BackgroundColor = new BaseColor(240, 240, 240);
                                celda13.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda13);

                                PdfPCell celda14 = new PdfPCell(new Phrase("$" + cant, font));
                                celda14.BackgroundColor = new BaseColor(240, 240, 240);
                                celda14.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                tabla.AddCell(celda14);
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_nombre.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_matricula.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_adscripcioon.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_tarjeta_cuenta.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_banco.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].pre_telefono.ToString(), font)));
                                //tabla.AddCell(new PdfPCell(new Phrase("$"+cant, font)));
                                //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].entrada.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                fecha = listaEmpleado[i].pre_fecha.ToString("dd/MM/yyyy");
                                Console.WriteLine(fecha);
                            }

                            doc.Add(tabla);
                            doc.Add(espacio);
                            string[] fecprestamo = fecha.Split('/');
                            //  string mes4_ = "";
                            string diap = fecprestamo[0];
                            string mesp = fecprestamo[1];
                            string aniop = fecprestamo[2];
                            string valmes = "";
                            switch (mesp)
                            {
                                case "01":
                                    valmes = "ENERO";
                                    break;
                                case "02":
                                    valmes = "FEBRERO";
                                    break;
                                case "03":
                                    valmes = "MARZO";
                                    break;
                                case "04":
                                    valmes = "ABRIL";
                                    break;
                                case "05":
                                    valmes = "MAYO";
                                    break;
                                case "06":
                                    valmes = "JUNIO";
                                    break;
                                case "07":
                                    valmes = "JULIO";
                                    break;
                                case "08":
                                    valmes = "AGOSTO";
                                    break;
                                case "09":
                                    valmes = "SEPTIEMBRE";
                                    break;
                                case "10":
                                    valmes = "OCTUBRE";
                                    break;
                                case "11":
                                    valmes = "NOVIEMBRE";
                                    break;
                                case "12":
                                    valmes = "DICIEMBRE";
                                    break;

                            }

                            Paragraph fec = new Paragraph("DURANGO,DGO A " + diap + " DE " + valmes + " DE " + aniop, font2);
                            fec.Alignment = Element.ALIGN_CENTER;
                            doc.Add(fec);
                            doc.Add(espacio);

                            Paragraph firma = new Paragraph("Firma:", font2);
                            firma.Alignment = Element.ALIGN_CENTER;
                            doc.Add(firma);
                            doc.Add(espacio);
                            doc.Add(espacio);

                            Paragraph linea = new Paragraph(" _______________________________________________________________", font2);
                            linea.Alignment = Element.ALIGN_CENTER;
                            doc.Add(linea);
                            Paragraph nom = new Paragraph(nombre, font2);
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

                    }

                    break;
            }


          

            return response;

        }



        [Route("api/ReimpresionSol")]
        [HttpGet]
        public HttpResponseMessage ReimpresionSol(int id)
        {
            try {
                String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
                String ruta_img2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
                iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 40, 40, 42, 35);
                byte[] buffer;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                List<ReimpresionCLS> listaEmpleado = null;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //string query = "  SELECT  pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_cancelado,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id and pad_id="+id;
                    //listaEmpleado = db.Database.SqlQuery<ReimpresionCLS>(query).ToList();

                    listaEmpleado = (from advos in db.Concurso_Plazas
                                     join plaza in db.Cat_Plazas
                                     on advos.pad_plaza_id equals plaza.catp_id
                                     where advos.pad_id == id

                                     select new ReimpresionCLS
                                     {
                                         pad_id = advos.pad_id,
                                         pad_plaza_id = advos.pad_plaza_id,
                                         pad_plaza_descrip = plaza.catp_descrip,
                                         pad_mat = advos.pad_mat,
                                         pad_nombre = advos.pad_nombre,
                                         pad_adscripcion = advos.pad_adscripcion,
                                         pad_categoria = advos.pad_categoria,
                                         pad_funcion = advos.pad_funcion,
                                         pad_situacion = advos.pad_situacion,
                                         pad_permanencia = advos.pad_permanencia,
                                         pad_f_ingreso = advos.pad_f_ingreso,
                                         pad_permisos = advos.pad_permisos,
                                         pad_f_antig = advos.pad_f_antig,
                                         pad_n_insaluble = advos.pad_n_insaluble,
                                         pad_adscrip_base = advos.pad_adscrip_base,
                                         pad_catego_base = advos.pad_catego_base,
                                         pad_funcion_base = advos.pad_funcion_base,
                                         pad_situacion_base = advos.pad_situacion_base,
                                         pad_num_contacto = advos.pad_num_contacto,
                                         pad_observaciones = advos.pad_observaciones,
                                         pad_string_fec = advos.pad_string_fec,
                                         catp_descrip = plaza.catp_descrip,
                                         catp_u_captura = plaza.catp_u_captura,
                                         catp_categoria = plaza.catp_categoria,
                                         catp_funcion = plaza.catp_funcion,
                                         catp_adscripcion = plaza.catp_adscripcion


                                     }
                                     ).ToList();

                }
                using (MemoryStream stream = new MemoryStream())
                {
                    string catp_descrip = "";
                    string catp_categoria = "";
                    string catp_funcion = "";
                    string catp_adscripcion = "";
                    string pad_nombre = "";
                    string pad_adscripcion = "";
                    string pad_categoria = "";
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
                    string pad_string_fec = "";



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
                    foreach (var i in listaEmpleado)
                    {
                        catp_descrip = i.catp_descrip;
                        catp_categoria = i.catp_categoria;
                        catp_funcion = i.catp_funcion;
                        catp_adscripcion = i.catp_adscripcion;
                        pad_nombre = i.pad_nombre;
                        pad_adscripcion = i.pad_adscripcion;
                        pad_categoria = i.pad_categoria;
                        pad_funcion = i.pad_funcion;
                        pad_situacion = i.pad_situacion;
                        pad_permanencia = i.pad_permanencia;
                        pad_f_ingreso = i.pad_f_ingreso;
                        pad_permisos = i.pad_permisos;
                        pad_f_antig = i.pad_f_antig;
                        pad_n_insaluble = i.pad_n_insaluble;
                        pad_adscrip_base = i.pad_adscrip_base;
                        pad_catego_base = i.pad_catego_base;
                        pad_funcion_base = i.pad_funcion_base;
                        pad_situacion_base = i.pad_situacion_base;
                        pad_num_contacto = i.pad_num_contacto;
                        pad_observaciones = i.pad_observaciones;
                        pad_string_fec = i.pad_string_fec;
                    }
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

                    PdfPCell celda2 = new PdfPCell(new Phrase(catp_descrip, font));
                    celda2.BackgroundColor = new BaseColor(240, 240, 240);
                    celda2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda2);

                    PdfPCell celda3 = new PdfPCell(new Phrase("Categoria:", font));
                    celda3.BackgroundColor = new BaseColor(240, 240, 240);
                    celda3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda3);

                    PdfPCell celda4 = new PdfPCell(new Phrase(catp_categoria, font));
                    celda4.BackgroundColor = new BaseColor(240, 240, 240);
                    celda4.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda4);

                    PdfPCell celda5 = new PdfPCell(new Phrase("Función:", font));
                    celda5.BackgroundColor = new BaseColor(240, 240, 240);
                    celda5.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda5);

                    PdfPCell celda6 = new PdfPCell(new Phrase(catp_funcion, font));
                    celda6.BackgroundColor = new BaseColor(240, 240, 240);
                    celda6.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda6);

                    PdfPCell celda7 = new PdfPCell(new Phrase("Adscripción:", font));
                    celda7.BackgroundColor = new BaseColor(240, 240, 240);
                    celda7.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda7);

                    PdfPCell celda8 = new PdfPCell(new Phrase(catp_adscripcion, font));
                    celda8.BackgroundColor = new BaseColor(240, 240, 240);
                    celda8.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla.AddCell(celda8);
                    //////Agregando la tabla al documento
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
                    doc.Add(espacio);
                    doc.Add(espacio);

                    if (pad_string_fec != null)
                    {
                       // string format = "dd/MM/yyyy HH:mm:ss tt";
                        //DateTime dt = DateTime.ParseExact(pad_string_fec, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        //string dt = Convert.ToDateTime(pad_string_fec).ToString(format);

                        //Arreglar
                       // string dt = Convert.ToDateTime(pad_string_fec).ToString(format);
                        //DateTime date = DateTime.ParseExact(pad_string_fec, "dd/MM/yyyy HH:mm:ss tt", null);
                        //DateTime dt = DateTime.ParseExact(pad_string_fec, format, CultureInfo.InvariantCulture);
                        //Console.WriteLine(s.ToString(format);
                        //DateTime myDate = DateTime.Parse(pad_string_fec);


                        //Arreglando
                        //Paragraph fec = new Paragraph("Fecha: " + dt, font2);
                        //linea.Alignment = Element.ALIGN_LEFT;
                        //doc.Add(fec);


                        Paragraph fec = new Paragraph("Fecha: " + pad_string_fec, font2);
                        linea.Alignment = Element.ALIGN_LEFT;
                        doc.Add(fec);


                        //Paragraph fecha = new Paragraph(listaEmpleado[i].pad_string_fec.ToLongDateString(), font2);
                        ////nom.Alignment = Element.ALIGN_CENTER;
                        //doc.Add(fecha);
                    }
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
            } catch (Exception e) {
                HttpResponseMessage response = Request.CreateResponse(e.Message);
                return response;
            }


        }

    }
}
