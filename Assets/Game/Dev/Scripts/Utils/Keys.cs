using UnityEngine;

namespace CardGame.Utils{

  public struct Keys{

    public struct IO{
      public const string CURRENCY = "currency";
      public const string WIN      = "total_win";
      public const string LOST     = "total_lose";

      public const int CURRENCY_DEFAULT = 50000;
    }

    public struct LayerMask{
      public const string PLAYER    = "Player";
      public const string WORLD     = "World";
      public const string DRAGGABLE = "Draggable";
      public const string HITTABLE  = "Hittable";
    }

    public struct Tween{
      public const string Card  = "CardMoveFromHandTween";
      public const string UI    = "UIElementScaleChangeTween";
      public const string Plate = "PlateScaleChangeTween";
    }

    public struct Sfx{
      public const string ButtonClickSfx = "ButtonClickSfx";
      public const string WinSfx         = "WinSfx";
      public const string LostSfx        = "LostSfx";
      public const string SnapSfx        = "SnapSfx";
    }

    public struct Tag{
      public const string PLAYER    = "Player";
      public const string GROUND    = "Ground";
      public const string DRAGGABLE = "Draggable";
    }

    public struct Point{
      public const int SNAP       = 10;
      public const int ACE        = 1; // for each ace +1
      public const int MORE_CARD  = 2;
      public const int MORE_CLUBS = 3; // sinek
    }

    public struct UI{
      public const string Main    = "Main Menu";
      public const string Lobby   = "Create Lobby";
      public const string Table   = "Create Table";
      public const string InGame  = "In Game";
      public const string Info    = "Player Info";
      public const string Warning = "Warning";
    }
    
    public struct Euler{
      public static readonly Vector3 Deck = new(-90f, 0f, -90f);
    }

    public struct Bet{
      public static readonly MinMax NEWBIES = new MinMax(NEWBIES_MIN, NEWBIES_MAX);
      public static readonly MinMax ROOKIES = new MinMax(ROOKIES_MIN, ROOKIES_MAX);
      public static readonly MinMax NOBLES  = new MinMax(NOBLES_MIN, NOBLES_MAX);

      const int NEWBIES_MIN = 250;
      const int NEWBIES_MAX = 5000;
      const int ROOKIES_MIN = 2500;
      const int ROOKIES_MAX = 100000;  // 100K
      const int NOBLES_MIN  = 50000;   // 50K
      const int NOBLES_MAX  = 1000000; // 1M
    }

  }

}