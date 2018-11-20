using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpViaturas : ContentPage
    {
        public AppService log = new AppService();
        private Picker _picker;
        private Button _button;
        public static string nomeUseraux;

        public PopUpViaturas (List<Matricula> x, string nomeUser)
		{
            nomeUseraux = nomeUser;
            this.Title = "Viaturas";
            List<string> viaturas = new List<string>();

            int i = 0;
            foreach (Matricula m in x)
            {
                Matricula aux = new Matricula
                {
                    matricula = x[i].matricula,
                };
                viaturas.Add(aux.matricula);
                i++;
            }

            Grid g = new Grid();
            g.RowSpacing = 0;

            RowDefinition one = new RowDefinition();
            RowDefinition two = new RowDefinition();
            RowDefinition tree = new RowDefinition();
            one.Height = 270;
            two.Height = 40;
            tree.Height = GridLength.Auto;
            g.RowDefinitions.Add(one);
            g.RowDefinitions.Add(two);
            g.RowDefinitions.Add(tree);

            Image img1 = new Image();
            img1.Source = "Entrada.jpg";
            img1.Aspect = Aspect.AspectFill;
            Grid.SetRowSpan(img1, 1);
           
            Image img2 = new Image();
            img2.Source = "carW.png";
            Grid.SetRowSpan(img2, 1);

            BoxView viewBox = new BoxView();
            viewBox.BackgroundColor = Color.LightSteelBlue;
            viewBox.Opacity = 0.8;
            Grid.SetRowSpan(viewBox, 1);

            StackLayout sl = new StackLayout();
            _picker = new Picker();
            _picker.Title = "Escolha a Viatura";
            _picker.ItemsSource = viaturas;
            sl.Children.Add(_picker);

            _button = new Button();
            _button.Text = "OK";
            _button.TextColor = Color.White;
            _button.FontAttributes = FontAttributes.Bold;
            _button.Clicked += _button_Clicked_viatura;
            sl.Children.Add(_button);

            Grid.SetRow(sl, 2);

            g.Children.Add(img1, 0, 0);
            g.Children.Add(img2, 0, 0);
            g.Children.Add(viewBox, 0, 0);

            g.Children.Add(sl, 0, 2);

            ScrollView sv = new ScrollView { Orientation = ScrollOrientation.Vertical };
            sv.Content = g;

            Content = sv;
        }

        private async void _button_Clicked_viatura(object sender, EventArgs e)
        {
            if (_picker.SelectedItem == null)
            {
                PopupNavigation.Instance.PushAsync(new ErrorPopUp("Tem de escolher uma viatura/matricula"));
            }
            else
            {
                Application.Current.Properties["matricula"] = _picker.SelectedItem.ToString();
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainMenu();
            }
        }
    }
}