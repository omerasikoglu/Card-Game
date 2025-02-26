namespace CardGame{

  public class Player : Entity{
    PlayerInput playerInput;

  #region Core
    void Awake(){
      PlayerHandManager = new(this, cardHoldTransforms);
      playerInput       = new(this);
    }

    // protected override void OnToggle(bool to){
    //   base.OnToggle(to);
    //   playerInput.OnToggle(to);
    // }

    protected override void OnTurnStart(Entity ctx){
      playerInput.OnToggle(ctx == this);
    }

    void Update(){
      playerInput.Update();
    }
  #endregion

    public void AddCardToHand(){
      PlayerHandManager.AddCardToYourHand();
    }
  }

}