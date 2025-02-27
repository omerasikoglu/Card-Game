using System.Collections.Generic;
using UnityEngine;

namespace CardGame{

  public class Player : Entity{
    public PlayerInput PlayerInput{get; private set;}

  #region Core
    public override void Init(IReadOnlyList<Transform> cardHoldTransforms){
      base.Init(cardHoldTransforms);
      PlayerInput = new(this);

    }

    public void OpenInput()  => PlayerInput.OnToggle(true);
    public void CloseInput() => PlayerInput.OnToggle(false);

    // public void OnToggle(bool to){
    //   base.OnToggle(to);
    //   PlayerInput.OnToggle(to);
    // }

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