using AppQ4evo.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmarViatura : ContentPage
	{
        public AppService log = new AppService();
        public GPSCoord gp = new GPSCoord();
        public static string nomeUserr;
		public ConfirmarViatura (string nomeUser)
		{
            InitializeComponent ();
            matriculaSaved.Text = GetAtualMatriculaDispositivo();
            nomeUserr = nomeUser;
            log.SetCheckLogOut(false);      
        }

        private void sim_Clicked(object sender, EventArgs e)
        {
             Application.Current.MainPage = new MainMenu();
            DependencyService.Get<IGPSService>().GetGpsStart();
        }

        private async void nao_Clicked(object sender, EventArgs e)
        {
            var x =  await log.GetMatricula();
            await Navigation.PushAsync(new PopUpViaturas(x,nomeUserr));
            DependencyService.Get<IGPSService>().GetGpsStart();
        }

        public string GetAtualMatriculaDispositivo()
        {
            var currentuserID = (Application.Current.Properties["matricula"].ToString()); //este chama Local DB
            return currentuserID;
        }
    }
}