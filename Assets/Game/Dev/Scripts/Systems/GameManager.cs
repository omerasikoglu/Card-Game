using System;
using UnityEngine;
using VContainer;

namespace Template.Systems{

  public class GameManager : MonoBehaviour{
    [Inject] readonly IObjectResolver resolver;

    public void Start(){ }

  }

}