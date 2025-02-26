using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class GameManager : MonoBehaviour{
    IObjectResolver resolver;
    SaveLoadSystem  saveLoadSystem;
    DeckManager     deckManager;
    BoardManager    boardManager;
    Player          player;
    TurnManager     turnManager;

    [Inject] public void Init(IObjectResolver resolver, SaveLoadSystem saveLoadSystem, TurnManager turnManager, DeckManager deckManager, BoardManager boardManager, Player player){
      this.resolver       = resolver;
      this.saveLoadSystem = saveLoadSystem;
      this.turnManager    = turnManager;
      this.deckManager    = deckManager;
      this.boardManager   = boardManager;
      this.player         = player;

      turnManager.SetPlayers(new Entity[]{ player, resolver.Resolve<Opponent>() });
    }

    [Button] public void CREATE_DECK()      => deckManager.CreateDeck();
    [Button] public void AddCardToPiles()   => boardManager.AddOneCardToEachPiles();
    [Button] public void AddCardToPlayers() => player.AddCardToHand();
    [Button] public void FIRST_TURN_START() => turnManager.FirstTurnStart();
    [Button] public void END_TURN()         => turnManager.EndTurn();

    void Update(){
      turnManager.Update();
    }
  }

}