using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class GameManager : MonoBehaviour{

  #region members
    [Title("Entities")]
    [SerializeField] Transform[] playerHandCardHoldRoots;   // main player
    [SerializeField] Transform[] opponentHandCardHoldRoots; // 2nd player

    SaveLoadSystem saveLoadSystem;
    DeckManager    deckManager;
    BoardManager   boardManager;
    Player         player;
    Opponent       opponent;
    TurnManager    turnManager;
  #endregion

    [Inject] public void Init(IObjectResolver resolver){
      saveLoadSystem = resolver.Resolve<SaveLoadSystem>();
      turnManager    = resolver.Resolve<TurnManager>();
      deckManager    = resolver.Resolve<DeckManager>();
      boardManager   = resolver.Resolve<BoardManager>();
      player         = resolver.Resolve<Player>();
      opponent       = resolver.Resolve<Opponent>();

      player.Init(playerHandCardHoldRoots);
      opponent.Init(opponentHandCardHoldRoots);

      turnManager.SetPlayers(new Entity[]{ player, opponent });
    }

    [Button] public void CREATE_DECK()    => deckManager.CreateDeck();
    [Button] public void AddCardToPiles() => boardManager.AddOneCardToEachPiles();

    [Button] public async UniTaskVoid AddCardToPlayers(){
      const int   handSize = 4;
      const float duration = 0.1f;

      for (int i = 0; i < handSize; i++){
        player.AddCardToHand();
        await UniTask.WaitForSeconds(duration);
        opponent.AddCardToHand();
        await UniTask.WaitForSeconds(duration);
      }
    }

    [Button] public void FIRST_TIME_START() => turnManager.FirstTimeStart();
    [Button] public void NEXT_PLAYER_TURN() => turnManager.NextPlayerTurn();

  #region Core
    void OnEnable()  => OnToggle(true);
    void OnDisable() => OnToggle(false);

    void OnToggle(bool to){
      player.OnToggle(to);
      opponent.OnToggle(to);
    }

    void Update(){
      player.Update();
    }
  #endregion

  }

}