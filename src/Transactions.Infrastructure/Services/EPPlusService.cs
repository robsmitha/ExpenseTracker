using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;

namespace Transactions.Infrastructure.Services
{
    public class EPPlusService : IExcelService
    {
        public async Task SaveExcelFileAsync<T>(ICollection<T> collection, string worksheetName)
        {
            const string directory = "budget";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using var stream = File.Open($"{directory}/{worksheetName}.xlsx", FileMode.Create);
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.Add(worksheetName);
            if (ws.Cells.Any())
            {
                
            }
            else
            {
                var range = ws.Cells["A1"].LoadFromCollection(collection, true);
                range.AutoFitColumns();
            }
            

            await package.SaveAsync();

            // TODO: upload to one drive


        }
    }
}
