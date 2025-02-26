using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class TurnManager{
    public event Action         OnCardDistributionStart = delegate{ };
    public event Action<Entity> OnTurnStart             = delegate{ };

    // [Inject] readonly Player      player;
    // [Inject] readonly Opponent    opponent;
    // [Inject] readonly DeckManager deckManager;

    List<Entity> entities; // AI included

    Entity currentEntity;


    public void SetPlayers(IEnumerable<Entity> entities){
      this.entities = entities.ToList();
    }

    public void Update(){
      // Debug.Log($" entities.Count: <color=cyan>{entities.Count}</color>");
    }
    
  #region States
    public void StartGame(){
      // deckManager.CreateDeck();
    }

    public void DistributeCards(){
      OnCardDistributionStart.Invoke();
    }

    public void FirstTurnStart(){
      currentEntity = entities[0];
      OnTurnStart.Invoke(currentEntity);
    }

    public void EndTurn(){
      NextTurn();

      void NextTurn(){
        int index = entities.IndexOf(currentEntity);
        index         = (index + 1) % entities.Count;
        currentEntity = entities[index];
        OnTurnStart.Invoke(currentEntity);
      }
    }
    
  #endregion
  }

}