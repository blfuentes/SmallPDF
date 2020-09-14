using System;
using System.Collections.Generic;
using System.Text;

namespace SmallPDF.Services
{
    public interface IDataService
    {
        void GetElements();
        void AddElement();
        void RemoveElement();
        void UpdateElement();
    }
}
