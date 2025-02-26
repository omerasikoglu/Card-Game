using System;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.Systems{

  public class SaveLoadSystem{
    public event Action<int> OnCurrencyUpdated  = delegate{ };
    public event Action<int> OnTotalWinChanged  = delegate{ };
    public event Action<int> OnTotalLoseChanged = delegate{ };

  #region Members
    int currency, totalWins, totalLosses;

    int Currency{
      get => currency;
      set{
        currency = value;
        Save(Keys.IO.CURRENCY, value);
        OnCurrencyUpdated.Invoke(value);
      }
    }

    int TotalWins{
      get => totalWins;
      set{
        totalWins = value;
        Save(Keys.IO.WIN, value);
        OnTotalWinChanged.Invoke(value);
      }
    }

    int TotalLosses{
      get => totalLosses;
      set{
        totalLosses = value;
        Save(Keys.IO.LOSE, value);
        OnTotalLoseChanged.Invoke(value);
      }
    }
  #endregion

    public SaveLoadSystem(){
      LoadGameData();
    }

    public void UpdateCurrency(int delta) => Currency += delta;
    public void IncreaseTotalWins()       => ++TotalWins;
    public void IncreaseTotalLosses()     => ++TotalLosses;

    void Save(string key, int to){
      PlayerPrefs.SetInt(key, to);
    }

    void LoadGameData(){
      Currency    = GetInt(Keys.IO.CURRENCY, Currency);
      TotalWins   = GetInt(Keys.IO.WIN, TotalWins);
      TotalLosses = GetInt(Keys.IO.LOSE, TotalLosses);

      Debug.Log($" <color=cyan>{"game data loaded"}</color>");
      
      int GetInt(string key, int defaultValue){
        return PlayerPrefs.GetInt(key, defaultValue);
      }
    }

  }

}