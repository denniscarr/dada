using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractionSettings))]
public class InteractionSettingsEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InteractionSettings myTarget = (InteractionSettings)target;

        if (GUILayout.Button("Print ableToBeCarried"))
        {
            Debug.Log(myTarget.name + " able to be carried: " + myTarget.ableToBeCarried);
        }
    }

}
