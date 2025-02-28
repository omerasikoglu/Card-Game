using CardGame.Utils;
using UnityEngine;

public class ResolutionManager{

  public ResolutionManager(){
    QualitySettings.vSyncCount  = 0;
    Application.targetFrameRate = Keys.Fps;

    if (Camera.main != null && Camera.main.pixelWidth > Keys.Resolution){
      Screen.SetResolution(Keys.Resolution, (int)((Camera.main.pixelHeight * Keys.Resolution) / Camera.main.pixelWidth), true);
    }
  }
}