using RunTogether.Extensions;
using UnityEngine;

namespace CardGame.World{

  public class CardData : ScriptableObject{

    enum CardType{
      Diamonds, // Karo
      Hearts, // Kupa
      Spades, // Maça
      Clubs // sinek
    }
    
    [SerializeField] GameObject prefab;
    [SerializeField] CardType sprite;
    [SerializeField] int number;
    [SerializeField] int point;

    public void OnHover(){
      // shine fx
    }
    public void OnUse(){
      // item move to board
    }
    
    public Card Create(){
      var newObject = Instantiate(prefab);
      newObject.name = prefab.name;
      newObject.transform.position = Vector3.zero;
      newObject.Toggle(false);
      
      Card card = newObject.GetOrAdd<Card>();
      card.Init(this);
      return card;

    }
  }

}