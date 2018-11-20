using AppQ4evo.ViewModels;
using AppQ4evo.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Http.Headers;
using Plugin.DeviceInfo;
using Rg.Plugins.Popup.Services;

namespace AppQ4evo.Services
{
    public class AppService : ContentPage
    {
        public static CookieContainer CookieContainer = new CookieContainer();
        public HttpClientHandler handler;
        public HttpClient client;
        public static User info { get; set; }
        RootObject ro = new RootObject();
        public string NomeUser { get; set; }
        RootobjectCoord rootCord { get; set; }
        public Rota rrr { get; set; }
        public List<Rota> listRotas { get; set; }
        public List<contacto> listMensagens { get; set; }
        public static List<contacto> listMensagensauxcoord { get; set; }
        public static Rota rotaServico { get; set; }
        public static string AnexoEscolhido { get; set; }
        public static string viaturaNome;
        public List<Matricula> matriculasAux { get; set; }
        public List<Anexo> AnexoAux { get; set; }
        public List<Estat> estatisticaServico { get; set; }
        public List<User> UsersListMSG { get; set; }
        public string oldString = string.Empty;
        public static bool checkLogOut = false;

        public AppService()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            rrr = new Rota();
            listRotas = new List<Rota>();
            listMensagens = new List<contacto>();
            listMensagensauxcoord = new List<contacto>();
            rootCord = new RootobjectCoord();
            rotaServico = new Rota();
            matriculasAux = new List<Matricula>();
            AnexoAux = new List<Anexo>();
            UsersListMSG = new List<User>();
            estatisticaServico = new List<Estat>();      
        }

  
        public void SetRotaServiço(Rota value)
        {
            rotaServico = value;
            Application.Current.Properties["numero_assistencia"] = rotaServico.numero_assistencia;
        }

        public User GetInfoUser()
        {
            return info;
        }

        public bool GetCheckLogOut()
        {
            return checkLogOut;
        }
        public void SetCheckLogOut(bool value)
        {
            checkLogOut = value;
        }

        public void SetViaturNome(string value)
        {
            viaturaNome = value;
        }

        public List<contacto> GetListCoordMsg()
        {
            return listMensagensauxcoord;
        }
        
