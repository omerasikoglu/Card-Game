using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunTogether.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.Utils{

  public static class UtilsClass{

    public static float GetMobileScaleMultiplier(){
      var screenRatio = (float) Screen.width / Screen.height;
      
      return screenRatio / (9f / 16f);
    }
    
    public static Vector2 GetPixelResolutionRatio(){
      float width  = 1080f / Screen.width;
      float height = 1920f / Screen.height;
      return new(width, height);
    }

  #region Color
    public static Color GetColorFromString(string color){
      color = color.RemoveSpecialCharacters();

      float red   = Convert.ToInt32(color[..2], 16) / 255f;
      float green = Convert.ToInt32(color.Substring(2, 2), 16) / 255f;
      float blue  = Convert.ToInt32(color[^2..], 16) / 255f;

      return new Color(red, green, blue);
    }

    public static Color GetEmissionColorFromNumerics(int R, int G, int B){
      float red   = R / 255f;
      float green = G / 255f;
      float blue  = B / 255f;

      return new Color(red, green, blue);
    }

    public static Color GetEmissionColorFromNumerics(int[] rgb){
      float red   = rgb[0] / 255f;
      float green = rgb[1] / 255f;
      float blue  = rgb[2] / 255f;

      return new Color(red, green, blue);
    }
  #endregion

    public static void SetActiveScene(string sceneName){
      var targetScene = SceneManager.GetSceneByName(sceneName);
      SceneManager.SetActiveScene(targetScene);
    }

  #region Time Management
    public static async UniTaskVoid CloseAfterDelay(Transform transform, float duration){
      await UniTask.WaitForSeconds(duration);
      transform.Toggle(false);
    }

    public static async UniTaskVoid Wait(Action function, float duration){ // wait then trigger
      await UniTask.WaitForSeconds(duration);
      function();
    }

    public static async UniTaskVoid WaitUntil(bool condition, Action function){ // wait then trigger
      await UniTask.WaitUntil(() => condition);
      function();
    }

    public static async UniTaskVoid Wait(Action function, float duration, CancellationToken cancelToken){
      await UniTask.WaitForSeconds(duration, cancellationToken: cancelToken);
      function();
    }

    public static async UniTaskVoid WaitFrame(Action function, int frameCount){
      Time.timeScale = 0f;
      await UniTask.DelayFrame(frameCount);
      Time.timeScale = 1f;
      function();
    }

    public static async UniTaskVoid WaitFrame(int frameCount){
      Time.timeScale = 0f;
      await UniTask.DelayFrame(frameCount);
      Time.timeScale = 1f;
    }
  #endregion

  #region Mechanim
    public static float GetClipDurationByName(Animator targetAnimator, string clipName){ // TODO: convert to extension method
      int hashId = Animator.StringToHash(clipName);
      foreach (var clip in targetAnimator.runtimeAnimatorController.animationClips){
        if (hashId != clip.GetHashCode()) continue;
        return clip.length;
      }

      // foreach (var clip in targetAnimator.runtimeAnimatorController.animationClips){
      //   if (clipName != clip.name) continue;
      //   return clip.length;
      // }

      Debug.Log($"<color=green>{"cant found animation clip"}</color>");
      return default;
    }
  #endregion

  
  }

  

}