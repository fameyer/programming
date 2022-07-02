using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScreensScript : MonoBehaviour {

	// Playscreen
	public void PlayAllClicked(){
		// Load Campaign Run
		Data.setMode (true);
		Application.LoadLevel (2);
	}
	public void PlayLevelClicked(int i){
		Data.setMode (false);
		Data.setCounter (i);
		Application.LoadLevel (2);
	}
	public void PlayCustomClicked(){
		Data.setMode (false);
		Application.LoadLevel (2);
	}
	// Name Input
	public void NameInputField(Text s){
		Data.setName (s.text);
	}
	// Custom Phase Timer Input
	public void PhaseInputField(Text s){
		float value;
		bool parsed = float.TryParse(s.text,out value);
		if(parsed){
			Data.setCustomPhaseTime(value);
		}
	}
	// Custom Gravity Timer Input
	public void GravityInputField(Text s){
		float value;
		bool parsed = float.TryParse (s.text, out value);
		if (parsed) {
			Data.setCustomGravityTime (value);
		}
	}

	// Quit Button
	public void YesClicked(){
		Application.Quit ();
	}
}
