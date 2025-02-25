using RunTogether.Extensions;
using UnityEngine;

namespace CardGame.World{

  [CreateAssetMenu(menuName = "Scriptables/Card Data", fileName = "Card Data", order = -1)]
  public class CardData : ScriptableObject{

    [SerializeField] GameObject prefab;
    [SerializeField] CardType   cardType;
    [SerializeField] int        number;
    [SerializeField] int        point;

    public void OnHover(){
      // shine fx
    }

    public void OnUse(){
      // item move to board
    }

    public Card Create(string cardNumber, int cardPoint, CardType cardType, string cardname = "NewCard"){
      var newObject = Instantiate(prefab);
      newObject.name               = prefab.name;
      newObject.transform.position = Vector3.zero;
      newObject.Toggle(false);

      this.cardType = cardType;



      Card card = new Card.Builder().
        WithName(cardname).
        WithNumber(cardNumber).
        WithPoint(cardPoint).
        WithSprite(cardType).
        Build(prefab);

      card.CastData(this);
      return card;

    }

  }

}