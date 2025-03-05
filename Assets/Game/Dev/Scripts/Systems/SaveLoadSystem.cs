using CardGame.Utils;
using UnityEngine;
using VContainer;

namespace CardGame.Systems{

  public class SaveLoadSystem{
    readonly TurnHandler turnHandler;

  #region Members
    int currency, totalWins, totalLosses;

    int Currency{
      get => currency;
      set{
        currency = value;
        Save(Keys.IO.CURRENCY, value);
      }
    }

    int TotalWins{
      get => totalWins;
      set{
        totalWins = value;
        Save(Keys.IO.WIN, value);
      }
    }

    int TotalLosses{
      get => totalLosses;
      set{
        totalLosses = value;
        Save(Keys.IO.LOST, value);
      }
    }

    public int CurrentBet{get; private set;} // !: 1 player's bet
  #endregion

    public SaveLoadSystem(TurnHandler turnHandler){
      this.turnHandler = turnHandler;

      LoadGameData();

      void LoadGameData(){
        Currency    = GetInt(Keys.IO.CURRENCY, Keys.IO.CURRENCY_DEFAULT);
        TotalWins   = GetInt(Keys.IO.WIN, 0);
        TotalLosses = GetInt(Keys.IO.LOST, 0);

        if (Currency <= Keys.IO.CURRENCY_PITY){
          UpdateCurrency(Keys.IO.CURRENCY_DEFAULT);
        }
      }
    }

    public void OnToggle(bool to){
      if (to){
        turnHandler.OnGameEnded += GameEnded;
      }
      else{
        turnHandler.OnGameEnded -= GameEnded;
      }

      void GameEnded(object sender, TurnHandler.ResultEventArgs e){
        if (e.IsWin){
          IncreaseTotalWins();
          UpdateCurrency(e.TotalBetSumCount);
        }
        else{
          IncreaseTotalLosses();
        }
      }
    }

    public void UpdateCurrency(int delta) => Currency += delta;
    public void SetCurrentBet(int to)     => CurrentBet = to;
    void        IncreaseTotalWins()       => ++TotalWins;
    void        IncreaseTotalLosses()     => ++TotalLosses;

    void Save(string key, int to){
      PlayerPrefs.SetInt(key, to);
    }

    public int Load(string key){
      return GetInt(key, 0);
    }

    int GetInt(string key, int defaultValue){
      return PlayerPrefs.GetInt(key, defaultValue);
    }
  }

}