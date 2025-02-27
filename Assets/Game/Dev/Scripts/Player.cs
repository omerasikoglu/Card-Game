using System.Collections.Generic;
using UnityEngine;

namespace CardGame{

  public class Player : Entity{
    public PlayerInput PlayerInput{get; private set;}

  #region Core
    public override void Init(GameObject platePrefab, IReadOnlyList<Transform> cardHoldTransforms, Transform plateRoot){
      base.Init(platePrefab, cardHoldTransforms, plateRoot);
      PlayerInput = new(this);

    }

    public void OpenInput()  => PlayerInput.OnToggle(true);
    public void CloseInput() => PlayerInput.OnToggle(false);

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
      // could be oppenents' turn
      
      bool isYourTurnStarted = ctx == this;
      PlayerInput.OnToggle(isYourTurnStarted);
    }

    public void Update(){
      PlayerInput.Update();
    }
  #endregion

  }

}