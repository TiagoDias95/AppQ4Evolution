using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using AppQ4evo.Views;
using Microcharts;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
namespace AppQ4evo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public AppService log = new AppService();
        public static List<Estat> stats { get; set; }
        public ConfirmarViatura cv {get;set;}

        public Menu (string username)
		{
            
            InitializeComponent();
            GetAnexos();
            NomeUser.Text = username;
            cv = new ConfirmarViatura(username); 
        }

        public void GetAnexos()
        {
            stats = Task.Run(() => log.GetMesServico()).Result;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var matricula = (Application.Current.Properties["matricula"].ToString());
            var entries = new List<Entry>();
            Random rnd = new Random();

            foreach (Estat auxEst in stats)
            {
                int n = rnd.Next(0, 9);
                var color = String.Format("#{0:X6}", rnd.Next(0x1000000));
                Entry mes = new Entry(float.Parse(auxEst.count))
                {
                    Label = auxEst.mes.Replace(" ",""),
                    ValueLabel = "" + float.Parse(auxEst.count),
                    TextColor = SKColor.Parse("#000000"),
                    Color = SKColor.Parse(color)
                };               
                entries.Add(mes);
            }

            MatriculaMenu.Text = matricula;
            if (!Application.Current.Properties.ContainsKey("numeroKmServico"))
            {
                Application.Current.Properties["numeroKmServico"] = "Por efetuar";

            }

            var numeroKmUltimoServ = (Application.Current.Properties["numeroKmServico"].ToString());
            numeroKm.Text = numeroKmUltimoServ;
            var chart = new DonutChart() { Entries = entries };            
            this.ChartServicos.Chart = chart;
            this.ChartServicos.Chart.LabelTextSize = 23;

        }

        private async void NomeUser_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Perfil());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var x = await log.GetMatricula();
            await Navigation.PushAsync(new PopUpViaturas(x, ""));
        }
    }
}