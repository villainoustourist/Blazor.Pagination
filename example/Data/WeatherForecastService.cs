using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorPagination;

namespace BlazorPagerExample.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static List<WeatherForecast> _summaries;

        public WeatherForecastService()
        {
            var rng = new Random();
            _summaries = new List<WeatherForecast>();
            var date = DateTime.Now;
            for (var i = 0; i < 48; i++)
                _summaries.Add(new WeatherForecast
                { Date = date.AddDays(i), Summary = Summaries[rng.Next(Summaries.Length)], TemperatureC = rng.Next(-20, 55), });
            ;
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