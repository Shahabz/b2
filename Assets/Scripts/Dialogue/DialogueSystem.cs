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

    int current = 0;

    bool active = false;

    public void StartDialogue() {
        onDialogueStart.Invoke();
        PlayNext();
        active = true;
    }

    //TODO call from player input or some shit?
    public void PlayNext() {
        TestPlayerController.s_instance.GetComponent<AudioSource>().Stop();
        talkerOne.Stop();
        talkerTwo.Stop();
        talkerThree.Stop();

        TextManager.s_instance.SetSubtitle(dialogueData.dialogue[current].text);
        switch (dialogueData.dialogue[current].talker)
        {
            case DialogueData.Talker.Player:
                TestPlayerController.s_instance.GetComponent<AudioSource>().clip = dialogueData.dialogue[current].sound;
                TestPlayerController.s_instance.GetComponent<AudioSource>().Play();
                break;
            case DialogueData.Talker.TalkerOne:
                talkerOne.clip = dialogueData.dialogue[current].sound;
                talkerOne.Play();
                break;
            case DialogueData.Talker.TalkerTwo:
                talkerOne.clip = dialogueData.dialogue[current].sound;
                talkerOne.Play();
                break;
            case DialogueData.Talker.TalkerThree:
                talkerOne.clip = dialogueData.dialogue[current].sound;
                talkerOne.Play();
                break;
            default:
                Debug.Log("No case");
                break;
        }   

        dialogueData.dialogue[current].unityEvent.Invoke();

        current++;
        if (current > dialogueData.dialogue.Count)
        {
            EndDialogue();
        }
    }

    void EndDialogue() {
        onDialogueEnd.Invoke();
    }

    void Update() {
        if (active)
        {
            if (NPInputManager.input.Aim.WasPressed || NPInputManager.input.Fire.WasPressed)
            {
                //TODO finsh this in TextManager
//                if (TextManager.s_instance.isScrolling)
//                { //Can make this a function idc
//                    TextManager.s_instance.FinishScrolling(); //Can change this name idc
//                }
//                else
                {
                    PlayNext();
                }
            }
        }
    }
}
