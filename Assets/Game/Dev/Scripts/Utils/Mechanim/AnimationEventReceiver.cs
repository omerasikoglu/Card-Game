using System.Collections.Generic;
using UnityEngine;

namespace Template.Utils.Mechanim{

  public class AnimationEventReceiver : MonoBehaviour{
    [SerializeField] List<AnimationEvent> animationEventList = new();

    public void OnAnimationEventTriggered(string eventName){
      AnimationEvent matchingEvent = animationEventList.Find(o => o.eventName == eventName);
      matchingEvent?.OnAnimationEvent?.Invoke();
    }
  }

}
