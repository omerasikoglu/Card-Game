using System.Collections.Generic;
using CardGame.World;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RunTogether.Extensions;
using UnityEngine;

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

    public async UniTask AddCard(Card card){
      card.SetAttachedCardPile(this);
      card.transform.SetParent(root);
      cards.Push(card);
      var endPosition = root.position + Vector3.zero.With(z: cards.Count * -cardPileCardHeight);

      card.transform.DOMove(endPosition, deckToBoardDuration);
      card.transform.DORotate(cardPileCardEuler, deckToBoardDuration);

      await UniTask.WaitForSeconds(deckToBoardDuration);

      // CheckCardCount();
    }

    public List<Card> GetAllCards(){
      return new(cards);
    }

    void CheckCardCount(){
      if (cards.Count <= 0){
        cards.Clear();
        boardManager.RemovePile(this);
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