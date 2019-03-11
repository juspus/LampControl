using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LampControl
{
    public class WeatherProcessor
    {
        public static async Task<WeatherWeather> LoadWeather()
        {
            string url = $"http://api.openweathermap.org/data/2.5/weather?id=598316&APPID=6f3b953e68f670e14976afa0c78200ce";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    WeatherWeather weather = await response.Content.ReadAsAsync<WeatherWeather>();
                    return weather;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
