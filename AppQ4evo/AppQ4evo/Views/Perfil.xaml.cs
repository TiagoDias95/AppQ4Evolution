using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Perfil : PopupPage
    {
        public AppService log = new AppService();
		public Perfil ()
		{
			InitializeComponent ();
            User info = new User();
            info = log.GetInfoUser();
            UserN.Text = info.username;
            Nome.Text = info.nome;
            Telefone.Text = info.telefone;
            Email.Text = info.email;
		}

        private async void ButtonOK_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}