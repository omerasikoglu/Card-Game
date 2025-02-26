using System.Collections.Generic;
using System.Linq;
using CardGame.World;
using UnityEngine;

namespace CardGame{

  public class Opponent : Entity{

    void Awake(){
      PlayerHandManager = new(this, cardHoldTransforms);
    }

    protected override void OnTurnStart(Entity ctx){
      if (ctx != this) return;
      PlayBestCard();

    }

    void PlayBestCard(){
      var holdingCards = PlayerHandManager.GetHoldingCards();
      var boardCards   = BoardManager.GetBoardCards();

      List<Card> intersectCards = new();

      foreach (Card holdingCard in holdingCards){
        foreach (Card boardCardVARIABLE in boardCards){
          if (holdingCard.CardType == boardCardVARIABLE.CardType){
            intersectCards.Add(holdingCard);
          }
        }
      }

      if (intersectCards.Count > 0){
        var bestPlay = intersectCards.First(o => o.CardPoint == intersectCards.Max(p => p.CardPoint));
        PlayAutomatically(bestPlay);
      }
      else{
        var randomPlay = holdingCards[Random.Range(0, holdingCards.Count)];
        PlayAutomatically(randomPlay);
      }

      void PlayAutomatically(Card card){
        var targetPile = BoardManager.GetAvailableCardPiles().First();
        BoardManager.AddCardToPile(targetPile, card);
        PlayerHandManager.RemoveCardFromYourHand(card);
      }
    }

  }

}