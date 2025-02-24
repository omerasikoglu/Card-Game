using VContainer;
using VContainer.Unity;

namespace CardGame.Systems{

  public class Scope : LifetimeScope{
    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);
      
      builder.RegisterComponentInHierarchy<GameManager>();
    }

  }

}