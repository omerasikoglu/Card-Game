using CardGame.Components;
using UnityEngine;

namespace CardGame{

  public class Player : MonoBehaviour{

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