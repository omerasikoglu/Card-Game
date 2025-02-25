namespace CardGame.Utils{

  public struct Keys{
    public struct LayerMask{
      public const string PLAYER    = "Player";
      public const string WORLD     = "World";
      public const string DRAGGABLE = "Draggable";
      public const string HITTABLE  = "Hittable";
    } 
    
    public struct Tween{
      public const string Card  = "CardMoveFromHandTween";
    }

    public struct Tag{
      public const string PLAYER    = "Player";
      public const string GROUND    = "Ground";
      public const string DRAGGABLE = "Draggable";
    }
    
  }

}