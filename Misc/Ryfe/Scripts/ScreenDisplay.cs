using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenDisplay : MonoBehaviour {

	private GameController gameController;

	// texts
	public Text levelText;
	public Text scoreText;
	public Text timeText;
	public Text playerText;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		else {
			Debug.Log("Cannot find 'GameController' script");
		}
		playerText.text = "Player: \t"+Data.getName ();
	}

	// Texts of the victory window
	public void setLevelText(string a){
		levelText.text = a;
	}
	public void setScoreText(string a){
		scoreText.text = a;
	}
	public void setTimeText(string a){
		timeText.text = a;
	}

	// Buttons
	public void NextButtonClicked(){
		gameController.incrCounter ();
		Application.LoadLevel (Application.loadedLevel);
	}
	public void RestartButtonClicked(){
		Application.LoadLevel (Application.loadedLevel);
	}
	public void QuitButtonClicked(){
		Application.LoadLevel (1);
	}

}
