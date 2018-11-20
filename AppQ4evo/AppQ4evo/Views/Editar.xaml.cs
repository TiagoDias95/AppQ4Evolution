using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Editar : ContentPage
	{
        public AppService log = new AppService();
        public static ObservableCollection<Rota> RoteEdit { get; set; }

        public Editar (Rota edit)
		{
            RoteEdit = new ObservableCollection<Rota>
            {
                edit
            };
            InitializeComponent();
            BindingContext = RoteEdit;
        }

        private async Task Button_Clicked_SaveAsync(object sender, EventArgs e)
        {
            await log.UpdateInfo(RoteEdit[0]);
            Navigation.RemovePage(this);
            var menu = new MainMenu();
            menu.Detail = new NavigationPage(new Servicos(""));
            Application.Current.MainPage = menu;
        }
    }
}