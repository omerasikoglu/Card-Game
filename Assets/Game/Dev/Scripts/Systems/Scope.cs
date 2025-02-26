using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardGame.Systems{

  public class Scope : LifetimeScope{

    // Deck Manager
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform  deckRoot;

    // BoardManager
    [SerializeField] Transform[] cardOpenRoots;
    [SerializeField] Transform   oneCardOpenRoot;

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterComponentInHierarchy<GameManager>();
      builder.RegisterComponentInHierarchy<Player>();
      builder.RegisterComponentInHierarchy<Opponent>();

      builder.Register<DeckManager>(Lifetime.Singleton).WithParameter(cardPrefab).WithParameter(deckRoot);
      builder.Register<BoardManager>(Lifetime.Singleton).WithParameter(cardOpenRoots).WithParameter(oneCardOpenRoot);
      builder.Register<TurnManager>(Lifetime.Singleton);
      builder.Register<SaveLoadSystem>(Lifetime.Singleton);
    }

  }

}