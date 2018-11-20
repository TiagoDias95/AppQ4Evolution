using System;
using System.Collections.Generic;
using System.Text;

namespace AppQ4evo.ViewModels
{
    public class RootobjectCoord
    {
        public Rssgpscoord rssGPSCoord { get; set; }
    }

    public class Rssgpscoord
    {
        public string codigo_dispositivo { get; set; }
        public string msg_id { get; set; }
        public Dados_GPS_List dados_GPS_List { get; set; }
    }

    public class Dados_GPS_List
    {
        public List<Dados_GPS> dados_GPS { get; set; }
    }

    public class Dados_GPS
    {
        public string ano_assistencia { get; set; }
        public string numero_assistencia { get; set; }
        public string matricula { get; set; }
        public Gpscoordlist gPSCoordList { get; set; }
    }

    public class Gpscoordlist
    {
        public List<Gpscoord> gPSCoord { get; set; }
    }

    public class Gpscoord
    {
        public string data_amostra { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string velocidade { get; set; }
    }
}