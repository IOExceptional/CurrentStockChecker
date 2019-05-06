using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlphaVantage.Net.Stocks;
using Newtonsoft.Json;

namespace current_stock_viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonText = File.ReadAllText("./input.json");

            var stocks = JsonConvert.DeserializeObject<Input>(jsonText).Stocks;

            Task.WaitAll(GetStock(stocks));
        }

        private static async Task GetStock(Stock[] stocks)
        {
            var apiKey = "86SYWP6Z7AJ23STX";
            var client = new AlphaVantageStocksClient(apiKey);

            foreach (var stock in stocks)
            {
                var timeSeries = await client.RequestDailyTimeSeriesAsync(stock.Symbol);


                var latest = timeSeries.DataPoints.First().ClosingPrice;

                Console.WriteLine($"{stock.Symbol}: {latest * stock.Quantity} ({latest})");
            }

        }
    }
}
