using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RunTogether.Extensions{

  public static class TransformExtensions{

  #region Core
    public static void Toggle(this Transform transform, bool? to = null){
      bool isActiveInHierarchy = transform.gameObject.activeInHierarchy;
      transform.gameObject.SetActive(to ?? !isActiveInHierarchy);
    }

    public static void Toggle(this IEnumerable<Transform> transformArray, bool to){
      foreach (var transform in transformArray.Where(o => o.gameObject.activeInHierarchy != to)){
        transform.gameObject.SetActive(to);
      }
    }

    public static void SetActive(this Transform transform, bool isActive) => transform.gameObject.SetActive(isActive);

    public static void Destroy(this Transform transform) => Object.Destroy(transform.gameObject);

    public static void TryDestroyComponent<T>(this Transform transform) where T : Component{
      transform.TryGetComponent(out T component);
      if (component is null) return;

      Object.Destroy(component);
    }
  #endregion

  #region Children
    public static T GetFirstChild<T>(this Transform transform, bool includeInactive = true, bool includeGrandChild = false) where T : Component{
      // !:  only children counts
      return transform.GetFromChildren<T>(includeInactive, includeGrandChild).FirstOrDefault();
    }

    public static T[] GetFromChildren<T>(this Transform transform, bool includeInactive = true, bool includeGrandChild = false) where T : Component{
      // !: if parent have T ignore it, only children counts

      if (includeGrandChild){
        var  allComponents         = transform.GetComponentsInChildren<T>(includeInactive);
        bool isParentHaveComponent = transform.TryGetComponent(out T component);
        return isParentHaveComponent ? allComponents[1..] : allComponents;
      }
      else{
        return Enumerable.Range(0, transform.childCount)
          .Select(transform.GetChild)
          .Select(child => child.GetComponent<T>())
          .Where(component => component != null && (includeInactive || component.gameObject.activeSelf))
          .ToArray();
      }

    }

    public static void ForEachChild(this Transform parent, Action<Transform> action, bool includeSelf = false, bool includeInactive = true, bool includeGrandchild = false){

      if (!includeGrandchild){ // !: grand children not included
        for (var i = parent.childCount - 1; i >= 0; i--){
          action(parent.GetChild(i));
        }

        if (includeSelf) action(parent);
      }

      else{ // !: grandchildren (child's childrens) included
        var everyTransform = parent.GetComponentsInChildren<Transform>(includeInactive);
        everyTransform.ForEach(action);

      }
    }

    public static void DestroyChildren(this Transform parent){
      parent.ForEachChild(child => Object.Destroy(child.gameObject));
    }
  #endregion

  }

}