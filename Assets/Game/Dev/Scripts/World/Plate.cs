using CardGame.Utils;
using DG.Tweening;
using RunTogether.Extensions;
using TMPro;
using UnityEngine;

namespace CardGame.World{

  [SelectionBase]
  public class Plate : MonoBehaviour, IClickInInteract, IRayInteract{

  #region Members
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text     scoreText;
    [SerializeField] TMP_Text     totalBetText;
    [SerializeField] TMP_Text     playerNameText;
    [SerializeField] TMP_Text     yourTurnText;

    Tween   scaleUpTween;
    Vector3 originalScale;

    Entity owner;
    bool   isPlayer;

    const float scaleMultiplier = 1.1f;
    const float duration        = 0.1f;
  #endregion

    void Awake(){
      originalScale = spriteRenderer.transform.localScale;
    }

    public void Init(Entity entity, Transform root){
      SetPlayerName(entity.GetType().Name);

      owner              = entity;
      transform.position = root.position;
      transform.rotation = root.rotation;
      ToggleYourTurnText(false);
      gameObject.Toggle(false);

      isPlayer = entity.GetType().Name == Keys.LayerMask.PLAYER;

      void SetPlayerName(string name){
        playerNameText.SetText(name);
      }
    }

  #region Implements
    public bool IsInteractEnable(){
      return isPlayer;
    }

    public void OnRayEnter(){
      DOTween.Kill(Keys.Tween.Plate);
      spriteRenderer.transform.DOScale(originalScale * scaleMultiplier, duration).SetId(Keys.Tween.Plate);
    }

    public void OnRayExit(){
      DOTween.Complete(Keys.Tween.Plate);
      spriteRenderer.transform.DOScale(originalScale, duration).SetId(Keys.Tween.Plate);
    }

    public void OnInteractJustPerformed(){
      DOTween.Kill(Keys.Tween.Plate);
      spriteRenderer.transform.localScale = originalScale;
    }
  #endregion

  #region Set
    public void SetScoreText(int score, Entity entity){
      if (entity != owner) return;
      scoreText.SetText(score.ToString());
    }

    public void SeTotalBetText(int totalBet){
      if (owner.GetType().Name != Keys.LayerMask.PLAYER) return;
      var result = $"Total Bet: {totalBet}";
      totalBetText.SetText(result);
    }

    public void ToggleYourTurnText(bool to){
      yourTurnText.gameObject.SetActive(to);
    }
  #endregion

  }

}