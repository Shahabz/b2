using UnityEngine;
using System.Collections;

public class PromptTrigger : MonoBehaviour {

	public string thisMessage;
	public string[] seriesOfMessages;
	bool hasMessagePlayed = false;
	[SerializeField] bool displayPrompt = true;
	[SerializeField] int importance;
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (!hasMessagePlayed) {
				if (displayPrompt) {
					TextManager.s_instance.SetPrompt (thisMessage);
				}
				if (importance > 0) {
					//TextManager.s_instance.AddNoteToSelf (new NoteToSelf (thisMessage));
				}
				hasMessagePlayed = true;
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			TextManager.s_instance.SetPrompt ("");
		}
	}
}
