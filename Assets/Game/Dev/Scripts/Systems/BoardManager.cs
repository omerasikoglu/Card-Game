using System.Collections.Generic;
using System.Linq;
using CardGame.Utils;
using CardGame.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class BoardManager{

    [Inject] readonly DeckManager deckManager;

    bool isFourCardPilesRemoved;

    readonly List<CardPile> cardPiles;
    readonly CardPile       oneCardPile;

    public CardPile ChosenBoardPile{get; private set;} = null;

    public BoardManager(Transform[] boardCardRoots, Transform boardOneCardRoot){
      cardPiles   = new();
      oneCardPile = new CardPile(this, boardOneCardRoot);

      for (int i = 0; i < 4; i++){
        CardPile newCardPile = new CardPile(this, boardCardRoots[i]);
        newCardPile.ToggleGreenSphere(false);
        cardPiles.Add(newCardPile);
      }
    }

    public async void AddCardToPiles(){
      isFourCardPilesRemoved = false;

      foreach (CardPile pile in cardPiles){
        var topDeckCard = deckManager.DrawCard();
        if (topDeckCard == null) return;
        pile.AddCard(topDeckCard).Forget();
        await UniTask.WaitForSeconds(0.1f);
      }

    }

    public void AddCardToPile(CardPile cardPile, Card card){
      cardPile.AddCard(card).Forget();
      ClearChosenBoardPile();
    }

    public void RemovePile(CardPile cardPile){
      if (isFourCardPilesRemoved) return;

      if (cardPiles.Count > 1){
        cardPiles.Remove(cardPile);
      }
      else{ // Last Pile
        CardPile remainingCardPile = cardPiles.First(o => o != null);
        var      remainingCards    = remainingCardPile.GetAllCards();
        remainingCards.ForEach(o => oneCardPile.AddCard(o).Forget());
        remainingCardPile.ClearAll();
        isFourCardPilesRemoved = true;
      }
    }

  # region Get
    public bool IsFourCardPilesRemoved(){
      return isFourCardPilesRemoved;
    }
  #endregion

    public bool IsBoardCard(Card card){
      return card.AttachedCardPile != null;
    }
    
    public void SetChosenBoardPile(CardPile cardPile){
      ClearChosenBoardPile();
      ChosenBoardPile = cardPile;
      cardPile.ToggleGreenSphere(true);
    }
    
    public void ClearChosenBoardPile(){
      ChosenBoardPile = null;
      cardPiles.ForEach(o => o.ToggleGreenSphere(false));
    }

    public void JumpTopBoardCards(){
      DOTween.Complete(Keys.Tween.Card);
      
      foreach (var cardPile in cardPiles){
        var targetCard = cardPile.PeekTopCard();
        targetCard.transform.DOMoveY(targetCard.transform.position.y + 0.05f, 0.1f).SetLoops(4, LoopType.Yoyo).SetId(Keys.Tween.Card);        
      }
    }
  }

}