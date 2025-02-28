using System.Collections.Generic;
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

    Plate previousPlateHit;

    const float MAX_RAY_DISTANCE = 3f;
  #endregion

    InputAction Touch        => inputActions.Inventory.Touch;
    InputAction TouchContact => inputActions.Inventory.TouchContact;
    InputAction FirstTouch   => inputActions.Inventory.FirstTouch;

    public PlayerInput(Player player){
      this.player  = player;
      boardManager = player.InjectedBoardManager;
      mainCam      = Camera.main;

      inputActions = new();
      inputActions.Disable();
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

      var results = interactableHits.Take(interactableHitCount).ToList();

      if (CheckPlateHit(results)) return;

      CheckCardHits(results);

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Local Functions 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      bool CheckPlateHit(IEnumerable<RaycastHit> results){
        var newPlateHit = results
          .Select(hit => hit.collider.GetComponent<Plate>())
          .FirstOrDefault(plate => plate != null && plate.IsInteractEnable());
        
        // if(newPlateHit is null) return false;
        
        bool wasRayEnterThisFrame   = newPlateHit != null && previousPlateHit == null;
        bool wasRayExitThisFrame    = newPlateHit == null && previousPlateHit != null;
        bool wasPlateTouchPerformed = previousPlateHit != null && newPlateHit != null && FirstTouch.WasPerformedThisFrame();

        if (wasRayEnterThisFrame){
          newPlateHit.OnRayEnter();
          previousPlateHit = newPlateHit;
          return true;
        }

        if (wasRayExitThisFrame){
          previousPlateHit.OnRayExit();
          previousPlateHit = newPlateHit;
          return true;
        }

        if (wasPlateTouchPerformed){
          previousPlateHit.OnInteractJustPerformed();
          player.PlayerInput.OnToggle(false);
          player.CanvasController.OpenPlayerInfoPanel();
          return true;
        }

        return false;
      }

      void CheckCardHits(IEnumerable<RaycastHit> results){
        targetCardHit = results.Select(hit => hit.collider.GetComponent<Card>()).FirstOrDefault(o => o != null);
        if (targetCardHit is null) return;

        if (FirstTouch.WasPerformedThisFrame()){
          Logic();
        }
      }
    }

    void Logic(){
      if(targetCardHit.IsInDeck)  return;
      
      if (boardManager.IsFourCardPilesRemoved()){
        if (targetCardHit.AttachedCardPile != null) return;
        // targetCardHit.OnInteractJustPerformed();
        boardManager.AddCardToOneCardPile(targetCardHit);
        player.HandManager.RemoveCardFromYourHand(targetCardHit);
        OnToggle(false);
      }
      else{ // there is more than 1 card pile
        if (boardManager.IsBoardCard(targetCardHit)){
          boardManager.SetChosenBoardPile(targetCardHit.AttachedCardPile);
        }
        else{
          if (boardManager.ChosenBoardPile != null){ // you picked board card before
            boardManager.AddCardToPile(boardManager.ChosenBoardPile, targetCardHit);
            player.HandManager.RemoveCardFromYourHand(targetCardHit);
            OnToggle(false);
          }
          else{ // you need pick board card first
            DOTween.Complete(Keys.Tween.Card);
            boardManager.AnimateJumpTopBoardCards();
            targetCardHit.transform.DOMoveY(targetCardHit.transform.position.y + 0.05f, 0.1f).SetLoops(4, LoopType.Yoyo).SetId(Keys.Tween.Card);
          }

        }

      }
    }
  }

}