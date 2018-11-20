using AppQ4evo.Services;
using AppQ4evo.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AppQ4evo.ViewModels
{
    public class RootobjectMensagens
    {
        public Rsscontactos rssContactos { get; set; }
    }

    public class Rsscontactos
    {
        public ContactosList contactosList { get; set; }
        public string error { get; set; }
        public string dti { get; set; }
        public string msg_id { get; set; }
    }

    public class ContactosList
    {
        public List<contacto> contactos { get; set; }
    }

    public class contacto
    {
        public List<Users> users { get; set; }
        public string userEscrito { get; set; }
        public string Status { get; set; }
        public string ParentAux { get; set; }
        public string numero { get; set; }
        public string numero_parente { get; set; }
        public string descricao { get; set; }
        public string data_hora { get; set; }
        public string users_in { get; set; } 
    }
}
