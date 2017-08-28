using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueData : ScriptableObject {

    public enum Talker
    {
        Player, TalkerOne, TalkerTwo, TalkerThree
    }

    #if UNITY_EDITOR
    [MenuItem("Assets/Create/DialogueData")]
    public static void CreateAsset ()
    {
        CustomAssetUtility.CreateAsset<DialogueData>(true); //Dialogue which is a scriptable object allow us to create an instance of it which does not need to be attached to gameobject / is not a monobehaviour
    }
    #endif

    [System.Serializable]
    public class Dialogue {
        public bool expanded; //Being lazy putting it here. For the custom editor.
        public UnityEvent unityEvent;
        public string text;
        public Talker talker;
//        public AudioSource talker;
        public AudioClip sound;
    }

    public List<Dialogue> dialogue;

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
