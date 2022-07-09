using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//  agregando las referencias
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using BDFIREBASE.Models;

namespace BDFIREBASE.Controllers
{
    public class MantenedorController : Controller
    {
        IFirebaseClient cliente;

        public MantenedorController()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "ipZrqSpELBuhttG1wDNzbm6UohXfcszyS3EG6MwS",
                BasePath = "https://appcompras-2752e-default-rtdb.firebaseio.com/"
            };

            cliente = new FirebaseClient(config); //  con esto ya estaría conectado a firebase 
        }
        // GET: Mantenedor
        public ActionResult Inicio()
        {
            Dictionary<string, Contacto> lista = new Dictionary<string, Contacto>();
            FirebaseResponse response = cliente.Get("Productos");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Contacto>>(response.Body);


            List<Contacto> listaContacto = new List<Contacto>();

            foreach (KeyValuePair<string, Contacto>elemento in lista)
            {
                listaContacto.Add(new Contacto()
                {
                    IdContacto = elemento.Key,                   
                    Descripcion = elemento.Value.Descripcion,
                    Icono = elemento.Value.Icono,
                    Marca = elemento.Value.Marca,
                    Precio = elemento.Value.Precio
                }) ;
            }

            return View(listaContacto);
        }
        public ActionResult Crear()
        {
            return View();
        }

        public ActionResult Editar(string idcontacto)
        {
            FirebaseResponse response = cliente.Get("Productos");

            Contacto ocontacto = response.ResultAs<Contacto>();


            ocontacto.IdContacto = idcontacto;



            return View(ocontacto);
        }

        public ActionResult Eliminar(string idcontacto)
        {
            FirebaseResponse response = cliente.Delete("Productos/" + idcontacto);
            return RedirectToAction("Inicio", "Mantenedor");
        }

        [HttpPost]
        public ActionResult Crear(Contacto oContacto)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");

            SetResponse response = cliente.Set("Productos/" + IdGenerado, oContacto);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Inicio", "Mantenedor");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult Editar(Contacto oContacto)
        {
            string idcontacto = oContacto.IdContacto;
            oContacto.IdContacto = null;

            FirebaseResponse response = cliente.Update("Productos/"+idcontacto, oContacto);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Inicio", "Mantenedor");
            }
            else
            {
                return View();
            }            
        }

    }
}