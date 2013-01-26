using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptionRange.Models
{
    public class OptionHistory
    {
        public string strike { get; set; }
        public string type { get; set; }
        public List<string> date { get; set; }
        public List<string> prices { get; set; }
    }
}