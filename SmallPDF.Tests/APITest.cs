using SmallPDF.Helpers;
using SmallPDF.Model.DTO;
using SmallPDF.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SmallPDF.Tests
{
    public class APITest
    {
        [Fact]
        public async Task HTTPRequestTest()
        {
            var uriAPI = $"https://api.ratesapi.io/api/latest";
            var apiResponse = await HTTPService.GET_CallAsync(uriAPI);

            Assert.True(apiResponse.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task HTTPRequestConversionTest()
        {
            string from = "EUR";
            string to = "GBP";
            var uriAPI = $"https://api.ratesapi.io/api/latest?base={from}&symbols={to}";
            var apiResponse = await HTTPService.GET_CallAsync(uriAPI);

            var exchangeResult = JsonSerializer.Deserialize<APIExchangeObject>(await apiResponse.Content.ReadAsStringAsync());

            Assert.True(apiResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.NotNull(exchangeResult);
        }
    }
}
