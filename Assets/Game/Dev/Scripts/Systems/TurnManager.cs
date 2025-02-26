using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class TurnManager{
    public event Action         OnCardDistributionStart = delegate{ };
    public event Action<Entity> OnNewTurnStart             = delegate{ };

    // [Inject] readonly Player      player;
    // [Inject] readonly Opponent    opponent;
    // [Inject] readonly DeckManager deckManager;

    List<Entity> entities; // AI included

    Entity currentEntity;


    public void SetPlayers(IEnumerable<Entity> entities){
      this.entities = entities.ToList();
    }

  #region States
    public void StartGame(){
      // deckManager.CreateDeck();
    }

    public void DistributeCards(){
      OnCardDistributionStart.Invoke();
    }

    public void FirstTimeStart(){
      currentEntity = entities[0];
      Debug.Log($" <color=red>{entities[0]}'s turn!</color>");
      OnNewTurnStart.Invoke(currentEntity);
    }

    public void NextPlayerTurn(){
      NextTurn();

      void NextTurn(){
        int index = entities.IndexOf(currentEntity);
        index         = (index + 1) % entities.Count;
        currentEntity = entities[index];
        OnNewTurnStart.Invoke(currentEntity);
      }
    }
    
  #endregion
  }

}