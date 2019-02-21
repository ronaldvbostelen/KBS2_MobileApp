using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Models.Interfaces;

namespace KBS2.WijkagentApp.DataModels
{
    /*
    * Baseclass for datamodels
    * Implements INotifyPropertyChanged
    */
    public class BaseDataModel : IDatabaseObject, INotifyPropertyChanged
    {
        public Guid id { get; set; }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        //basic method with implementation of [CallerMemberName]
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //overloading method with params implementation (for double dipping properties)
        public void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyname in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                }
            }
        }
        #endregion
    }
}
