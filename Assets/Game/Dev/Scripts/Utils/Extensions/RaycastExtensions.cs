using UnityEngine;

namespace RunTogether.Extensions{

  public static class RaycastExtensions{
    public static T GetComponent<T>(this RaycastHit hit){
      return hit.transform.GetComponent<T>();
    }
  }

}