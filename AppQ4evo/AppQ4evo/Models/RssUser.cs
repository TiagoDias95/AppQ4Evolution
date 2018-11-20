using AppQ4evo.Services;
using AppQ4evo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppQ4evo.ViewModels
{
    public class RssUser 
    {
            public string user { get; set; }
            public string pass { get; set; }
            public string company { get; set; }
            public string matricula { get; set; }
            public string codigo_dispositivo { get; set; }
            public string index { get; set; }
    }

    public class RootObject
    {
        public RssUser rssUser { get; set; }
    }
}
