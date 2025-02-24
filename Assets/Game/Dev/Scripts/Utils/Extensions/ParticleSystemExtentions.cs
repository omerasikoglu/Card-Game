using UnityEngine;

namespace RunTogether.Extensions{

  public static class ParticleSystemExtentions{
    public static void Toggle(this ParticleSystem particleSystem, bool? to = null){
      var  targetObject        = particleSystem.gameObject;
      bool isActiveInHierarchy = targetObject.activeInHierarchy;
      targetObject.SetActive(to ?? !isActiveInHierarchy);
    }
  }

}