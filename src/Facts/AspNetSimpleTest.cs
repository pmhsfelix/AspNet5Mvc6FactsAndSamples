using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
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

    public class AspNetSimpleTest
    {
        [Fact]
        public async Task CanHostAspNet()
        {
            var server = new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            var res = await client.GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            var body = await res.Content.ReadAsStringAsync();
            Assert.Equal("Hello Web!", body);
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure(IApplicationBuilder app)
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Hello Web!");
                });
            }
        }
    }
}
