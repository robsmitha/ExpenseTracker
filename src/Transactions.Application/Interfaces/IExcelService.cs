using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Interfaces
{
    public interface IExcelService
    {
        Task SaveExcelFileAsync<T>(ICollection<T> collection, string worksheetName);
    }
}
