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

    [Inject] readonly DeckManager    deckManager;
    [Inject] readonly SaveLoadSystem saveLoadSystem;

    bool isStartingBoardPilesRemoved;

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

    public async void AddOneCardToEachPiles(){
      isStartingBoardPilesRemoved = false;

      foreach (CardPile pile in cardPiles){
        var topDeckCard = deckManager.DrawCard();
        if (topDeckCard == null) return;
        pile.AddCard(topDeckCard).Forget();
        await UniTask.WaitForSeconds(0.1f);
      }

    }

    public List<CardPile> GetAvailableCardPiles(){
      var fourPileList = cardPiles.Where(o => o != null).ToList();
      var onePileList  = new List<CardPile>{ oneCardPile };

      return isStartingBoardPilesRemoved ? onePileList : fourPileList;
    }

    public void AddCardToOneCardPile(Card card){
      oneCardPile.AddCard(card).Forget();
    }

    public void AddCardToPile(CardPile cardPile, Card card){
      cardPile.AddCard(card).Forget();
      ClearChosenBoardPile();
    }

    public void CheckPiles(CardPile cardPile){
      if (isStartingBoardPilesRemoved) return;

      if (cardPiles.Count > 1){
        cardPiles.Remove(cardPile);
      }
      else{ // Last Pile
        CardPile remainingCardPile = cardPiles.First(o => o != null);
        var      remainingCards    = remainingCardPile.GetAllCards();
        Debug.Log($"remainingCards.Count: <color=green>{remainingCards.Count}</color>");

        foreach (Card card in remainingCards){
          AddCardToPile(oneCardPile, card);
        }

        remainingCardPile.ClearAll();
        isStartingBoardPilesRemoved = true;
      }
    }

  # region Get
    public bool IsFourCardPilesRemoved(){
      return isStartingBoardPilesRemoved;
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

    public void GainPoint(int point){
      saveLoadSystem.UpdateCurrency(point);
    }

    public IEnumerable<Card> GetBoardTopCards(){
      List<Card> topCards;

      if (IsFourCardPilesRemoved()){
        topCards = new List<Card>{ oneCardPile.PeekTopCard() };
      }
      else{
        topCards = cardPiles.Where(o => o.PeekTopCard() != null).Select(o => o.PeekTopCard()).ToList();
      }

      return topCards;
    }

  }

}