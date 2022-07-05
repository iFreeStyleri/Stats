using Stats.Abstractions.Common;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Abstractions
{
    public interface ICovidExcelService : IExcelService<CountryInfo>
    {
    }
}
