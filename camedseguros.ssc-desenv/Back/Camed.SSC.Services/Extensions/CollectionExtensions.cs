using System;
using System.Collections.Generic;
using System.Linq;

namespace Camed.SSC.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static void UpdateCollection<TCollection>(this ICollection<TCollection> collection, int[] items, string keyProperty = "Id") 
            where TCollection : class
        {
            if (items != null)
            {
                IEnumerable<int> collectionItens = collection.Select(s => (int)s.GetType().GetProperty(keyProperty).GetValue(s, null));

                if (collection.Any())
                {
                    var itemsToDelete = collectionItens.Except(items);

                    foreach (var it in itemsToDelete.ToList())
                    {
                        var aux = collection.FirstOrDefault(f => (int)f.GetType().GetProperty(keyProperty).GetValue(f, null) == it);
                        collection.Remove(aux);
                    }
                }

                var itemsToAdd = items.Except(collectionItens);
                foreach (var it in itemsToAdd)
                {
                    var item = (TCollection)Activator.CreateInstance(typeof(TCollection), new object[] { });
                    item.GetType().GetProperty(keyProperty).SetValue(item, it);

                    collection.Add(item);
                }
            } else
            {
                IEnumerable<int> collectionItens = collection.Select(s => (int)s.GetType().GetProperty(keyProperty).GetValue(s, null));

                var itemsToDelete = collectionItens;
                
                foreach (var it in itemsToDelete.ToList())
                {
                    var aux = collection.FirstOrDefault(f => (int)f.GetType().GetProperty(keyProperty).GetValue(f, null) == it);
                    collection.Remove(aux);
                }
                
            }
        }
    }
}
