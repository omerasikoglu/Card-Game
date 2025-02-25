using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.World{

  public static class ItemDatabase{
    public static Dictionary<CardType, Sprite> CardTypeSpriteDic{get; private set;}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void Init(){
      InitCardTypeSpriteDic();

      void InitCardTypeSpriteDic(){
        int capacity = Enum.GetValues(typeof(CardType)).Length;
        CardTypeSpriteDic = new(capacity);
        var cardSpriteContainer = Resources.Load<CardSpriteContainer>(nameof(CardSpriteContainer));

        for (int i = 0; i < capacity; i++){
          CardTypeSpriteDic.Add((CardType)i, cardSpriteContainer[(CardType)i]);
        }
      }
    }
  }

}