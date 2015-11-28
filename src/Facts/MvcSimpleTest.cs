using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Facts
{
    public class MvcSimpleTest
    {
        [Fact]
        public async Task CanHostMvc()
        {
            var server = new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            var res = await client.GetAsync("/api/Resources");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            var body = await res.Content.ReadAsStringAsync();
            Assert.Equal("Hello Web!", body);
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc().AddControllersAsServices(new[]
                {
                    typeof(ResourcesController)
                }); 
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMvc();
            }
        }

        [Route("api/[controller]")]
        public class ResourcesController : Controller
        {
            [HttpGet()]
            public string Get()
            {
                return "Hello Web!";
            }
        }
    }
}

