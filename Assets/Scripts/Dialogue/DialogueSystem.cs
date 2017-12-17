using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

	GameObject subtitlePanel;
	public bool showSubtitlePanel = true;

	public bool ignoreAudioLogic;

    int current = 0;
	public bool isDialogueInfinitelyRepeatable = true;

    bool active = false;

    public void StartDialogue() {
        onDialogueStart.Invoke();
        PlayNext();
		TestPlayerController.s_instance.SetPlayerMode (PlayerMode.Cutscene);
		StartCoroutine ("InputLoopHackFix");
		if (showSubtitlePanel)subtitlePanel.GetComponent<Image> ().enabled = true;
    }

	IEnumerator InputLoopHackFix(){
		yield return new WaitForSeconds (.01f);
		active = true;
	}

	void Start () {
		if (!ignoreAudioLogic) {
			if (!GetComponent<AudioSource> ())
				gameObject.AddComponent<AudioSource> ();
			if (GetComponent<AudioSource> () || talkerOne == null)
				talkerOne = GetComponent<AudioSource> ();
		}
		subtitlePanel = GameObject.FindGameObjectWithTag ("SubtitlePanel");
	}

    //TODO call from player input or some shit?
    public void PlayNext() {
		if (current >= dialogueData.dialogue.Count)
		{
			EndDialogue();
			return;
		}
		if (!ignoreAudioLogic) {
			
			TestPlayerController.s_instance.GetComponent<AudioSource> ().Stop ();
			if (talkerOne)
				talkerOne.Stop ();
			if (talkerTwo)
				talkerTwo.Stop ();
			if (talkerThree)
				talkerThree.Stop ();
		}
        TextManager.s_instance.SetSubtitle(dialogueData.dialogue[current].text);
        switch (dialogueData.dialogue[current].talker)
        {
		case DialogueData.Talker.Player:
			if (!ignoreAudioLogic) {
				
				TestPlayerController.s_instance.GetComponent<AudioSource> ().clip = dialogueData.dialogue [current].sound;
				TestPlayerController.s_instance.GetComponent<AudioSource> ().Play ();
			}
			onPlayer.Invoke ();
                break;
		case DialogueData.Talker.TalkerOne:
			if (!ignoreAudioLogic) {
				talkerOne.clip = dialogueData.dialogue [current].sound;
				talkerOne.Play ();
			}
			onTalkerOne.Invoke ();
                break;
		case DialogueData.Talker.TalkerTwo:
			onTalkerTwo.Invoke ();
			if (!ignoreAudioLogic) {
				
				talkerOne.clip = dialogueData.dialogue [current].sound;
				talkerOne.Play ();
			}
                break;
		case DialogueData.Talker.TalkerThree:
			onTalkerThree.Invoke ();
			if (!ignoreAudioLogic) {
				
				talkerOne.clip = dialogueData.dialogue [current].sound;
				talkerOne.Play ();
			}
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
		subtitlePanel.GetComponent<Image> ().enabled = false;
        onDialogueEnd.Invoke();
		TextManager.s_instance.subtitle.text = "";
		if (isDialogueInfinitelyRepeatable)
			current = 0;
    }

	public void SetActiveState(bool newActive)
	{
		active = newActive;
		subtitlePanel.SetActive(newActive);
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
