using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySceneSpawns))]
public class EnemySceneSpawnsEditor : Editor {

    public static int focusedEnemyInfo = -1;

    public override void OnInspectorGUI()
    {
        EnemySceneSpawns sceneSpawns = (EnemySceneSpawns)target;
        if (target == null)
            return;

        if (sceneSpawns.spawnInfo == null)
        {
            sceneSpawns.spawnInfo = new List<EnemySceneSpawns.EnemySpawnInfo>();
        }

        if (GUILayout.Button("New Enemy"))
        {
            EnemySceneSpawns.EnemySpawnInfo newSpawnInfo = new EnemySceneSpawns.EnemySpawnInfo();
            sceneSpawns.spawnInfo.Add(newSpawnInfo);
        }
        EditorGUILayout.Separator();

        for (int i = 0; i < sceneSpawns.spawnInfo.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel = 1;
            GUILayout.Space ((EditorGUI.indentLevel+1) * 10);
            EnemySceneSpawns.EnemySpawnInfo spawnInfo = sceneSpawns.spawnInfo[i];
            if (GUILayout.Button("Spawn " + i + " - " + spawnInfo.enemyType.ToString()))
            {
                if (focusedEnemyInfo == i)
                    focusedEnemyInfo = -1;
                else
                    focusedEnemyInfo = i;
            }

            if (focusedEnemyInfo == i)
            {
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel = 2;
                spawnInfo.enemyType = (EnemySceneSpawns.EnemySpawnInfo.EnemyType)EditorGUILayout.EnumPopup("Enemy Type", spawnInfo.enemyType);
                spawnInfo.spawnPosition = EditorGUILayout.Vector3Field("Spawn Position", spawnInfo.spawnPosition);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space ((EditorGUI.indentLevel+1) * 10);
                if(GUILayout.Button("-")) {
                    sceneSpawns.spawnInfo.RemoveAt(i);
                    i--;
                }
                if(GUILayout.Button("Clone")) {
                    EnemySceneSpawns.EnemySpawnInfo newSpawnInfo = new EnemySceneSpawns.EnemySpawnInfo(spawnInfo);
                    sceneSpawns.spawnInfo.Insert(i, newSpawnInfo);
                }
                if(GUILayout.Button("Frame")) {
                    SceneView.lastActiveSceneView.LookAt(spawnInfo.spawnPosition);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(); //Just hacky to undo horinzontal if we're expanded
            }
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndHorizontal();
        }
    }

//    void OnEnable()
//    {
//        SceneView.onSceneGUIDelegate += OnSceneGUI;
//    }
//
//    void OnDisable()
//    {
//        SceneView.onSceneGUIDelegate -= OnSceneGUI;
//    }

//    void OnSceneGUI(SceneView sceneView)
//    {
//        EnemySceneSpawns sceneSpawns = (EnemySceneSpawns)target;
//        if (target == null)
//            return;
//
//        if (sceneSpawns.spawnInfo == null)
//            return;
//        
//        for (int i = 0; i < sceneSpawns.spawnInfo.Count; i++)
//        {
//            EnemySceneSpawns.EnemySpawnInfo spawnInfo = sceneSpawns.spawnInfo[i];
//
//            if(focusedEnemyInfo == i)   {
//            } else {
////                Graphics.DrawWireSphere(spawnInfo.spawnPosition, 1f);
//            }
//        }
//    }
}