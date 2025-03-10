using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CardGame.Utils;
using CardGame.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RunTogether.Extensions;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class BoardManager{
    readonly DeckManager  deckManager;
    readonly TurnHandler  turnHandler;
    readonly AudioManager audioManager;

  #region Members
    public Action<bool> OnCardPlayed       = delegate{ }; // !: is Dealer's Draw, krupiye
    public Action       OnCardPilesCreated = delegate{ };

    bool isFourCardPilesRemoved;

    List<CardPile> cardPiles;
    CardPile       oneCardPile;

    readonly Transform[] boardCardRoots;
    readonly Transform   boardOneCardRoot;

    CancellationTokenSource tokenSource = new();

    public CardPile ChosenBoardPile{get; private set;} = null;
  #endregion

  #region Core
    public BoardManager(Transform[] boardCardRoots, Transform boardOneCardRoot, DeckManager deckManager, TurnHandler turnHandler, AudioManager audioManager){

      this.deckManager      = deckManager;
      this.turnHandler      = turnHandler;
      this.boardCardRoots   = boardCardRoots;
      this.boardOneCardRoot = boardOneCardRoot;
      this.audioManager     = audioManager;

      CreatePiles();
    }

    public void OnToggle(bool to){
      if (to){
        deckManager.OnDeckCreated += AddOneCardToEachPiles;
        turnHandler.OnGameEnded   += GameEnded;
      }
      else{
        deckManager.OnDeckCreated -= AddOneCardToEachPiles;
        turnHandler.OnGameEnded   -= GameEnded;
      }

      async void AddOneCardToEachPiles(){
        isFourCardPilesRemoved = false;

        CreatePiles();

        foreach (CardPile pile in cardPiles){
          var topDeckCard = deckManager.DrawCard();
          if (topDeckCard == null) return;
          pile.AddCard(topDeckCard, true).Forget();
          await UniTask.WaitForSeconds(0.1f, cancellationToken: tokenSource.Token).SuppressCancellationThrow();
        }

        OnCardPilesCreated.Invoke();

      }

      void GameEnded(object sender, TurnHandler.ResultEventArgs e){
        tokenSource?.Cancel();
        tokenSource = new CancellationTokenSource();

      }
    }

    void CreatePiles(){
      oneCardPile = new CardPile(this, boardOneCardRoot);
      cardPiles   = new();

      for (int i = 0; i < 4; i++){
        CardPile newCardPile = new CardPile(this, boardCardRoots[i]);
        newCardPile.ToggleGreenSphere(false);
        cardPiles.Add(newCardPile);
      }
    }
  #endregion

    public void AddCardToOneCardPile(Card card){
      oneCardPile.AddCard(card, false).Forget();
    }

    public void AddCardToPile(CardPile cardPile, Card card, bool isDealerDraw = false){
      cardPile.AddCard(card, false).Forget();
      audioManager.PlaySound(SoundType.ButtonClickSfx);
      ClearChosenBoardPile();
    }

    public void CheckPiles(CardPile cardPile){
      if (isFourCardPilesRemoved) return;

      cardPiles.Remove(cardPile);

      if (cardPiles.IsNullOrEmpty()){ // Last Pile
        // CardPile remainingCardPile = cardPiles.First(o => o != null);
        // var      remainingCards    = remainingCardPile.GetAllCards();
        // Debug.Log($"remainingCards.Count: <color=green>{remainingCards.Count}</color>");
        //
        // foreach (Card card in remainingCards){
        //   AddCardToPile(oneCardPile, card, true);
        // }
        //
        // remainingCardPile.ClearAll();
        isFourCardPilesRemoved = true;
      }
    }

    public void AnimateJumpTopBoardCards(){
      DOTween.Complete(Keys.Tween.Card);

      foreach (var cardPile in cardPiles){
        var targetCard = cardPile.PeekTopCard();
        targetCard.transform.DOMoveY(targetCard.transform.position.y + 0.05f, 0.1f).SetLoops(4, LoopType.Yoyo).SetId(Keys.Tween.Card);
      }
    }

  # region Get
    public CardPile GetOneCardPile() => oneCardPile;

    public List<CardPile> GetAvailableCardPiles(){
      var fourPileList = cardPiles.Where(o => o != null).ToList();
      var onePileList  = new List<CardPile>{ oneCardPile };

      return isFourCardPilesRemoved ? onePileList : fourPileList;
    }

    public bool IsFourCardPilesRemoved(){
      return isFourCardPilesRemoved;
    }

    public bool IsBoardCard(Card card){
      return card.AttachedCardPile != null;
    }

    public List<Card> GetBoardTopCards(){
      List<Card> topCards;

      if (IsFourCardPilesRemoved()){
        topCards = new List<Card>{ oneCardPile.PeekTopCard() };
      }
      else{
        topCards = cardPiles.Where(o => o.PeekTopCard() != null).Select(o => o.PeekTopCard()).ToList();
      }

      return topCards;
    }
  #endregion

  #region Set
    public void SetChosenBoardPile(CardPile cardPile){
      ClearChosenBoardPile();
      ChosenBoardPile = cardPile;
      cardPile.ToggleGreenSphere(true);
    }

    public void ClearChosenBoardPile(){
      ChosenBoardPile = null;
      cardPiles.ForEach(o => o.ToggleGreenSphere(false));
    }

    public void UpdateScore(int delta, bool isSnap, int cardCount, int aceCount, int clubsCount){
      var activeEntity = turnHandler.GetActiveEntity();
      activeEntity.UpdateScore(delta);
      activeEntity.UpdateCardCount(cardCount);
      activeEntity.UpdateAceCount(aceCount);
      activeEntity.UpdateClubsCount(clubsCount);
      if (isSnap) activeEntity.UpdateSnapCount(1);

    }

    public void ResetBoard(){
      cardPiles.ForEach(o => o.Reset());
      oneCardPile.Reset();
      isFourCardPilesRemoved = false;
    }
  #endregion

  }

}