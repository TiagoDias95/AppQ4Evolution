using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorPopUp : PopupPage
    {
		public ErrorPopUp (string erroQual)
		{
            InitializeComponent();
            errorText.Text = erroQual;

            if (CrossConnectivity.Current.IsConnected)
            {
                ButtonError.IsEnabled = true;
            }
            else
            {
                ButtonError.IsEnabled = false;
            }
        }

        private async Task Button_Clicked_Error(object sender, EventArgs e) 
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}