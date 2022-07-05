using Stats.Abstractions.Common;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Abstractions
{
    public interface ICovidDataService : IDataService<CountryInfo>
    {
        IEnumerable<ConfirmedCount> GetConfirmedCounts(CountryInfo countryInfo);
        IEnumerable<DateTime> GetDates();
        List<CountryInfo> LoadAllConfirmedCounts();

    }
}
