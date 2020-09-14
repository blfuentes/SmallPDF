using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmallPDF.Model.DTO
{
    public class APIBaseObject
    {
        [JsonPropertyName("base")]
        public string _base { get; set; }
        public DateTime date { get; set; }
    }
}
