using AppQ4evo.ViewModels;
using Xamarin.Forms;

namespace AppQ4evo.Services
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FromTemplate { get; set; }
        public DataTemplate ToTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((contacto)item).Status.ToUpper().Equals("SENT") ? FromTemplate : ToTemplate;
        }
    }
}
