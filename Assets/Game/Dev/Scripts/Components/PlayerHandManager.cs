using System.Collections.Generic;
using CardGame.World;
using RunTogether.Extensions;
using UnityEngine;

namespace CardGame.Components{

  public class PlayerHandManager{

  #region Members
    readonly List<Card>  holdingCards;
    readonly Player      player;
    readonly Transform[] fourCardTransforms, threeCardTransforms, twoCardTransforms, oneCardTransform;

    const int HOLDING_MAX_CARD_COUNT = 4;
  #endregion

    public PlayerHandManager(Player player, IReadOnlyList<Transform> cardHoldTransforms){
      this.player = player;

      holdingCards = new();

      if(cardHoldTransforms.IsNullOrEmpty()) return;
      fourCardTransforms  = new Transform[4]{ cardHoldTransforms[0], cardHoldTransforms[2], cardHoldTransforms[4], cardHoldTransforms[6] };
      threeCardTransforms = new Transform[3]{ cardHoldTransforms[1], cardHoldTransforms[3], cardHoldTransforms[5] };
      twoCardTransforms   = new Transform[2]{ cardHoldTransforms[2], cardHoldTransforms[4] };
      oneCardTransform    = new Transform[1]{ cardHoldTransforms[3] };
    }

  #region Set
    public void AddCardToYourHand(Card card){
      holdingCards.Add(card);
      UpdateCardPositions();
    }

    public void RemoveCardFromYourHand(Card card){
      holdingCards.Remove(card);
      UpdateCardPositions();
    }
  #endregion

    void UpdateCardPositions(){
      // TODO: DOTween
      holdingCards.ForEach(o => o.transform.position =
        GetTargetTransforms()[holdingCards.IndexOf(o)].position);
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
  }

}