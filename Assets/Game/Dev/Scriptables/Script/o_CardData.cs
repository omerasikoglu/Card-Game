using RunTogether.Extensions;
using UnityEngine;

namespace CardGame.World{

  public class o_CardData : ScriptableObject{

    [SerializeField] public GameObject prefab;
    [SerializeField] public CardType   cardType;
    [SerializeField] public Sprite     cardSprite;
    [SerializeField] public int        cardNumber; // 1, 2.. 11, 12, 13
    [SerializeField] public string     cardText;   // 1, 2.. Q, K
    [SerializeField] public int        point;

    public void OnHover(){
      // shine fx
    }

    public void OnUse(){
      // item move to board
    }

    public Card Create(int cardNumber, CardType cardType){
      var newObject = Instantiate(prefab);
      newObject.name               = prefab.name;
      newObject.transform.position = Vector3.zero;
      newObject.Toggle(false);

      this.cardType = cardType;

      Card card = new Card.Builder().
        WithNumber(cardNumber).
        WithCardType(cardType).
        Build(prefab);

      return card;

    }

  

  }

}