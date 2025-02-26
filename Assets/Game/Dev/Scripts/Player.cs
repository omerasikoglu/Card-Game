using System.Collections.Generic;
using UnityEngine;

namespace CardGame{

  public class Player : Entity{
    PlayerInput playerInput;

  #region Core
    public override void Init(IReadOnlyList<Transform> cardHoldTransforms){
      base.Init(cardHoldTransforms);
      playerInput = new(this);

    }

    public override void OnToggle(bool to){
      base.OnToggle(to);
      playerInput.OnToggle(to);
    }

    protected override void OnNewTurnStart(Entity ctx){ 
      // could be oppenents' turn
      
      bool isYourTurnStarted = ctx == this;
      playerInput.OnToggle(isYourTurnStarted);
    }

    public void Update(){
      playerInput.Update();
    }
  #endregion

  }

}