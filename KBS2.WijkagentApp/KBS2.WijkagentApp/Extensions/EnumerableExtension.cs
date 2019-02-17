using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KBS2.WijkagentApp.Extensions
{
    public static class EnumerableExtension
    {
        //simple converter IEnumerable to ObservableCollection, no fuss
        public static ObservableCollection<T> EnumerableToObservableCollection<T>(this IEnumerable<T> list) => new ObservableCollection<T>(list);
    }
}
