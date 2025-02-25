using CardGame.Utils;
using Unity.Cinemachine;
using UnityEngine;

public class ResolutionManager : MonoBehaviour{

  [SerializeField] CinemachineCamera vCam;

  [SerializeField] private float baseFOV = 70f;

  public int Resolution = 1080;
  public int Fps        = 60;

  void Awake(){
    QualitySettings.vSyncCount = 0;
    if (Camera.main.pixelWidth > Resolution){
      Screen.SetResolution(Resolution, (int)((Camera.main.pixelHeight * Resolution) / Camera.main.pixelWidth), true);
    }

    Application.targetFrameRate = Fps;
  }

  void Start(){
    AdjustFOV();
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
  }

  void AdjustFOV(){
    float scaleMultiplier = UtilsClass.GetMobileScaleMultiplier();
    float newFOV          = baseFOV * scaleMultiplier;

    vCam.Lens.FieldOfView = newFOV;
  }
}