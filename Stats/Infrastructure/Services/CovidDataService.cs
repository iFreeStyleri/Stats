using Stats.Abstractions;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Infrastructure.Services
{
    public class CovidDataService : ICovidDataService
    {
        private const string covidStat = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        private async Task<Stream> GetStream()
        {
            using var client = new HttpClient();
            var response = client.GetAsync(covidStat, HttpCompletionOption.ResponseHeadersRead).Result;
            return await response.Content.ReadAsStreamAsync();
        }

        public IEnumerable<DateTime> GetDates()
            => GetLines()
            .First()
            .Trim()
            .Split(',')
            .Skip(4)
            .Select(DateTime.Parse);
        private IEnumerable<string> GetLines()
        {

            using var sr = new StreamReader(GetStream().Result);
            while (!sr.EndOfStream)
            {
                var line = CountryFormat(sr.ReadLine());
                if (string.IsNullOrWhiteSpace(line)) continue;
                yield return line;
            }
        }

        private IEnumerable<CountryInfo> GetCountries()
        {
            var countryList = new List<CountryInfo>();
            var lines = GetLines().Skip(1).Select(f => f.Split(','));
            foreach(var row in lines)
                countryList.Add(ToCountry(row));
            return countryList;
        }

        public IEnumerable<CountryInfo> GetData()
        {
            return GetCountries();
        }
        private CountryInfo ToCountry(string[] data, bool isCounts = false)
        {
            var province = data[0].Trim() ?? string.Empty;
            var countryName = data[1].Trim() ?? string.Empty;
            var country = new CountryInfo()
            {
                Name = countryName,
                Province = province
            };
            if (isCounts)
                country.Counts = GetConfirmedCounts(data);
            return country;
        }

        public Task<IEnumerable<CountryInfo>> GetDataAsync()
            => Task.Run(() => GetData());

        public IEnumerable<ConfirmedCount> GetConfirmedCounts(CountryInfo countryInfo)
        {
            var list = new List<ConfirmedCount>();
            var counts = GetLines()
                .Where(w => w.Contains(countryInfo.Name) && w.Contains(countryInfo.Province))
                .First().Split(',')
                .Skip(4)
                .Select(int.Parse)
                .ToList();

            var dates = GetDates().ToList();
            for (int i = 0; i < dates.Count; ++i)
               list.Add(new ConfirmedCount { Count = counts[i], Date = dates[i] });
            return list;
        }

        public IEnumerable<ConfirmedCount> GetConfirmedCounts(string[] data)
        {
            var list = new List<ConfirmedCount>();
            var counts = data.Skip(4).ToList();
            var dates = GetDates().ToList();
            for (int i = 0; i < counts.Count; ++i)
                list.Add(new ConfirmedCount
                {
                    Date = dates[i],
                    Count = decimal.Parse(counts[i])
                });
            return list;
        }

        public List<CountryInfo> LoadAllConfirmedCounts()
        {
            var list = new List<CountryInfo>();
            var lines = GetLines().Skip(1).ToList();
            lines
                .AsParallel()
                .ForAll(f => list.Add(ToCountry(f.Split(','), true)));
            return list;
        }

        private string CountryFormat(string data)
        {
            var newData = data
                .Replace("Korea, North", "Korea North")
                .Replace("Korea, South", "Korea South")
                .Replace("Bonaire, Sint", "Bonaire Sint")
                .Replace("Saint Helena, Ascension", "Saint Helena Ascension");
            return newData;
        }
    }
}
