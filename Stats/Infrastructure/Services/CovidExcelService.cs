using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Stats.Abstractions;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Infrastructure.Services
{
    public class CovidExcelService : ICovidExcelService
    {
        private readonly ICovidDataService _covidDataService;
        public CovidExcelService(ICovidDataService covidDataService)
        {
            _covidDataService = covidDataService;
        }
        public void Save(string path, string fileName, IEnumerable<CountryInfo> model)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();
            package.Workbook.Worksheets.Add(fileName);
            var worksheet = package.Workbook.Worksheets[0];
            SetHeaders(worksheet);
            SetCells(worksheet.Cells, model.ToArray());
            package.SaveAsAsync(Path.Combine(path, fileName));
        }
        private void SetHeaders(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Province";
            worksheet.Cells[1, 1].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
            worksheet.Cells[1, 2].Value = "Country";
            worksheet.Cells[1, 2].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
            var dates = _covidDataService.GetDates().ToList();
            for (int i = 0; i < dates.Count; ++i)
            {
                worksheet.Cells[1, i+3].Value = dates[i].ToString("dd/MM/yy");
                if(i % 2 == 0)
                    worksheet.Cells[1, i+3].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
                if (i % 2 != 0)
                    worksheet.Cells[1, i+3].Style.Fill.SetBackground(System.Drawing.Color.LightYellow);
            }

        }
        private void SetCells(ExcelRange cells, CountryInfo[] countries)
        {
            for(int i = 0; i < countries.Length; ++i)
            {
                cells[i+2, 1].Value = countries[i].Province;
                cells[i+2, 2].Value = countries[i].Name;
                var count = countries[i].Counts.Select(confirmedCount => confirmedCount.Count).ToArray();
                for(int j = 0; j < count.Length; ++j)
                {
                    cells[i+2,j+3].Value = count[j];
                    if (j % 2 == 0)
                        cells[i + 2, j + 3].Style.Fill.SetBackground(System.Drawing.Color.LightGreen);
                    if (j % 2 != 0)
                        cells[i + 2, j + 3].Style.Fill.SetBackground(System.Drawing.Color.LightYellow);
                }
            }
        }
    }
}
