using System;
using System.Collections.Generic;
using System.Linq;
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

            ML.Result result = BL.Materia.GetAllSP();

            if (result.Correct)
            {
                materia.Materias = result.Objects; //Unboxing
            }
            else
            {
                materia.Materias = new List<object>();
                result.ErrorMessage = "Error";
            }

            return View(materia);
        }
        [HttpPost]
        public ActionResult Form(ML.Materia materia)
        {
            //Aqui cachamos la imagen
            HttpPostedFileBase file = Request.Files["imagenUsuario"];
            if(file != null && file.ContentLength > 0)
            {
                materia.Imagen = ConvertirAArrayBytes(file);
            }
            

            if (materia.IdMateria == 0)
            {
                BL.Materia.AddSP(materia);
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
                ML.Result result = BL.Materia.GetById(IdMateria.Value);
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

            ML.Result resultSemestre = BL.Semestre.GetAllSP();
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
    }
}