using System.Collections.Generic;
using CardGame.Systems;
using CardGame.UI;
using UnityEngine;
using VContainer;

namespace CardGame{

  public class Player : Entity{
    public PlayerInput PlayerInput{get; private set;}

  #region Core
    public override void Init(GameObject platePrefab, IReadOnlyList<Transform> cardHoldTransforms, Transform plateRoot,
      CanvasController canvasController, TurnHandler turnHandler, BoardManager boardManager, DeckManager deckManager){
      base.Init(platePrefab, cardHoldTransforms, plateRoot, canvasController, turnHandler, boardManager, deckManager);
      PlayerInput = new(this);

    }

    public override void OnToggle(bool to){
      base.OnToggle(to);
      if (to){
        CanvasController.OnPlayerInputToggle += PlayerInput.OnToggle;
      }
      else{
        CanvasController.OnPlayerInputToggle -= PlayerInput.OnToggle;
      }
    }

    protected override void OnNewTurnStart(Entity ctx){
      base.OnNewTurnStart(ctx);
      
      bool isYourTurnStarted = ctx == this;
      PlayerInput.OnToggle(isYourTurnStarted);
    }

    public void Update(){
      PlayerInput.Update();
    }
  #endregion

  }

}