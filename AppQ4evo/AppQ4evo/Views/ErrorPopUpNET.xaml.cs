using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorPopUpNET : PopupPage
    {
		public ErrorPopUpNET (string erroNet)
		{
            InitializeComponent();
            errorText.Text = erroNet;
		}

        private void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                CloseWhenBackgroundIsClicked = false;
            }
            else
            {
                CloseWhenBackgroundIsClicked = true;
            }
        }
    }
}