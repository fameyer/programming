
// Script to define stone behaviour
using UnityEngine;
using System.Collections;

[System.Serializable]

public class bar : MonoBehaviour {

	private GameController gameController;

	// coloured stage objects
	public GameObject[] stages = new GameObject[12];

	private float phase_time = 0.0f;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		else {
			Debug.Log("Cannot find 'GameController' script");
		}
		// get phase time 
		phase_time = gameController.getPhaseTime ();

		// start bar
		StartCoroutine( fillBar());

	}

	IEnumerator fillBar(){
		int i = 0;
		while (true) {
			if(i == 12){
				// indicate full phase bar
				gameController.setBarFull();
				break;
			}
			yield return new WaitForSeconds (phase_time/4.0f);

			stages[i].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
			++i;

		}
	}
}
