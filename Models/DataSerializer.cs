using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using SAILI.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Web.Hosting;
using System.Text.RegularExpressions;

namespace SAILI.Models
{
    public class DataSerializer<T> where T : IConvertible
    {
        private static ApplicationDbContext DefaultConnection = new ApplicationDbContext();

        public static void Serialise(string path, Vector<T> vector)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, vector);
            }
        }

        public void deserialise(string path, ref Vector<T> vector)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                vector = (Vector<T>)bin.Deserialize(stream);

            }
        }

        public static void LoadVectorFromTextFile(string path, ref Vector<T> vector)
        {
            vector = new Vector<T>();
            string line = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    //This would work only for primitive types
                    vector.Add((T)Convert.ChangeType(line, typeof(T)));
                }
                sr.Close();
            }
        }

        public static void SaveVectorToTextFile(string path, Vector<T> vector)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                var count = vector.Count;
                for (int i = 0; i < count; i++)
                {
                    sw.WriteLine(vector[i]);
                }
                sw.Close();
            }
        }


        public static void SaveVectorToTextFile(string path, string error)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(error);
                sw.Close();
            }
        }

        public static void SaveErrorInRequest(string path, string[] listings)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.Write(listings[0] + ",");
                sw.Write(listings[1] + listings[2] + ",");
                sw.Write(listings[3] + ",");
                sw.WriteLine();
                sw.Close();
            }
        }

        public static void GetPriceListings(ref List<Listings> listings, string symbols)
        {
            symbols = symbols.ToUpper();
            string csvData = null;

            try
            {
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=" + symbols + "&f=sol1");
                    if (csvData != null)
                        listings = ParsePriceListings(listings, csvData);
                }
            }
            catch
            {

            }
        }

        // This static method returns a Listings of many symbols.

        private static List<Listings> ParsePriceListings(List<Listings> listings, string csvData)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}', '>', '<' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            listings = new List<Listings>(rows.Count());
            SailiRepository repository = new SailiRepository();
            Listings list = null;


            foreach (string row in rows)
            {

                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                decimal convert = 0;

                bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                if (!result)
                {
                    result = Decimal.TryParse(cols[3].ToString(), out convert);
                    if (result)
                    {
                        cols[1] = cols[3];
                        list = new Listings();
                        list.Symbol = cols[0];
                        list.Open = Convert.ToDecimal(cols[1]);
                        list.Close = Convert.ToDecimal(cols[3]);
                        list.Change = list.Close - list.Open;
                        listings.Add(list);
                    }
                    else
                    {
                        SaveErrorInRequest(path, cols);
                        continue;
                    }
                }
                result = Decimal.TryParse(cols[2].ToString(), out convert);
                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                else
                {
                    list = new Listings();
                    list.Symbol = cols[0];
                    list.Open = Convert.ToDecimal(cols[1]);
                    list.Close = Convert.ToDecimal(cols[2]);
                    list.Change = list.Close - list.Open;
                    listings.Add(list);
                }
            }
            return listings;
        }

        // Returns listing of a single symbol
        internal static void GetSingleListings(ref List<Listings> listings, string symbol)
        {
            symbol = symbol.ToUpper();
            string csvData = null;

            try
            {
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + symbol + "&apikey=HWSKUZE5XRJUIFA1&datatype=csv");
                    listings = ParseListings(listings, csvData, symbol);
                }
            }
            catch
            {

            }
        }

        private static List<Listings> ParseListings(List<Listings> listings, string csvData, string symbol)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            Listings list = null;
            SailiRepository repository = new SailiRepository();
            bool pass = false;
            int count = 0;

            if (listings == null)
                listings = new List<Listings>();

            Company company = null;
            company = repository.GetCompany(company, symbol);

            if(company != null)
            {
                foreach (string row in rows)
                {
                    if (!pass)
                    {
                        pass = true;
                        continue;
                    }

                    if (string.IsNullOrEmpty(row)) continue;

                    string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                    decimal convert = 0;

                    bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                    if (!result)
                    {
                        SaveErrorInRequest(path, cols);
                        continue;
                    }
                    result = Decimal.TryParse(cols[4].ToString(), out convert);
                    if (!result)
                    {
                        SaveErrorInRequest(path, cols);
                        continue;
                    }
                    count++;
                    if (count == 2)
                        break;
                    list = new Listings();
                    list.Symbol = symbol;
                    list.CompanyName = company.CompanyName;
                    list.Open = Convert.ToDecimal(cols[1].ToString());
                    list.Close = Convert.ToDecimal(cols[4].ToString());
                    list.Change = list.Close - list.Open;
                    listings.Add(list);
                }
            }
            return listings;
        }

        private static Listings CreateUnknownListing(Listings model, string symbol, string[] cols)
        {
            string csvData = null;
            char[] delimiter = { ',','\t', '\r', '\n', '"', '/', '{', '}', '>', '<' };
            //Note the <> brakes was used to specifically split known scripting tags.
            //The theory here is that by splinting it and removing it, how can a scipt 
            // exist without the accompaning scripting tag?

            try
            {
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=" + symbol + "&f=n");
                }

                string[] temp = csvData.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                model = new Listings();
                model.Symbol = symbol;
                if(temp.Count() > 1){
                    model.CompanyName = temp[0] + " " + temp[1];
                }
                else{
                    model.CompanyName = temp[0];
                }

                if(Regex.IsMatch(model.CompanyName, @"^[A-Za-z ]+$")){
                    DefaultConnection.Companies.Add(new Company()
                    {
                        Symbol = model.Symbol,
                        CompanyName = model.CompanyName,
                        SectorID = 0,
                        IsDeleted = false
                    });
                    DefaultConnection.SaveChanges();
                }
                else{
                    model.CompanyName = "false";
                }


            }
            catch
            {

            }

            return model;
        }

        internal static void GetStockHistory(ref List<Historical> historicalList, Portfolio portfolio)
        {
            string csvData = null;
            Historical historical = null;
            historicalList = new List<Historical>();

            foreach (var company in portfolio.Buys)
            {
                try
                {
                    using (WebClient web = new WebClient())
                    {
                        csvData = web.DownloadString("http://www.google.com/finance/historical?q=" + company.TickerSymbol + "&histperiod=monthly&output=csv");

                        if (csvData != null)
                        {
                            historical = new Historical();
                            historical.historicalPrices = new List<HistoricalPrices>();
                            historical.Symbol = company.TickerSymbol;
                            historical = ParseHistoryListings(historical, csvData);
                            historicalList.Add(historical);
                        }

                    }
                }
                catch
                {

                }
            }
        }

        private static Historical ParseHistoryListings(Historical historical, string csvData)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            HistoricalPrices price = null;
            SailiRepository repository = new SailiRepository();
            bool pass = false;


            foreach (string row in rows)
            {
                if (!pass)
                {
                    pass = true;
                    continue;
                }

                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                decimal convert = 0;

                bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                result = Decimal.TryParse(cols[2].ToString(), out convert);
                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                price = new HistoricalPrices();
                price.Date = cols[0].ToString();
                price.Open = Convert.ToDecimal(cols[1].ToString());
                price.Close = Convert.ToDecimal(cols[4].ToString());
                historical.historicalPrices.Add(price);
            }
            return historical;
        }

        internal static void GetScores(ref List<Listings> listings, string symbol)
        {
            string csvData = null;

            try
            {
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=" + symbol + "&f=sol1");
                    listings = ParseScoreListings(listings, csvData, symbol);
                }
            }
            catch
            {

            }
        }

        private static List<Listings> ParseScoreListings(List<Listings> listings, string csvData, string symbol)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            SailiRepository repository = new SailiRepository();
            bool check = false;
            Listings listing = null;


            foreach (string row in rows)
            {

                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                decimal convert = 0;

                bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                if (!result)
                {
                    result = Decimal.TryParse(cols[2].ToString(), out convert);
                    if (result)
                    {
                        cols[1] = cols[2];
                    }
                    else
                    {
                        SaveErrorInRequest(path, cols);
                        continue;
                    }
                }
                result = Decimal.TryParse(cols[2].ToString(), out convert);
                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                if (!check)
                {
                    listing = new Listings();
                    listing.Symbol = cols[0];
                    listing.Open = Convert.ToDecimal(cols[1]);
                    listing.Close = Convert.ToDecimal(cols[2]);
                    listing.Change = listing.Close - listing.Open;
                }
                else
                {
                    listing = new Listings();
                    listing.Symbol = cols[0];
                    listing.Open = Convert.ToDecimal(cols[1]);
                    listing.Close = Convert.ToDecimal(cols[2]);
                    listing.Change = listing.Close - listing.Open;
                }

            }
            return listings;
        }

        public static void SecurityPriorityNumberOne(string userId)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/SecurityAlert.txt");
            DateTime timeStamp = DateTime.Now;

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.Write(timeStamp + ",");
                sw.Write(userId);
                sw.WriteLine();
                sw.Close();
            }
        }

        // Drastic action needed to be taken due to now the csv line for Yahoo is down.
        // Now both Yahoo and Google do not work. There is only one left. But this means
        // longer time for it it calculate all 

        public static void GetCurrentPerformance(ref PortfolioViewModel portfolio)
        {
            decimal[] stats = new decimal[2];
            foreach (var itemPortfolio in portfolio.Buys)
            {
                string csvData = null;

                try
                {
                    using (WebClient web = new WebClient())
                    {
                        csvData = web.DownloadString("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + itemPortfolio.TickerSymbol + "&apikey=HWSKUZE5XRJUIFA1&datatype=csv");
                        //csvData = web.DownloadString("https://www.google.com.au/search?source=hp&ei=J9P7WfGJAoSZ8wWJxKDADw&q=" + company.Symbol+ " + stock + quote & oq ="+ company.Symbol + "+ stock + &gs_l = psy - ab.1.1.0l10.4103.15196.0.18018.28.20.6.0.0.0.442.3194.2 - 6j4j1.11.0....0...1.1.64.psy - ab..13.15.2718.0..0i131k1j0i10k1j0i22i30k1j0i22i10i30k1.0.a_b25pvD5F8");
                        //csvData = web.DownloadString("https://finance.yahoo.com/quote/" + company.Symbol + "?p=" + company.Symbol + "?p=");
                        stats = CompletePortfolioPerformance(portfolio, csvData, itemPortfolio.TickerSymbol);
                        itemPortfolio.Open = stats[0];
                        itemPortfolio.Close = stats[1];
                        itemPortfolio.Performance = (itemPortfolio.Close - itemPortfolio.PurchasePrice) * itemPortfolio.Quantity;
                    }
                }
                catch
                {

                }
            }
        }

        private static decimal[] CompletePortfolioPerformance(PortfolioViewModel portfolio, string csvData, string symbol)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}', '!', '<' , '>' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            SailiRepository repository = new SailiRepository();
            bool pass = false;
            decimal[] stats = new decimal[2];

            foreach (string row in rows)
            {
                if (!pass)
                {
                    pass = true;
                    continue;
                }

                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                decimal convert = 0;

                bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                result = Decimal.TryParse(cols[4].ToString(), out convert);
                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }

                stats[0] = Convert.ToDecimal(cols[1].ToString());
                stats[1] = Convert.ToDecimal(cols[4].ToString());
                break;
            }
            return stats;
        }

        public static void GetCurrentPrice(ref List<Listings> listings, List<Company> companies)
        {
            listings = new List<Listings>();

            foreach (var company in companies)
            {
                string csvData = null;


                company.Symbol = company.Symbol.ToUpper();
                try
                {
                    using (WebClient web = new WebClient())
                    {
                        csvData = web.DownloadString("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + company.Symbol + "&apikey=HWSKUZE5XRJUIFA1&datatype=csv");

                        listings = ParseGoogleListings(listings, csvData, company.Symbol, company.CompanyName);
                    }
                }
                catch
                {

                }
            }
        }

        private static List<Listings> ParseGoogleListings(List<Listings> listings, string csvData, string symbol, string companyName)
        {
            string path = HostingEnvironment.MapPath("~/Error Log File/Error.txt");
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            string[] rows = csvData.Replace("\r", "").Split('\n');
            Listings list = null;
            SailiRepository repository = new SailiRepository();
            bool pass = false;
            int count = 0;

            foreach (string row in rows)
            {
                if (!pass)
                {
                    pass = true;
                    continue;
                }

                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                decimal convert = 0;

                bool result = Decimal.TryParse(cols[1].ToString(), out convert);

                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                result = Decimal.TryParse(cols[4].ToString(), out convert);
                if (!result)
                {
                    SaveErrorInRequest(path, cols);
                    continue;
                }
                count++;
                if (count == 2)
                    break;
                list = new Listings();
                list.Symbol = symbol;
                list.CompanyName = companyName;
                list.Open = Convert.ToDecimal(cols[1].ToString());
                list.Close = Convert.ToDecimal(cols[4].ToString());
                list.Change = list.Close - list.Open;
                listings.Add(list);
            }
            return listings;
        }

    }
}