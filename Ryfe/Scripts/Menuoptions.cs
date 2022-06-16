using UnityEngine;
using System.Collections;

public class Menuoptions : MonoBehaviour {

	// Screens in Main Menu
	public GameObject PlayScreen;
	public GameObject HighScoreScreen;
	public GameObject OptionScreen;
	public GameObject QuitScreen;

	// titles to switch
	public GameObject titleSmall;
	public GameObject titleLarge;

	void Start(){
		titleSmall.SetActive (false);
		titleLarge.SetActive (true);
	}
		
	// events for buttons clicked
	public void StartButtonClicked(){
		if (PlayScreen.activeSelf) {
			PlayScreen.SetActive (false);
			titleSmall.SetActive (false);
			titleLarge.SetActive(true);
		} 
		else {
			HighScoreScreen.SetActive (false);
			OptionScreen.SetActive (false);
			QuitScreen.SetActive (false);
			PlayScreen.SetActive (true);
			titleSmall.SetActive (true);
			titleLarge.SetActive(false);
		}
	}

	public void HighScoreButtonClicked(){
		if (HighScoreScreen.activeSelf) {
			HighScoreScreen.SetActive (false);
			titleSmall.SetActive (false);
			titleLarge.SetActive(true);
		} 
		else {
			PlayScreen.SetActive (false);
			OptionScreen.SetActive (false);
			QuitScreen.SetActive (false);
			HighScoreScreen.SetActive (true);
			titleSmall.SetActive (true);
			titleLarge.SetActive(false);
		}
	}

	public void OptionButtonClicked(){
		if (OptionScreen.activeSelf) {
			OptionScreen.SetActive (false);
			titleSmall.SetActive (false);
			titleLarge.SetActive(true);
		} 
		else {
			PlayScreen.SetActive (false);
			HighScoreScreen.SetActive (false);
			QuitScreen.SetActive (false);
			OptionScreen.SetActive (true);
			titleSmall.SetActive (true);
			titleLarge.SetActive(false);
		}
	}

	// Quit Buttons
	public void QuitButtonClicked(){
		if (QuitScreen.activeSelf) {
			QuitScreen.SetActive (false);
			titleSmall.SetActive (false);
			titleLarge.SetActive(true);
		} 
		else {
			PlayScreen.SetActive (false);
			HighScoreScreen.SetActive (false);
			OptionScreen.SetActive (false);
			QuitScreen.SetActive (true);
			titleSmall.SetActive (true);
			titleLarge.SetActive(false);
		}
	}
	// Quit Button
	public void YesClicked(){
		Application.Quit ();
	}
	public void NoClicked(){
		QuitScreen.SetActive (false);
		titleSmall.SetActive (false);
		titleLarge.SetActive(true);
	}
}
