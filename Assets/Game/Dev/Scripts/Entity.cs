using System.Collections.Generic;
using CardGame.Systems;
using CardGame.UI;
using CardGame.World;
using RunTogether.Extensions;
using UnityEngine;

namespace CardGame{

  public abstract class Entity{
    public CanvasController CanvasController{get; private set;}
    public TurnHandler      TurnHandler     {get; private set;}
    public BoardManager     BoardManager    {get; private set;}
    public DeckManager      DeckManager     {get; private set;}

  #region Members
    Plate plate;

    public HandManager HandManager{get; private set;} // holding cards

    public int Score     {get; private set;}
    public int SnapCount {get; private set;}
    public int AceCount  {get; private set;}
    public int CardCount {get; private set;}
    public int ClubsCount{get; private set;}
  #endregion

    public virtual void Init(GameObject platePrefab, IReadOnlyList<Transform> cardHoldTransforms, Transform plateRoot,
      CanvasController canvasController, TurnHandler turnHandler, BoardManager boardManager, DeckManager deckManager){
      HandManager = new(this, cardHoldTransforms);

      CanvasController = canvasController;
      TurnHandler      = turnHandler;
      BoardManager     = boardManager;
      DeckManager      = deckManager;

      SpawnPlate(platePrefab, plateRoot);

      void SpawnPlate(GameObject prefab, Transform root){
        plate = Object.Instantiate(prefab, root).GetComponent<Plate>();
        plate.Init(this, root);
      }
    }

    public virtual void OnToggle(bool to){
      if (to){
        TurnHandler.OnGameStart       += GameStarted;
        TurnHandler.OnNewTurnStart    += OnNewTurnStart;
        TurnHandler.OnGameEnded       += GameEnded;
        CanvasController.OnQuitInGame += GameQuit;
      }
      else{
        TurnHandler.OnGameStart       -= GameStarted;
        TurnHandler.OnNewTurnStart    -= OnNewTurnStart;
        TurnHandler.OnGameEnded       -= GameEnded;
        CanvasController.OnQuitInGame -= GameQuit;
      }

      void GameStarted(int totalBet){
        plate.SeTotalBetText(totalBet);
        SetScore(default);
        SetSnapCount(default);
        SetAceCount(default);
        SetCardCount(default);
        SetClubsCount(default);
      }
      void GameEnded(object sender, TurnHandler.ResultEventArgs e){
        plate.gameObject.Toggle(false);
      }

      void GameQuit(){
        plate.gameObject.Toggle(false);
      }

    }

    protected virtual void OnNewTurnStart(Entity entity){
      plate.ToggleYourTurnText(entity == this);
    }

  #region Set
    public void TogglePlate(bool to){
      plate.gameObject.Toggle(to);
    }

    void SetScore(int to){
      Score = to;
      plate.SetScoreText(Score, this);
    }

    void SetSnapCount(int to)  => SnapCount = to;
    void SetAceCount(int to)   => AceCount = to;
    void SetCardCount(int to)  => CardCount = to;
    void SetClubsCount(int to) => ClubsCount = to;

    public void UpdateScore(int delta){
      Score += delta;
      plate.SetScoreText(Score, this);
    }

    public void UpdateSnapCount(int delta){
      SnapCount += delta;
    }

    public void UpdateAceCount(int delta){
      AceCount += delta;
    }

    public void UpdateCardCount(int delta){
      CardCount += delta;
    }

    public void UpdateClubsCount(int delta){
      ClubsCount += delta;
    }
  #endregion

  }

}