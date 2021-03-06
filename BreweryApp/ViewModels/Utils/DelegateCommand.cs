﻿using System;
using System.Windows.Input;

namespace BreweryApp.ViewModels.Utils
{
    /// <summary>
    /// ICommand implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand where T : class
    {
        private Action<T> _execute;
        private bool _canExecute;


        public event EventHandler CanExecuteChanged;


        public DelegateCommand(Action<T> execute, bool canExecute = true)
        {
            _execute = execute;
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

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public void Execute(T parameter)
        {
            _execute(parameter);
        }
    }
}
