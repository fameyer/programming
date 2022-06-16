using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class HighScoreScreen : MonoBehaviour {

	// bool to indicate ordering in HighScore
	private bool score_order = true;

	// highscore texts
	public Text[] level = new Text[10];

	// Use this for initialization
	void Start () {
		// load data
		SaveLoad.Load ();

		// Display highscore
		display ();
	}

	// Sort and reset buttons
	public void ScoreSortButtonClicked(){
		score_order = true;
		display ();
	}
	public void TimeSortButtonClicked(){
		score_order = false;
		display ();
	}
	public void ResetHighScoreButtonClicked(){
		// delete Highscore file
		File.Delete(SaveLoad.file_path);
		display ();
	}
	// Show highscore
	public void display(){
		int score = 0;
		double time = 10000.0f;
		if (File.Exists (SaveLoad.file_path)) {
			//sort by score
			if (score_order) {
				for (int i = 0; i< level.Length; ++i) {
					score = 0;
					foreach (stage entry in SaveLoad.save) {
						if (entry.level == i) {
							if (entry.score > score) {
								score = entry.score;
								level [i].text = (i + 1).ToString () + "\n" + "\n" + entry.name + "\n" + "\n" + score.ToString () + "\n" + "\n" + entry.time.ToString ();
							}
						}
					}
				}
			} 
		// sort by time
		else {
				for (int i = 0; i< level.Length; ++i) {
					time = 10000.0f;
					foreach (stage entry in SaveLoad.save) {
						if (entry.level == i) {
							if (entry.time < time) {
								time = entry.time;
								level [i].text = (i + 1).ToString () + "\n" + "\n" + entry.name + "\n" + "\n" + entry.score.ToString () + "\n" + "\n" + entry.time.ToString ();
							}
						}
					}
				}
			}
		}
		// file does not exist
		else {
			for (int i = 0; i< level.Length; ++i) {
				level [i].text = "";
			}
		}
	}
}
