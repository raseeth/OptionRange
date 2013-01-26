using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using OptionRange.Models;
using OptionRange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OptionRange.Controllers
{
    public class HistoryController : Controller
    {
        //
        // GET: /History/

        public ActionResult Index(string id, string symbol,string expiry, string range)
        {

            string upperStrike = id.Split('-')[1];
            string lowerStrike = id.Split('-')[0];
            string dateRange = range;

            OptionHistory upperOptionHistory = Utils.PageParser.getHistory(symbol, expiry,"CE", upperStrike,dateRange);
            OptionHistory lowerOptionHistory = Utils.PageParser.getHistory(symbol, expiry, "PE", lowerStrike, dateRange);

            var xValues = upperOptionHistory.date.ToArray();
            var upperYValues = upperOptionHistory.prices.ToArray();
            var lowerYValues = lowerOptionHistory.prices.ToArray();
            object[] rangeYValues = new object[xValues.Length];

            if (upperYValues.Length == lowerYValues.Length)
            {
                for(int i=0; i< upperYValues.Length;i++)
                {
                    rangeYValues[i] = Convert.ToDecimal(upperYValues[i]) + Convert.ToDecimal(lowerYValues[i]);
                }
            }

            HistoryChart historyChart = new HistoryChart();

            historyChart.upperChart = new Highcharts("upper")
               .InitChart(new Chart
               {
                   DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Line,
                   MarginRight = 130,
                   MarginBottom = 25,
                   ClassName = "chart"
               })
               .SetTitle(new Title
               {
                   Text = symbol + " " + upperStrike,
                   X = -20
               })
               .SetSubtitle(new Subtitle
               {
                   Text = xValues[0] + " - " + xValues[xValues.Length-1],
                   X = -20
               })
               .SetXAxis(new XAxis { Categories = xValues })
               .SetYAxis(new YAxis
               {
                   Title = new XAxisTitle { Text = "Price" },
                   TickInterval = "10",
                   PlotLines = new[]
                                          {
                                              new XAxisPlotLines
                                              {
                                                  Value = 0,
                                                  Width = 1,
                                                  Color = System.Drawing.ColorTranslator.FromHtml("#808080")
                                              }
                                          }
               })
               .SetTooltip(new Tooltip
               {
                   Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +'.Rs';
                                }"
               })
               .SetLegend(new Legend
               {
                   Layout = DotNet.Highcharts.Enums.Layouts.Vertical,
                   Align = DotNet.Highcharts.Enums.HorizontalAligns.Right,
                   VerticalAlign = DotNet.Highcharts.Enums.VerticalAligns.Top,
                   X = -10,
                   Y = 100,
                   BorderWidth = 0
               })
               .SetSeries(new[]
                           {
                               new Series { Name = upperStrike, Data = new Data(upperYValues) }
                           }
               );

            historyChart.lowerChart = new Highcharts("lower")
               .InitChart(new Chart
               {
                   DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Line,
                   MarginRight = 130,
                   MarginBottom = 25,
                   ClassName = "chart"
               })
               .SetTitle(new Title
               {
                   Text = symbol + " " + lowerStrike,
                   X = -20
               })
               .SetSubtitle(new Subtitle
               {
                   Text = xValues[0] + " - " + xValues[xValues.Length - 1],
                   X = -20
               })
               .SetXAxis(new XAxis { Categories = xValues })
               .SetYAxis(new YAxis
               {
                   Title = new XAxisTitle { Text = "Price" },
                   TickInterval = "10",
                   PlotLines = new[]
                                          {
                                              new XAxisPlotLines
                                              {
                                                  Value = 0,
                                                  Width = 1,
                                                  Color = System.Drawing.ColorTranslator.FromHtml("#808080")
                                              }
                                          }
               })
               .SetTooltip(new Tooltip
               {
                   Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +'.Rs';
                                }"
               })
               .SetLegend(new Legend
               {
                   Layout = DotNet.Highcharts.Enums.Layouts.Vertical,
                   Align = DotNet.Highcharts.Enums.HorizontalAligns.Right,
                   VerticalAlign = DotNet.Highcharts.Enums.VerticalAligns.Top,
                   X = -10,
                   Y = 100,
                   BorderWidth = 0
               })
               .SetSeries(new[]
                           {
                               new Series { Name = lowerStrike, Data = new Data(lowerYValues) }
                           }
               );

            historyChart.rangeChart = new Highcharts("range")
               .InitChart(new Chart
               {
                   DefaultSeriesType = DotNet.Highcharts.Enums.ChartTypes.Line,
                   MarginRight = 130,
                   MarginBottom = 25,
                   ClassName = "chart"
               })
               .SetTitle(new Title
               {
                   Text = symbol + " " + id,
                   X = -20
               })
               .SetSubtitle(new Subtitle
               {
                   Text = xValues[0] + " - " + xValues[xValues.Length - 1],
                   X = -20
               })
               .SetXAxis(new XAxis { Categories = xValues })
               .SetYAxis(new YAxis
               {
                   Title = new XAxisTitle { Text = "Price" },
                   TickInterval = "10",
                   PlotLines = new[]
                                          {
                                              new XAxisPlotLines
                                              {
                                                  Value = 0,
                                                  Width = 1,
                                                  Color = System.Drawing.ColorTranslator.FromHtml("#808080")
                                              }
                                          }
               })
               .SetTooltip(new Tooltip
               {
                   Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +'.Rs';
                                }"
               })
               .SetLegend(new Legend
               {
                   Layout = DotNet.Highcharts.Enums.Layouts.Vertical,
                   Align = DotNet.Highcharts.Enums.HorizontalAligns.Right,
                   VerticalAlign = DotNet.Highcharts.Enums.VerticalAligns.Top,
                   X = -10,
                   Y = 100,
                   BorderWidth = 0
               })
               .SetSeries(new[]
                           {
                               new Series { Name = id, Data = new Data(rangeYValues) }
                           }
               );

            return View(historyChart);
        }
    }
}
