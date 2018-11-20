using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Servicos : ContentPage
    {
        public static Rota r;

        public Servicos (string data)
		{
            r = new Rota();
            InitializeComponent();            
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = BindingContext as ServicosMenu;
            var ser = e.Item as Rota;         
            vm.HideOrShowService(ser);
           setr(ser);          
        }

        public void setr(Rota value)
        {
            r = value;
        }     

        private async Task Button_Clicked_DetalhesAsync(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Detalhes(r));
        }
 
        private async Task Button_Clicked_AnexosAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Anexos(r));
        }

        private async Task Calendario_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Calendario());
        }    
    }
}