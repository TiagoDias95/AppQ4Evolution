using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopKm : PopupPage
    {
        public AppService log = new AppService();
        public static ObservableCollection<Rota> RoteStart { get; set; }

        public PopKm(Rota popRote)
        {
            RoteStart = new ObservableCollection<Rota>
            {
                 popRote
            };
            log.SetRotaServiço(popRote);
            InitializeComponent();
        }

        private async Task Button_Clicked_Iniciar(object sender, EventArgs e)
        {
            var user = (Application.Current.Properties["user"].ToString());
            var currentMatr = (Application.Current.Properties["matricula"].ToString()); //este chama Local DB

            if (RoteStart[0].carga_kms == "")
            {
                if (RoteStart[0].matricula != currentMatr)
                {
                    PopupNavigation.Instance.PushAsync(new ErrorPopUp("Este serviço requer o veiculo com a matricula " + RoteStart[0].matricula + ", mude de veiculo na página inicial"));
                }
                else
                {
                    RoteStart[0].carga_kms = KmIn.Text;
                    await log.UpdateInfo(RoteStart[0]);
                    await PopupNavigation.Instance.PopAsync();
                    var menu = new MainMenu();
                    menu.Detail = new NavigationPage(new Servicos(""));
                    Application.Current.MainPage = menu;
                }
            }
            else
            {
                if (Int32.Parse(KmIn.Text) < Int32.Parse(RoteStart[0].carga_kms))
                {
                    PopupNavigation.Instance.PushAsync(new ErrorPopUp("O número de Km's inseridos é menor do que o número Km's do inicio do serviço"));
                }
                else
                {
                    RoteStart[0].descarga_kms = KmIn.Text;
                    await Task.Run(() => log.UpdateInfo(RoteStart[0]));
                    RoteStart[0].numero_assistencia = "0";
                    Application.Current.Properties["numero_assistencia"] = "0";
                    var numeroKmServico = Int32.Parse(RoteStart[0].descarga_kms) - Int32.Parse(RoteStart[0].carga_kms);
                    Application.Current.Properties["numeroKmServico"] = numeroKmServico.ToString();
                    await Application.Current.SavePropertiesAsync();
                    await PopupNavigation.Instance.PopAsync();
                    var menu = new MainMenu();
                    menu.Detail = new NavigationPage(new Servicos(""));
                    Application.Current.MainPage = menu;
                }
            }
        }
    }
}