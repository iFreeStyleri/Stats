using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Abstractions.Common
{
    public interface IDataService<TModel> where TModel : class
    {
        IEnumerable<TModel> GetData();
        Task<IEnumerable<TModel>> GetDataAsync();
    }
}
