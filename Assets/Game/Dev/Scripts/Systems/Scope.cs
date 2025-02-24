using VContainer;
using VContainer.Unity;

namespace Template.Systems{

  public class Scope : LifetimeScope{
    protected override void Configure(IContainerBuilder builder){
      base.Configure(builder);
      
      builder.RegisterComponentInHierarchy<GameManager>();
    }

  }

}