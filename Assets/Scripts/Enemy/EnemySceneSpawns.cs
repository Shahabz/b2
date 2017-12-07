#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemySceneSpawns : ScriptableObject {

    #if UNITY_EDITOR
    [MenuItem("HXC/Current Scene EnemySpawns")]
    public static void GetAsset ()
    {
        string path = "Assets/EnemySpawnInfo/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "SceneSpawns";
        EnemySceneSpawns enemyLevelSpawns = AssetDatabase.LoadAssetAtPath<EnemySceneSpawns>(path);
        if (enemyLevelSpawns == null)
        {
            CustomAssetUtility.CreateAssetAtPath<EnemySceneSpawns>(path, true); //Dialogue which is a scriptable object allow us to create an instance of it which does not need to be attached to gameobject / is not a monobehaviour
        }
        else
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = enemyLevelSpawns;
        }
    }
    #endif

    public List<EnemySpawnInfo> spawnInfo;

    [System.Serializable]
    public class EnemySpawnInfo {
        public EnemySpawnInfo() {
            enemyType = EnemyType.Cat;
            if(SceneView.lastActiveSceneView && SceneView.lastActiveSceneView.camera)
                spawnPosition = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 10f;
            else
                spawnPosition = Vector3.zero;
            patrolPoints = new List<Vector3>();
        }
        public EnemySpawnInfo(EnemySpawnInfo copy) {
            enemyType = copy.enemyType;
            spawnPosition = copy.spawnPosition;
            patrolPoints = new List<Vector3>(copy.patrolPoints);
        }

        public enum EnemyType {
            Cat, 
        }

        public EnemyType enemyType;
        public Vector3 spawnPosition;
        public List<Vector3> patrolPoints;
    }

    void OnDrawGizmos() {
        Debug.LogError("error");
    }
}
#endif
