using System;
using RunTogether.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.World{

  [SelectionBase]
  public class Card : MonoBehaviour{
    [InlineEditor, ReadOnly] public CardData CardData;

    public void CastData(CardData cardData){
      CardData = cardData;
    }

    public string CardName  {get; private set;}
    public string CardNumber{get; private set;}
    public int    CardPoint {get; private set;}
    public Sprite CardSprite{get; private set;}

    public class Builder{
      string cardname;
      string cardnumber; // 1,2,3 .. V,Q,K
      int    cardPoint;
      Sprite cardSprite;

      public Builder WithName(string cardname){
        this.cardname = cardname;
        return this;
      }

      public Builder WithNumber(string cardnumber){
        this.cardnumber = cardnumber;
        return this;
      }

      public Builder WithPoint(int cardPoint){
        this.cardPoint = cardPoint;
        return this;
      }

      public Builder WithSprite(CardType cardType){
        
        this.cardSprite = ItemDatabase.CardTypeSpriteDic[cardType];
        return this;
      }

      public Card Build(GameObject prefab){
        if (prefab == null) return default;

        GameObject newObject = Instantiate(prefab);

        Card card = newObject.GetOrAdd<Card>();
        card.CardName   = cardname;
        card.CardNumber = cardnumber;
        card.CardPoint  = cardPoint;
        card.CardSprite = cardSprite;
        return card;
      }
    }

    
    // Sprite GetCardSprite(CardType cardType){
    //   return cardType switch{
    //     CardType.Diamonds => diamondsSprite, 
    //     CardType.Hearts   => heartsSprite, 
    //     CardType.Spades   => spadesSprite, 
    //     CardType.Clubs    => clubsSprite, 
    //     _                 => null
    //   };
  }

}