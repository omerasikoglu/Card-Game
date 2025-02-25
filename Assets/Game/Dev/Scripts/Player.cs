using System;
using CardGame.Components;
using CardGame.Systems;
using UnityEngine;
using VContainer;

namespace CardGame{

  public class Player : MonoBehaviour{

    [Inject] public BoardManager BoardManager{get; private set;} 
    [Inject] public DeckManager DeckManager{get; private set;} 
    
    [SerializeField] Transform[] cardHoldTransforms;
    
    public PlayerHandManager PlayerHandManager{get; private set;}
    public PlayerInput       PlayerInput      {get; private set;}

  #region Core
    void Awake(){
      PlayerHandManager = new (this, cardHoldTransforms);
      PlayerInput       = new (this);
    }

    void OnEnable(){
      PlayerInput.OnEnable();
    }

    void OnDisable(){
      PlayerInput.OnDisable();
    }


    void Update(){
      PlayerInput.Update();
    }
  #endregion

    public void AddCardToHand(){
      PlayerHandManager.AddCardToYourHand();
    }
  }

}