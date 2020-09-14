using System;
using System.Collections.Generic;
using System.Text;

namespace SmallPDF.Model.DTO
{
    public class APIExchangeObject : APIBaseObject
    {
        public ConversionRate rates { get; set; }
    }
}
