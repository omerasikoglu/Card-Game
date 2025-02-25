using System.Linq;
using CardGame.Utils;
using CardGame.World;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardGame{

  public class PlayerInput{

  #region Members
    Card    cardHit;
    bool    isDragging;
    Vector3 dragVelocity = Vector3.zero;

    Card targetCard;
    Card previousFrameTargetCard;

    readonly Camera mainCam;

    readonly Player       player;
    readonly InputActions inputActions;
    readonly RaycastHit[] cardHits = new RaycastHit[10];

    readonly RaycastHit[] interactableHits = new RaycastHit[10];

    const float MAX_RAY_DISTANCE = 3f;
  #endregion

    InputAction Touch        => inputActions.Inventory.Touch;
    InputAction TouchContact => inputActions.Inventory.TouchContact;
    InputAction FirstTouch   => inputActions.Inventory.FirstTouch;

    public PlayerInput(Player player){
      this.player = player;

      mainCam = Camera.main;

      inputActions = new();
      inputActions.Enable();
    }

    public void OnEnable(){
      inputActions.Enable();
    }

    public void OnDisable(){
      inputActions.Disable();
    }

    public void Update(){
      TouchPerformed();
    }

    void TouchPerformed(){
      if (isDragging) return;
      if (!TouchContact.IsPressed()) return;

      Ray ray = mainCam.ScreenPointToRay(Touch.ReadValue<Vector2>());

      int interactableHitCount = Physics.RaycastNonAlloc(ray, interactableHits, MAX_RAY_DISTANCE);

      if (interactableHitCount <= 0) return;

      var results = interactableHits.Take(interactableHitCount);
      targetCard = results.Select(
        hit => hit.collider.GetComponent<Card>()).FirstOrDefault(draggable => draggable != null);

      if (targetCard is null) return;

      if (FirstTouch.WasPerformedThisFrame()){
        if (player.BoardManager.IsFourCardPilesRemoved()){
          targetCard.OnInteractJustPerformed();
        }
        else{ // there is more than 1 card pile
          if (player.BoardManager.IsBoardCard(targetCard)){
            player.BoardManager.SetChosenBoardPile(targetCard.AttachedCardPile);
          }
          else{
            if (player.BoardManager.ChosenBoardPile != null){ // you picked board card before
              player.BoardManager.AddCardToPile(player.BoardManager.ChosenBoardPile, targetCard);
              player.PlayerHandManager.RemoveCardFromYourHand(targetCard);
            }
            else{ // you need pick board card first
              DOTween.Complete(Keys.Tween.Card);
              player.BoardManager.JumpTopBoardCards();
              targetCard.transform.DOMoveY(targetCard.transform.position.y + 0.05f, 0.1f).SetLoops(4, LoopType.Yoyo).SetId(Keys.Tween.Card);
            }

          }

        }
      }
    }

  }

}