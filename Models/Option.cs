using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptionRange.Models
{
    public class Option
    {
        public string symbol { get; set; }
        public string expiry { get; set; }
        public string type { get; set; }
        public string strike { get; set; }
        public decimal price { get; set; }
        public string reference { get; set; }
    }
}