using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Intro : MonoBehaviour {

	public Text PlayText;

	private bool go_on = true;

	// Use this for initialization
	void Start () {
		StartCoroutine (Fading ());
	}

	IEnumerator Fading(){
		while (go_on) {
			// fade to transparent over 500ms.
			PlayText.CrossFadeAlpha (0.0f, 2.0f, false);

			yield return new WaitForSeconds (2.0f);
			// and back over 500ms.
			PlayText.CrossFadeAlpha (1.0f, 2.0f, false);
			yield return new WaitForSeconds (2.0f);
		}
	}

	// Update is called once per frame
	void Update () {
		// if user presses any key go to main screen
		if (Input.anyKeyDown ) {
			go_on = false;
			Application.LoadLevel(1);
		}
	}
}
