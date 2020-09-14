using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SmallPDF.Model
{
    [XmlRoot("currencies")]
    public class CurrencyCollection
    {
        [XmlElement("currency")]
        public List<Currency> Currencies { get; set; }
    }
}
