using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CardGame.UI;
using Cysharp.Threading.Tasks;

namespace CardGame.Systems{

  public class TurnHandler{
    public event Action<int>                   OnGameStart = delegate{ }; // totalBet
    public event EventHandler<ResultEventArgs> OnGameEnded = delegate{ };

    public class ResultEventArgs : EventArgs{
      public readonly bool IsWin;
      public readonly int  Score;
      public readonly int  TotalBetSumCount;

      public readonly int  TotalSnapCount;
      public readonly int  TotalAceCount;
      public readonly bool MoreCards;
      public readonly bool MoreClubs;

      public ResultEventArgs(bool isWin, int totalBetSumCount, int score, int totalSnapCount, int totalAceCount, bool moreCards, bool moreClubs){
        IsWin            = isWin;
        TotalBetSumCount = totalBetSumCount;
        Score            = score;
        TotalSnapCount   = totalSnapCount;
        TotalAceCount    = totalAceCount;
        MoreCards        = moreCards;
        MoreClubs        = moreClubs;
      }
    }

    public event Action<Entity> OnNewTurnStart = delegate{ };

  #region Members
    CanvasController canvasController;
    DeckManager      deckManager;
    BoardManager     boardManager;
    SaveLoadSystem   saveLoadSystem;

    List<Entity> entities; // AI included
    List<Entity> currentEntities;

    Entity whoIsTurn;
    
    CancellationTokenSource distributeTokenSource = new();
  #endregion

    public void Init(CanvasController canvasController, DeckManager deckManager, 
      BoardManager boardManager, SaveLoadSystem saveLoadSystem){
      this.canvasController = canvasController;
      this.deckManager      = deckManager;
      this.boardManager     = boardManager;
      this.saveLoadSystem   = saveLoadSystem;
    }

    public void SetEntities(IEnumerable<Entity> entities){
      this.entities   = entities.ToList();
      currentEntities = new();
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

      void StartGame(bool isTwoPlayerMode){
        if (isTwoPlayerMode){
          currentEntities = new List<Entity>{ entities[0], entities[1] };
        }
        else{
          currentEntities = new List<Entity>{ entities[0], entities[1], entities[2], entities[3] };
        }
        whoIsTurn = null;
        
        entities.ForEach( o => o.TogglePlate(false));
        for (int i = 0; i < currentEntities.Count; i++){
          currentEntities[i].TogglePlate(true);
        }
        
        OnGameStart.Invoke(saveLoadSystem.CurrentBet * currentEntities.Count);
      }

      void QuitGame(){
        distributeTokenSource?.Cancel();
        
        OnGameEnded.Invoke(this,
          new ResultEventArgs(
            false,
            saveLoadSystem.CurrentBet * currentEntities.Count,
            currentEntities.First().Score,
            currentEntities.First().SnapCount,
            currentEntities.First().AceCount,
            currentEntities.Where(o => o != currentEntities[0]).All(o => currentEntities.First().CardCount > o.CardCount),
            currentEntities.First().ClubsCount >= 2
          ));

        deckManager.ResetDeck();
        boardManager.ResetBoard();
        currentEntities.ForEach(o => o.HandManager.ResetHand());
        currentEntities.Clear();
        whoIsTurn = null;
      }

      async void DistributeCards(){
        const float duration = 0.2f;

        for (int i = 0; i < 4; i++){
          DistributeOneCardToEveryone();
          await UniTask.WaitForSeconds(duration);
        }

        NewTurnStarted();

        async void DistributeOneCardToEveryone(){
          distributeTokenSource = new CancellationTokenSource();
          
          currentEntities[0].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration, cancellationToken: distributeTokenSource.Token).SuppressCancellationThrow ();
          currentEntities[1].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration, cancellationToken: distributeTokenSource.Token).SuppressCancellationThrow ();

          if (currentEntities.Count > 2){
            currentEntities[2].HandManager.AddCardToHand();
            await UniTask.WaitForSeconds(duration, cancellationToken: distributeTokenSource.Token).SuppressCancellationThrow() ; 
            currentEntities[3].HandManager.AddCardToHand();
            await UniTask.WaitForSeconds(duration, cancellationToken: distributeTokenSource.Token).SuppressCancellationThrow() ;
          } 

        }
      }

      void CheckAllHandsAreEmpty(bool isDealerDraw){
        if (isDealerDraw) return;
        bool isAllHandsEmpty = currentEntities.All(o => o.HandManager.GetHoldingCardCount() == 0);
        bool isDeckEmpty     = deckManager.IsDeckEmpty();

        if (isAllHandsEmpty && isDeckEmpty){ // Game Ended
          var  player       = currentEntities.First();
          bool isPlayerWin  = currentEntities.All(o => player.Score >= o.Score);
          bool haveMoreCard = currentEntities.Where(o => o != currentEntities[0]).All(o => player.CardCount > o.CardCount);

          OnGameEnded.Invoke(this,
            new ResultEventArgs(
              isPlayerWin,
              saveLoadSystem.CurrentBet * currentEntities.Count,
              player.Score,
              player.SnapCount,
              player.AceCount,
              haveMoreCard,
              player.ClubsCount >= 2
            ));
          boardManager.ResetBoard();
        }
        else if (isAllHandsEmpty){
          DistributeCards();
        }
        else{
          NewTurnStarted();
        }
      }

      void NewTurnStarted(){

        if (whoIsTurn == null){ // first turn
          whoIsTurn = currentEntities[0];
        }
        else{
          int index = currentEntities.IndexOf(whoIsTurn);
          index     = (index + 1) % currentEntities.Count;
          whoIsTurn = currentEntities[index];
        }

        OnNewTurnStart.Invoke(whoIsTurn);
      }
    }

    public Entity GetActiveEntity(){
      return whoIsTurn; // returns which player's turn
    }
  }

}