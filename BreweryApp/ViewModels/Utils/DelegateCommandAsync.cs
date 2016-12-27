using ResotelApp.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BreweryApp.ViewModels.Utils
{
    /// <summary>
    /// Asynchronous ICommand implementations.
    /// Used for any ViewModel manipulating the data from a Repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommandAsync<T> : ICommand where T : class
    {
        private Func<T, Task> _executeAsync;
        private bool _canExecute;


        public event EventHandler CanExecuteChanged;


        public DelegateCommandAsync(Func<T,Task>executeAsync, bool canExecute = true)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }

        public void ChangeCanExecute()
        {
            _canExecute = !_canExecute;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        public async Task ExecuteAsync(T parameter)
        {
            try
            {
                await _executeAsync(parameter);
            }
            catch (Exception ex)
            {

                Logger.Log(ex);
            }
        }
    }
}
