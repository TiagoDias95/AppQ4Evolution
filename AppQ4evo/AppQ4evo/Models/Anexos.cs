using System;
using System.Collections.Generic;
using System.Text;

namespace AppQ4evo.ViewModels
{
    public class Anexo
    {
        public string ano_assistencia { get; set; }
        public string numero_assistencia { get; set; }
        public string nome_ficheiro_anexo { get; set; }
        public string codigo_tipo { get; set; }
        public string anexo { get; set; }
    }

    public class AnexosList
    {
        public List<Anexo> anexos { get; set; }
    }

    public class RssAnexos
    {
        public AnexosList anexosList { get; set; }
        public string error { get; set; }
    }

    public class RootObjectAnexos
    {
        public RssAnexos rssAnexos { get; set; }
    }
}
