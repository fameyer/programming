using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	// Sounds for Stone fitting
	public AudioSource StoneFit;
	public AudioSource StoneNotFit;
	public AudioSource ButtonPressed;
	public AudioSource FireLoop;
	public AudioSource Music;

	// Use this for initialization
	void Start () {
		// play music if set up
		if (Data.getMusic ()) {
			Music.Play ();
		}
	}

	// Play sounds for stone fitting
	public void PlayStoneFit(){
		if (Data.getSound ()) {
			StoneFit.Play ();
		}
	}
	public void PlayStoneNotFit(){
		if (Data.getSound ()) {
			StoneNotFit.Play ();
		}
	}
	// ButtonSound
	public void PlayButtonPressed(){
		if (Data.getSound ()) {
			ButtonPressed.Play ();
		}
	}
	// sound for fire
	public void FireSoundOn(){
		FireLoop.loop = true;
		if (Data.getSound ()) {
			FireLoop.Play ();
		}
	}
	public void FireSoundOff(){
		Wait (3.0f);
		FireLoop.loop = false;
	}
	IEnumerator Wait(float s){
		yield return new WaitForSeconds (s);
	}

	// Update is called once per frame
	void Update () {
	}
}
