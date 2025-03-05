using CardGame.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardGame.Systems{

  public class Scope : LifetimeScope{

    [Title("Deck Manager")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform deckRoot;

    [Title("Board Manager")]
    [SerializeField] Transform[] fourCardPileRoots;
    [SerializeField] Transform oneCardPileRoot;

    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);

      builder.RegisterInstance(cardPrefab);
      builder.RegisterInstance(deckRoot);
      builder.RegisterInstance(fourCardPileRoots);
      builder.RegisterInstance(oneCardPileRoot);
      
      builder.RegisterComponentInHierarchy<GameManager>();
      builder.RegisterComponentInHierarchy<AudioManager>();
      builder.RegisterComponentInHierarchy<CanvasController>();

      // builder.Register<Entity, Player>(Lifetime.Singleton);
      // builder.Register<Entity, Opponent>(Lifetime.Singleton);
      // builder.Register< Entity,Opponent, Opponent2>(Lifetime.Singleton);;
      // builder.Register< Entity, Opponent, Opponent3>(Lifetime.Singleton);;
      
      
      // builder.Register<Player>(Lifetime.Singleton).As<Entity>().AsSelf();
      // builder.Register<Opponent>(Lifetime.Singleton).As<Entity>().AsSelf();;
      // builder.Register<Opponent2>(Lifetime.Singleton).As<Opponent>().As<Entity>().AsSelf();;
      // builder.Register<Opponent3>(Lifetime.Singleton).As<Opponent>().As<Entity>().AsSelf();;

      builder.Register<DeckManager>(Lifetime.Singleton).WithParameter(cardPrefab).WithParameter(deckRoot);
      builder.Register<BoardManager>(Lifetime.Singleton).WithParameter(fourCardPileRoots).WithParameter(oneCardPileRoot);
      builder.Register<TurnHandler>(Lifetime.Singleton);
      builder.Register<SaveLoadSystem>(Lifetime.Singleton);
    }

  }

}