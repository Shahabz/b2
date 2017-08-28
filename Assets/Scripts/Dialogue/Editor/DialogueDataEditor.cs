using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor {

    public override void OnInspectorGUI(){
        EditorStyles.textField.wordWrap = true;

        serializedObject.Update();
        DialogueData data = (DialogueData)target;
        if (data.dialogue == null)
        {
            data.dialogue = new List<DialogueData.Dialogue>();
        }
        for (int i = 0; i < data.dialogue.Count; i++)
        {
//            if (GUILayout.Button(data.dialogue[i].text))
//            {
//                data.dialogue[i].expanded = !data.dialogue[i].expanded;
//            }
            data.dialogue[i].expanded = EditorGUILayout.Foldout(
                data.dialogue[i].expanded, 
                "[" + data.dialogue[i].talker.ToString() + "] " + data.dialogue[i].text
            );

            if (data.dialogue[i].expanded)
            {
                EditorGUI.indentLevel = 1;
                data.dialogue[i].text = EditorGUILayout.TextArea(data.dialogue[i].text, GUILayout.MinHeight(60f));

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15f * EditorGUI.indentLevel);
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("dialogue").GetArrayElementAtIndex(i).FindPropertyRelative("unityEvent")
                );
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("dialogue").GetArrayElementAtIndex(i).FindPropertyRelative("talker")
                );
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("dialogue").GetArrayElementAtIndex(i).FindPropertyRelative("sound")
                );

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15f * EditorGUI.indentLevel);
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Delete dialogue?", "Delete dialogue?", "Yes", "No"))
                    {
                        data.dialogue.RemoveAt(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = 0;
            }
        }
        if (GUILayout.Button("New"))
        {
            data.dialogue.Add(new DialogueData.Dialogue());
            data.dialogue[data.dialogue.Count - 1].expanded = true;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(data);

        EditorStyles.textField.wordWrap = false;
    }

//	void Start () {
//		
//	}
//	
//	void Update () {
//		
//	}
}
