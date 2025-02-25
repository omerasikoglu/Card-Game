using CardGame.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardGame{

  public class PlayerInput{

  #region Members
    Card    cardHit;
    bool    isDragging;
    Vector3 dragVelocity = Vector3.zero;

    readonly Player       player;
    readonly InputActions inputActions;
    readonly RaycastHit[] cardHits = new RaycastHit[10];
    #endregion

    InputAction Touch        => inputActions.Inventory.Touch;
    InputAction TouchContact => inputActions.Inventory.TouchContact;
    InputAction FirstTouch   => inputActions.Inventory.FirstTouch;

    public PlayerInput(Player player){
      this.player = player;

      inputActions = new();
      inputActions.Enable();
    }

    public void OnEnable(){
      inputActions.Enable();
    }

    public void OnDisable(){
      inputActions.Disable();
    }

    public void Update(){
      TouchPerformed();
    }

    void TouchPerformed(){
      if (isDragging) return;
      if (!TouchContact.IsPressed()) return;
    }

  }

}