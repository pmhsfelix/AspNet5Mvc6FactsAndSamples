using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Facts
{
    public class ActionResultHandlingTests
    {
        private readonly HttpClient _client;
        public ActionResultHandlingTests()
        {
            _client = new MvcTestServer<ResourcesController>().CreateClient();
        }

        [Fact]
        public async Task StringResultIsFormattedAsTextPlain()
        {
            var res = await _client.GetAsync("api/resources");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("text/plain", res.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task ComplexResultIsFormattedAsJson()
        {
            var res = await _client.GetAsync("api/resources/complex");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("application/json", res.Content.Headers.ContentType.MediaType);
            var body = await res.Content.ReadAsStringAsync();
            Assert.Equal('{', body[0]);
        }

        [Fact]
        public async Task ArrayResultIsFormattedAsJson()
        {
            var res = await _client.GetAsync("api/resources/array");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("application/json", res.Content.Headers.ContentType.MediaType);
            var body = await res.Content.ReadAsStringAsync();
            Assert.Equal('[', body[0]);
        }

        [Fact]
        public async Task ArrayResultIsFormattedAsJsonEvenWhenUsingAccept()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "api/resources/array");
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            var res = await _client.SendAsync(req);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("application/json", res.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task ComplexResultIsFormattedAsXmlWhenUsingAcceptAndXmlFormatterIsConfigured()
        {
            var testServer = new MvcTestServer<ResourcesController>(options =>
            {
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            var client = testServer.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Get, "api/resources/complex");
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            var res = await client.SendAsync(req);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("application/xml", res.Content.Headers.ContentType.MediaType);
        }

        [Route("api/[controller]")]
        class ResourcesController : Controller
        {
            [HttpGet]
            public string GetString()
            {
                return "ok";
            }

            [HttpGet("complex")]
            public SomeClass GetComplex()
            {
                return new SomeClass
                {
                    A = "42",
                    B = 42,
                };
            }

            [HttpGet("array")]
            public string[] GetArray()
            {
                return new[]
                {
                    "42",
                    "abc",
                };
            }
        }

        public class SomeClass
        {
            public string A { get; set; }
            public int B { get; set; }
        }
    }    
}
