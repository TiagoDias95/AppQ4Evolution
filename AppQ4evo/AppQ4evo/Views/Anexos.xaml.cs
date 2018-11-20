using AppQ4evo.Services;
using AppQ4evo.ViewModels;
using Plugin.FilePicker;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Content;

namespace AppQ4evo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Anexos  : ContentPage 
	{
        AppService log = new AppService();
        public static Rota aux { get; set; }
        public static List<Anexo> anexoList { get; set; }

		public Anexos (Rota r)
		{
            InitializeComponent();
            aux = r;
            anexoList = new List<Anexo>();
            GetAnexos();
            List<string> anexosNome = new List<string>();
            int i = 0;
            foreach (Anexo m in anexoList)
            {
                Anexo aux = new Anexo
                {
                    nome_ficheiro_anexo = anexoList[i].nome_ficheiro_anexo,
                    codigo_tipo = anexoList[i].codigo_tipo
                };
                string juntar = ""+aux.nome_ficheiro_anexo+"."+aux.codigo_tipo+"";
                anexosNome.Add(juntar);
                i++;
            }
            ListAnexos.ItemsSource = anexosNome;           
        }

        public void GetAnexos()
        {
            anexoList = Task.Run(() => log.GetListAnexos(aux)).Result;
        }

        private async void _button_Clicked_File(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();
            var nomeFicheiro = file.FileName;  
            string toBase64 = Convert.ToBase64String(file.DataArray);
            log.EnviarFicheiro(aux, nomeFicheiro,toBase64);
        }

        private async Task ListAnexos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    var nomeFicheiroSelecionado = ListAnexos.SelectedItem.ToString();
                    var nomeEscolhido = nomeFicheiroSelecionado.Split('.')[0];
                    var anexo = Task.Run(() => log.EscolheAnexo(aux, nomeEscolhido)).Result;
                    var bytes = Convert.FromBase64String(anexo);
                    var directory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                    var file = Path.Combine(directory.ToString(), nomeFicheiroSelecionado);

                    using (FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }

                    var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads + "/" + nomeFicheiroSelecionado;
                    Java.IO.File filet = new Java.IO.File(externalPath);
                    filet.SetReadable(true);
                    string application = "";
                    string extension = Path.GetExtension(file);

                    switch (extension.ToLower())
                    {
                        case ".txt":
                            application = "text/plain";
                            break;
                        case ".doc":
                        case ".docx":
                            application = "application/msword";
                            break;
                        case ".pdf":
                            application = "application/pdf";
                            break;
                        case ".xls":
                        case ".xlsx":
                            application = "application/vnd.ms-excel";
                            break;
                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                            application = "image/jpeg";
                            break;
                        default:
                            application = "*/*";
                            break;
                    }

                       Android.Net.Uri uri = Android.Net.Uri.FromFile(filet);
                    // Android.Net.Uri uri = DependencyService.Get<FileDevice>().GetFileProviderWorking(filet);

                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetFlags(ActivityFlags.GrantPersistableUriPermission);
                    intent.SetDataAndType(uri, application);
                    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);                  
                }
                else if (status != PermissionStatus.Unknown)
                {
                    PopupNavigation.PushAsync(new ErrorPopUp("Acesso negado. Tente novamente"));
                }
            }
        catch (Exception ex)
            {

                PopupNavigation.PushAsync(new ErrorPopUp("Erro: " + ex));
            }
        } 
    }    
}