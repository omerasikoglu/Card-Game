using System;
using CardGame.UI;
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

    SaveLoadSystem   saveLoadSystem;
    DeckManager      deckManager;
    BoardManager     boardManager;
    Player           player;
    Opponent         opponent;
    TurnHandler      turnHandler;
    CanvasController canvasController;
  #endregion

    [Inject] public void Init(IObjectResolver resolver){
      saveLoadSystem   = resolver.Resolve<SaveLoadSystem>();
      turnHandler      = resolver.Resolve<TurnHandler>();
      deckManager      = resolver.Resolve<DeckManager>();
      boardManager     = resolver.Resolve<BoardManager>();
      player           = resolver.Resolve<Player>();
      opponent         = resolver.Resolve<Opponent>();
      canvasController = resolver.Resolve<CanvasController>();

      player.Init(playerHandCardHoldRoots);
      opponent.Init(opponentHandCardHoldRoots);

      turnHandler.SetPlayers(new Entity[]{ player, opponent });
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

    [Button] public void FIRST_TIME_START()  => turnHandler.FirstTimeStart();
    [Button] public void NEXT_PLAYER_TURN()  => turnHandler.NextPlayerTurn();
    [Button] public void OPEN_PLAYER_INPUT() => player.OpenInput();
    [Button] public void CLOSE_PLAYER_INPUT() => player.CloseInput();
    

  #region Core
    void OnEnable()  => OnToggle(true);
    void OnDisable() => OnToggle(false);

    void OnToggle(bool to){
      // deckManager.OnToggle(to);
      player.OnToggle(to);
      opponent.OnToggle(to);

      if (to){
        canvasController.OnGameStart += turnHandler.StartGame;
      }
      else{
        canvasController.OnGameStart -= turnHandler.StartGame;
      }
    }

    void Start(){
      deckManager.Start();
    }

    void Update(){
      player?.Update();
    }
  #endregion

  }

}