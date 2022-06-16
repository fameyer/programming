// Script to define stone behaviour
using UnityEngine;
using System.Collections;

[System.Serializable]
// colors of the stone
public class colors : MonoBehaviour{
	// colors in each direction
	public int[] color = new int[4];
}

public class fixClass : MonoBehaviour{
	// state of mobility of the stone
	public bool fix = false;
}

public class gridCoord : MonoBehaviour{
	// coordinates in grid
	public int[] grid_coord = new int[2];
}

public class StoneScript : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 pos;
	private Transform trans;

	public float speed;

	// colors on each edge
	private colors myColors;

	// colors to be chosen
	public int left, down, right, up;

	// Fixed or moving?
	public fixClass myFix;

	// Coordinates in grid
	public gridCoord myCoord;

	// individual scorevalue of this stone
	public int score = 10;

	// Use this for initialization
	void Start () {
		// 0 for yellow, 1 for red
		myColors = this.gameObject.AddComponent<colors>();
		myColors.color[0] = left; 	// left
		myColors.color[1] = down; 	// down
		myColors.color[2] = right; 	// right
		myColors.color[3] = up; 	// up
		myFix = this.gameObject.AddComponent<fixClass> ();
		myCoord = this.gameObject.AddComponent<gridCoord> ();
	}

	public int getScore(){
		return score;
	}

	// Update is called once per frame
	void Update () {
		if (!myFix.fix) {

			// When player presses space, the stone rotates
			if (Input.GetMouseButtonDown (1)) {
				trans = GetComponent<Transform> ();
				trans.Rotate (new Vector3 (0.0f, 0.0f, 90.0f));
				int[] d = {myColors.color[0], myColors.color[1], myColors.color[2], myColors.color[3]};
				myColors.color[0] = d[3];
				myColors.color[1] = d[0];
				myColors.color[2] = d[1];
				myColors.color[3] = d[2];
			}
		} 
	}

}
