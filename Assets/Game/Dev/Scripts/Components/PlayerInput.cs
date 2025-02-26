using System.Linq;
using CardGame.Systems;
using CardGame.Utils;
using CardGame.World;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardGame{

  public class PlayerInput{

  #region Members
    Card targetCardHit;

    readonly Player       player;
    readonly BoardManager boardManager;
    readonly Camera       mainCam;
    readonly InputActions inputActions;
    readonly RaycastHit[] interactableHits = new RaycastHit[10];

    const float MAX_RAY_DISTANCE = 3f;
  #endregion

    InputAction Touch        => inputActions.Inventory.Touch;
    InputAction TouchContact => inputActions.Inventory.TouchContact;
    InputAction FirstTouch   => inputActions.Inventory.FirstTouch;

    public PlayerInput(Player player){
      this.player  = player;
      boardManager = player.BoardManager;
      mainCam      = Camera.main;

      inputActions = new();
      inputActions.Enable();
    }

    public void OnToggle(bool to){
      if (to){
        inputActions.Enable();
      }
      else{
        inputActions.Disable();
      }
    }

    public void Update(){
      TouchPerformed();
    }

    void TouchPerformed(){
      if (!TouchContact.IsPressed()) return;

      Ray ray = mainCam.ScreenPointToRay(Touch.ReadValue<Vector2>());

      int interactableHitCount = Physics.RaycastNonAlloc(ray, interactableHits, MAX_RAY_DISTANCE);

      if (interactableHitCount <= 0) return;

      var results = interactableHits.Take(interactableHitCount);
      targetCardHit = results.Select(
        hit => hit.collider.GetComponent<Card>()).FirstOrDefault(draggable => draggable != null);

      if (targetCardHit is null) return;

      if (FirstTouch.WasPerformedThisFrame()){
        Logic();
      }
    }

    void Logic(){
      if (boardManager.IsFourCardPilesRemoved()){
        if (targetCardHit.AttachedCardPile != null) return;
        // targetCardHit.OnInteractJustPerformed();
        boardManager.AddCardToOneCardPile(targetCardHit);
        player.PlayerHandManager.RemoveCardFromYourHand(targetCardHit);
      }
      else{ // there is more than 1 card pile
        if (boardManager.IsBoardCard(targetCardHit)){
          boardManager.SetChosenBoardPile(targetCardHit.AttachedCardPile);
        }
        else{
          if (boardManager.ChosenBoardPile != null){ // you picked board card before
            boardManager.AddCardToPile(boardManager.ChosenBoardPile, targetCardHit);
            player.PlayerHandManager.RemoveCardFromYourHand(targetCardHit);
          }
          else{ // you need pick board card first
            DOTween.Complete(Keys.Tween.Card);
            boardManager.JumpTopBoardCards();
            targetCardHit.transform.DOMoveY(targetCardHit.transform.position.y + 0.05f, 0.1f).SetLoops(4, LoopType.Yoyo).SetId(Keys.Tween.Card);
          }

        }

      }
    }
  }

}