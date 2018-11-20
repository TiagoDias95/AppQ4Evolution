using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mensagens : ContentPage, INotifyPropertyChanged
    {
        public AppService log = new AppService();
        public List<contacto> sms = new List<contacto>();
        public List<string> envolvidos = new List<string>();
        public static contacto r;
        public static ObservableCollection<contacto> ListSms { get; set; }
        public static ObservableCollection<contacto> ListSmsFilter { get; set; }
        public static ObservableCollection<contacto> ListFinal { get; set; }
        public static ObservableCollection<contacto> ListMensagensRecentesDisplay { get; set; }
        private readonly HashSet<int> hashes = new HashSet<int>();
        public static string dataPassagem { get; set; }

        public Mensagens(string datahora, string dataDezDias)
        {
            if (dataDezDias == "")
            {
                DateTime defaultDate = DateTime.Today.AddDays(-10);
                var d = defaultDate.Day;
                var m = defaultDate.Month;
                var y = defaultDate.Year;
                var time = "00:00:00";
                dataPassagem = "" + d + "." + m + "." + y + " " + time + "";
            }
            else
            {
                dataPassagem = datahora;
            }

            r = new contacto();
            ListSms = new ObservableCollection<contacto>();
            ListSmsFilter = new ObservableCollection<contacto>();
            ListFinal = new ObservableCollection<contacto>();
            ListMensagensRecentesDisplay = new ObservableCollection<contacto>();
            Task.Run(() => getMen());
            InitializeComponent();
            BindingContext = ListMensagensRecentesDisplay;
        }

        public void setr(contacto value)
        {
            r = value;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public async void getMen()
        {
            var currentuserUser = (Application.Current.Properties["user"].ToString());
            sms = await log.GetMensagens(dataPassagem);
            int i = 0;

            foreach (contacto c in sms)
            {
                foreach (Users us in sms[i].users)
                {
                    envolvidos.Add(us.user);
                }
                string combindedString = string.Join(", ", envolvidos.ToArray());
                contacto x = new contacto
                {
                    users = sms[i].users,
                    userEscrito = combindedString,
                    numero = sms[i].numero,
                    numero_parente = sms[i].numero_parente,
                    descricao = sms[i].descricao,
                    data_hora = sms[i].data_hora,
                    users_in = sms[i].users_in,
                };

                if (x.users_in == currentuserUser.ToUpper())
                {
                    x.Status = "Sent";
                }
                else
                {
                    x.Status = "Received";
                }

                ListSms.Add(x);
                i++;
                combindedString = "";
                envolvidos.Clear();
            }
            GetListMensagem();
        }

        public void GetListMensagem()
        {
            int i = 0;

            for (int y = 0; y < ListSms.Count; y++)
            {
                foreach (contacto c in ListSms.Where(d => d.numero_parente == ListSms[y].numero))
                {
                    if (c.numero == ListSms[y].numero || c.numero_parente == ListSms[y].numero)
                    {
                        ListFinal.Add(c);
                    }

                }
            }
            var res = ListFinal.GroupBy(ii => ii.numero_parente)
                     .Select(fd => fd.OrderByDescending(ii => ii.numero)
                     .First()).ToList();

            foreach (contacto co in ListSms)
            {
                if (co.numero_parente == "")
                {
                    if (res.Exists(f => f.numero_parente == co.numero))
                    {
                        ListSmsFilter.Add(co);
                    }
                    else
                    {
                        res.Add(co);
                    }
                }
            }

            foreach (contacto final in res)
            {
                UpdateSource(ListMensagensRecentesDisplay, final);
            }
        }

        public void UpdateSource(ObservableCollection<contacto> source, contacto newItem)
        {
            source.Add(newItem);
            SortSource(source, newItem);
        }

        private void SortSource(ObservableCollection<contacto> source, contacto item)
        {
            var oldIndex = source.IndexOf(item);
            var list = source.OrderBy(_ => _.numero).ToList();
            var newIndex = list.IndexOf(item);
            source.Move(oldIndex, newIndex);
        }

        public async void ListView_ItemTapped_AbrirChat(object sender, ItemTappedEventArgs e)
        {
            var msg = e.Item as contacto;

            if (msg.numero_parente == "")
            {
                setr(msg);
                await Navigation.PushAsync(new Chat(ListSms, msg.numero));
            }
            else
            {
                setr(msg);
                await Navigation.PushAsync(new Chat(ListSms, msg.numero_parente));
            }
        }

        private async void icon_Clicked_Send_Mens(object sender, EventArgs e)
        {
            var usersMSG = await log.GetUserMsg();
            await Navigation.PushAsync(new EnviarMensagem(usersMSG, ListSms));
        }

        private async void iconCalendario_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new CalendarioTime());
        }
    }
}
