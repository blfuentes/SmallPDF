using SmallPDF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace SmallPDF.Model
{
    [XmlRoot("currency")]
    public class Currency : BaseViewModel
    {
        private int _id;
        [XmlAttribute("Id")]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value; OnPropertyChanged("Id");
            }
        }
        private string _code;
        [XmlAttribute("code")]
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value; OnPropertyChanged("Code");
            }
        }
        private string _name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value; OnPropertyChanged("Name");
            }
        }
        [XmlAttribute("symbol")]
        public string Symbol { get; set; }
        public string TextDisplay => $"{Name} ({Code})";
    }
}
