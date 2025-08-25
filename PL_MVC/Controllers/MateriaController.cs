using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class MateriaController : Controller
    {
        // GET: Materia
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Materia materia = new ML.Materia();
            materia.Semestre = new ML.Semestre();
            materia.Nombre = "";
            materia.Semestre.IdSemestre = 0;

            ML.Result result = BL.Materia.GetAllEF(materia);

            if (result.Correct)
            {
                materia.Materias = result.Objects; //Unboxing
            }
            else
            {
                materia.Materias = new List<object>();
                result.ErrorMessage = "Error";
            }

            ML.Result resultSemestre = BL.Semestre.GetAllLinq();
            if (resultSemestre.Correct)
            {
                materia.Semestre.Semestres = resultSemestre.Objects; //Unboxing
            }
            else
            {
                materia.Semestre.Semestres = new List<object>();
            }

            return View(materia);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Materia materia)
        {
            materia.Nombre = materia.Nombre == null ? "" : materia.Nombre;


            ML.Result result = BL.Materia.GetAllEF(materia);

            if (result.Correct)
            {
                materia.Materias = result.Objects; //Unboxing
            }
            else
            {
                materia.Materias = new List<object>();
                result.ErrorMessage = "Error";
            }

            ML.Result resultSemestre = BL.Semestre.GetAllLinq();
            if (resultSemestre.Correct)
            {
                materia.Semestre.Semestres = resultSemestre.Objects; //Unboxing
            }
            else
            {
                materia.Semestre.Semestres = new List<object>();
            }

            return View(materia);
        }
        [HttpPost]
        public ActionResult Form(ML.Materia materia)
        {
            //Aqui cachamos la imagen
            HttpPostedFileBase file = Request.Files["imagenUsuario"];
            if (file != null && file.ContentLength > 0)
            {
                materia.Imagen = ConvertirAArrayBytes(file);
            }


            if (materia.IdMateria == 0)
            {
                BL.Materia.AddEF(materia);
            }
            else
            {
                BL.Materia.UpdateSP(materia);
            }
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult Form(int? IdMateria)
        {
            ML.Materia materia = new ML.Materia();
            materia.Semestre = new ML.Semestre();

            if (IdMateria != null)
            {
                ML.Result result = BL.Materia.GetByIdSP(IdMateria.Value);
                if (result.Correct)
                {
                    materia = (ML.Materia)result.Object;

                }
                else
                {
                    result.ErrorMessage = "Error";
                    //ViewBag
                }
            }

            ML.Result resultSemestre = BL.Semestre.GetAllLinq();
            if (resultSemestre.Correct)
            {
                materia.Semestre.Semestres = resultSemestre.Objects; //Unboxing
            }
            else
            {
                materia.Semestre.Semestres = new List<object>();
            }


            return View(materia);
        }

        public ActionResult Delete(int IdMateria)
        {
            ML.Result result = BL.Materia.Delete(IdMateria);
            return RedirectToAction("GetAll");
        }

        public byte[] ConvertirAArrayBytes(HttpPostedFileBase Foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(Foto.InputStream);
            byte[] data = reader.ReadBytes((int)Foto.ContentLength);
            return data;
        }
        [HttpPost]
        public ActionResult ddsr(HttpPostedFileBase archivoTxt)
        {
            var registrosCorrectos = new List<ML.Materia>();
            var registrosIncorrectos = new List<string>();

            if (archivoTxt != null)
            {
                using (var reader = new StreamReader(archivoTxt.InputStream))
                {
                    bool encabezados = true;
                    string linea;
                    int numLinea = 1;

                    while ((linea = reader.ReadLine()) != null)
                    {
                        if (encabezados)
                        {
                            encabezados = false;
                            numLinea++;
                            continue;
                        }

                        string[] datos = linea.Split('|');

                        try
                        {
                            if (datos.Length < 6)
                                throw new Exception("Faltan columnas");

                            // Solo validamos el nombre
                            if (!Regex.IsMatch(datos[0], @"^[a-zA-Z\s]+$"))
                                throw new Exception("Nombre inválido");

                            // Inicializamos materia de forma desglosada
                            ML.Materia materiaRegistro = new ML.Materia();
                            materiaRegistro.Semestre = new ML.Semestre();

                            materiaRegistro.Nombre = datos[0];
                            materiaRegistro.Creditos = Convert.ToByte(datos[1]);
                            materiaRegistro.Costo = Convert.ToDecimal(datos[2]);
                            materiaRegistro.Semestre.IdSemestre = Convert.ToByte(datos[3]);
                            materiaRegistro.Imagen = null;
                            materiaRegistro.FechaNacimiento = datos[5]; // no validamos

                            registrosCorrectos.Add(materiaRegistro);
                        }
                        catch (Exception ex)
                        {
                            registrosIncorrectos.Add($"Línea {numLinea}: {ex.Message} → {linea}");
                        }

                        numLinea++;
                    }
                }
            }

            // --- traemos de nuevo los datos normales de GetAll ---
            ML.Materia materia = new ML.Materia();
            materia.Semestre = new ML.Semestre();
            materia.Nombre = "";
            materia.Semestre.IdSemestre = 0;

            ML.Result result = BL.Materia.GetAllEF(materia);
            materia.Materias = result.Correct ? result.Objects : new List<object>();

            ML.Result resultSemestre = BL.Semestre.GetAllLinq();
            materia.Semestre.Semestres = resultSemestre.Correct ? resultSemestre.Objects : new List<object>();

            // Pasamos los resultados de validación a la vista
            ViewBag.RegistrosCorrectos = registrosCorrectos ?? new List<ML.Materia>();
            ViewBag.RegistrosIncorrectos = registrosIncorrectos ?? new List<string>();

            return View("GetAll", materia);
        }

        [HttpPost]
        public ActionResult CargaMasiva(HttpPostedFileBase archivoTxt)
        {
            var registrosCorrectos = new List<ML.Materia>();
            var registrosIncorrectos = new List<string>();

            if (archivoTxt != null)
            {
                using (var reader = new StreamReader(archivoTxt.InputStream))
                {
                    bool encabezados = true;
                    string linea;

                    int numeroLinea = 1;

                    while ((linea = reader.ReadLine()) != null)
                    {
                        if (encabezados)
                        {
                            encabezados = false;
                            numeroLinea++;
                            continue;
                        }

                        string[] datos = linea.Split('|');

                        try
                        {
                            if (datos.Length < 6)
                                throw new Exception("Faltan datos");

                            if (!Regex.IsMatch(datos[0], @"[a-zA-z]+$"))
                                throw new Exception("Nombre no es valido");

                            ML.Materia materia = new ML.Materia();
                            materia.Semestre = new ML.Semestre();

                            materia.Nombre = datos[0];
                            materia.Creditos = Convert.ToByte(datos[1]);
                            materia.Costo = Convert.ToByte(datos[2]);
                            materia.Semestre.IdSemestre = Convert.ToByte(datos[3]);
                            materia.Imagen = null;
                            materia.FechaNacimiento = datos[5];

                        }
                        catch (Exception e)
                        {
                            registrosIncorrectos.Add($"Linea {numeroLinea}: {e.Message}");
                        }

                        numeroLinea++;


                    }
                }
            }
            ViewBag.RegistrosCorrectos = registrosCorrectos ?? new List<ML.Materia>();
            ViewBag.RegistrosInorrectos = registrosIncorrectos ?? new List<string>();



            return View("GetAll", );
        }


    }
}