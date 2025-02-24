namespace CardGame.World{

  public class InteractSeggregations{ }

  public interface ITargetable{
    bool IsInteractEnable();
  }

#region Ray
  public interface IRayInInteract : ITargetable{
    void OnRayEnter();
  }
  
  public interface IRayOutInteract : ITargetable{
    void OnRayExit();
  }
  
  public interface IRayInteract : IRayInInteract, IRayOutInteract { }
#endregion

#region Click
  public interface IClickInInteract : ITargetable{
    void InteractJustPerformed();
  }

  public interface IClickOutInteract : ITargetable{
    void InteractJustReleased();
  }

  public interface IClickInteract : IClickInInteract, IClickOutInteract{ }
#endregion

  public interface IHoldInteract : ITargetable{
    void InteractPressed();
  }

  public interface IClickHoldInteract : IClickInteract, IHoldInteract{ }

  public interface IInteractable : IRayInteract, IClickHoldInteract{ }
  
  
}