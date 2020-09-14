using SmallPDF.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SmallPDF.Tests
{
    public class APITest
    {
        [Fact]
        public async Task HTTPRequestTest()
        {
            string apiKey = "5ed7abf4a14bb9e20ad0f63a";
            string currency = "USD";
            var uriAPI = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{currency}";
            var apiResponse = await HTTPService.GET_CallAsync(uriAPI);

            Assert.True(apiResponse.StatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}
