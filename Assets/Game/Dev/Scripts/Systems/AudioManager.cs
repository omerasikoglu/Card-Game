using System;
using RunTogether.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Systems{

  public enum SoundType : byte{
    ButtonClickSfx,
    LostSfx,
    WinSfx,
  }

  [Serializable] public class Sound{
    [HideInInspector]
    public AudioSource Source;
    public                  AudioClip AudioClip;
    public                  SoundType SoundType;
    [Range(0f, 1f)]  public float     Volume;
    [Range(.1f, 3f)] public float     Pitch;
    public                  bool      PlayOnAwake;
  }

  public class AudioManager : MonoBehaviour{
    public Sound[] Sounds;

    void Awake(){
      foreach (Sound s in Sounds){
        s.Source             = gameObject.AddComponent<AudioSource>();
        s.Source.clip        = s.AudioClip;
        s.Source.volume      = s.Volume;
        s.Source.pitch       = s.Pitch;
        s.Source.playOnAwake = s.PlayOnAwake;
      }
    }

    [Button] public void PlaySound(SoundType soundType){
      Sound s = Array.Find(Sounds, sound => sound.SoundType == soundType);
      if (s == null){
        Debug.LogWarning($"Sound with type {soundType} not found in AudioManager.");
        return;
      }

      s.Source.Play();
    }

    public void StopSound(SoundType soundType){
      Sound s = Array.Find(Sounds, sound => sound.SoundType == soundType);
      if (s == null){
        Debug.LogWarning($"Sound with type {soundType} not found in AudioManager.");
        return;
      }

      s.Source.Stop();
    }

    public void SetSoundEffectsMute(bool isMuted){
      foreach (Sound s in Sounds){
        s.Source.mute = isMuted;
      }
    }
  }

}