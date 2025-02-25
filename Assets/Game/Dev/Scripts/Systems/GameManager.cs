using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class GameManager : MonoBehaviour{
    [Inject] readonly IObjectResolver resolver;
    [Inject] readonly DeckManager     deckManager;
    [Inject] readonly BoardManager boardManager;
    [Inject] readonly Player player;

    [Button] public void CREATE_DECK()    => deckManager.CreateDeck();
    [Button] public void AddCardToPiles() => boardManager.AddCardToPiles();
    [Button] public void AddCardToPlayers() => player.AddCardToHand();

  }

}