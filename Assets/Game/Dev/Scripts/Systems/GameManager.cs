using System.Collections.Generic;
using CardGame.UI;
using CardGame.Utils;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class GameManager : MonoBehaviour{

  #region Members
    [Title("Entities")]
    [SerializeField] GameObject platePrefab;
    [SerializeField] Transform[] playerHandCardHoldRoots;
    [SerializeField] Transform[] opponentHandCardHoldRoots;
    [SerializeField] Transform[] opponent2HandCardHoldRoots;
    [SerializeField] Transform[] opponent3HandCardHoldRoots;
    [SerializeField] Transform[] plateRoots;

    [Title("Deck Manager")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform deckRoot;

    [Title("Board Manager")]
    [SerializeField] Transform[] fourCardPileRoots;
    [SerializeField] Transform oneCardPileRoot;

    [SerializeField] CanvasController canvasController;
    [SerializeField] AudioManager     audioManager;

    SaveLoadSystem saveLoadSystem;
    DeckManager    deckManager;
    BoardManager   boardManager;
    Player         player;
    List<Opponent> opponents;
    TurnHandler    turnHandler;
  #endregion

    public void Awake(){

      QualitySettings.vSyncCount  = 0;
      Application.targetFrameRate = Keys.Fps;

      if (Camera.main != null && Camera.main.pixelWidth > Keys.Resolution){
        Screen.SetResolution(Keys.Resolution, (int)((Camera.main.pixelHeight * Keys.Resolution) / Camera.main.pixelWidth), true);
      }

      turnHandler    = new();
      saveLoadSystem = new(turnHandler);
      deckManager    = new(cardPrefab, deckRoot, turnHandler);
      boardManager   = new(fourCardPileRoots, oneCardPileRoot, deckManager, turnHandler, audioManager);

      player = new();
      var opponent1 = new Opponent();
      var opponent2 = new Opponent2();
      var opponent3 = new Opponent3();

      opponents = new(){ opponent1, opponent2, opponent3 };

      canvasController.Init(audioManager, saveLoadSystem, turnHandler);
      player.Init(platePrefab, playerHandCardHoldRoots, plateRoots[0], canvasController, turnHandler, boardManager, deckManager);
      opponents[0].Init(platePrefab, opponentHandCardHoldRoots, plateRoots[1], canvasController, turnHandler, boardManager, deckManager);
      opponents[1].Init(platePrefab, opponent2HandCardHoldRoots, plateRoots[2], canvasController, turnHandler, boardManager, deckManager);
      opponents[2].Init(platePrefab, opponent3HandCardHoldRoots, plateRoots[3], canvasController, turnHandler, boardManager, deckManager);

      // Init TurnHandler
      turnHandler.Init(canvasController, deckManager, boardManager, saveLoadSystem);
      turnHandler.SetEntities(new Entity[]{ player, opponents[0], opponents[1], opponents[2] });
    }

  #region Core
    void OnEnable() => OnToggle(true);
    void OnDisable() => OnToggle(false);

    async void OnToggle(bool to){
      await UniTask.WaitUntil(() => deckManager != null && boardManager != null && player != null && opponents != null && turnHandler != null && saveLoadSystem != null);

      deckManager.OnToggle(to);
      boardManager.OnToggle(to);
      player.OnToggle(to);
      opponents.ForEach(o => o.OnToggle(to));
      turnHandler.OnToggle(to);
      saveLoadSystem.OnToggle(to);
    }

    void Update(){
      player?.Update();
    }
  #endregion

  }

}