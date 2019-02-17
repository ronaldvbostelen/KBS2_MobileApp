using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KBS2.WijkagentApp.Extensions
{
    public static class ObservableCollectionExtension
    {
        //simple converter list to ObservableCollection, no fuss
        public static ObservableCollection<T> ListToObservableCollection<T>(this List<T> list) => new ObservableCollection<T>(list);
    }
}
