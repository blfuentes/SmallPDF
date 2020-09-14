using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SmallPDF.Model
{
    [XmlRoot("currency")]
    public class Currency
    {
        [XmlAttribute("code")]
        public string Code { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("symbol")]
        public string Symbol { get; set; }
    }
}
