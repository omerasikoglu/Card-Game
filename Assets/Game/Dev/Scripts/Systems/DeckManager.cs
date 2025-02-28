using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CardGame.Utils;
using CardGame.World;
using Cysharp.Threading.Tasks;
using RunTogether.Extensions;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace CardGame.Systems{

  public class DeckManager{

    public event Action OnDeckCreated = delegate{ };

    [Inject] readonly TurnHandler turnHandler;

  #region Members
    Stack<Card> deck;
    List<Card>  temporaryDeck;

    CancellationTokenSource deckCreateTokenSource;

    readonly GameObject   cardPrefab;
    readonly Transform    deckRoot;
    readonly List<Player> player;

    const float CARD_HEIGHT     = 0.001f;
    const int   DECK_SIZE       = 52;
    const int   MAX_CARD_NUMBER = 13;

    readonly int   maxTypeCount            = Enum.GetNames(typeof(CardType)).Length;
    const    float oneCardCreationDuration = 0.01f;
  #endregion

    public DeckManager(GameObject cardPrefab, Transform deckRoot){
      this.cardPrefab = cardPrefab;
      this.deckRoot   = deckRoot;

      deck      = new();
    }

    public void OnToggle(bool to){
      if (to){
        turnHandler.OnGameStart += CreateDeck;
      }
      else{
        turnHandler.OnGameStart -= CreateDeck;
      }
    }

    public async void CreateDeck(int totalBet){
      ClearDeck();

      for (int i = 1; i <= MAX_CARD_NUMBER; i++){
        for (int j = 0; j < maxTypeCount; j++){
          CreateCard(i, (CardType)j);
          await UniTask.WaitForSeconds(oneCardCreationDuration,
            cancellationToken: deckCreateTokenSource.Token).SuppressCancellationThrow() ;
        }
      }

      ShuffleDeck();
      OnDeckCreated.Invoke();

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Local Functions 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void ClearDeck(){
        deck.ForEach(o => Object.Destroy(o.gameObject));
        deck.Clear();
        deckCreateTokenSource?.Cancel();
        deckCreateTokenSource = new CancellationTokenSource();
      }

      void CreateCard(int cardNumber, CardType cardType){
        Card card = new Card.Builder().WithNumber(cardNumber).WithCardType(cardType).Build(cardPrefab);
        card.transform.SetParent(deckRoot);
        card.transform.position      = deckRoot.position + Vector3.zero.With(y: deck.Count * CARD_HEIGHT);
        card.transform.localRotation = Quaternion.Euler( Keys.Euler.Deck);

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

    public Card DrawCard(){
      if (deck.Count <= 0) return null;
      Card card = deck.Pop();
      card.transform.SetParent(null);

      return card;
    }

    public void OpenCardToTable(){ }

    public int GetDeckCount() => deck.Count;

    public Stack<Card> GetDeck() => deck;

    public bool IsDeckEmpty(){
      return deck.Count == 0;
    }
    
    public bool IsDeckFull(){
      return deck.Count == DECK_SIZE;
    }

    public void ResetDeck(){
      deckCreateTokenSource?.Cancel();
      deck.ForEach(o => Object.Destroy(o.gameObject));
      deck.Clear();
    }
  }

}