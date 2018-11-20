using System;
using System.Collections.Generic;
using System.Text;

    namespace AppQ4evo.ViewModels
    {
        public class Rootobject
        {
            public Rssuser rssUser { get; set; }
        }

        public class Rssuser
        {
            public Userlist userList { get; set; }
            public string index { get; set; }
            public string gps_distancia { get; set; }
            public string gps_tempo { get; set; }
            public string error { get; set; }
        }

        public class Userlist
        {
            public User[] user { get; set; }
        }

        public class User
        {       
            public Estat[] estat { get; set; } 
            public string username { get; set; }
            public string nome { get; set; }
            public string telefone { get; set; }
            public string email { get; set; }
            public string sigla { get; set; }
        }

    public class Estat
    {
        public string mes { get; set; }
        public string count { get; set; }
    }
}
