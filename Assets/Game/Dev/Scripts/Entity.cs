using System.Collections.Generic;
using CardGame.Components;
using CardGame.Systems;
using CardGame.UI;
using CardGame.World;
using RunTogether.Extensions;
using UnityEngine;
using VContainer;

namespace CardGame{

  public abstract class Entity{
    [Inject] public CanvasController CanvasController    {get; private set;}
    [Inject] public TurnHandler      TurnHandler         {get; private set;}
    [Inject] public BoardManager     InjectedBoardManager{get; private set;}
    [Inject] public DeckManager      DeckManager         {get; private set;}

    public    HandManager HandManager{get; private set;} // holding cards
    protected Plate       Plate      {get; private set;}

    public virtual void Init(GameObject platePrefab, IReadOnlyList<Transform> cardHoldTransforms, Transform plateRoot){
      HandManager = new(this, cardHoldTransforms);

      SpawnPlate(platePrefab, plateRoot);
    }

    public virtual void OnToggle(bool to){
      if (to){
        TurnHandler.OnNewTurnStart          += OnNewTurnStart;
        TurnHandler.OnGameStart             += EnablePlate;
        TurnHandler.OnGameEnded             += DisablePlate;
        InjectedBoardManager.OnScoreChanged += Plate.SetScoreText;
      }
      else{
        TurnHandler.OnNewTurnStart          -= OnNewTurnStart;
        TurnHandler.OnGameStart             -= EnablePlate;
        TurnHandler.OnGameEnded             -= DisablePlate;
        InjectedBoardManager.OnScoreChanged -= Plate.SetScoreText;
      }

      void EnablePlate(){
        Plate.gameObject.Toggle(true);
      }
      void DisablePlate(object sender, TurnHandler.ResultEventArgs e){
        Plate.gameObject.Toggle(false);
      }

    }

    void SpawnPlate(GameObject prefab, Transform root){
      Plate = Object.Instantiate(prefab, root).GetComponent<Plate>();
      Plate.Init(this, root);
    }

    protected virtual void OnNewTurnStart(Entity entity){
      Plate.ToggleYourTurn(entity == this);
    }

  }

}