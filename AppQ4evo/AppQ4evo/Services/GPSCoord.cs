using AppQ4evo.ViewModels;
using AppQ4evo.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppQ4evo.Services
{

    public class GPSCoord  
    {
        public AppService log = new AppService();
        public List<Dados_GPS> dados_GPS { get; set; }
        public Dados_GPS_List dados_GPS_List { get; set; }
        public List<Gpscoord> gPSCoord { get; set; }
        public Gpscoordlist gPSCoordList { get; set; }
        public string oldString = string.Empty;
        public List<Gpscoord> teste { get; set; }
        RootobjectCoord rootCord { get; set; }
        public static List<contacto> listMensagensauxcoord { get; set; }

        public GPSCoord()
        {
            dados_GPS = new List<Dados_GPS>();
            dados_GPS_List = new Dados_GPS_List();
            gPSCoord = new List<Gpscoord>();
            gPSCoordList = new Gpscoordlist();
            teste = new List<Gpscoord>();
            rootCord = new RootobjectCoord();
            listMensagensauxcoord = new List<contacto>();
        }

        public async Task GetGPSGeral()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            log.handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            log.client = new HttpClient(log.handler);
            var codeDispositivo = CrossDeviceInfo.Current.Id;
            var currentMsgId = (Application.Current.Properties["numero"].ToString()); //este chama Local DB
            var verificaPassagemDados = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            try
            {
                if (CrossConnectivity.Current.IsConnected && verificaPassagemDados == true)
                {
                    int v = 0;

                    if (dados_GPS_List.dados_GPS == null || gPSCoordList.gPSCoord == null)
                    {
                        dados_GPS_List.dados_GPS = new List<Dados_GPS>();
                        gPSCoordList.gPSCoord = new List<Gpscoord>();
                    }
                    
                    foreach(Gpscoord c in gPSCoordList.gPSCoord)
                    {
                        if(gPSCoordList.gPSCoord.FindAll(d => d.data_amostra == c.data_amostra).Count > 1)
                        {
                            gPSCoordList.gPSCoord.Remove(c);
                        }
                    }

                    foreach (Dados_GPS d in dados_GPS_List.dados_GPS)
                    {
                        int pos = 0;

                        for (int i = 0; i < dados_GPS_List.dados_GPS[v].gPSCoordList.gPSCoord.Count; i += 2, pos++)
                        {
                            dados_GPS_List.dados_GPS[v].gPSCoordList.gPSCoord[pos] = dados_GPS_List.dados_GPS[v].gPSCoordList.gPSCoord[i];
                        }

                        dados_GPS_List.dados_GPS[v].gPSCoordList.gPSCoord.RemoveRange(pos, dados_GPS_List.dados_GPS[v].gPSCoordList.gPSCoord.Count - pos);
                        v++;
                    }
             
                    Rssgpscoord coordgps = new Rssgpscoord
                    {
                        codigo_dispositivo = codeDispositivo,
                        msg_id = currentMsgId,
                        dados_GPS_List = dados_GPS_List
                    };

                    rootCord.rssGPSCoord = coordgps;
                    var jsonObj = JsonConvert.SerializeObject(rootCord);

                    using (log.client)
                    {
                        StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                        var request = new HttpRequestMessage()
                        {
                            RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOInsertCoordinates.php"),
                            Method = HttpMethod.Post,
                            Content = content
                        };

                        var response = await log.client.SendAsync(request);
                        var responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            dynamic obj = JsonConvert.DeserializeObject<RootobjectMensagens>(responseString);

                            if (obj.rssContactos.error == "0")
                            {
                                List<string> filterNum = new List<string>();

                                foreach (contacto cc in obj.rssContactos.contactosList.contactos)
                                {
                                    filterNum.Add(cc.numero);
                                    listMensagensauxcoord.Add(cc);
                                }
                                int maxNum = int.MinValue;

                                foreach (string num in filterNum)
                                {
                                    if (int.Parse(num) > maxNum)
                                    {
                                        maxNum = int.Parse(num);
                                        Application.Current.Properties["numero"] = maxNum;
                                        var duration = TimeSpan.FromSeconds(1);
                                        Vibration.Vibrate(duration);
                                    }
                                }
                            }
                            else
                            {
                                string outError = obj.rssContactos.error;
                                Debug.WriteLine(outError);
                            }
                        }
                        else
                        {
                            throw new HttpRequestException("nada na lista");
                        }
                    }
                    dados_GPS_List.dados_GPS.Clear();
                    gPSCoordList.gPSCoord.Clear();
                    dados_GPS_List.dados_GPS = new List<Dados_GPS>();
                    gPSCoordList.gPSCoord = new List<Gpscoord>();
                }
                else
                {
                    foreach (Gpscoord c in gPSCoordList.gPSCoord)
                    {
                        if (gPSCoordList.gPSCoord.FindAll(d => d.data_amostra == c.data_amostra).Count > 1)
                        {
                            gPSCoordList.gPSCoord.Remove(c);
                        }
                    }

                }
            }
            catch(HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                await GetGPSGeral();
            }
        }

        public async Task StartLocAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                         Debug.WriteLine("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    if (CrossGeolocator.Current.IsListening)
                        return;
                    int tempoWebService = Int32.Parse(Application.Current.Properties["gps_tempo"].ToString());
                    int distanciaWebService = Int32.Parse(Application.Current.Properties["gps_distancia"].ToString());

                    await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(tempoWebService), distanciaWebService, true, new Plugin.Geolocator.Abstractions.ListenerSettings
                    {
                        ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                        AllowBackgroundUpdates = true,
                        DeferLocationUpdates = true,
                        DeferralDistanceMeters = 1,
                        DeferralTime = TimeSpan.FromSeconds(1),
                        ListenForSignificantChanges = true,
                        PauseLocationUpdatesAutomatically = false
                    });

                    CrossGeolocator.Current.PositionChanged += PositionChanged;
                    CrossGeolocator.Current.PositionError += PositionError;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    PopupNavigation.Instance.PushAsync(new ErrorPopUp("Acesso negado. Tente novamente"));
                }
            }
            catch (Exception ex)
            {
                PopupNavigation.Instance.PushAsync(new ErrorPopUp("Erro: " + ex));
            }
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //para os dados
            var currentuserID = (Application.Current.Properties["matricula"].ToString());
            DateTime y = DateTime.Today;
            string year = y.Year.ToString();
            //--Para o GpsCoord
            DateTime foo = DateTime.UtcNow;
            DateTime now = DateTime.Today;
            int dia = now.Day;
            int mes = now.Month;
            int ano = now.Year;
            var time = DateTime.Now.ToString("HH:mm:ss");
            string format = "" + dia + "." + mes + "." + ano + " " + time + "";
            var numASS = (Application.Current.Properties["numero_assistencia"].ToString());
            var position = e.Position;

            if (numASS == null)
            {
                numASS = "0";
            }            

                if (numASS != oldString || dados_GPS_List.dados_GPS == null || gPSCoordList.gPSCoord == null || dados_GPS_List.dados_GPS.Count == 0)
                {
                dados_GPS.Add(new Dados_GPS
                {
                    ano_assistencia = year,
                    numero_assistencia = numASS,
                    matricula = currentuserID,
                    gPSCoordList = new Gpscoordlist
                    {
                        gPSCoord = new List<Gpscoord>{
                    new Gpscoord
                    {
                        data_amostra = format.ToString(),
                        longitude = position.Longitude.ToString().Replace(",", "."),
                        latitude = position.Latitude.ToString().Replace(",", "."),
                        velocidade = Math.Round((position.Speed) * 3.6).ToString()
                    }
                }
                        }
                    });

                    dados_GPS_List.dados_GPS = dados_GPS;
                }         
            else
            {
                var count = dados_GPS_List.dados_GPS.Count;
                dados_GPS_List.dados_GPS[count - 1].gPSCoordList.gPSCoord.Add(new Gpscoord
                {
                    data_amostra = format.ToString(),
                    longitude = position.Longitude.ToString().Replace(",", "."),
                    latitude = position.Latitude.ToString().Replace(",", "."),
                    velocidade = Math.Round((position.Speed) * 3.6).ToString()
                });
            }
            oldString = numASS;

            // output de teste para ver as coordenadas
            var positions = e.Position;
            var output = "Full: Lat: " + positions.Latitude.ToString().Replace(",", ".") + " Long: " + positions.Longitude.ToString().Replace(",", ".");
            output += "\n" + $"Time: {format.ToString()}";
            output += "\n" + $"Heading: {positions.Heading}";
            output += "\n" + $"Speed: {positions.Speed}";
            output += "\n" + $"Accuracy: {positions.Accuracy}";
            output += "\n" + $"Altitude: {positions.Altitude}";
            output += "\n" + $"Altitude Accuracy: {positions.AltitudeAccuracy}";
            Debug.WriteLine(output);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
        }

        public async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }
}
