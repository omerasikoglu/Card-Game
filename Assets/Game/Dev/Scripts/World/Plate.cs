using CardGame.UI;
using CardGame.Utils;
using DG.Tweening;
using RunTogether.Extensions;
using TMPro;
using UnityEngine;
using VContainer;

namespace CardGame.World{

  [SelectionBase]
  public class Plate : MonoBehaviour, IClickInInteract, IRayInteract{

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text playerNameText;

    MeshRenderer meshRenderer;
    Tween        scaleUpTween;
    Vector3      originalScale;

    Entity owner;
    
    const float scaleMultiplier = 1.1f;
    const float duration        = 0.1f;

    void Awake(){
      meshRenderer  = transform.GetFirstChild<MeshRenderer>();
      originalScale = meshRenderer.transform.localScale;
    }

    public void Init(Entity entity, Transform root){
      SetPlayerName(entity.ToString());
      
      owner = entity;
      transform.position = root.position;
      transform.rotation = root.rotation;
      gameObject.Toggle(false);
      
      void SetPlayerName(string name){
        playerNameText.SetText(name);
      }
    }

    public void SetScoreText(int score, Entity entity){
      if(entity != owner) return;
      scoreText.SetText(score.ToString("C0"));
    }

  #region Implements
    public bool IsInteractEnable(){
      return true;
    }

    public void OnRayEnter(){
      DOTween.Kill(Keys.Tween.Plate);
      meshRenderer.transform.DOScale(originalScale * scaleMultiplier, duration).SetId(Keys.Tween.Plate);
    }

    public void OnRayExit(){
      DOTween.Complete(Keys.Tween.Plate);
      meshRenderer.transform.DOScale(originalScale, duration).SetId(Keys.Tween.Plate);
    }

    public void OnInteractJustPerformed(){
      DOTween.Kill(Keys.Tween.Plate);
      meshRenderer.transform.localScale = originalScale;
    }
  #endregion

  }

}