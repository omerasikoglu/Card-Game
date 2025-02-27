using System;
using System.Globalization;
using CardGame.Systems;
using CardGame.Utils;
using DG.Tweening;
using RunTogether.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace CardGame.UI{

  public class CanvasController : MonoBehaviour{

    [Inject] readonly AudioManager   audioManager;
    [Inject] readonly SaveLoadSystem saveLoadSystem;
    [Inject] readonly TurnHandler    turnHandler;

    public event Action OnGameStart  = delegate{ };
    public event Action OnQuitInGame = delegate{ };
    public event Action<bool> OnPlayerInputToggle = delegate{ };

  #region Members
    [Title(Keys.UI.Main)]
    [TabGroup(Keys.UI.Main)][SerializeField] Transform mainMenuPanel;
    [TabGroup(Keys.UI.Main)][SerializeField] Button createLobbyButton;
    [TabGroup(Keys.UI.Main)][SerializeField] Button exitGameButton;

    [Title(Keys.UI.Lobby)]
    [TabGroup(Keys.UI.Lobby)][SerializeField] Transform lobbyPanel;
    [TabGroup(Keys.UI.Lobby)][SerializeField] Button   playerInfoButton;
    [TabGroup(Keys.UI.Lobby)][SerializeField] TMP_Text playerLobbyCurrencyText;

    [TabGroup(Keys.UI.Lobby)][SerializeField] Button exitLobbyPanelButton;
    [TabGroup(Keys.UI.Lobby)][SerializeField] Button createNewbieLobbyButton;
    [TabGroup(Keys.UI.Lobby)][SerializeField] Button createRookieLobbyButton;
    [TabGroup(Keys.UI.Lobby)][SerializeField] Button createNobleLobbyButton;

    [Title(Keys.UI.Table)]
    [TabGroup(Keys.UI.Table)]
    [TabGroup(Keys.UI.Table)][SerializeField] Transform createTablePanel;
    [TabGroup(Keys.UI.Table)][SerializeField] Scrollbar betScrolbar;
    [TabGroup(Keys.UI.Table)][SerializeField] TMP_Text  minBetText;
    [TabGroup(Keys.UI.Table)][SerializeField] TMP_Text  maxBetText;
    [TabGroup(Keys.UI.Table)][SerializeField] TMP_Text  currentBetText;
    [TabGroup(Keys.UI.Table)][SerializeField] TMP_Text  betRangeText;
    [TabGroup(Keys.UI.Table)][SerializeField] Toggle    twoPlayerToggle;
    [TabGroup(Keys.UI.Table)][SerializeField] Toggle    fourPlayerToggle;
    [TabGroup(Keys.UI.Table)][SerializeField] Button    createTableButton;
    [TabGroup(Keys.UI.Table)][SerializeField] Button    exitTableButton;

    [Title(Keys.UI.InGame)]
    [TabGroup(Keys.UI.InGame)][SerializeField] Transform inGamePanel;
    [TabGroup(Keys.UI.InGame)][SerializeField] Button returnToMainMenuInGameButton;

    [Title(Keys.UI.Info)]
    [TabGroup(Keys.UI.Info)][SerializeField] Transform playerInfoPanel;
    [TabGroup(Keys.UI.Info)][SerializeField] TMP_Text playerInfoCurrencyText;
    [TabGroup(Keys.UI.Info)][SerializeField] TMP_Text playerInfoWinCountText;
    [TabGroup(Keys.UI.Info)][SerializeField] TMP_Text playerInfoLostCountText;
    [TabGroup(Keys.UI.Info)][SerializeField] Button   exitPlayerInfoButton;

    [Title(Keys.UI.Warning)]
    [TabGroup(Keys.UI.Warning)][SerializeField] Transform warningPanel;
    [TabGroup(Keys.UI.Warning)][SerializeField] TMP_Text warningBetCurrencyText;
    [TabGroup(Keys.UI.Warning)][SerializeField] Button   returnToGameButton;
    [TabGroup(Keys.UI.Warning)][SerializeField] Button   exitToMainMenuWarningButton;

    Transform[] allPanels;
    Toggle[]    allToggles;
    Button[]    allButtons;
    Button[]    mainMenuButtons,lobbyButtons;

    const float OPEN_DURATION = 0.4f;
  #endregion

  #region Core
    void Awake(){
      allPanels = new[]{ mainMenuPanel, lobbyPanel, createTablePanel, inGamePanel, playerInfoPanel, warningPanel };
      allButtons = new[]{
        createLobbyButton, exitGameButton, playerInfoButton, exitLobbyPanelButton, createTableButton, exitPlayerInfoButton,
        createNewbieLobbyButton, createRookieLobbyButton, createNobleLobbyButton, exitToMainMenuWarningButton, returnToGameButton,
        exitTableButton, returnToMainMenuInGameButton
      };
      allToggles   = new[]{ twoPlayerToggle, fourPlayerToggle };
      mainMenuButtons = new[]{ createLobbyButton, exitGameButton };
      lobbyButtons = new[]{ exitLobbyPanelButton, createNewbieLobbyButton, createRookieLobbyButton, createNobleLobbyButton };

      allButtons.ForEach(o => AddDefaultTriggers(o.transform));
      allToggles.ForEach(o => AddDefaultTriggers(o.transform));

      AddOnClickTriggers();
    }

    void Start(){
      SwitchPanel(mainMenuPanel);
    }
  #endregion

    void AddDefaultTriggers(Transform uiElement){
      EventTrigger eventTrigger = uiElement.gameObject.GetOrAdd<EventTrigger>();

      EventTrigger.Entry pointerEnter  = new(){ eventID = EventTriggerType.PointerEnter };
      EventTrigger.Entry pointerExit   = new(){ eventID = EventTriggerType.PointerExit };
      EventTrigger.Entry buttonClicked = new(){ eventID = EventTriggerType.PointerClick };

      pointerEnter.callback.AddListener(PointerEnter);
      pointerExit.callback.AddListener(PointerExit);
      buttonClicked.callback.AddListener(ButtonClicked);

      eventTrigger.triggers.Add(pointerEnter);
      eventTrigger.triggers.Add(pointerExit);
      eventTrigger.triggers.Add(buttonClicked);

      void PointerEnter(BaseEventData args){
        uiElement.transform.DOScale(1.1f, OPEN_DURATION).SetEase(DelayedOutElastic).SetId(Keys.Tween.UI);
      }

      void PointerExit(BaseEventData args){
        uiElement.transform.DOScale(1f, OPEN_DURATION).SetEase(Ease.OutQuad).SetId(Keys.Tween.UI);
      }

      void ButtonClicked(BaseEventData args){
        if (uiElement.TryGetComponent(out Button button)){
          if (!button.interactable) return;
        }

        audioManager.PlaySound(SoundType.ButtonClickSfx);
      }
    }

    void AddOnClickTriggers(){
      // Main Menu
      createLobbyButton.onClick.AddListener(CreateLobbyButtonPressed);
      exitGameButton.onClick.AddListener(ExitGameButtonPressed);
      // Lobby
      playerInfoButton.onClick.AddListener(PlayerInfoButtonPressed);
      exitLobbyPanelButton.onClick.AddListener(ExitLobbyPanelButtonPressed);
      createNewbieLobbyButton.onClick.AddListener(CreateNewbieLobbyButtonPressed);
      createRookieLobbyButton.onClick.AddListener(CreateRookieLobbyButtonPressed);
      createNobleLobbyButton.onClick.AddListener(CreateNobleLobbyButtonPressed);
      // Table
      exitTableButton.onClick.AddListener(ExitTableButtonPressed);
      betScrolbar.onValueChanged.AddListener(BetScrolbarValueChanged);
      twoPlayerToggle.onValueChanged.AddListener(TwoPlayerTogglePressed);
      fourPlayerToggle.onValueChanged.AddListener(FourPlayerTogglePressed);
      createTableButton.onClick.AddListener(CreateGameTableButtonPressed);
      // InGame
      returnToMainMenuInGameButton.onClick.AddListener(ReturnToMainMenuInGameButtonPressed);
      // Player Info
      exitPlayerInfoButton.onClick.AddListener(ExitPlayerPanelButtonPressed);
      // Warning
      returnToGameButton.onClick.AddListener(ReturnToGameWarningButtonPressed);
      exitToMainMenuWarningButton.onClick.AddListener(ExitToMainMenuWarningButtonPressed);

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Main Menu 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹
      void CreateLobbyButtonPressed(){
        CompleteTweens();
        mainMenuButtons.ForEach(o => o.transform.localScale = Vector3.one);
        mainMenuPanel.Toggle(false);

        playerLobbyCurrencyText.SetText(saveLoadSystem.Load(Keys.IO.CURRENCY).ToString("C0"));

        lobbyPanel.Toggle(true);
      }

      void ExitGameButtonPressed(){
        Application.Quit();
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Lobby 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹
      void PlayerInfoButtonPressed(){
        CompleteTweens();
        lobbyButtons.ForEach(o => o.interactable = false);

        playerInfoCurrencyText.SetText(saveLoadSystem.Load(Keys.IO.CURRENCY).ToString());
        playerInfoWinCountText.SetText(saveLoadSystem.Load(Keys.IO.WIN).ToString());
        playerInfoLostCountText.SetText(saveLoadSystem.Load(Keys.IO.LOST).ToString());

        playerInfoPanel.Toggle(true);
      }

      void ExitLobbyPanelButtonPressed(){
        CompleteTweens();
        lobbyPanel.Toggle(false);
        mainMenuPanel.Toggle(true);
      }

      void CreateNewbieLobbyButtonPressed(){
        CompleteTweens();
        lobbyPanel.Toggle(false);
        createTablePanel.Toggle(true);
        betScrolbar.value = 0;
        minBetText.SetText(Keys.Bet.NEWBIES.Min.ToString(CultureInfo.CurrentCulture));
        maxBetText.SetText(Keys.Bet.NEWBIES.Max.ToString(CultureInfo.CurrentCulture));
        currentBetText.SetText(Keys.Bet.NEWBIES.Min.ToString(CultureInfo.CurrentCulture));
        betRangeText.SetText($"<color=#52A6FF>Bet Range</color>\n{Keys.Bet.NEWBIES.Min} - {Keys.Bet.NEWBIES.Max}");

      }

      void CreateRookieLobbyButtonPressed(){
        CompleteTweens();
        lobbyPanel.Toggle(false);
        createTablePanel.Toggle(true);
        betScrolbar.value = 0;
        minBetText.SetText(Keys.Bet.ROOKIES.Min.ToString(CultureInfo.CurrentCulture));
        maxBetText.SetText(Keys.Bet.ROOKIES.Max.ToString(CultureInfo.CurrentCulture));
        currentBetText.SetText(Keys.Bet.ROOKIES.Min.ToString(CultureInfo.CurrentCulture));
        betRangeText.SetText($"<color=#52A6FF>Bet Range</color>\n{Keys.Bet.ROOKIES.Min} - {Keys.Bet.ROOKIES.Max}");
      }

      void CreateNobleLobbyButtonPressed(){
        CompleteTweens();
        lobbyPanel.Toggle(false);
        createTablePanel.Toggle(true);
        betScrolbar.value = 0;
        minBetText.SetText(Keys.Bet.NOBLES.Min.ToString(CultureInfo.CurrentCulture));
        maxBetText.SetText(Keys.Bet.NOBLES.Max.ToString(CultureInfo.CurrentCulture));
        currentBetText.SetText(Keys.Bet.NOBLES.Min.ToString(CultureInfo.CurrentCulture));
        betRangeText.SetText($"<color=#52A6FF>Bet Range</color>\n{Keys.Bet.NOBLES.Min} - {Keys.Bet.NOBLES.Max}");
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Table 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void ExitTableButtonPressed(){
        CompleteTweens();
        createTablePanel.Toggle(false);
        lobbyPanel.Toggle(true);
      }

      void BetScrolbarValueChanged(float value){
        var min = minBetText.text.ConvertToInt();
        var max = maxBetText.text.ConvertToInt();

        var span          = max - min;
        var rawResult     = value * span + min;
        var roundedResult = rawResult.Ceil50();
        currentBetText.SetText(roundedResult.ToString("C0"));
      }

      void TwoPlayerTogglePressed(bool to){
        fourPlayerToggle.isOn = !to;
      }

      void FourPlayerTogglePressed(bool to){
        twoPlayerToggle.isOn = !to;
      }

      void CreateGameTableButtonPressed(){
        CompleteTweens();
        saveLoadSystem.SetCurrentBet(currentBetText.text.ConvertToInt());
        createTablePanel.Toggle(false);
        inGamePanel.Toggle(true);
        OnGameStart.Invoke();
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 InGame 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹
      void ReturnToMainMenuInGameButtonPressed(){
        CompleteTweens();
        inGamePanel.Toggle(false);
        warningPanel.Toggle(true);
        warningBetCurrencyText.SetText(saveLoadSystem.CurrentBet.ToString());
      }

      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Player Info 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹
      void ExitPlayerPanelButtonPressed(){
        CompleteTweens();
        playerInfoPanel.Toggle(false);
        lobbyButtons.ForEach(o => o.interactable = true);
        returnToMainMenuInGameButton.interactable = true;
        OnPlayerInputToggle.Invoke(true);
      }
      // 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹 Warning 🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹🔹

      void ReturnToGameWarningButtonPressed(){
        CompleteTweens();
        warningPanel.Toggle(false);
        inGamePanel.Toggle(true);
      }

      void ExitToMainMenuWarningButtonPressed(){
        CompleteTweens();
        warningPanel.Toggle(false);
        mainMenuPanel.Toggle(true);
        OnQuitInGame.Invoke();
      }

    }

    void SwitchPanel(Transform to){
      allPanels.ForEach(o => o.Toggle(to == o));
    }

    public void OpenPlayerInfoPanel(){
      returnToMainMenuInGameButton.interactable = false;
      playerInfoPanel.Toggle(true);
      playerInfoCurrencyText.SetText(saveLoadSystem.Load(Keys.IO.CURRENCY).ToString());
      playerInfoWinCountText.SetText(saveLoadSystem.Load(Keys.IO.WIN).ToString());
      playerInfoLostCountText.SetText(saveLoadSystem.Load(Keys.IO.LOST).ToString());
    }

    void CompleteTweens(){
      DOTween.Complete(Keys.Tween.UI);
    }

    float DelayedOutElastic(float time, float duration, float overshootOrAmplitude, float period){
      float slowFactor = 0.6f; // Başlangıcı yavaşlatma katsayısı

      time = Mathf.Pow(time, slowFactor); // Zaman ilerleyişini yavaşlat
      return Mathf.Pow(2, -10 * time) * Mathf.Sin((time - 0.075f) * (2 * Mathf.PI) / 0.3f) + 1;
    }
  }

}