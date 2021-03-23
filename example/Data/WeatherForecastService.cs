using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPagination;

namespace BlazorPagerExample.Data
{
    public class WeatherForecastService
    {
        private static List<WeatherForecast> _summaries;
        private static Random rand = new Random();
        public WeatherForecastService()
        {
            _summaries = new List<WeatherForecast>();
            var date = DateTime.Now;
            for (int i = 0; i < 141; i++)
            {
                _summaries.Add(new WeatherForecast { Date = date.AddDays(i), Summary = "Freezing", TemperatureC = rand.Next(6, 22) });
            };
        }

        public Task<PagedResult<WeatherForecast>> GetForecastAsync(DateTime startDate, int page, int pageSize, string sortColumn, string sortDirection)
        {
            return Task.FromResult(_summaries
                .AsQueryable()
                .OrderBy(sortColumn, sortDirection)     // custom sort extension to sort on a string representing a column
                .ToPagedResult(page, pageSize));
        }
    }
}
