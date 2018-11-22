using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopUpMoradas : PopupPage
    {
        public static ObservableCollection<Rota> Rote { get; set; }


        public PopUpMoradas (Rota rote)
        {

            Rote = new ObservableCollection<Rota>
            {
                 rote
            };

            InitializeComponent();
            Inicio.Text ="Partida: " + Rote[0].carga_local;
            Fim.Text ="Chegada: "+ Rote[0].descarga_local;
        }

        private async Task TapGestureRecognizer_Tapped_Inicio(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = "Portugal",
                AdminArea = Rote[0].carga_local
            };
            var options = new MapsLaunchOptions { MapDirectionsMode = MapDirectionsMode.Driving };
            await Maps.OpenAsync(placemark, options);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void TapGestureRecognizer_Tapped_Fim(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = "Portugal",
                AdminArea = Rote[0].descarga_local
            };
            var options = new MapsLaunchOptions { MapDirectionsMode = MapDirectionsMode.Driving };
            await Maps.OpenAsync(placemark, options);
            await PopupNavigation.Instance.PopAsync();

        }
    }
}