using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Detalhes : ContentPage
	{
        public static ObservableCollection<Rota> Rote { get; set; }

        public Detalhes (Rota det)
		{
            Rote = new ObservableCollection<Rota>
            {
                det
            };

            Rote[0].IsVisible = true;

            if (Rote[0].carga_kms != "")
            {
                Rote[0].IsVisible = false;
                Rote[0].IsVisiblee = true;
            }

            if (Rote[0].descarga_kms != "")
            {
                Rote[0].IsVisible = false;
                Rote[0].IsVisiblee = false;
            }

            InitializeComponent();
            BindingContext = Rote;
        }

        private async Task OnEdit_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Editar(Rote[0]));
        }

        private async Task Button_Clicked_Start(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopKm(Rote[0]));
            Rote[0].IsVisible = false;
            Rote[0].IsVisiblee = true;
        }

        private async Task Button_Clicked_Fechar(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopKm(Rote[0]));
            Rote[0].IsVisiblee = false;
        }

        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private void TapGestureRecognizer_Tapped_Telefone(object sender, EventArgs e)
        {
            PlacePhoneCall(Rote[0].contacto_telefone);
        }

        private async void TapGestureRecognizer_Tapped_Carga(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = "Portugal",
                AdminArea = Rote[0].carga_local
            };
            var options = new MapsLaunchOptions { MapDirectionsMode = MapDirectionsMode.Driving };
            await Maps.OpenAsync(placemark, options);
        }

        private async void TapGestureRecognizer_Tapped_Descarga(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = "Portugal",
                AdminArea = Rote[0].descarga_local              
            };
            var options = new MapsLaunchOptions { MapDirectionsMode = MapDirectionsMode.Driving };
            await Maps.OpenAsync(placemark, options);
        }
    }
}