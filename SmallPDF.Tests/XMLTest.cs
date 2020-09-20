using SmallPDF.Model;
using SmallPDF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Xunit;

namespace SmallPDF.Tests
{
    public class XMLTest
    {
        [Fact]
        public void ReadCurrenciesTest()
        {
            string path = @"TempData\AvailableCurrencies.xml";
            XMLDataService xMLDataService = new XMLDataService();
            var result = xMLDataService.GetElements<CurrencyCollection>(path);
            Assert.True(result.Currencies.Count == 33);
        }

        [Fact]
        public void AddCurrenciesTest()
        {
            string path = @"TempData\AvailableCurrenciesAdd.xml";
            XMLDataService xMLDataService = new XMLDataService();
            xMLDataService.AddElement<Currency>(path, "currencies", "currency",
                                  new Dictionary<string, object>()
                                  {
                                      {"Id", 50 },
                                      {"code", "AAA" },
                                      {"name", "TestCurrency" },
                                      {"symbol", "" }
                                  });
            var result = xMLDataService.GetElements<CurrencyCollection>(path);
            Assert.True(result.Currencies.Count == 34);
        }

        [Fact]
        public void UpdateCurrenciesTest()
        {
            string path = @"TempData\AvailableCurrenciesUpdate.xml";
            XMLDataService xMLDataService = new XMLDataService();
            xMLDataService.UpdateElement<Currency>(path,"currency", "Id", "1",
                                  new Dictionary<string, object>()
                                  {
                                      {"Id", 1 },
                                      {"code", "AAA" },
                                      {"name", "TestCurrency" },
                                      {"symbol", "" }
                                  });
            var result = xMLDataService.GetElements<CurrencyCollection>(path);
            Assert.True(result.Currencies.First().Id == 1);
            Assert.True(result.Currencies.First().Code == "AAA");
            Assert.True(result.Currencies.First().Name == "TestCurrency");
            Assert.True(result.Currencies.Count == 33);
        }

        [Fact]
        public void DeleteCurrenciesTest()
        {
            string path = @"TempData\AvailableCurrenciesDelete.xml";
            XMLDataService xMLDataService = new XMLDataService();
            xMLDataService.RemoveElement<Currency>(path, "currency", "Id", "1");
            var result = xMLDataService.GetElements<CurrencyCollection>(path);
            Assert.True(result.Currencies.First().Id != 1);
            Assert.True(result.Currencies.Count == 32);
        }
    }
}
