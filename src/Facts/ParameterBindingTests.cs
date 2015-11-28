using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Facts
{
    public class ParameterBindingTests
    {
        private readonly HttpClient _client;

        public ParameterBindingTests()
        {
            _client = new MvcTestServer<ResourcesController>().GetClient();
        }

        [Fact]
        public async Task CanBindFromUrl()
        {
            var res = await _client.GetAsync("api/resources?page=1&pageSize=10");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        
        [Fact]
        public async Task CanBindFromHeader()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "api/resources/fromheader");
            req.Headers.Add("detail", "42");
            var res = await _client.SendAsync(req);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        
        [Fact]
        public async Task CanBindFromFormBody()
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "api/resources");
            req.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("page","1"),
                new KeyValuePair<string, string>("pageSize","10"),
            });            
            var res = await _client.SendAsync(req);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }  

        [Route("api/[controller]")]
        class ResourcesController : Controller
        {         
            public string Get(PagingReqModel pageInfo)
            {
                Assert.Equal(1, pageInfo.Page);
                Assert.Equal(10, pageInfo.PageSize);
                return "Ok";
            }

            // FromHeader is required for the binding to occur
            [HttpGet("fromheader")]
            public string GetWithBindingFromHeader(int id, [FromHeader] string detail)
            {
                Assert.Equal("42", detail);
                return "Ok";
            }
        }
        
        public class PagingReqModel
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
    }
}
