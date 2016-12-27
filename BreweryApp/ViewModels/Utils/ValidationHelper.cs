using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BreweryApp.ViewModels.Utils
{
    public class ValidationHelper : IDataErrorInfo
    {
        private IDictionary<string, Func<string>> _validators;

        public string this[string columnName]
        {
            get
            {
                string errMessage = null;
                if (_validators.ContainsKey(columnName))
                {
                    errMessage = _validators[columnName]();
                }
                return errMessage;
            }
        }

        public string Error
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach(KeyValuePair<string, Func<string>> keyValidator in _validators)
                {
                    string errMessage = keyValidator.Value();
                    if(errMessage != null)
                    {
                        sb.AppendLine(errMessage);
                    }
                }
                return sb.ToString();
            }
        }

        public ValidationHelper(IDictionary<string, Func<string>> validators)
        {
            _validators = validators;
        }
    }
}
