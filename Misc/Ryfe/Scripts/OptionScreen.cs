using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionScreen : MonoBehaviour {
	// for resolution
	private int res_x, res_y;

	// for toggles
	public Toggle music_on;
	public Toggle sound_on;

	// for connection to screens
	public GameObject screens;

	void Start(){
		music_on.isOn = Data.getMusic ();
		sound_on.isOn = Data.getSound ();
	} 

	// Set new solution in full screen mode	
	public void resolutionClickedx(int x){
		res_x = x;
	}
	public void resolutionClickedy(int y){
		res_y = y;
	}
	public void resolutionClicked(){
		Screen.SetResolution (res_x, res_y, true);
	}
	// Soundoptions
	public void soundChanged(Slider a){
		AudioListener.volume = a.value;
	}
	public void musicOn(Toggle t){
		Data.setMusic (t.isOn);
		// change playing music
		if (t.isOn) {
			screens.GetComponent<ButtonAudio> ().music.mute = false;
		}
		else {
			screens.GetComponent<ButtonAudio> ().music.mute = true;
		}
	}
	public void soundOn(Toggle t){
		Data.setSound (t.isOn);
	}
}
