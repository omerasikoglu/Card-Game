using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Utils;
using CardGame.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RunTogether.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardGame.Systems{

  public class CardPile{

    
    readonly BoardManager boardManager;
    readonly Stack<Card>  cards;
    readonly Transform    root;
    readonly Vector3      cardPileCardEuler;
    readonly Transform    greenSphere;

    const float deckToBoardDuration = 0.2f;
    const float cardPileCardHeight  = 0.001f;

    public CardPile(BoardManager boardManager, Transform root){
      this.boardManager = boardManager;
      this.root         = root;
      cards             = new();

      greenSphere = root.GetFirstChild<SphereCollider>().transform;

      cardPileCardEuler = new(45f, 0f, 0f);
    }

    public async UniTask AddCard(Card card, bool isDealerDraw){
      card.SetAttachedCardPile(this);
      card.transform.SetParent(root);
      cards.Push(card);
      var endPosition = root.position + Vector3.zero.With(z: cards.Count * -cardPileCardHeight);

      DOTween.Complete(Keys.Tween.Card);
      card.transform.DOMove(endPosition, deckToBoardDuration).SetId(Keys.Tween.Card);
      card.transform.DORotate(cardPileCardEuler, deckToBoardDuration).SetId(Keys.Tween.Card);

      await UniTask.WaitUntil(() => card.transform.position == endPosition);

      CompareTopTwoCards();
      boardManager.OnCardPlayed.Invoke(isDealerDraw);
  }

    void CompareTopTwoCards(){
      if (cards.Count < 2) return;

      Card topCard  = cards.Peek();
      Card nextCard = cards.ElementAt(1);

      if (topCard.CardNumber == nextCard.CardNumber){
        GainPile();
      }

      async void GainPile(){
        var  point        = 0;
        bool isSnap       = cards.Count == 2;
        if (isSnap) point += Keys.Point.SNAP;

        point += cards.Sum(o => o.CardPoint);

        boardManager.GainScorePoint(point);

        foreach (Card card in cards){
          await PopCard(card);
          Object.Destroy(card.gameObject);
        }

        cards.Clear();
        boardManager.CheckPiles(this);
      }

      async UniTask PopCard(Card card){
        var duration = 0.1f;
        card.transform.DOMoveY(card.transform.position.y + 0.1f, duration).SetEase(Ease.Linear);
        await UniTask.WaitForSeconds(duration);
      }
    }

    public List<Card> GetAllCards(){
      return new(cards);
    }

    void CheckCardCount(){
      if (cards.Count <= 0){
        cards.Clear();
        boardManager.CheckPiles(this);
      }
    }

    public List<Card> GetCards(){
      return new(cards);
    }

    public void ClearAll(){
      cards.Clear();
    }

    public Card PeekTopCard(){
      return cards.Peek();
    }

    public void ToggleGreenSphere(bool to){
      greenSphere.Toggle(to);
    }

  }

}