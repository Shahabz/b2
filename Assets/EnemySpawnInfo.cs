using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawnInfo : MonoBehaviour {

    public enum EnemyType {
        Cat, 
    }

	public GameObject catPrefab;

    public EnemyType enemyType = EnemyType.Cat;
    public List<Vector3> patrolPoints;

	void Start () {
        patrolPoints = new List<Vector3>();
	}

    public GameObject Spawn() {
		if(enemyType == EnemyType.Cat) {
			GameObject cat = (GameObject)Instantiate(catPrefab, transform.position, transform.rotation);
			cat.SetActive(false);
			return cat;
		}
        return null;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, 1f);
        Mesh enemyMesh = null;
        if (enemyType == EnemyType.Cat)
        {
            enemyMesh = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Characters/Cat/Dark_Cat.fbx").GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        }
        Gizmos.DrawMesh(enemyMesh, transform.position, transform.rotation);
    }
    #endif
}
