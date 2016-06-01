using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUWP.Extentions
{
    public static class CollectionExtentions
    {
        public static bool Remove<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            bool result = true;
            foreach (var item in items)
            {
                result = collection.Remove(item);
            }
            return result;
        }
    }
}
