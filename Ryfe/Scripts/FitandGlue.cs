// Script to handle collision of stones and corresponding glue
using UnityEngine;
using System.Collections;

public class FitandGlue : MonoBehaviour {

	private GameController gameController;
	private SoundController soundController;
	private PhysicsController physicsController;

	private bool glue;
	private Vector3 stand_point;

	// number of stones in one direction
	public int numb = 8;

	// group of fixed rotating stones
	GameObject stones_group; 

	GameObject start_stone;

	// Use this for initialization
	void Start () {
		// find game- and soundcontroller
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		else {
			Debug.Log("Cannot find 'GameController' script");
		}
		GameObject soundControllerObject = GameObject.FindWithTag("SoundController");
		if (soundControllerObject != null) {
			soundController = soundControllerObject.GetComponent<SoundController> ();
		} 
		else {
			Debug.Log("Cannot find 'SoundController' script");
		}
		// get physics controller
		GameObject physicsControllerObject = GameObject.FindWithTag("PhysicsController");
		if (physicsControllerObject != null) {
			physicsController = physicsControllerObject.GetComponent<PhysicsController> ();
		} 
		else {
			Debug.Log("Cannot find 'PhysicsController' script");
		}

		stones_group = GameObject.FindGameObjectWithTag ("Stones");

		start_stone = GameObject.FindGameObjectWithTag ("StartStone");
	}

	void Update () {
		// If Game is not finished
		if (!gameController.getRestart () && !gameController.getBarFull()) {
			if (this.gameObject.GetComponent<fixClass> ().fix == false) {
				// Check position of left click
				if (Input.GetMouseButtonDown (0)) {
					GameObject stone = this.gameObject;
					colors stone_colors = stone.GetComponent<colors> ();

					// scoreValue for this stone - depends on stone color type
					int scoreValue = stone.GetComponent<StoneScript> ().getScore ();

					// get click position
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

					// Substract rotation from start point
					float angle = start_stone.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

					// rotation from (0.5,0.5,0) with angle from rotation above
					float x = (ray.origin.x - 0.5f) * Mathf.Cos (angle) + (ray.origin.y - 0.5f) * Mathf.Sin (angle) + 0.5f;
					float y = (-1) * (ray.origin.x - 0.5f) * Mathf.Sin (angle) + (ray.origin.y - 0.5f) * Mathf.Cos (angle) + 0.5f;

					// search for grid position
					int pos_x = Mathf.FloorToInt (x) + numb;
					int pos_y = Mathf.FloorToInt (y) + numb;
				
					// test if positions valid, if not exit function
					if(pos_x < 0 || pos_x > gameController.getGridDimension() || pos_y < 0 || pos_y > gameController.getGridDimension())
					{
						return;
					}
					// fix stone just if the entry in the grid is not occupied
					if (gameController.checkGridEntry (pos_x, pos_y)) {
						// check for correct colors
						if (gameController.checkColors (stone_colors, pos_x, pos_y)) {

							// change stone tag
							stone.tag = "InputStones";

							// get correct distance of midpoints of cubes
							float x_proj = pos_x - numb + 0.5f;
							float y_proj = pos_y - numb + 0.5f;

							// assign new position to stone
							stone.GetComponent<Transform> ().position = new Vector3 ((x_proj - 0.5f) * Mathf.Cos (angle) + (-1) * (y_proj - 0.5f) * Mathf.Sin (angle) + 0.5f, (x_proj - 0.5f) * Mathf.Sin (angle) + (y_proj - 0.5f) * Mathf.Cos (angle) + 0.5f, 0.0f);

							// fix stone
							stone.GetComponent<fixClass> ().fix = true;

							// correct for colors in reference grid
							float rotation_angle = stones_group.transform.rotation.eulerAngles.z;
							int div = (int)System.Math.Round (rotation_angle / 90.0f, 0);

							// shift angle about 45 degree for physics check
							float shift_angle = rotation_angle + 45.0f < 360.0f ? rotation_angle + 45.0f : (rotation_angle + 45.0f - 360.0f);
							int div_shift = (int)System.Math.Floor (shift_angle / 90.0f);

							// Have to change div_shift due to changing gravity in effect
							div_shift -= physicsController.getGravity ();
							div_shift += 4;
							div_shift %= 4;
							// check for physics
							if (physicsController.checkPhysics (stone, pos_x, pos_y, div_shift)) {

								// sound
								soundController.PlayStoneFit();

								gameController.setGridEntry (pos_x, pos_y, stone_colors.color [(0 + div) % 4], stone_colors.color [(1 + div) % 4], stone_colors.color [(2 + div) % 4], stone_colors.color [(3 + div) % 4]);

								// save stone position
								Vector3 pos_this = stone.GetComponent<Transform> ().position;
								gameController.setStonePosition (pos_this);

								// save coordinate
								stone.GetComponent<gridCoord> ().grid_coord [0] = pos_x;
								stone.GetComponent<gridCoord> ().grid_coord [1] = pos_y;

								// Update score
								gameController.AddScore (scoreValue);

								float temp_angle = (float)((int)stones_group.transform.rotation.eulerAngles.z) % 90;
								float correct_angle = temp_angle > 45.0f ? temp_angle - 90.0f : temp_angle; 
						
								// Relate stone to the Stones group
								stone.transform.Rotate (new Vector3 (0.0f, 0.0f, correct_angle));//(-1)*stone.transform.rotation.eulerAngles.z));
								stone.transform.parent = stones_group.transform;
							}
							gameController.new_stone (stone);
						} else {
							// sound for not fitting colors
							soundController.PlayStoneNotFit();
						}
					}
				}
			}
		}
	}
}
