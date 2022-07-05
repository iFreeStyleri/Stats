using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Stats.Abstractions;
using Stats.Common;
using Stats.Infrastructure.Commands;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Stats.Infrastructure.Extensions;
using Microsoft.Win32;

namespace Stats.ViewModels
{
    public class CovidStatsViewModel : ViewModelBase
    {
        private CountryInfo _selectedCountry;
        private PlotModel _covidPlotModel;
        private PlotModel _covidPiePlotModel;
        private readonly ICovidDataService _covidDataService;
        private readonly ICovidExcelService _covidExcelService;
        private bool isExport;

        public bool IsExport
        {
            get => isExport;
            set => Set(ref isExport, value);
        }

        public CountryInfo SelectedCountry 
        { 
            get => _selectedCountry;
            set => Set(ref _selectedCountry, value);
        }
        public PlotModel CovidPlotModel
        {
            get => _covidPlotModel;
            set => Set(ref _covidPlotModel, value);
        }

        public PlotModel CovidPiePlotModel
        {
            get => _covidPiePlotModel;
            set => Set(ref _covidPiePlotModel, value);
        }
        public IEnumerable<CountryInfo> Countries { get; set; }
        private List<CountryInfo> SelectedCountries { get; set; }
        public ICommand SelectCountryCommand { get; set; }
        public ICommand AddCovidSeriesCommand { get; set; }
        public ICommand ExportSelectedData { get; set; }
        public ICommand ExportAllData { get; set; }
        public ICommand RemoveCovidSeriesCommand { get; set; }
        public CovidStatsViewModel(ICovidExcelService excelService,ICovidDataService covidDataService)
        {
            _covidDataService = covidDataService;
            _covidExcelService = excelService;
            IsExport = true;
            SelectedCountries = new List<CountryInfo>();
            Countries = _covidDataService.GetData();
            SelectCountryCommand = new AsyncRelayCommand<CountryInfo>(country =>
            {
                SelectedCountries.Clear();
                SelectedCountries.Add(country);
                CovidPlotModel = new PlotModel() 
                .CovidPlotConfigure()
                .AddSeriesCovidPlot(SelectedCountries, _covidDataService);
                CovidPiePlotModel = new PlotModel()
                .AddPieSeriesCovidPlot(SelectedCountries);
            });

            AddCovidSeriesCommand = new AsyncRelayCommand<CountryInfo>(country =>
            {
                SelectedCountries.Add(country);
                CovidPlotModel = new PlotModel()
                .CovidPlotConfigure()
                .AddSeriesCovidPlot(SelectedCountries, _covidDataService);
                CovidPiePlotModel = new PlotModel()
                .AddPieSeriesCovidPlot(SelectedCountries);
            });

            RemoveCovidSeriesCommand = new AsyncRelayCommand<CountryInfo>(country =>
            {
                SelectedCountries.Remove(country);
                CovidPlotModel = new PlotModel()
                .AddSeriesCovidPlot(SelectedCountries, _covidDataService);
                CovidPiePlotModel = new PlotModel()
                .AddPieSeriesCovidPlot(SelectedCountries);
            });

            ExportSelectedData = new AsyncRelayCommand<object>(obj =>
            {
                IsExport = false;
                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = ".xlsx | *.xlsx";
                fileDialog.FileName = "selectedData.xlsx";
                if (fileDialog.ShowDialog() ?? false)
                    _covidExcelService.Save(fileDialog.InitialDirectory, fileDialog.FileName, SelectedCountries);
                IsExport = true;
            });

            ExportAllData = new AsyncRelayCommand<object>(obj =>
            {
                IsExport = false;
                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = ".xlsx | *.xlsx";
                fileDialog.FileName = "allData.xlsx";
                if (fileDialog.ShowDialog() ?? false)
                {
                    var data = _covidDataService.LoadAllConfirmedCounts();
                    _covidExcelService.Save(fileDialog.InitialDirectory, fileDialog.FileName, data.ToList());
                }
                IsExport = true;
            });
            
        }

        public CovidStatsViewModel() { }
    }
}
