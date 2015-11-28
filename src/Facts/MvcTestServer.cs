using Microsoft.AspNet.Builder;
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
        private readonly HttpClient _client;

        public MvcTestServer()
        {
            var server = new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc().AddControllersAsServices(new[]
                {
                    typeof(TController)
                });
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMvc();
            }
        }
    }
}
