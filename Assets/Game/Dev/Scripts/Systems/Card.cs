using System;
using CardGame.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.World{

  public class Card : MonoBehaviour{
    [InlineEditor, ReadOnly] public CardData CardData;

    public void Init(CardData cardData){
      CardData = cardData;
    }

  }

}