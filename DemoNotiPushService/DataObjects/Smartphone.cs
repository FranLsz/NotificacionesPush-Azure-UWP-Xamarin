using Microsoft.WindowsAzure.Mobile.Service;

namespace DemoNotiPushService.DataObjects
{
    public class Smartphone : EntityData
    {
        public string Modelo { get; set; }
        public string Fabricante { get; set; }
        public double Precio { get; set; }
    }
}