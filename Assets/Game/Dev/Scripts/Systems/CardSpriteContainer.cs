using UnityEngine;

namespace CardGame.World{

  public enum CardType{
    Diamonds, // Karo
    Hearts,   // Kupa
    Spades,   // Maça
    Clubs     // sinek
  }
  
  [CreateAssetMenu(menuName = "Scriptables/Card Sprite Container", fileName = nameof(CardSpriteContainer), order = -1)]
  public class CardSpriteContainer : ScriptableObject{
    [SerializeField] Sprite diamondsSprite, heartsSprite, spadesSprite, clubsSprite;

    public Sprite this[CardType type]{
      get{return type switch{ 
        (CardType)0 => diamondsSprite, 
        (CardType)1 => heartsSprite, 
        (CardType)2 => spadesSprite, 
        (CardType)3 => clubsSprite, 
        _           => null };}
    }
  }

}