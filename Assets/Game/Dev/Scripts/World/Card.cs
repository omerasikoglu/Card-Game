using CardGame.Systems;
using RunTogether.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.World{

  [SelectionBase]
  public class Card : MonoBehaviour, IClickInInteract{

  #region Members
    Sprite CardSprite;
    string CardText; // 1,2.. Q, K

    [ShowInInspector] public CardPile AttachedCardPile{get; private set;}

    public CardType CardType  {get; private set;}
    public int      CardNumber{get; private set;} // 1,2.. 12, 13
    public int      CardPoint {get; private set;}
    public bool     IsInDeck  {get; private set;} = true;

  #endregion

    void Init(){
      transform.GetFirstChild<Image>(includeGrandChild: true).sprite = CardSprite;
      transform.GetFirstChild<TMP_Text>(includeGrandChild: true).SetText(CardText);
      IsInDeck = true;
    }

    public class Builder{
      int      cardnumber;
      CardType cardType;

      string cardText;
      int    cardPoint;
      Sprite cardSprite;

      public Builder WithNumber(int cardnumber){
        this.cardnumber = cardnumber;

        cardText = cardnumber switch{
          1    => "A",
          < 11 => cardnumber.ToString(),
          11   => "V",
          12   => "Q",
          13   => "K",
          _    => string.Empty
        };

        cardPoint = cardnumber switch{
          < 11 => cardnumber,
          11   => 10,
          12   => 10,
          13   => 10,
          _    => 0
        };

        return this;
      }

      public Builder WithCardType(CardType cardType){

        this.cardType = cardType;
        cardSprite    = ItemDatabase.CardTypeSpriteDic[cardType];
        return this;
      }

      public Card Build(GameObject prefab){
        if (prefab == null) return default;

        GameObject newObject = Instantiate(prefab);

        Card card = newObject.GetOrAdd<Card>();

        card.CardNumber = cardnumber;
        card.CardType   = cardType;
        card.CardPoint  = cardPoint;
        card.CardText   = cardText;
        card.CardSprite = cardSprite;
        card.Init();

        return card;
      }
    }

  #region Implements
    public bool IsInteractEnable(){
      return !IsInDeck;
    }

    public void OnInteractJustPerformed(){
      // TODO: if there is more than 1 card on the table choose it

    }
  #endregion

    public void SetAttachedCardPile(CardPile attachedCardPile){
      AttachedCardPile = attachedCardPile;
    }

    public void SetIsInDeck(bool to){
      IsInDeck = to;
    }

  }

}