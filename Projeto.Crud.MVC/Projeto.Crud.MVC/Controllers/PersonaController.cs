using Generico.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Projeto.Crud.MVC.Controllers
{
    public class PersonaController : Controller
    {
        // GET: Persona
        [HttpGet]
        public ActionResult Create()
        {
            using (Repositorio<Persona_Tipo> obj = new Repositorio<Persona_Tipo>())
            {
                obj.Exception += obj_Exception;
                ViewBag.Persona_Tipo = obj.Filter(x => true);
            }

            using (Repositorio<Estatu> obj = new Repositorio<Estatu>())
            {
                obj.Exception += obj_Exception;
                ViewBag.Estatus = obj.Filter(x => true);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Persona persona)
        {
            Thread.Sleep(5000);
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Exception += obj_Exception;
                obj.Create(persona);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult CreateComAjax(Persona persona)
        {
            Thread.Sleep(5000);
            bool result = false;
            string mensaje = "Error al crear el registro";
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Exception += obj_Exception;
                if (obj.Create(persona) != null)
                {
                    result = true;
                    mensaje = "Registro creado!";
                }
            }
            return Json(new { result = result, mensaje = mensaje },
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Listar()
        {
            List<vConsultaPersona> dados = new List<vConsultaPersona>();
            return View(dados);
        }


        public ActionResult ListarSinAjax()
        {
            Thread.Sleep(5000);
            List<vConsultaPersona> datos = new List<vConsultaPersona>();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                datos = obj.Filter(x => true);
            }
            return View("Listar", datos);
        }

        [HttpGet]
        public ActionResult ListarAjax()
        {
            Thread.Sleep(1000);
            List<vConsultaPersona> datos = new List<vConsultaPersona>();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                obj.Exception += obj_Exception;
                datos = obj.Filter(x => true);
            }
            return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
        }



        private void obj_Exception(object sender, ExceptionEventArgs e)
        {
            if (e.EntityValidationErros != null)
                throw new DbEntityValidationException(e.message,  e.EntityValidationErros, e.InnerException) { Source = e.Source };
            else
                throw new Exception(e.message, e.InnerException) { Source = e.Source };
        }


    }
}