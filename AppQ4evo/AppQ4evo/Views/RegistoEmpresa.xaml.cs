using AppQ4evo.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistoEmpresa : ContentPage
	{
        public AppService _log = new AppService();

        public RegistoEmpresa()
		{
			InitializeComponent ();
		}

        private async void ResgistoButton_clicked(object sender, EventArgs e)
        {
             _log.RegistoEmpresa(user.Text, pass.Text, empresa.Text, matricula.Text);
            await Application.Current.SavePropertiesAsync();
            await Navigation.PushAsync(new LoginPage());
        }
    }
}