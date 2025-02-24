using System;
// using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Template.Utils.Mechanim{

  [Serializable]
  public class AnimationEvent{

    // [SerializeField, VolumeComponent.Indent, LabelText(nameof(eventName), SdfIconType.Play), ValueDropdown(nameof(_events))]
    public string eventName;
    public UnityEvent OnAnimationEvent;

    // readonly string[] _events = Keys.Observer.ALL;
  }

}