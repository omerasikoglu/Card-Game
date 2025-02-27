using System.Collections.Generic;
using System.Linq;
using CardGame.Systems;
using CardGame.World;
using Cysharp.Threading.Tasks;
using RunTogether.Extensions;
using UnityEngine;

namespace CardGame{

  public class Opponent : Entity{

    const float moveCalculateDuration = 0.8f; 
    
    protected override void OnNewTurnStart(Entity ctx){
      base.OnNewTurnStart(ctx);
      
      bool isYourTurnStarted = ctx == this;
      if(!isYourTurnStarted) return;
      
      PlayBestCard();

    }

    async void PlayBestCard(){
      await UniTask.WaitForSeconds(moveCalculateDuration);

      var holdingCards = HandManager.GetHoldingCards();
      var boardCards   = InjectedBoardManager.GetBoardTopCards();

      if (boardCards.IsNullOrEmpty()){
        PlayAutomatically(holdingCards.Last(), InjectedBoardManager.GetOneCardPile());
        return;
      }

      List<Card> intersectCards = new();

      foreach (Card holdingCard in holdingCards){
        foreach (Card boardCard in boardCards){
          if(boardCard == null) continue;
          if (holdingCard.CardNumber == boardCard.CardNumber){
            intersectCards.Add(holdingCard);
          }
        }
      }
 
      if (intersectCards.Count > 0){
        var bestPlay       = intersectCards.First(o => o.CardPoint == intersectCards.Max(p => p.CardPoint));
        var availablePiles = InjectedBoardManager.GetAvailableCardPiles();
        var targetPile     = availablePiles.First(o => o.PeekTopCard().CardNumber == bestPlay.CardNumber);
        PlayAutomatically(bestPlay, targetPile);
      }
      else{
        var randomPlay = holdingCards[Random.Range(0, holdingCards.Count)];
        var randomPile = InjectedBoardManager.GetAvailableCardPiles().OrderBy(o => Random.value).First();
        PlayAutomatically(randomPlay, randomPile);
      }

      void PlayAutomatically(Card card, CardPile targetPile){
        InjectedBoardManager.AddCardToPile(targetPile, card);
        HandManager.RemoveCardFromYourHand(card);
      }
    }

  }

}