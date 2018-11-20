using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace AppQ4evo.ViewModels
{
    public class Rota 
    {
        public string ano_assistencia { get; set; }
        public string pago_no_local { get; set; }
        public string espera_em_minutos { get; set; }
        public string numero_acompanhantes { get; set; }
        public string numero_assistencia { get; set; }
        public string carga_hora { get; set; }
        public string descarga_hora { get; set; }
        public string codigo_terceiro { get; set; }
        public string cliente { get; set; }
        public string estado { get; set; }
        public string contacto_nome { get; set; }
        public string contacto_telefone { get; set; }
        public string carga_local { get; set; }
        public string descarga_local { get; set; }
        public string carga_kms { get; set; }
        public string descarga_kms { get; set; }
        public string carga_observacaoes { get; set; }
        public string descarga_observacoes { get; set; }
        public string matricula { get; set; }
        public bool IsVisible { get; set; }
        public bool IsVisiblee { get; set; }
        public string corEstado { get; set ; } 
    }

    public class RotasList
    {
        public List<Rota> rotas { get; set; }
        public Rota rota { get; set; }
    }

    public class RssRotas
    {
        public RotasList rotasList { get; set; }
        public string data { get; set; }
        public int error { get; set; }
    }

    public class RootObjectR
    {
        public RssRotas rssRotas { get; set; }
    }
}
