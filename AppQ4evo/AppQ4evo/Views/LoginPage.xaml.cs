using AppQ4evo.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public AppService _log = new AppService();

        public LoginPage()
        {
            InitializeComponent();
        }     

        public async Task Button_ClickedAsync()
        {
            await _log.LoginAsync(user.Text, pass.Text);
            string j =_log.NomeUser;
            Application.Current.Properties["user"] = user.Text;
            Application.Current.Properties["pass"] = pass.Text;
            Application.Current.Properties["numero"] = "n";
            Application.Current.Properties["gps_distancia"] = "d";
            Application.Current.Properties["gps_tempo"] = "t";
            Application.Current.Properties["numero_assistencia"] = "0";
            var numeroMemoriaInterna = (Application.Current.Properties["numero"].ToString());

            if (numeroMemoriaInterna == "n" && j !="" )
            {
                await   _log.GetLastIndex();
                await Application.Current.SavePropertiesAsync();
                await Navigation.PushAsync(new ConfirmarViatura(j));
            }
        }

        private async void loginButton_ClickedAsync(object sender, EventArgs e)
        {
            string j = _log.NomeUser;           
            await Button_ClickedAsync();
        }

        private async void RegistEmpre_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistoEmpresa());
        }

        private  void lembrarSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if(lembrarSwitch.IsToggled == true)
            {
                Application.Current.Properties["logStatus"] = "T";
            }
        }
    }
}
