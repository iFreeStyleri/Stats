using Stats.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stats.Infrastructure.Commands
{
    public class AsyncRelayCommand<TParameter> : CommandBase where TParameter : class
    {
        private Action<TParameter> _action;
        private Func<TParameter, bool> _func;

        public AsyncRelayCommand(Action<TParameter> action, Func<TParameter, bool> func = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _func = func;
        }
        public override bool CanExecute(object parameter)
            => _func?.Invoke((TParameter)parameter) ?? true;

        public override void Execute(object parameter)
            => ExecuteAsync(parameter);
        private Task ExecuteAsync(object parameter)
            => Task.Run(() => _action((TParameter)parameter));
    }
}
