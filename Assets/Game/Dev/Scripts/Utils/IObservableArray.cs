using System;

namespace RunTogether.Utils{

  public interface IObservableArray<T>{
    event Action<T[]> AnyValueChanged;

    int Count{get;}
    T this[int index]{get;}

    void Swap(int index1, int index2);
    void Clear();
    bool TryAdd(T item);
    bool TryAddAt(int index, T item);
    bool TryRemove(T item);
    bool TryRemoveAt(int index);
  }

}