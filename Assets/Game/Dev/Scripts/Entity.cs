using System.Collections.Generic;
using CardGame.Components;
using CardGame.Systems;
using UnityEngine;
using VContainer;

namespace CardGame{

  public abstract class Entity{
    [Inject] public BoardManager BoardManager{get; private set;}
    [Inject] public DeckManager  DeckManager {get; private set;}
    [Inject] public TurnManager  TurnManager {get; private set;}

    public HandManager HandManager{get; private set;} // holding cards

    public virtual void Init(IReadOnlyList<Transform> cardHoldTransforms){
      HandManager = new(this, cardHoldTransforms);
    }

    public virtual void OnToggle(bool to){
      if (to){
        TurnManager.OnNewTurnStart += OnNewTurnStart;
      }
      else{
        TurnManager.OnNewTurnStart -= OnNewTurnStart;
      }
    }

    public void AddCardToHand(){
      HandManager.AddCardToYourHand(this);
    }

    protected abstract void OnNewTurnStart(Entity ctx);

  }

}