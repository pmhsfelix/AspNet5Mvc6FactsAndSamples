using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Facts
{
    public class MvcTestServer<TController>
    {
      
        private readonly TestServer _server;

        public MvcTestServer()
            : this(_ => {})
        {            
        }

        public MvcTestServer(Action<MvcOptions> configure)
        {
            _server = new TestServer(TestServer.CreateBuilder().UseStartup(
                configureApp: app => 
                {
                    app.UseMvc();
                },
                configureServices: services =>
                {
                    services.AddMvc(configure)                    
                    .AddControllersAsServices(new[]
                    {
                        typeof(TController)
                    });
                }
                ));                
        }

        public HttpClient CreateClient()
        {
            return _server.CreateClient();                            
        }          
    }
}
