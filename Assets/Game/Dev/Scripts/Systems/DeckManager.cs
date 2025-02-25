using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.World;
using RunTogether.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace CardGame.Systems{

  public class DeckManager : MonoBehaviour{

    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform deckRoot;

    Stack<Card> deck;
    List<Card>  temporaryDeck;

    readonly List<Player> player;

    Vector3 deckEuler;

    const float CARD_HEIGHT     = 0.001f;
    const int   DECK_SIZE       = 52;
    const int   MAX_CARD_NUMBER = 13;

    void Awake(){
      deck      = new();
      deckEuler = new(-90f, 0f, -90f);
    }

    [Button] void CreateDeck(){
      var maxTypeCount = Enum.GetNames(typeof(CardType)).Length;

      for (int i = 1; i <= MAX_CARD_NUMBER; i++){
        for (int j = 0; j < maxTypeCount; j++){
          CreateCard(i, (CardType)j);
        }
      }

      ShuffleDeck();

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Local Functions 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void CreateCard(int cardNumber, CardType cardType){
        Card card = new Card.Builder().WithNumber(cardNumber).WithCardType(cardType).Build(cardPrefab);
        card.transform.SetParent(deckRoot);
        card.transform.position      = deckRoot.position + Vector3.zero.With(y: deck.Count * CARD_HEIGHT);
        card.transform.localRotation = Quaternion.Euler(deckEuler);

        deck.Push(card);
      }

      void ShuffleDeck(){
        Random rng          = new Random();
        var    shuffledList = new List<Card>(deck.OrderBy(o => rng.Next()));

        for (int i = 0; i < shuffledList.Count; i++){
          shuffledList[i].transform.position = deckRoot.position + new Vector3(0, i * CARD_HEIGHT, 0);
        }

        deck = new Stack<Card>(shuffledList);
      }
    }

    // public DeckManager(List<Player> player){
    //   this.player = player;
    // }

    void DrawCard(){
      // Draw a card from the deck
    }

    public Stack<Card> GetDeck() => deck;
  }

}