using LPS.Controls.PropertyGrid.Parts.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LPS.Controls.PropertyGrid.Parts
{
    public class PropertyItem : INotifyPropertyChanged
    {

        #region 属性
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }
        public object Instance { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                SetValue(value);
            }
        }

        #endregion 属性

        #region 事件
        /// <summary>
        /// Event raised when an error is encountered attempting to set the Value
        /// </summary>
        public event EventHandler<ExceptionEventArgs> ValueError;
        /// <summary>
        /// Raises the ValueError event
        /// </summary>
        /// <param name="ex">The exception</param>
        private void OnValueError(Exception ex)
        {
            if (null != ValueError)
                ValueError(this, new ExceptionEventArgs(ex));
        }
        #endregion 事件

        #region 构造函数

        public PropertyItem(object instance, PropertyInfo propertyInfo)
        {
            Instance = instance;
            PropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            DisplayName = AttributeServices.GetDisplayName(propertyInfo);
            Category = AttributeServices.GetCategory(propertyInfo);
            Description = AttributeServices.GetDescription(propertyInfo);
            Value = propertyInfo.GetValue(instance, null);
        }

        #endregion

        #region 私有函数

        private void SetValue(object value)
        {
            if (_value == value ||
                (_value != null && _value.Equals(value)) ||
                (value != null && value.Equals(_value)) ||
                !PropertyInfo.CanWrite)
            {
                return;
            }

            try
            {
                Type type = PropertyInfo.PropertyType;

                if (((type == typeof(object)) || ((value == null) && type.IsClass)) || ((value != null) && type.IsAssignableFrom(value.GetType())))
                {
                    _value = value;
                    PropertyInfo.SetValue(Instance, value, (BindingFlags.NonPublic | BindingFlags.Public), null, null, null);
                    RaisePropertyChanged("Value");
                }
                else if (type.IsEnum)
                {
                    object val = Enum.Parse(PropertyInfo.PropertyType, value.ToString(), false);
                    _value = value;
                    PropertyInfo.SetValue(Instance, val, (BindingFlags.NonPublic | BindingFlags.Public), null, null, null);
                    RaisePropertyChanged("Value");
                }
                else
                {
                    TypeConverter tc = TypeConverterHelper.GetConverter(type);
                    if (tc != null)
                    {
                        object convertedValue = tc.ConvertFrom(value);
                        _value = value;
                        PropertyInfo.SetValue(Instance, convertedValue, null);
                        RaisePropertyChanged("Value");
                    }
                }
            }
            catch (Exception ex)
            {
                OnValueError(ex);
            }
        }

        #endregion

        #region 属性通知接口

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion 属性通知接口
    }
}
