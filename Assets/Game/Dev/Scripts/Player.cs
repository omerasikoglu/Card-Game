using System;
using CardGame.Components;
using CardGame.Systems;
using UnityEngine;
using VContainer;

namespace CardGame{

  public class Player : MonoBehaviour{

    [Inject] public BoardManager BoardManager{get; private set;} 
    
    [SerializeField] Transform[] cardHoldTransforms;
    
    PlayerHandManager playerHandManager;
    PlayerInput       playerInput;

    void Awake(){
      playerHandManager = new (this, cardHoldTransforms);
      playerInput       = new (this);
    }

    void OnEnable(){
      playerInput.OnEnable();
    }

    void OnDisable(){
      playerInput.OnDisable();
    }


    void Update(){
      playerInput.Update();
    }

  }

}