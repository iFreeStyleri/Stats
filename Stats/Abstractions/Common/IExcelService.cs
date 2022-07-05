using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Abstractions.Common
{
    public interface IExcelService<TModel>
    {
        void Save(string path, string fileName, IEnumerable<TModel> model);
    }
}
