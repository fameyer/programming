using UnityEngine;
using System.Collections;

public class ButtonAudio : MonoBehaviour {

	public AudioSource SoundButtonClicked;
	public AudioSource music;

	// Use this for initialization
	void Start () {
		// Play Music if set up
		if (Data.getMusic ()) {
			music.Play ();
		}
	}

	public void clicked(){
		// play Sound if set up
		if (Data.getSound()) {
			SoundButtonClicked.Play ();
		}
	}
}
