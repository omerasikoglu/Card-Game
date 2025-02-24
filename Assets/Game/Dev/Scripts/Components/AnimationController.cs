using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Components{

  public class AnimationController{
    readonly Animator animator;

    public AnimationController(Animator animator){
      this.animator = animator;
    }

  #region Set
    public void StopAllAnimations(IEnumerable<string> allAnimations){
      foreach (var animationName in allAnimations){
        int id = StringToHash(animationName);
        animator.SetBool(id, false);
      }
    }

    public void SetTrigger(string animationName){
      int id = StringToHash(animationName);
      animator.SetTrigger(id);
    }

    public void SetFloat(string animationName, float blend){
      int id = StringToHash(animationName);
      animator.SetFloat(id, blend);
    }

    public void SetBool(string animationName, bool isActive){
      int id = StringToHash(animationName);
      animator.SetBool(id, isActive);
    }

    int StringToHash(string animationName){
      return Animator.StringToHash(animationName);
    }
  #endregion

  #region Get
    public float GetCurrentClipDuration(){
      return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    public string GetCurrentClipName(){
      return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public float GetClipDurationByName(string clipName){
      foreach (var clip in animator.runtimeAnimatorController.animationClips){
        if (clipName != clip.name) continue;
        return clip.length;
      }

      Debug.Log($"<color=green>{"cant found animation clip"}</color>");
      return default;
    }
  #endregion

  }

}