using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour {

    [HideInInspector]
    public bool triggered = false;
    private bool initialized = false;

//    public EnemySceneSpawns spawnInfo;
    public EnemySpawnInfo[] spawnInfo;

    GameObject[] enemies;

	void Start () {
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("SpawnTrigger has no collider", this);
            this.enabled = false;
            return;
        }
        if (GetComponent<Collider>().isTrigger == false)
        {
            Debug.LogError("SpawnTrigger collider isn't trigger", this);
            GetComponent<Collider>().isTrigger = true;
        }
        InitialSpawn(); //Temp until called from load time
	}

    void OnTriggerEnter(Collider col) {
        if (triggered)
            return;

        triggered = true;

        if (col.gameObject.tag == "Player")
        {
            ActivateEnemies();
        }
    }

    /// <summary>
    /// Initialize the enemies and sets them inactive. To be called at loading time.
    /// </summary>
    public void InitialSpawn() {
        enemies = new GameObject[spawnInfo.Length];
        //TODO move this to a spawnManager
        for(int i = 0; i < spawnInfo.Length; i++) {
            enemies[i] = spawnInfo[i].Spawn();
        }
		initialized = true;
    }

    /// <summary>
    /// Activates the initalized enemies.
    /// </summary>
    public void ActivateEnemies() {
        if (!initialized)
        {
            Debug.LogError("Enemy spawn trigger not initialized", this);
            return;
        }
		for(int i = 0; i < spawnInfo.Length; i++) {
			enemies[i].SetActive(true);
		}
    }
}
