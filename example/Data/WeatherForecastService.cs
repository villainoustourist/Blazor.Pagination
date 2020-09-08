using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorPagination;

namespace example.Data
{
    public class WeatherForecastService
    {
        private static List<WeatherForecast> _summaries;
        public WeatherForecastService()
        {
            _summaries = new List<WeatherForecast>
            {
                new WeatherForecast {Date = new DateTime(2020, 1, 1), Summary = "Freezing", TemperatureC = 2},
                new WeatherForecast {Date = new DateTime(2020, 1, 2), Summary = "Cold", TemperatureC = 4},
                new WeatherForecast {Date = new DateTime(2020, 1, 3), Summary = "Chilly", TemperatureC = 1},
                new WeatherForecast {Date = new DateTime(2020, 1, 4), Summary = "Cold", TemperatureC = 6},
                new WeatherForecast {Date = new DateTime(2020, 1, 5), Summary = "Bracing", TemperatureC = 7},
                new WeatherForecast {Date = new DateTime(2020, 1, 6), Summary = "Chilly", TemperatureC = 10},
                new WeatherForecast {Date = new DateTime(2020, 1, 7), Summary = "Mild", TemperatureC = 6},
                new WeatherForecast {Date = new DateTime(2020, 1, 7), Summary = "Mild", TemperatureC = 6},
                new WeatherForecast {Date = new DateTime(2020, 1, 8), Summary = "Cool", TemperatureC = 11},
                new WeatherForecast {Date = new DateTime(2020, 1, 9), Summary = "Mild", TemperatureC = 14},
                new WeatherForecast {Date = new DateTime(2020, 1, 10), Summary = "Freezing", TemperatureC = 17},
                new WeatherForecast {Date = new DateTime(2020, 1, 11), Summary = "Freezing", TemperatureC = 19},
                new WeatherForecast {Date = new DateTime(2020, 1, 12), Summary = "Freezing", TemperatureC = 13}
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
