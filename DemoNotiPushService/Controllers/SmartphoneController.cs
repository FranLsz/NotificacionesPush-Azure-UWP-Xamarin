using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using DemoNotiPushService.DataObjects;
using DemoNotiPushService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;

namespace DemoNotiPushService.Controllers
{
    public class SmartphoneController : TableController<Smartphone>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DemoNotiPushContext context = new DemoNotiPushContext();
            DomainManager = new EntityDomainManager<Smartphone>(context, Request, Services);
        }

        // GET tables/Smartphone
        public IQueryable<Smartphone> GetAllSmartphones()
        {
            return Query();
        }

        // GET tables/Smartphone/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Smartphone> GetSmartphone(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Smartphone/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Smartphone> PatchSmartphone(string id, Delta<Smartphone> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Smartphone
        public async Task<IHttpActionResult> PostSmartphone(Smartphone smartphone)
        {
            Smartphone current = await InsertAsync(smartphone);

            // Android (Google)
            var datosGoogle = new Dictionary<string,string>()
            {
                {"mensaje", JsonConvert.SerializeObject(smartphone) }
            };
            var mensajeGoogle = new GooglePushMessage(datosGoogle, TimeSpan.FromHours(1));

            // UWP
            var datosUwp = @"<?xml version=""1.0"" encoding=""utf - 8""?>
<toast>
    <visual>
        <binding template=""ToastText01"">
            <text id=""1"">Nuevo smartphone</text>
            <text id=""1"">Modelo: " + smartphone.Modelo + @"</text>
            <text id=""1"">Fabricante: " + smartphone.Fabricante + @"</text>
        </binding>
    </visual>
	<actions>
			<action content=""Ver"" arguments=""check"" />
			<action content=""Descartar"" arguments=""cancel""/>
	</actions>
</toast>";
            var mensajeUwp = new WindowsPushMessage() {XmlPayload = datosUwp};

            // Tag
            var tag = "NuevoSmartphone";
            var tags = new List<string>();
            tags.Add(tag);


            try
            {
                // Android (Google) Push
                var resultadoGoogle = await Services.Push.SendAsync(mensajeGoogle, tags);
                Services.Log.Info("Google - " + resultadoGoogle.State);


                // UWP Push
                var resultadoUwp = await Services.Push.SendAsync(mensajeUwp, tags);
                Services.Log.Info("Microsoft - " + resultadoUwp.State);
            }
            catch (Exception e)
            {
                Services.Log.Error(e.Message);
            }


            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Smartphone/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSmartphone(string id)
        {
            return DeleteAsync(id);
        }
    }
}
