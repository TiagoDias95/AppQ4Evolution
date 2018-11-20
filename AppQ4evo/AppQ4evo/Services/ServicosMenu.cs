using AppQ4evo.ViewModels;
using AppQ4evo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace AppQ4evo.Services
{
    public class ServicosMenu : BindableObject
    {
        private Rota _oldSer;
        public AppService lo = new AppService();
        public ObservableCollection<Rota> ListSer { get; set; }
        public ObservableCollection<Rota> OneSer { get; set; }//foi introduido pra testar 1 so servico
        public List<Rota> y = new List<Rota>();
        public RssRotas rts = new RssRotas();
        public Calendario ccc = new Calendario();

        public ServicosMenu()
        {
            rts.data = ccc.datacal;
            GetListSerAsync();
            ListSer = new ObservableCollection<Rota>();
            OneSer = new ObservableCollection<Rota>();
        }

        public void settt(string value)
        {
            rts.data = value;
        }

        void InitProperties(object obj)
        {
            foreach (var prop in obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite))
            {
                var type = prop.PropertyType;
                var constr = type.GetConstructor(Type.EmptyTypes); //find paramless const
                if (type.IsClass && constr != null)
                {
                    var propInst = Activator.CreateInstance(type);
                    prop.SetValue(obj, propInst, null);
                    InitProperties(propInst);
                }
            }
        }

        public async void GetListSerAsync()
        {
            DateTime now = DateTime.Today;
            int dia = now.Day;
            int mes = now.Month;
            int ano = now.Year;
            string format = "" + dia + "." + mes + "." + ano + "";

            if (rts.data == ".")
            {
                y = await lo.GetInfo(format);
            }
            else
            {
                y = await lo.GetInfo(rts.data);
            }

            int i = 0;
            foreach (Rota r in y)
            {
                Rota x = new Rota
                {
                    cliente = y[i].cliente,
                    ano_assistencia = y[i].ano_assistencia,
                    pago_no_local = y[i].pago_no_local,
                    espera_em_minutos = y[i].espera_em_minutos,
                    numero_acompanhantes = y[i].numero_acompanhantes,
                    numero_assistencia = y[i].numero_assistencia,
                    carga_hora = y[i].carga_hora,
                    descarga_hora = y[i].descarga_hora,
                    codigo_terceiro = y[i].codigo_terceiro,
                    contacto_nome = y[i].contacto_nome,
                    contacto_telefone = y[i].contacto_telefone,
                    carga_kms = y[i].carga_kms.Split('.')[0],
                    descarga_kms = y[i].descarga_kms.Split('.')[0],
                    estado = y[i].estado,
                    carga_local = y[i].carga_local,
                    descarga_local = y[i].descarga_local,
                    carga_observacaoes = y[i].carga_observacaoes,
                    descarga_observacoes = y[i].descarga_observacoes,
                    matricula = y[i].matricula,
                    IsVisible = false,
                    IsVisiblee = false,
                    corEstado = "White"
                };

                if (x.carga_kms != "" && x.descarga_kms == "")
                {
                    x.corEstado = "Green";
                }
                else if (x.descarga_kms != "")
                {
                    x.corEstado = "Red";
                }
                ListSer.Add(x);
                i++;
            }
        }

        internal void HideOrShowService(Rota ser)
        {
            if (_oldSer == ser)
            {
                ser.IsVisible = !ser.IsVisible;
                UpdateServicos(ser);
            }
            else
            {
                if (_oldSer != null)
                {
                    _oldSer.IsVisible = false;
                    UpdateServicos(_oldSer);
                }
                ser.IsVisible = true;
                UpdateServicos(ser);
            }
            _oldSer = ser;
        }


        public static string GetEnumDescription(Enum enumVal)
        {
            MemberInfo[] memInfo = enumVal.GetType().GetMember(enumVal.ToString());
            DescriptionAttribute attribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(memInfo[0]);
            return attribute.Description;
        }

        private void UpdateServicos(Rota ser)
        {
            var index = ListSer.IndexOf(ser);
            ListSer.Remove(ser);
            ListSer.Insert(index, ser);
        }
    }
}
