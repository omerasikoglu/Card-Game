using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RunTogether.Extensions{

  public static class GameObjectExtensions{
    public static void SetActiveSceneThis(this GameObject gameObject){
      if (SceneManager.GetActiveScene() == gameObject.scene) return;
      SceneManager.SetActiveScene(gameObject.scene);
    }

    public static void Toggle(this GameObject gameObject, bool? to = null){
      bool isActiveInHierarchy = gameObject.activeInHierarchy;
      gameObject.SetActive(to ?? !isActiveInHierarchy);
    }

    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component{
      T component               = gameObject.GetComponent<T>();
      if (!component) component = gameObject.AddComponent<T>();

      return component;
    }

    public static void DestroyChildren(this GameObject gameObject){
      gameObject.transform.DestroyChildren();
    }

    public static void TryDestroyComponent<T>(this GameObject gameObject) where T : Component{
      gameObject.TryGetComponent(out T component);
      if (component is null) return;

      Object.Destroy(component);
    }

  #region UI - Prototype Pattern
    public static TMP_Text GetTextComponent(this GameObject gameObject, string objectName){
      // !: include grandchildren
      return gameObject.transform.GetComponentsInChildren<Transform>(true).
        First(o => o.name == objectName).GetComponent<TMP_Text>();
    }

    public static Sprite GetImage(this GameObject gameObject, string objectName){
      return gameObject.transform.Find(objectName).GetComponent<Image>().sprite;
    }
  #endregion

  }

}