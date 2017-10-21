using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour {

    public DialogueData dialogueData;

    public AudioSource talkerOne;
    public AudioSource talkerTwo;
    public AudioSource talkerThree;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

	public UnityEvent onTalkerOne;
	public UnityEvent onTalkerTwo;
	public UnityEvent onTalkerThree;
	public UnityEvent onPlayer;


    int current = 0;
	public bool isDialogueInfinitelyRepeatable = true;

    bool active = false;

    public void StartDialogue() {
        onDialogueStart.Invoke();
        PlayNext();
		TestPlayerController.s_instance.SetPlayerMode (PlayerMode.Cutscene);
		StartCoroutine ("InputLoopHackFix");
    }

	IEnumerator InputLoopHackFix(){
		yield return new WaitForSeconds (.01f);
		active = true;
	}

	void Start () {
		if (GetComponent<AudioSource>() || talkerOne==null)
			talkerOne = GetComponent<AudioSource> ();
	}

    //TODO call from player input or some shit?
    public void PlayNext() {
		if (current >= dialogueData.dialogue.Count)
		{
			EndDialogue();
			return;
		}
        TestPlayerController.s_instance.GetComponent<AudioSource>().Stop();
		if (talkerOne) talkerOne.Stop();
		if (talkerTwo) talkerTwo.Stop();
		if (talkerThree) talkerThree.Stop();

        TextManager.s_instance.SetSubtitle(dialogueData.dialogue[current].text);
        switch (dialogueData.dialogue[current].talker)
        {
		case DialogueData.Talker.Player:
			TestPlayerController.s_instance.GetComponent<AudioSource> ().clip = dialogueData.dialogue [current].sound;
			TestPlayerController.s_instance.GetComponent<AudioSource> ().Play ();
			onPlayer.Invoke ();
                break;
		case DialogueData.Talker.TalkerOne:
			talkerOne.clip = dialogueData.dialogue [current].sound;
			talkerOne.Play ();
			onTalkerOne.Invoke ();
                break;
		case DialogueData.Talker.TalkerTwo:
			onTalkerTwo.Invoke ();
                talkerOne.clip = dialogueData.dialogue[current].sound;
                talkerOne.Play();
                break;
		case DialogueData.Talker.TalkerThree:
			onTalkerThree.Invoke ();
                talkerOne.clip = dialogueData.dialogue[current].sound;
                talkerOne.Play();
                break;
            default:
                Debug.Log("No case");
                break;
        }   

        dialogueData.dialogue[current].unityEvent.Invoke();
        current++;
    }

    void EndDialogue() {
		TestPlayerController.s_instance.SetPlayerMode (PlayerMode.Normal);
		active = false;
        onDialogueEnd.Invoke();
		TextManager.s_instance.subtitle.text = "";
		if (isDialogueInfinitelyRepeatable)
			current = 0;
    }

    void Update() {
        if (active)
        {
			if (NPInputManager.input.Aim.WasPressed || NPInputManager.input.Fire.WasPressed || NPInputManager.input.Interact.WasPressed)
            {
				if (TextManager.s_instance.GetIsTypeWriting())
                { //Can make this a function idc
					 TextManager.s_instance.CompleteTypeWrite(); //Can change this name idc
                }
                 else
                {
                    PlayNext();
                }
            }
        }
    }
}
