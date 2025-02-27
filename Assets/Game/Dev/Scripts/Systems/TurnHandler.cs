using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class TurnHandler{
    public event Action OnGameStart = delegate{ };
    public event Action OnGameEnded = delegate{ };
    // public event Action         OnCardDistributeEnded = delegate{ };
    public event Action<Entity> OnNewTurnStart = delegate{ };

    CanvasController canvasController;
    DeckManager      deckManager;
    BoardManager     boardManager;

    List<Entity> entities; // AI included

    Entity currentEntity;

    public void Init(CanvasController canvasController, DeckManager deckManager, BoardManager boardManager){
      this.canvasController = canvasController;
      this.deckManager      = deckManager;
      this.boardManager     = boardManager;
    }

    public void SetPlayers(IEnumerable<Entity> entities){
      this.entities = entities.ToList();
    }

    public void OnToggle(bool to){
      if (to){
        canvasController.OnGameStart += StartGame;
        boardManager.OnCardPilesCreated += DistributeCards;
        boardManager.OnCardPlayed       += CheckAllHandsAreEmpty;
      }
      else{
        canvasController.OnGameStart -= StartGame;
        boardManager.OnCardPilesCreated -= DistributeCards;
        boardManager.OnCardPlayed    -= CheckAllHandsAreEmpty;
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Local Functions 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void StartGame(){
        OnGameStart.Invoke();
      }

      async void DistributeCards(){
        const float duration = 0.2f;

        for (int i = 0; i < 4; i++){
          DistributeOneCardToEveryone();
          await UniTask.WaitForSeconds(duration);
        }

        OnNewTurnStarted();

        async void DistributeOneCardToEveryone(){
          entities[0].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration);
          entities[1].HandManager.AddCardToHand();
          await UniTask.WaitForSeconds(duration);
        }
      }

      void CheckAllHandsAreEmpty(bool isAutoPlay){
        if(isAutoPlay) return;
        
        if (deckManager.IsDeckFull()) return;
        bool isAllHandsEmpty = entities.All(o => o.HandManager.GetHoldingCardCount() == 0);
        bool isDeckEmpty     = deckManager.IsDeckEmpty();

        if (isAllHandsEmpty && isDeckEmpty){
          OnGameEnded.Invoke();
        }
        else if (isAllHandsEmpty && !isDeckEmpty){
          DistributeCards();
        }
      }
      
      void OnNewTurnStarted(){

        if (currentEntity == null){ // first turn
          currentEntity = entities[0];
        }
        else{
          int index = entities.IndexOf(currentEntity);
          index         = (index + 1) % entities.Count;
          currentEntity = entities[index];
        }

        Debug.Log($"currentEntity: <color=green>{currentEntity}</color>");
        OnNewTurnStart.Invoke(currentEntity);
      }
    }
  }

}