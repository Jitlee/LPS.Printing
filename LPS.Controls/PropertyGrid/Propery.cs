using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPS.Controls
{
    public class Property : INotifyPropertyChanged
    {
        public string Category { get; private set; }
        public string PropertyName { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public Dictionary<object, object> Source { get; set; }

        private bool _isEnabled;
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = true; RaisePropertyChanged("IsEnabled"); } }

        public Property(string category, string propertyName, string displayName, string description, bool isEnabled = true)
        {
            Category = category;
            PropertyName = propertyName;
            DisplayName = displayName;
            Description = description;
            _isEnabled = isEnabled;
        }

        public Property(string category, string propertyName, string displayName, string description, Dictionary<object, object> source, bool isEnabled = true)
        {
            Category = category;
            PropertyName = propertyName;
            DisplayName = displayName;
            Description = description;
            Source = source;
            _isEnabled = isEnabled;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
