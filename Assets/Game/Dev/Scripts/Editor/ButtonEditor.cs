using Template.Systems;
using UnityEditor;
using UnityEngine;

public class ButtonEditor : Editor{ }

[CustomEditor(typeof(GameManager))]
public class GameManagerButtonEditor : Editor{

  public override void OnInspectorGUI(){
    GameManager gameManager = (GameManager)target;

    DrawDefaultInspector();

    if (GUILayout.Button("Start")){
      gameManager.Start();
    }
  }
}