#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelHelper))]
public class LevelHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelHelper levelHelper = (LevelHelper)target;

        if(GUILayout.Button("SetLevel"))
        {
            levelHelper.SetLevel(levelHelper.levelNumber);
        }
    
        if(GUILayout.Button("ResetPlayerPrefs"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif