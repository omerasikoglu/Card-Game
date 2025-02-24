using System;
using System.Collections.Generic;
using System.Linq;

namespace RunTogether.Extensions{
  
  public static class CollectionExtensions{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action){
      IEnumerable<T> forEach = source as T[] ?? source.ToArray();
      foreach (T obj in forEach)
        action(obj);
      return forEach;
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list){
      return list == null || !list.Any();
    }
    
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable){
      return enumerable == null || !enumerable.Any();
    }

    public static List<T> Clone<T>(this IList<T> list){
      List<T> newList = new List<T>();
      foreach (T item in list){
        newList.Add(item);
      }

      return newList;
    }

    public static void Swap<T>(this IList<T> list, int indexA, int indexB){
      (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }
  }

}