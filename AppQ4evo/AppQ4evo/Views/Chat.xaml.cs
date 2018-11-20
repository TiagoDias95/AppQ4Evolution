using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Chat : ContentPage
	{
        public AppService log = new AppService();
        public List<contacto> msg { get; set; }
        ObservableCollection<contacto> listSmsNumParente { get; set; }
        public List<Users> us { get; set; }
        public string parenteAPassar { get; set; } 

        public Chat(ObservableCollection<contacto> listSms, string numID)
		{
            InitializeComponent();
            listSmsNumParente = new ObservableCollection<contacto>();
            us = new List<Users>();

            foreach (contacto c in listSms)
            {
                if(c.numero == numID || c.numero_parente == numID)
                {
                    listSmsNumParente.Add(c);
                }
            }
            listMensagens.ItemsSource = listSmsNumParente.OrderBy(item => item.numero);
        }

        public void GetUsersConversa()
        {      
                foreach(Users u in listSmsNumParente[0].users)
                {
                    us.Add(new Users { user = u.user });                   
                }            
        }

        private async void Button_Clicked_Mandar(object sender, EventArgs e)
        {
            string DataDezDias = "";
            var currentIdMsg = (Application.Current.Properties["numero"].ToString()); //este chama Local DB
            DateTime now = DateTime.Today;
            int dia = now.Day;
            int mes = now.Month;
            int ano = now.Year;
            var time = DateTime.Now.ToString("HH:mm:ss");
            GetUsersConversa();
            string format = "" + dia + "." + mes + "." + ano + " " + time + "";
            int minNum = int.MaxValue;

            foreach (contacto cc in listSmsNumParente)
            {
                if(int.Parse(cc.numero) < minNum)
                {
                    minNum = int.Parse(cc.numero);
                }
            }

            string ParenteNumMin = "" + minNum;

            if(EnviarText.Text == null)
            {
                PopupNavigation.PushAsync(new ErrorPopUp("A mensagem não tem texto"));

            }else if (us.Count == 0)
            {
                PopupNavigation.PushAsync(new ErrorPopUp("Deve colocar destinatário/os na mensagem"));
            }
            else
            {
                await log.EnviarMensagemChat(currentIdMsg, us, ParenteNumMin, EnviarText.Text, format);
                var vUpdatedPage = new Mensagens(format, DataDezDias);
                Navigation.InsertPageBefore(vUpdatedPage, this);
                Navigation.PopAsync();
                Navigation.PopAsync();
            }
        }
    }
}