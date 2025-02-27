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
    [SerializeField] GameObject  platePrefab;               
    [SerializeField] Transform[] playerHandCardHoldRoots;   // main player
    [SerializeField] Transform[] opponentHandCardHoldRoots; // 2nd player
    [SerializeField] Transform[] plateRoots;                // player, oppenents

    SaveLoadSystem   saveLoadSystem;
    DeckManager      deckManager;
    BoardManager     boardManager;
    Player           player;
    Opponent         opponent;
    TurnHandler      turnHandler;
    CanvasController canvasController;
  #endregion

    [Inject] public void Init(IObjectResolver resolver, Player player, Opponent opponent){
      saveLoadSystem = resolver.Resolve<SaveLoadSystem>();
      turnHandler    = resolver.Resolve<TurnHandler>();
      deckManager    = resolver.Resolve<DeckManager>();
      boardManager   = resolver.Resolve<BoardManager>();
      canvasController = resolver.Resolve<CanvasController>();
      this.player    = player;
      this.opponent  = opponent;

      player.Init(platePrefab,playerHandCardHoldRoots, plateRoots[0]);
      opponent.Init(platePrefab,opponentHandCardHoldRoots, plateRoots[1]);
      turnHandler.Init(canvasController, deckManager, boardManager);

      turnHandler.SetPlayers(new Entity[]{ player, opponent });
    }

    [Button] public void CREATE_DECK()    => deckManager.CreateDeck();
    // [Button] public void AddCardToPiles() => boardManager.AddOneCardToEachPiles();

    [Button] public async UniTaskVoid AddCardToPlayers(){
      const int   handSize = 4;
      const float duration = 0.1f;

      for (int i = 0; i < handSize; i++){
        player.HandManager.AddCardToHand();
        await UniTask.WaitForSeconds(duration);
        opponent.HandManager.AddCardToHand();
        await UniTask.WaitForSeconds(duration);
      }
    }

    // [Button] public void FIRST_TIME_START()   => turnHandler.FirstTimeStart();
    // [Button] public void NEXT_PLAYER_TURN()   => turnHandler.NextPlayerTurn();
    [Button] public void OPEN_PLAYER_INPUT()  => player.OpenInput();
    [Button] public void CLOSE_PLAYER_INPUT() => player.CloseInput();

  #region Core
    void OnEnable()  => OnToggle(true);
    void OnDisable() => OnToggle(false);

    void OnToggle(bool to){
      deckManager.OnToggle(to);
      boardManager.OnToggle(to);
      player.OnToggle(to);
      opponent.OnToggle(to);
      turnHandler.OnToggle(to);
    }

    // void Start(){
    //   deckManager.Start();
    // }

    void Update(){
      player?.Update();
    }
  #endregion

  }

}