using SmallPDF.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
            XmlSerializer serializer = new XmlSerializer(typeof(CurrencyCollection));
            FileStream fs = new FileStream(path, FileMode.Open);
            TextReader reader = new StreamReader(fs);
            var result = (CurrencyCollection)serializer.Deserialize(reader);

            Assert.True(result.Currencies.Count == 33);
        }
    }
}
