using Java.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AppQ4evo.ViewModels
{
   public class ServicosModel
    {
        public int numeroId { get; set; }
        public DateTime DateTimeInico { get; set; }
        public DateTime DateTimeFim { get; set; }
        public string Cliente { get; set; }
        public string nomePessoa { get; set; }
        public int telefone { get; set; }
        public Estado estado { get; set; }
        public string EstadoNome { get; set; }
        public string LocalPartida { get; set; }
        public string LocalDestino { get; set; }
        public bool IsVisible { get; set; }    
    }
    
    public enum Estado
    {
        [Description("Por Iniciar")]
        PorAbrir = 1,
        [Description("Iniciado")]
        Aberto = 2,
        [Description("Fechado")]
        Fechado = 3
    }
}

