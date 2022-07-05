using Stats.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Infrastructure.Commands
{
    internal class RelayCommand<TParameter> : CommandBase
    {
        private readonly Action<TParameter> _action;
        private readonly Func<TParameter, bool> _func;

        public RelayCommand(Action<TParameter> action, Func<TParameter, bool> func = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _func = func;
        }
        public override bool CanExecute(object parameter) 
            => _func?.Invoke((TParameter)parameter) ?? true;

        public override void Execute(object parameter)
            => _action((TParameter)parameter);
    }
}
