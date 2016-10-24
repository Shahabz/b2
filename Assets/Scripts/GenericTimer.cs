using UnityEngine;
using System.Collections;

public class GenericTimer : MonoBehaviour {

	public static bool RunGenericTimer(float time, ref float timer) {
		if (timer < time) {
			timer += Time.deltaTime;
			return false;
		} else {
			timer = 0;
			return true;
		}
	}
}
