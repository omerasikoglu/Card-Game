using System.Collections.Generic;
using System.Linq;
using CardGame.Systems;
using CardGame.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame{

  public class Opponent : Entity{

    const float moveCalculateDuration = 0.6f; 
    
    protected override void OnNewTurnStart(Entity ctx){
      bool isYourTurnStarted = ctx == this;
      if(!isYourTurnStarted) return;
      
      PlayBestCard();

    }

    async void PlayBestCard(){
      await UniTask.WaitForSeconds(moveCalculateDuration);

      var holdingCards = HandManager.GetHoldingCards();
      var boardCards   = BoardManager.GetBoardTopCards();

      List<Card> intersectCards = new();

      foreach (Card holdingCard in holdingCards){
        foreach (Card boardCardVARIABLE in boardCards){
          if (holdingCard.CardNumber == boardCardVARIABLE.CardNumber){
            intersectCards.Add(holdingCard);
          }
        }
      }

      if (intersectCards.Count > 0){
        var bestPlay       = intersectCards.First(o => o.CardPoint == intersectCards.Max(p => p.CardPoint));
        var availablePiles = BoardManager.GetAvailableCardPiles();
        var targetPile     = availablePiles.First(o => o.PeekTopCard().CardNumber == bestPlay.CardNumber);
        PlayAutomatically(bestPlay, targetPile);
      }
      else{
        var randomPlay = holdingCards[Random.Range(0, holdingCards.Count)];
        var randomPile = BoardManager.GetAvailableCardPiles().OrderBy(o => Random.value).First();
        PlayAutomatically(randomPlay, randomPile);
      }

      void PlayAutomatically(Card card, CardPile targetPile){
        BoardManager.AddCardToPile(targetPile, card);
        HandManager.RemoveCardFromYourHand(card);
      }
    }

  }

}