        public async Task LogOut()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            Application.Current.Properties["logStatus"] = "F";
            await App.Current.SavePropertiesAsync();
            var jsonObj = "";

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOLogOutGps.php"),
                    Method = HttpMethod.Post,
                    Content = content


                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootobjectMatri>(responseString);
                    checkLogOut = true;
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
            }
        }

        //Registo da empresa naquele dispositivo//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async void RegistoEmpresa(string user, string pass, string empresa, string matricula)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var codeDispositivo = CrossDeviceInfo.Current.Id;
            RssUser userInfos = new RssUser { user = user, pass = pass, company = empresa, matricula = matricula, codigo_dispositivo = codeDispositivo };
            ro.rssUser = userInfos;
            var jsonObj = JsonConvert.SerializeObject(ro);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");
                var authData = string.Format("{0}:{1}", user, pass);
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOLoginGpsADM.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                string dataResult = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(dataResult);
                    var error = obj.rssUser.error;

                    if (error != "0")
                    {
                        PopupNavigation.Instance.PushAsync(new ErrorPopUp("Algum dos dados introduzidos durante o registo não estão corretos, por favor repita"));
                    }
                    else
                    {
                        Application.Current.Properties["matricula"] = userInfos.matricula;
                        Application.Current.Properties["company"] = userInfos.company;
                        await Application.Current.SavePropertiesAsync();
                        var currentuserID = (Application.Current.Properties["matricula"].ToString()); //este chama Local DB
                    }
                }
                else
                {
                    throw new HttpRequestException("O seu Username ou Password estão incorretos");
                }
            }
        }

        public async Task<List<Matricula>> GetMatricula()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var jsonObj = "";

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOMatriculas.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootobjectMatri>(responseString);

                        foreach (Matricula m in obj.rssMatriculas.matriculasList.matriculas)
                        {
                            matriculasAux.Add(m);
                        }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return matriculasAux;
            }
        }
  
        //--------------------------------------------------------------LOGIN-----------------------------------------------------------------------------------------------------//

        public async Task<string> LoginAsync(string user, string pass)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var companhia = Application.Current.Properties["company"].ToString();
            RssUser userInfos = new RssUser { user = user, pass = pass, company = companhia };
            ro.rssUser = userInfos;
            var jsonObj = JsonConvert.SerializeObject(ro);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");
                var authData = string.Format("{0}:{1}", user, pass);
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                Application.Current.Properties["token"] = authHeaderValue;
                await Application.Current.SavePropertiesAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOLoginGps.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                string dataResult = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(dataResult);
                    var error = obj.rssUser.error;

                    if (error != "0")
                    {
                        PopupNavigation.Instance.PushAsync(new ErrorPopUp("O Username ou Password introduzidos estão incorretos"));
                        NomeUser = "";
                    }
                    else
                    {
                        info = new User
                        {
                            username = obj.rssUser.userList.user[0].username,
                            nome = obj.rssUser.userList.user[0].nome,
                            telefone = obj.rssUser.userList.user[0].telefone,
                            email = obj.rssUser.userList.user[0].email
                        };
                        string value = obj.rssUser.userList.user[0].username;
                        NomeUser = value;
                    }                                 
                }
                else
                {
                  await  DisplayAlert("Login Inválido", "Por favor verifique o username e/ou a password", "OK");
                }
                return NomeUser;
            }
        }

        public async Task GetTokenLogin()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var currentuserToken = (Application.Current.Properties["token"].ToString()); //este chama Local DB
            var currentuserCompany = (Application.Current.Properties["company"].ToString()); //este chama Local DB
            string p0 = Encoding.UTF8.GetString(Convert.FromBase64String(currentuserToken));
            string user = p0.Split(':')[0];
            string pass = p0.Split(':')[1];
            RssUser userInfos = new RssUser { user = user, pass = pass, company = currentuserCompany };
            ro.rssUser = userInfos;
            var jsonObj = JsonConvert.SerializeObject(ro);
            StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

            using (client)
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOLoginGps.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                string dataResult = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(dataResult);
                    info = new User
                    {
                        username = obj.rssUser.userList.user[0].username,
                        nome = obj.rssUser.userList.user[0].nome,
                        telefone = obj.rssUser.userList.user[0].telefone,
                        email = obj.rssUser.userList.user[0].email
                    };
                    string value = obj.rssUser.userList.user[0].username;
                    NomeUser = value;
                }
                else
                {
                    await DisplayAlert("Login Inválido", "Por favor verifique o username e/ou a password", "OK");
                }
            }
        }

        //--------------------------------------------------------------RECEBE INFO-----------------------------------------------------------------------------------------------------//

        public async Task<List<Rota>> GetInfo(string data)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootObjectR Rrsrotas = new RootObjectR();
            RssRotas rr = new RssRotas { data = data };
            if (rr.data != data)
            {
                Debug.WriteLine("nesta data não existem serviços");
            }
            RootObjectR raiz = new RootObjectR();
            raiz.rssRotas = rr;
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BORotas.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootObjectR>(responseString);
                    int i = 0;

                    foreach (Rota r in obj.rssRotas.rotasList.rotas)
                    {
                        listRotas.Add(obj.rssRotas.rotasList.rotas[i]);
                        i++;
                    }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return listRotas;
            }
        }

        //--------------------------------------------------------------UPDATE INFO-----------------------------------------------------------------------------------------------------//

        public async Task<List<Rota>> UpdateInfo(Rota rotaEdit)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootObjectR raiz = new RootObjectR { rssRotas = new RssRotas { rotasList = new RotasList { rota = rotaEdit } } };
            Debug.WriteLine(raiz);
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOUpdateRotas.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                return raiz.rssRotas.rotasList.rotas;
            }
        }

        //--------------------------------------------------------------MENSAGENS-----------------------------------------------------------------------------------------------------//

        public async Task<List<contacto>> GetMensagens(string datahora)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootobjectMensagens Rrsconstactos = new RootobjectMensagens();
            Rsscontactos rr = new Rsscontactos { dti = datahora };
            if (rr.dti != datahora)
            {
                Debug.WriteLine("nesta data não existem serviços");
            }

            RootobjectMensagens raiz = new RootobjectMensagens();
            raiz.rssContactos = rr;
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOContactosData.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootobjectMensagens>(responseString);
                    int i = 0;

                    foreach (contacto c in obj.rssContactos.contactosList.contactos)
                    {
                        listMensagens.Add(obj.rssContactos.contactosList.contactos[i]);
                        i++;
                    }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return listMensagens;
            }
        }

        //---------------------------------------------------------------------------------------- User's para mensagens --------------------------------------------------------------------------------------

        public async Task<List<User>> GetUserMsg()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var jsonObj = "";

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOUtiltzadores.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(responseString);
                    foreach (User u in obj.rssUser.userList.user)
                    {
                        UsersListMSG.Add(u);
                    }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return UsersListMSG;
            }
        }

        //---------------------------------------------- Enviar mensagem -------------------------------------------------------

        public async Task<List<contacto>> EnviarMensagemChat(string numIdMsg, List<Users> usersEnviar, string numParente, string descri, string dataHora)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootobjectMensagens raiz = new RootobjectMensagens
            {
                rssContactos = new Rsscontactos
                {
                    msg_id = numIdMsg,
                    contactosList = new ContactosList{
                        contactos = new List<contacto>()
                    }
                }
            };
            contacto contactoEnviarMsg = new contacto { users = usersEnviar, numero_parente = numParente, descricao = descri, data_hora = dataHora };
            raiz.rssContactos.contactosList.contactos.Add(contactoEnviarMsg);
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                  var request = new HttpRequestMessage()
                  {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOInsertContacto.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                return raiz.rssContactos.contactosList.contactos;
            }
        }

        public async Task GetLastIndex()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var jsonObj = "";

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOMaxIndex.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(responseString);
                    Application.Current.Properties["numero"] = obj.rssUser.index;
                    Application.Current.Properties["gps_distancia"] = obj.rssUser.gps_distancia;
                    Application.Current.Properties["gps_tempo"] = obj.rssUser.gps_tempo;
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
            }
        }
        public async Task<List<Anexo>> GetListAnexos(Rota rotaAnexos)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootObjectAnexos raiz = new RootObjectAnexos
            {
                rssAnexos = new RssAnexos
                {
                    anexosList = new AnexosList
                    { anexos = new List<Anexo> { new Anexo { ano_assistencia = rotaAnexos.ano_assistencia, numero_assistencia = rotaAnexos.numero_assistencia } } }
                }
            };
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOAnexosList.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootObjectAnexos>(responseString);
                    foreach (Anexo m in obj.rssAnexos.anexosList.anexos)
                    {
                        AnexoAux.Add(m);
                    }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return AnexoAux;
            }
        }

        public async Task<string> EscolheAnexo(Rota rotaAnexos, string nomeFicheiro)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootObjectAnexos raiz = new RootObjectAnexos
            {
                rssAnexos = new RssAnexos
                {
                    anexosList = new AnexosList
                    { anexos = new List<Anexo> { new Anexo { ano_assistencia = rotaAnexos.ano_assistencia, numero_assistencia = rotaAnexos.numero_assistencia, nome_ficheiro_anexo = nomeFicheiro} } }
                }
            };
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOAnexo.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootObjectAnexos>(responseString);
                    AnexoEscolhido = obj.rssAnexos.anexosList.anexos[0].anexo;
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
                return AnexoEscolhido;
            }
        }

        public async void EnviarFicheiro(Rota rotaAnexos, string nomeFicheiro, string anexoBase64)
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            RootObjectAnexos raiz = new RootObjectAnexos
            {
                rssAnexos = new RssAnexos
                {
                    anexosList = new AnexosList
                    { anexos = new List<Anexo> { new Anexo { ano_assistencia = rotaAnexos.ano_assistencia, numero_assistencia = rotaAnexos.numero_assistencia, nome_ficheiro_anexo = nomeFicheiro, anexo = anexoBase64 } } }
                }
            };
            var jsonObj = JsonConvert.SerializeObject(raiz);

            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOInsertAnexo.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<RootObjectAnexos>(responseString);
                    var error = obj.rssAnexos.error;
                    if (error == "0")
                    {
                        PopupNavigation.Instance.PushAsync(new ErrorPopUp("Ficheiro com o nome '" + nomeFicheiro + "' foi enviado com sucesso"));
                    }
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
            }
        }

        public async Task<List<Estat>>  GetMesServico()
        {
            handler = new HttpClientHandler { CookieContainer = AppService.CookieContainer };
            client = new HttpClient(handler);
            var jsonObj = "";
            using (client)
            {
                StringContent content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://evolus.ddns.net/Q4Evolution/php/phpGps/BOEstatistica.php"),
                    Method = HttpMethod.Post,
                    Content = content
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic obj = JsonConvert.DeserializeObject<Rootobject>(responseString);
                    foreach(Estat est in obj.rssUser.userList.user[0].estat)
                    {
                        estatisticaServico.Add(est);
                    }             
                }
                else
                {
                    throw new HttpRequestException("nada na lista");
                }
            }
            return estatisticaServico;
        }
    }
}


