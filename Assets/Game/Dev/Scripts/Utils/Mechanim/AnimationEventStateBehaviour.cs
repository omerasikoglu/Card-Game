using UnityEngine;
using UnityEngine.Rendering;

namespace CardGame.Utils.Mechanim{

  public class AnimationEventStateBehaviour : StateMachineBehaviour{

    // [SerializeField, VolumeComponent.Indent, LabelText(nameof(eventName), SdfIconType.Play), ValueDropdown(nameof(_events))]
    string eventName;
    [SerializeField, Range(0f, 1f)] float triggerTime;

    AnimationEventReceiver receiver;

    bool hasTriggered;

    // readonly string[] _events = Keys.Observer.ALL;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
      hasTriggered = false;
      receiver     = animator.GetComponent<AnimationEventReceiver>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
      float currentTime = stateInfo.normalizedTime % 1f;

      if (hasTriggered || currentTime < triggerTime) return;
      NotifyReceiver(animator);
      hasTriggered = true;
    }

    void NotifyReceiver(Animator animator){
      if (receiver == null) return;
      receiver.OnAnimationEventTriggered(eventName);

    }
  }

}