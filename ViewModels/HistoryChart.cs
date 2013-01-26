using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;

namespace OptionRange.ViewModels
{
    public class HistoryChart
    {
        public Highcharts upperChart { get; set; }
        public Highcharts lowerChart { get; set; }
        public Highcharts rangeChart { get; set; }
    }
}