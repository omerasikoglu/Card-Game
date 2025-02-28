using System;
using System.Collections.Generic;
using CardGame.UI;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class GameManager : MonoBehaviour{

  #region members
    [Title("Entities")]
    [SerializeField] GameObject platePrefab;
    [SerializeField] Transform[] playerHandCardHoldRoots;
    [SerializeField] Transform[] opponentHandCardHoldRoots;
    [SerializeField] Transform[] opponent2HandCardHoldRoots;
    [SerializeField] Transform[] opponent3HandCardHoldRoots;
    [SerializeField] Transform[] plateRoots;

    SaveLoadSystem   saveLoadSystem;
    DeckManager      deckManager;
    BoardManager     boardManager;
    Player           player;
    List<Opponent>   opponents;
    TurnHandler      turnHandler;
    CanvasController canvasController;
  #endregion

    [Inject] public void Init(IObjectResolver resolver, Player player){
      saveLoadSystem   = resolver.Resolve<SaveLoadSystem>();
      turnHandler      = resolver.Resolve<TurnHandler>();
      deckManager      = resolver.Resolve<DeckManager>();
      boardManager     = resolver.Resolve<BoardManager>();
      canvasController = resolver.Resolve<CanvasController>();
      this.player      = player;

      var opponent1 = resolver.Resolve<Opponent>();
      var opponent2 = resolver.Resolve<Opponent2>();
      var opponent3 = resolver.Resolve<Opponent3>();
      
      opponents = new(){ opponent1, opponent2, opponent3 };


      player.Init(platePrefab, playerHandCardHoldRoots, plateRoots[0]);
      opponents[0].Init(platePrefab, opponentHandCardHoldRoots, plateRoots[1]);
      opponents[1].Init(platePrefab, opponent2HandCardHoldRoots, plateRoots[2]);
      opponents[2].Init(platePrefab, opponent3HandCardHoldRoots, plateRoots[3]);

      turnHandler.Init(canvasController, deckManager, boardManager, saveLoadSystem);

      turnHandler.SetEntities(new Entity[]{ player, opponents[0], opponents[1], opponents[2] });
    }

  #region Core
    void OnEnable()  => OnToggle(true);
    void OnDisable() => OnToggle(false);

    void OnToggle(bool to){
      deckManager.OnToggle(to);
      boardManager.OnToggle(to);
      player.OnToggle(to);
      opponents.ForEach(o => o.OnToggle(to));
      turnHandler.OnToggle(to);
      saveLoadSystem.OnToggle(to);
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