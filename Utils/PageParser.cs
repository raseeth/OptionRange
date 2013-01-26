using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OptionRange.Models;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace OptionRange.Utils
{
    public class PageParser
    {
        public static Option getOption(string shortName, string type, string strike, string expiry)
        {
            Option option = new Option();

            WebRequest request = WebRequest.Create(string.Format("http://www.moneycontrol.com/stocks/company_info/fno_element.php?sc_id={0}&short_name={0}", shortName));
            request.Method = "POST";

            //string postData = "instrument_type=OPTSTK&option_type=PE&post_flag=true&sc_id=SBI&sel_exp_date=2012-12-27&sel_strike_price=2100.00&short_name=SBI&user_sel_price=2100.00";
            string postData = string.Format("instrument_type=OPTSTK&option_type={1}&post_flag=true&sc_id={0}&sel_exp_date={2}&sel_strike_price={3}&short_name={0}&user_sel_price={3}", shortName, type, expiry, strike);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            var regex = new Regex(@"_20 FL PR5""><strong>(.*?)</strong></div>");
            var match = regex.Match(responseFromServer);
            try
            {
                option.price = Convert.ToDecimal(match.Groups[1].Value);
            }
            catch
            {
                option.price = 0.0M;
            }

            regex = new Regex(@"<input type=""hidden"" name=""sel_exp_date"" id=""sel_exp_date"" value=""(.*?)"">");
            match = regex.Match(responseFromServer);
            option.expiry = match.Groups[1].Value;

            regex = new Regex(@"<input type=""hidden"" name=""symbol"" id=""symbol"" value=""(.*?)"">");
            match = regex.Match(responseFromServer);
            option.symbol = match.Groups[1].Value;

            regex = new Regex(@"/OPTSTK/" + type + "/(.*?)/true");
            match = regex.Match(responseFromServer);
            option.strike = match.Groups[1].Value;
            option.type = type;

            regex = new Regex(@"href=""/india/fnoquote/(.*?)/true");
            match = regex.Match(responseFromServer);
            option.reference = "http://www.moneycontrol.com/india/fnoquote/" + match.Groups[1].Value + "/true";

            return option;
        }

        public static OptionHistory getHistory(string symbol, string expiryDate, string optionType, string strike, string dateRange)
        {
            OptionHistory optionHistory = new OptionHistory();
            optionHistory.date = new List<string>();
            optionHistory.prices = new List<string>();

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://www.nseindia.com/products/dynaContent/common/productsSymbolMapping.jsp?instrumentType=OPTSTK&symbol=SBIN&expiryDate=31-01-2013&optionType=CE&strikePrice=2500&dateRange=3month&fromDate=&toDate=&segmentLink=9&symbolCount="));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://www.nseindia.com/products/dynaContent/common/productsSymbolMapping.jsp?instrumentType=OPTSTK&symbol={0}&expiryDate={1}&optionType={2}&strikePrice={3}&dateRange={4}&fromDate=&toDate=&segmentLink=9&symbolCount=", symbol, expiryDate, optionType, strike, dateRange));
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.52 Safari/537.17";
            request.Accept = "*/*";
            //request.ContentType = "text/plain; charset=utf-8";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseFromServer);
            foreach (HtmlNode table in document.DocumentNode.SelectNodes("//table"))
            {
                HtmlNodeCollection rows = table.SelectNodes("tr");
                foreach (HtmlNode row in rows)
                {
                    int column = 0;
                    HtmlNodeCollection cells = row.SelectNodes("th|td");
                    foreach (HtmlNode cell in cells)
                    {
                        column++;
                        Console.WriteLine("cellatt: " + cell.GetAttributeValue("class", ""));
                        Console.WriteLine("cell: " + cell.InnerText);

                        if (cell.GetAttributeValue("class", "") == "date" || cell.GetAttributeValue("class", "") == "number")
                        {
                            if (column == 2)
                            {
                                optionHistory.date.Add(cell.InnerText);
                            }
                            else if (column == 9)
                            {
                                optionHistory.prices.Add(cell.InnerText);
                            }
                        }
                    }
                }
            }
            return optionHistory;
            
        }
    }
}