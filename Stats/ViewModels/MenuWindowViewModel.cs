using Stats.Common;
using Stats.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Stats.ViewModels
{
    public class MenuWindowViewModel : ViewModelBase
    {
        private ICommand _closeApplicationCommand;

        public ICommand CloseApplicationCommand 
            => _closeApplicationCommand 
            ??= _closeApplicationCommand = new RelayCommand<object>((p) => Application.Current.Shutdown());

        public CovidStatsViewModel CovidStatsViewModel { get; set; }
        public MenuWindowViewModel(CovidStatsViewModel covidStatsViewModel)
        {
            CovidStatsViewModel = covidStatsViewModel;
        }
    }
}
