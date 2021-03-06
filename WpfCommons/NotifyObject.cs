﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace WpfCommons
{
    public class NotifyObject : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsDisposed { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChangedEventHandler handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception)
            {
            }
        }

        protected virtual void OnPropertyChanged(Expression<Func<object>> extension)
        {
            try
            {
                PropertyChangedEventHandler handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null);
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(Helpers.GetPropertyName(extension)));
            }
            catch (Exception)
            {
            }
        }

        public virtual void Dispose()
        {
            if (PropertyChanged != null)
            {
                var delgates = PropertyChanged.GetInvocationList().ToList();
                foreach (var del in delgates)
                {
                    PropertyChanged -= (PropertyChangedEventHandler)del;
                }
            }

            IsDisposed = true;
        }
    }
}
