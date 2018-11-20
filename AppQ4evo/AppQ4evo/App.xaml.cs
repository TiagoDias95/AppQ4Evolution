using AppQ4evo.Services;
using AppQ4evo.Views;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppQ4evo
{
    public partial class App : Application
    {
        public AppService lp = new AppService();
        public App()
        {
            Application.Current.Properties["backgroundCheck"] = "F";
            InitializeComponent();
            if (!Application.Current.Properties.ContainsKey("logStatus"))
            {
                Application.Current.Properties["logStatus"] = "F";
            }
            var statusLog = Application.Current.Properties["logStatus"].ToString();
            if (statusLog == "F")
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var userStore = (Application.Current.Properties["user"].ToString());
                Task.Run(() => lp.GetTokenLogin()).Wait();
                MainPage = new NavigationPage(new ConfirmarViatura(userStore));
            }
        }

        protected override void OnStart()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    PopupNavigation.Instance.PushAsync(new ErrorPopUpNET("Neste momento está sem dados móveis, aguarde"));
                }
                else
                {
                    PopupNavigation.Instance.PopAsync();
                }
            };
        }

        protected override void OnSleep()
        {
            Application.Current.SavePropertiesAsync();
        }

        protected override void OnResume()
        {

        }
    }
}
