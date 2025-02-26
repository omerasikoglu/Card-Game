using System.Collections.Generic;
using System.Linq;
using CardGame.Utils;
using CardGame.World;
using DG.Tweening;
using RunTogether.Extensions;
using UnityEngine;

namespace CardGame.Components{

  public class HandManager{

  #region Members
    readonly List<Card>  holdingCards;
    readonly Entity      entity;
    readonly Transform[] fourCardTransforms, threeCardTransforms, twoCardTransforms, oneCardTransform;

    const int HOLDING_MAX_CARD_COUNT = 4;
  #endregion

    public HandManager(Entity entity, IReadOnlyList<Transform> cardHoldTransforms){
      this.entity = entity;

      holdingCards = new();

      if (cardHoldTransforms.IsNullOrEmpty()) return;
      fourCardTransforms  = new Transform[4]{ cardHoldTransforms[0], cardHoldTransforms[2], cardHoldTransforms[4], cardHoldTransforms[6] };
      threeCardTransforms = new Transform[3]{ cardHoldTransforms[1], cardHoldTransforms[3], cardHoldTransforms[5] };
      twoCardTransforms   = new Transform[2]{ cardHoldTransforms[2], cardHoldTransforms[4] };
      oneCardTransform    = new Transform[1]{ cardHoldTransforms[3] };
    }

  #region Set
    public void AddCardToYourHand(Entity entity){
      if (holdingCards.Count >= HOLDING_MAX_CARD_COUNT) return;
      var topDeckCard = entity.DeckManager.DrawCard();
      if (topDeckCard == null) return;

      holdingCards.Add(topDeckCard);

      topDeckCard.transform.SetParent(GetTargetRoot());

      var handCardEuler = GetCardEuler(this.entity);
      var duration      = 0.2f;
      var endPosition   = GetTargetRoot().position;

      DOTween.Complete(Keys.Tween.Card);
      topDeckCard.transform.DOMove(endPosition, duration).SetId(Keys.Tween.Card);
      topDeckCard.transform.DORotate(handCardEuler, duration).SetId(Keys.Tween.Card);

      holdingCards.Where(o => o != holdingCards.Last()).ForEach(o => o.transform.position =
        GetTargetTransforms()[holdingCards.IndexOf(o)].position);
    }

    public void RemoveCardFromYourHand(Card card){
      holdingCards.Remove(card);

      holdingCards.ForEach(o => o.transform.position =
        GetTargetTransforms()[holdingCards.IndexOf(o)].position);
    }
  #endregion
    
    Vector3 GetCardEuler(Entity entity){
      return entity switch{
        Player   => new Vector3(60f, 0f, 0f),
        Opponent => new Vector3(210f, 0f, 0f),
        _        => Vector3.zero
      };
    }

    Transform GetTargetRoot(){
      return holdingCards.Count switch{
        4 => fourCardTransforms[3],
        3 => threeCardTransforms[2],
        2 => twoCardTransforms[1],
        1 => oneCardTransform[0],
        _ => null
      };
    }

    Transform[] GetTargetTransforms(){
      return holdingCards.Count switch{
        4 => fourCardTransforms,
        3 => threeCardTransforms,
        2 => twoCardTransforms,
        1 => oneCardTransform,
        _ => null
      };

    }

    public List<Card> GetHoldingCards() => holdingCards;

  }

}