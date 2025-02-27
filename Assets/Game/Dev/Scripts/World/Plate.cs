using System;
using CardGame.UI;
using CardGame.Utils;
using DG.Tweening;
using RunTogether.Extensions;
using UnityEngine;
using VContainer;

namespace CardGame.World{

  [SelectionBase]
  public class Plate : MonoBehaviour, IClickInInteract, IRayInteract{

    [Inject] readonly CanvasController canvasController;
    [Inject] readonly Player player;

    MeshRenderer meshRenderer;
    Vector3     originalScale;

    const float scaleMultiplier = 1.1f;
    const float duration        = 0.1f;
    
    Tween scaleUpTween;

    void Awake(){
      meshRenderer = transform.GetFirstChild<MeshRenderer>();
      originalScale = meshRenderer.transform.localScale;
    }

  #region Implements
    public bool IsInteractEnable(){
      return true;
    }

    public void OnRayEnter(){
      DOTween.Kill(Keys.Tween.Plate);
      meshRenderer.transform.DOScale( originalScale * scaleMultiplier, duration).SetId(Keys.Tween.Plate);
    }

    public void OnRayExit(){
      DOTween.Complete(Keys.Tween.Plate);
      meshRenderer.transform.DOScale( originalScale, duration).SetId(Keys.Tween.Plate);
    }

    public void OnInteractJustPerformed(){
      canvasController.OpenPlayerInfoPanel();
      DOTween.Kill(Keys.Tween.Plate);
      transform.localScale = originalScale;
      player.PlayerInput.OnToggle(false);
    }
  }
#endregion

}