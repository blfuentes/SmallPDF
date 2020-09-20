using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SmallPDF.Helpers
{
    public static class ExtendedMethods
    {
        public static double GetValue(this String jsonObject, string propertyName)
        {
            var expConverter = new ExpandoObjectConverter();
            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(jsonObject, expConverter);
            if (((IDictionary<string, object>)obj.rates).ContainsKey(propertyName))
                return (double)((IDictionary<string, object>)obj.rates)[propertyName];
            else
                return 0.0;
        }
    }
}
