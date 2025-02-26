using CardGame.Components;
using CardGame.Systems;
using UnityEngine;
using VContainer;

namespace CardGame{

  public abstract class Entity : MonoBehaviour{
    [Inject] public BoardManager BoardManager{get; private set;}
    [Inject] public DeckManager  DeckManager {get; private set;}
    [Inject] public TurnManager  TurnManager {get; private set;}

    [SerializeField] protected Transform[] cardHoldTransforms;

    public PlayerHandManager PlayerHandManager{get; protected set;}

    void OnEnable()  => OnToggle(true);
    void OnDisable() => OnToggle(false);

    protected virtual void OnToggle(bool to){
      if (to){
        TurnManager.OnTurnStart += OnTurnStart;
      }else{
        TurnManager.OnTurnStart -= OnTurnStart;
      }
    }

    protected abstract void OnTurnStart(Entity ctx);

  }

}