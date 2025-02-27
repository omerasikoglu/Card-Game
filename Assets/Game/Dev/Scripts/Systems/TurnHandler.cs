using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class TurnHandler{
    public event Action                        OnGameStart = delegate{ };
    public event EventHandler<ResultEventArgs> OnGameEnded = delegate{ };

    public class ResultEventArgs : EventArgs{
      public readonly bool IsWin;
      public readonly int  Score;
      public readonly int  BetCount;

      public readonly int  TotalSnapCount;
      public readonly int  TotalAceCount;
      public readonly bool MoreCards;
      public readonly bool MoreClubs;

      public ResultEventArgs(bool isWin, int betCount, int score, int totalSnapCount, int totalAceCount, bool moreCards, bool moreClubs){
        IsWin          = isWin;
        BetCount       = betCount;
        Score          = score;
        TotalSnapCount = totalSnapCount;
        TotalAceCount  = totalAceCount;
        MoreCards      = moreCards;
        MoreClubs      = moreClubs;
      }
    }

    // public event Action         OnCardDistributeEnded = delegate{ };
    public event Action<Entity> OnNewTurnStart = delegate{ };

    CanvasController canvasController;
    DeckManager      deckManager;
    BoardManager     boardManager;
    SaveLoadSystem  saveLoadSystem;

    List<Entity> entities; // AI included

    Entity currentEntity;

    public void Init(CanvasController canvasController, DeckManager deckManager, BoardManager boardManager, SaveLoadSystem saveLoadSystem){
      this.canvasController = canvasController;
      this.deckManager      = deckManager;
      this.boardManager     = boardManager;
      this.saveLoadSystem     = saveLoadSystem;
    }

    public void SetPlayers(IEnumerable<Entity> entities){
      this.entities = entities.ToList();
    }

    public void OnToggle(bool to){
      if (to){
        canvasController.OnGameStart    += StartGame;
        canvasController.OnQuitInGame   += QuitGame;
        boardManager.OnCardPilesCreated += DistributeCards;
        boardManager.OnCardPlayed       += CheckAllHandsAreEmpty;
      }
      else{
        canvasController.OnGameStart    -= StartGame;
        canvasController.OnQuitInGame   -= QuitGame;
        boardManager.OnCardPilesCreated -= DistributeCards;
        boardManager.OnCardPlayed       -= CheckAllHandsAreEmpty;
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Local Functions 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void StartGame(){
        OnGameStart.Invoke();
      }

      void QuitGame(){ }

      async void DistributeCards(){
        const float duration = 0.2f;

        for (int i = 0; i < 4; i++){
          DistributeOneCardToEveryone();
          await UniTask.WaitForSeconds(duration);
        }

        NewTurnStarted();

        async void DistributeOneCardToEveryone(){
          entities[0].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration);
          entities[1].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration);
        }
      }

      void CheckAllHandsAreEmpty(bool isDealerDraw){
        if (isDealerDraw) return;
        bool isAllHandsEmpty = entities.All(o => o.HandManager.GetHoldingCardCount() == 0);
        bool isDeckEmpty     = deckManager.IsDeckEmpty();

        if (isAllHandsEmpty && isDeckEmpty){ // Game Ended
          OnGameEnded.Invoke(this,
            new ResultEventArgs(
              true,
              saveLoadSystem.CurrentBet * 2,
              boardManager.Score,
              0,
              0,
              false,
              false
            ));
        }
        else if (isAllHandsEmpty && !isDeckEmpty){
          DistributeCards();
        }
        else{
          NewTurnStarted();
        }
      }

      void NewTurnStarted(){

        if (currentEntity == null){ // first turn
          currentEntity = entities[0];
        }
        else{
          int index = entities.IndexOf(currentEntity);
          index         = (index + 1) % entities.Count;
          currentEntity = entities[index];
        }

        OnNewTurnStart.Invoke(currentEntity);
      }
    }

    public Entity GetActiveEntity(){
      return currentEntity; // returns which player's turn
    }
  }

}