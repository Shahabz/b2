using UnityEngine;
using System.Collections;

/// <summary>
/// Generic timer, returns false while waiting, returns true on reset once timer > time
/// </summary>
public static class GenericTimer {

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
