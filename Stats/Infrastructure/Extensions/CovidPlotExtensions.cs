using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Stats.Abstractions;
using Stats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Infrastructure.Extensions
{
    public static class CovidPlotExtensions
    {
        public static PlotModel CovidPlotConfigure(this PlotModel model)
        {
            model.Axes.Clear();
            var dtAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Months,
                MinorIntervalType = DateTimeIntervalType.Months,
                StringFormat = "dd/mm/yy"
            };
            model.PlotType = PlotType.XY;
            model.Axes.Add(dtAxis);
            return model;
        }
        public static PlotModel AddSeriesCovidPlot(this PlotModel model, IEnumerable<CountryInfo> countries, ICovidDataService covidDataService)
        {
            foreach(var country in countries)
            {
                var title = country.Name + " " + country.Province;
                var series = new LineSeries
                {
                    DataFieldX = "Date",
                    DataFieldY = "Count",
                    Title = title,
                };
                if (country.Counts == null)
                    country.Counts = covidDataService.GetConfirmedCounts(country);
                series.ItemsSource = country.Counts;
                model.Series.Add(series);
            }
            return model;
        }

        public static PlotModel CovidPiePlotConfigure(this PlotModel model)
        {
            return model;
        }

        public static PlotModel AddPieSeriesCovidPlot(this PlotModel model, List<CountryInfo> countries)
        {
            var series = new PieSeries { StartAngle = 0, AngleSpan = 360};
            foreach (var country in countries)
            {
                var count = country.Counts.Last().Count;
                var slice = new PieSlice(country.Name, Convert.ToDouble(count)) { IsExploded = true};
                series.Slices.Add(slice);
            }
            model.Series.Add(series);
            return model;
        }
    }
}
