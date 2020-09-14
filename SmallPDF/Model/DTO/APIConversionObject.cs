using System;
using System.Collections.Generic;
using System.Text;

namespace SmallPDF.Model.DTO
{
    public class APIConversionObject : APIBaseObject
    {
        public string target_code { get; set; }
        public double conversion_rate { get; set; }
    }
}
