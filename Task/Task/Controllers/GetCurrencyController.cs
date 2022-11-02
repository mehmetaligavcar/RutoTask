using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.Models.Domain;
using Task.Models.ViewModels;
using RestSharp;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Task.Controllers
{
    public class GetCurrencyController
    {
        private TaskDbContext dbContext { get; set; }
        public GetCurrencyController(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Currency> getCurrencies(GetCurrencyViewModel viewModel)
        {
            var currencies = new List<Currency>();
            var query = from c in dbContext.Currencies
                        where c.date >= viewModel.date1 && c.date < viewModel.date2
                        select c;

            var tempList = query.ToArray<Currency>();
            if (tempList != null)
            {
                if(tempList.Length < (viewModel.date2 - viewModel.date1).TotalDays)
                {
                    saveToDatabase(viewModel.date1, viewModel.date2, tempList);
                    query = from c in dbContext.Currencies
                                where c.date >= viewModel.date1 && c.date < viewModel.date2
                                select c;

                    tempList = query.ToArray<Currency>();
                }
                currencies.AddRange(tempList);
            }
            getCurrencyFromApi(viewModel.date1);
            return currencies;
        }
        private Currency getCurrencyFromApi(DateTime date)
        {
            var client = new RestClient("https://openexchangerates.org/api/");
            var request = new RestRequest("historical/"+date.Year+"-" + date.Month.ToString("D2") 
                                                   + "-"+date.Day.ToString("D2") + ".json?app_id=153b5ca8c4e94c138e1147b76b055bf9", Method.Get);

            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var queryResult = client.Execute(request);
            JObject jsonResult = JObject.Parse(queryResult.Content);
            double value = Convert.ToDouble(jsonResult["rates"]["TRY"]);
            return new Currency(date, value);
        }
        private void saveToDatabase(DateTime date1, DateTime date2, Currency[] existingCurrencies)
        {
            foreach (DateTime day in EachDay(date1, date2))
            {
                List<Currency> existingCurrenciesList = existingCurrencies.ToList();
                if (!existingCurrenciesList.Select(x => x.date).ToArray().Contains(day))
                {
                    Currency temp = getCurrencyFromApi(day);
                    existingCurrenciesList.Add(temp);
                    dbContext.Add(temp);
                    dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
