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
                AuthSecret = "gAMb4Pbfku1AwMdAFsObBJt1OOJHIDhW4zA497vd",
                BasePath = "https://bdfirebase-4f03f-default-rtdb.firebaseio.com/"
            };

            cliente = new FirebaseClient(config); //  con esto ya estaría conectado a firebase 
        }
        // GET: Mantenedor
        public ActionResult Inicio()
        {
            Dictionary<string, Contacto> lista = new Dictionary<string, Contacto>();
            FirebaseResponse response = cliente.Get("contactos");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Contacto>>(response.Body);


            List<Contacto> listaContacto = new List<Contacto>();

            foreach (KeyValuePair<string, Contacto>elemento in lista)
            {
                listaContacto.Add(new Contacto()
                {
                    IdContacto = elemento.Value.IdContacto,
                    Nombre = elemento.Value.Nombre,
                    Correo = elemento.Value.Correo,
                    Telefono = elemento.Value.Telefono
                });
            }

            return View(listaContacto);
        }
        public ActionResult Crear()
        {
            return View();
        }

        public ActionResult Editar()
        {
            return View();
        }

        public ActionResult Eliminar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Contacto oContacto)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");

            SetResponse response = cliente.Set("contactos/" + IdGenerado, oContacto);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View();
            }
            else
            {
                return View();
            }

        }

    }
}