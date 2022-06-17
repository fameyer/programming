// Script to define StartStone behaviour
using UnityEngine;
using System.Collections;

[System.Serializable]

public class StartScript2Color : MonoBehaviour {

	private GameController gameController;
	public float speed;

	// colors on each edge
	private colors myColors;

	// Fixed or moving?
	public fixClass myFix;

	// Coordinates in grid
	public gridCoord myCoord;
	
	// Use this for initialization
	void Start () {
		// get GameController
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		else {
			Debug.Log("Cannot find 'GameController' script");
		}

		// Get startstone style
		GameObject stone = gameController.getStone (Random.Range (0, 7));

		// set style
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = stone.GetComponent<SpriteRenderer> ().sprite;

		// 0 for yellow, 1 for red
		myColors = this.gameObject.AddComponent<colors>();
		myColors.color[0] = stone.GetComponent<StoneScript>().left; 	// left
		myColors.color[1] = stone.GetComponent<StoneScript>().down; 	// down
		myColors.color[2] = stone.GetComponent<StoneScript>().right; 	// right
		myColors.color[3] = stone.GetComponent<StoneScript>().up; 		// up
		myFix = this.gameObject.AddComponent<fixClass> ();
		myCoord = this.gameObject.AddComponent<gridCoord> ();
		myCoord.grid_coord = new int[] {10,10};

		// Set position and colors ({left, down, right, up]) in grid (is midpoint)
		gameController.setGridEntry (8, 8, myColors.color[0], myColors.color[1], myColors.color[2], myColors.color[3]);
	}

	// Update is called once per frame
	void Update (){ 
	}

}
