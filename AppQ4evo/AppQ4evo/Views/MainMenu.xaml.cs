using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : MasterDetailPage
	{
        AppService log = new AppService();
        public List<MainMenuItem> MainMenuItems { get; set; }
        public List<MainMenuItem> LogOutList { get; set; }
        public string username = (Application.Current.Properties["user"].ToString());

        public MainMenu()
        {
            BindingContext = this;
            this.MasterBehavior = MasterBehavior.Popover;
            LogOutList = new List<MainMenuItem>()
            {
                new MainMenuItem(){Title = "LogOut", Icon = "Logout_b.png"}
            };

            MainMenuItems = new List<MainMenuItem>()
            {
                new MainMenuItem(){Title = "Inicio", Icon ="Home_icon.png", TargetType = typeof(Menu)},
                new MainMenuItem(){Title = "Mensagens", Icon ="commonImage.png", TargetType = typeof(Mensagens)},
                new MainMenuItem(){Title = "Serviços", Icon ="commonImage2.png", TargetType = typeof(Servicos)}
            };

            Detail = new NavigationPage(new Menu(username));
            InitializeComponent();
            UserName.Text = username;
        }

        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            string DataDezDias = "";
            DateTime now = DateTime.Today;
            int dia = now.Day;
            int mes = now.Month;
            int ano = now.Year;
            var time = DateTime.Now.ToString("HH:mm:ss");
            string format = "" + dia + "." + mes + "." + ano + " " + time + "";
            var item = e.SelectedItem as MainMenuItem;

            if (item != null)
            {
                if (item.Title.Equals("Mensagens"))
                {
                    Detail = new NavigationPage(new Mensagens(format, DataDezDias));
                }
                else if (item.Title.Equals("Serviços"))
                {
                    Detail = new NavigationPage(new Servicos(""));
                }else if (item.Title.Equals("Inicio"))
                {
                    Detail = new NavigationPage(new Menu(username));
                }
                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async void LogOutList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await log.LogOut();
            DependencyService.Get<IGPSService>().StopGPS();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }     
    }
}