using System.Collections.Generic;
using CardGame.World;
using UnityEngine;

namespace CardGame.Systems{

  public class DeckManager : MonoBehaviour{

    
    
    Stack<Card> deck = new();
    
    readonly List<Player> player;
    
    const int DECK_SIZE = 52;
    

    // public DeckManager(List<Player> player){
    //   this.player = player;
    // }
    
    
    
    public void CreateDeck(){
      
      // var Card = new Card.Builder().WithName()
      
      for (int i = 0; i < DECK_SIZE; i++){
        // Create a new card
      }
    }
    
    void DrawCard(){
      // Draw a card from the deck
    }
    
    public Stack<Card> GetDeck() => deck;
  }

}