using System;
using System.Collections.Generic;
using System.Text;

namespace AppQ4evo.ViewModels
{
    public class RootobjectMatri
    {
        public Rssmatriculas rssMatriculas { get; set; }
    }

    public class Rssmatriculas
    {
        public Matriculaslist matriculasList { get; set; }
        public int error { get; set; }
    }

    public class Matriculaslist
    {
        public Matricula[] matriculas { get; set; }
    }

    public class Matricula
    {
        public string matricula { get; set; }
    }
}
