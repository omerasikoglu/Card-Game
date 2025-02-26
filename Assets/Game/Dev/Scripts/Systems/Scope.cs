using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardGame.Systems{

  public class Scope : LifetimeScope{

    [Title("Deck Manager")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform  deckRoot;

    [Title("Board Manager")]
    [SerializeField] Transform[] fourCardPileRoots;
    [SerializeField] Transform   oneCardPileRoot;

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterComponentInHierarchy<GameManager>();
      
      builder.Register<Player>(Lifetime.Singleton);
      builder.Register<Opponent>(Lifetime.Singleton);
      
      
      builder.Register<DeckManager>(Lifetime.Singleton).WithParameter(cardPrefab).WithParameter(deckRoot);
      builder.Register<BoardManager>(Lifetime.Singleton).WithParameter(fourCardPileRoots).WithParameter(oneCardPileRoot);
      builder.Register<TurnManager>(Lifetime.Singleton);
      builder.Register<SaveLoadSystem>(Lifetime.Singleton);
    }

  }

}