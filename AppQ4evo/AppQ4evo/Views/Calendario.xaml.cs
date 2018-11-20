using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Calendario : PopupPage
    {
        public string datacal;
        public static string passagemValor;

        public Calendario ()
		{
            setV(".");
            InitializeComponent();
            if (datacal == "." && passagemValor != null)
            {
                setV(passagemValor);
            }
        }

        public void setV(string value)
        {
            datacal = value;
        }
        public void setVpassagem(string value)
        {
            passagemValor = value;
        }

        private async Task MainDatePicker_DateSelectedAsync(object sender, DateChangedEventArgs e)
        {
            MainLabel.Text = e.NewDate.ToLongDateString();
            int dia = e.NewDate.Day;
            int mes = e.NewDate.Month;
            int ano = e.NewDate.Year;
            string format = "" + dia + "." + mes + "." + ano + "";
            setV(format);
            setVpassagem(datacal);
            await PopupNavigation.Instance.PopAsync();
            var menu = new MainMenu();
            menu.Detail = new NavigationPage(new Servicos(datacal));
            Application.Current.MainPage = menu;
        }
    }
}