using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnviarMensagem : ContentPage
    {
        public AppService log = new AppService();
        public List<Users> us { get; set; }
        public static List<contacto> listCopy { get; set; }
        public int countN = 3;
        public Grid gd;
        private Label lbTitle;
        private Picker _picker;
        private Label _label;
        private Label _dest;
        private Editor _editor;
        private Button _button;

        public EnviarMensagem (List<User> userList, ObservableCollection<contacto> listSms)
		{
            us = new List<Users>();
            listCopy = new List<contacto>();
            
            foreach(contacto aux in listSms)
            {
                listCopy.Add(aux);
            }

            List<string> users = new List<string>();
            int i = 0;
            foreach (User u in userList)
            {
                User aux = new User
                {
                    username = userList[i].username,
                    nome = userList[i].nome
                };
                users.Add(aux.username);
                i++;
            }
            Application.Current.MainPage.BackgroundColor = Color.White;

            gd = new Grid();
            gd.BackgroundColor = Color.White;
            RowDefinition rd1 = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();
            RowDefinition rd3 = new RowDefinition();
            RowDefinition rd4 = new RowDefinition();
            RowDefinition rd5 = new RowDefinition();
            RowDefinition rd6 = new RowDefinition();

            rd1.Height = GridLength.Auto;
            rd2.Height = GridLength.Auto;
            rd3.Height = GridLength.Auto;
            rd4.Height = GridLength.Auto;
            rd5.Height = GridLength.Auto;
            rd6.Height = GridLength.Auto;

            gd.RowDefinitions.Add(rd1);
            gd.RowDefinitions.Add(rd2);
            gd.RowDefinitions.Add(rd3);
            gd.RowDefinitions.Add(rd4);
            gd.RowDefinitions.Add(rd5);
            gd.RowDefinitions.Add(rd6);

            lbTitle = new Label(); _label = new Label();
            lbTitle.Text = "Iniciar Conversa";
            lbTitle.TextColor = Color.White;
            lbTitle.BackgroundColor = Color.DimGray;
            lbTitle.HorizontalTextAlignment = TextAlignment.Center;
            lbTitle.VerticalTextAlignment = TextAlignment.Center;
            lbTitle.FontSize = 25;
            lbTitle.FontAttributes = FontAttributes.Bold;
            Grid.SetRow(lbTitle,0);
           
            _picker = new Picker();
            _picker.Title = "Escolha o/os destinatários";
            _picker.ItemsSource = users;
            _picker.SelectedIndexChanged += _picker_SelectedIndexChanged;
            Grid.SetRow(_picker,1);
     
            _editor = new Editor();
            _editor.HeightRequest = 150;
            Grid.SetRow(_editor, 4);

            _button = new Button();
            _button.BackgroundColor = Color.LightSlateGray;
            _button.HeightRequest = 42;
            _button.Opacity = 0.8;
            _button.TextColor = Color.White;
            _button.FontAttributes = FontAttributes.Bold;
            _button.CornerRadius = 10;
            _button.Margin = 2;
            _button.Text = "Enviar";
            _button.Clicked += Button_Clicked_Mandar;
            Grid.SetRow(_button, 5);

            gd.Children.Add(lbTitle, 0, 0);
            gd.Children.Add(_picker, 0, 1);
            gd.Children.Add(_editor, 0, 4);
            gd.Children.Add(_button, 0, 5);

            Content = gd;
        }

        private void _picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            us.Add(new Users { user = _picker.SelectedItem.ToString() });
            _dest = new Label();
            _dest.Text = "Destinatários: ";
            _dest.TextColor = Color.Black;
            _dest.FontAttributes = FontAttributes.Bold;
            _label.Text +=  _picker.SelectedItem.ToString()+", ";
            _label.TextColor = Color.Black;
            if(_label.Text != "")
            {
                Grid.SetRow(_dest, 2);
                gd.Children.Add(_dest, 0,2);
                Grid.SetRow(_label, 3);
                gd.Children.Add(_label, 0, 3);
            }
        }


        private async void Button_Clicked_Mandar(object sender, EventArgs e)
        {
            string DataDezDias = "";
            var currentIdMsg = (Application.Current.Properties["numero"].ToString()); //este chama Local DB
            DateTime now = DateTime.Today;
            int dia = now.Day;
            int mes = now.Month;
            int ano = now.Year;
            var time = DateTime.Now.ToString("HH:mm:ss");
            string format = "" + dia + "." + mes + "." + ano + " " + time + "";
            string parenteAPassar = "";

            if (_editor.Text == null)
            {
                PopupNavigation.Instance.PushAsync(new ErrorPopUp("A mensagem não tem texto"));
            }
            else if (us.Count == 0)
            {
                PopupNavigation.Instance.PushAsync(new ErrorPopUp("Deve colocar destinatário/os na mensagem"));
            }
            else
            {
                await log.EnviarMensagemChat(currentIdMsg, us, parenteAPassar, _editor.Text, format);
                var menu = new MainMenu();
                menu.Detail = new NavigationPage(new Mensagens(format, DataDezDias));
                Application.Current.MainPage = menu;
            }
        }
    }
}