using Java.IO;
using Android.Support.V4.Content;
using AppQ4evo.Services;
using AppQ4evo.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(FileAndroid))]
namespace AppQ4evo.Droid
{
    public class FileAndroid : FileDevice
    {
       public Android.Net.Uri GetFileProviderWorking(File file)
        {
            Android.Net.Uri uri = FileProvider.GetUriForFile(Android.App.Application.Context, "com.companyname.AppQ4evo.fileprovider", file);
            return uri;
        }
    }
}