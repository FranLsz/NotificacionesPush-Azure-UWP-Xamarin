using MvvmCross.Plugins.Messenger;

namespace PushApp.Core.Utiles
{
    public class ViewModelMessage : MvxMessage
    {
        public string Accion { get; set; }
        public object Elemento { get; set; }

        public ViewModelMessage(object sender, string accion, object elemento) : base(sender)
        {
            Accion = accion;
            Elemento = elemento;
        }
    }
}