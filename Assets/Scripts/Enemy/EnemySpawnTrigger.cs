using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour {

    [HideInInspector]
    public bool triggered = false;
    private bool initialized = false;

    public EnemySceneSpawns spawnInfo;

	void Start () {
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
        //TODO move this to a spawnManager
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

    }
}
