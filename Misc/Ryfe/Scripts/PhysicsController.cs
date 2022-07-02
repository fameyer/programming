using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhysicsController : MonoBehaviour {

	private GameController gameController;

	// dimension of grid
	private int dim;  

	// counter for gravity change, 0 for down, 1 for right, 2 for up, 3 for left
	private int gravity = 0;
	
	public Text GravityCounterText;

	// arrow sprite to indicate gravity direction and arrow positions as well as rotations
	public GameObject gravityArrow;
	private Vector3[] gravity_pos_rot = new Vector3[8];
	
	// initial rotation speed and rate to increase
	public float rotation_speed = 0.1f;
	public float rotation_rate = 0.03f;

	// time to pass for gravity change
	public float waiting_time = 10.0f;

	// group of fixed rotating stones
	GameObject stones_group; 

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		else {
			Debug.Log("Cannot find 'GameController' script");
		}
		// get waiting time to change gravity
		waiting_time = Data.getWaitingTime();

		// get grid dimension
		dim = gameController.getGridDimension ();

		stones_group = GameObject.FindGameObjectWithTag ("Stones");

		// initialize gravityArrow data
		gravity_pos_rot [0] = new Vector3 (0.5f,-8.67f,0.0f); // down position
		gravity_pos_rot [1] = new Vector3 (0.0f,0.0f,0.0f); // down rotation
		gravity_pos_rot [2] = new Vector3 (9.65f,0.5f,0.0f); // right position
		gravity_pos_rot [3] = new Vector3 (0.0f,0.0f,90.0f); // right rotation
		gravity_pos_rot [4] = new Vector3 (0.5f,9.62f,0.0f); // up position
		gravity_pos_rot [5] = new Vector3 (0.0f,0.0f,180.0f); // up rotation
		gravity_pos_rot [6] = new Vector3 (-8.65f,0.5f,0.0f); // left position
		gravity_pos_rot [7] = new Vector3 (0.0f,0.0f,-90.0f); // left rotation

		// start enumerators
		StartCoroutine( GravityChangeTime());
		StartCoroutine( increaseRotation());
	}

	IEnumerator GravityChangeTime(){
		while (true) {
			if(gameController.getRestart()){
				break;
			}
			// waiting time
			GravityCounterText.text = waiting_time.ToString()+":00";
			// wait one millisecond
			float i = waiting_time;
			while (i >= 0) {
				GravityCounterText.text = ((int)i).ToString()+":"+(Mathf.Round ((i-(int)i)*100)).ToString();
				yield return new WaitForSeconds (0.001f);
				i-=waiting_time/(waiting_time*100.0f);
			}
			
			// (really) change gravity randomly
			int rand = 0;
			do{
				rand = Random.Range (0, 4);
			}while(rand == gravity);
			
			gravity = rand;
			gravityArrow.GetComponent<Transform> ().position = gravity_pos_rot [2 * gravity];
			gravityArrow.transform.Rotate((-1)*gravityArrow.transform.rotation.eulerAngles+gravity_pos_rot[2*gravity+1]);
			
			// change physical gravity overall
			switch(gravity){
			case 0:
				Physics2D.gravity = new Vector2(0.0f,-9.81f);
				Physics.gravity = new Vector3(0.0f,-9.81f,0.0f);
				break;
			case 1:
				Physics2D.gravity = new Vector2(9.81f,0.0f);
				Physics.gravity = new Vector3(9.81f,0.0f,0.0f);
				break;
			case 2:
				Physics2D.gravity = new Vector2(0.0f,9.81f);
				Physics.gravity = new Vector3(0.0f,9.81f,0.0f);
				break;
			case 3:
				Physics2D.gravity = new Vector2(-9.81f,0.0f);
				Physics.gravity = new Vector3(-9.81f,0.0f,0.0f);
				break;
			}
		}
	}
	
	public int getGravity(){
		return gravity;
	}

	IEnumerator increaseRotation(){
		while (true) {
			if(gameController.getRestart()){
				for(int i = 0; i<10; ++i) {
					yield return new WaitForSeconds (1.0f);
					rotation_speed += 1.0f;
				}
				break;
			}
			// 10 rounds of increasing speed
			yield return new WaitForSeconds (2*waiting_time);
			
			// if phase bar full, increase speed big time
			if(gameController.getBarFull()) rotation_speed += 0.3f;
			
			if(rotation_speed < 5.0f){
				rotation_speed += rotation_rate;
			}
		}
	}
	
	public float getRotationSpeed(){
		return rotation_speed;
	}

	// Check if stones are not singely connected in dependence of gravity given by rotation angle
	public bool checkPhysics(GameObject stone, int coord_x, int coord_y, int div){
		// test sides of fixed stone
		bool phy_left = false;
		bool phy_down = false;
		bool phy_right = false;
		bool phy_up = false;
		
		if (div == 0 || div == 2) {
			// left
			if(coord_x < 8){
				if (coord_x + 1 < dim && coord_y - 1 >= 0 && coord_y + 1 < dim) {
					if (gameController.Grid (coord_x + 1, coord_y, 0) == 1 && gameController.Grid (coord_x + 1, coord_y - 1, 0) + gameController.Grid (coord_x + 1, coord_y + 1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x , coord_y - 1, 0) + gameController.Grid (coord_x , coord_y + 1, 0) : 0) >= gameController.getPhase ()) {
						phy_left = true;
					}
				}
			}
			// down - can just be true for stacking on start stone
			if(coord_y < 8){
				if (div == 0) {
					if (coord_y + 1 < dim && coord_x - 1 >= 0 && coord_x + 1 < dim) {
						if (gameController.Grid (coord_x, coord_y + 1, 0) == 1 && gameController.Grid (coord_x - 1, coord_y + 1, 0) + gameController.Grid (coord_x + 1, coord_y + 1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x + 1 , coord_y, 0) + gameController.Grid (coord_x - 1, coord_y, 0) : 0) >= gameController.getPhase ()) {
							phy_down = true;
						}
					}
				}
				if (div == 2) { // Special case stacking on top
					if (coord_y + 1 < dim) {
						if (gameController.Grid (coord_x, coord_y + 1, 0) == 1) {
							phy_down = true;
						}
					}
					
				}
			}
			// right
			if(coord_x > 8){
				if (coord_x - 1 >= 0 && coord_y - 1 >= 0 && coord_y + 1 < dim) {
					if (gameController.Grid (coord_x - 1, coord_y, 0) == 1 && gameController.Grid (coord_x - 1, coord_y - 1, 0) + gameController.Grid (coord_x - 1, coord_y + 1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x , coord_y - 1, 0)+ gameController.Grid (coord_x , coord_y + 1, 0) : 0) >= gameController.getPhase ()) {
						phy_right = true;
					}
				}
			}
			// up
			if(coord_y > 8){
				if (div == 0) { // Special case stacking on top
					if (coord_y - 1 >= 0) {
						if (gameController.Grid (coord_x, coord_y - 1, 0) == 1) {
							phy_up = true;
						}
					}
				}
				if (div == 2) {
					if (coord_y - 1 >= 0 && coord_x - 1 >= 0 && coord_x + 1 < dim) {
						if (gameController.Grid (coord_x, coord_y - 1, 0) == 1 && gameController.Grid (coord_x - 1, coord_y - 1, 0) + gameController.Grid (coord_x + 1, coord_y - 1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x + 1 , coord_y, 0) + gameController.Grid (coord_x - 1 , coord_y, 0) : 0) >= gameController.getPhase ()) {
							phy_up = true;
						}
					}
				}
			}
		}
		if (div == 1 || div == 3) {
			// left
			if(coord_x < 8){
				if(div == 1){
					if(coord_x+1 < dim && coord_y-1 >= 0 && coord_y+1 < dim){
						if( gameController.Grid (coord_x+1, coord_y, 0) == 1 && gameController.Grid (coord_x+1, coord_y-1, 0) + gameController.Grid (coord_x+1, coord_y+1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x , coord_y - 1, 0) + gameController.Grid (coord_x , coord_y + 1, 0) : 0) >= gameController.getPhase ()){
							phy_left = true;
						}
					}
				}
				if(div == 3){ // Special case stacking on top
					if(coord_x + 1 < dim){
						if(gameController.Grid (coord_x+1,coord_y,0) == 1){
							phy_left = true;
						}
					}
				}
			}
			// down
			if(coord_y < 8){
				if(coord_y+1 < dim && coord_x-1 >= 0 && coord_x+1 < dim){
					if(gameController.Grid (coord_x, coord_y+1, 0) == 1 && gameController.Grid (coord_x-1, coord_y+1, 0) + gameController.Grid (coord_x+1, coord_y+1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x - 1 , coord_y, 0) + gameController.Grid (coord_x + 1 , coord_y, 0) : 0) >= gameController.getPhase ()){
						phy_down = true;
					}
				}
			}
			// right
			if(coord_x > 8){
				if(div == 1){ // Special case stacking on top
					if(coord_x-1 >= 0){
						if(gameController.Grid (coord_x-1,coord_y,0) == 1){
							phy_right = true;
						}
					}
				}
				if(div == 3){
					if(coord_x-1 >= 0 && coord_y-1 >= 0 && coord_y+1 < dim){
						if( gameController.Grid (coord_x-1, coord_y, 0) == 1 && gameController.Grid (coord_x-1, coord_y-1, 0) + gameController.Grid (coord_x-1, coord_y+1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x , coord_y - 1, 0) + gameController.Grid (coord_x , coord_y + 1, 0) : 0) >= gameController.getPhase ()){
							phy_right = true;
						}
					}
				}
			}
			// up
			if(coord_y > 8){
				if(coord_y-1 >= 0 && coord_x-1 >= 0 && coord_x+1 < dim){
					if( gameController.Grid (coord_x, coord_y-1, 0) == 1 && gameController.Grid (coord_x+1, coord_y-1, 0) + gameController.Grid (coord_x-1, coord_y-1, 0) + (gameController.getPhase() == 2 ? gameController.Grid (coord_x - 1, coord_y, 0) + gameController.Grid (coord_x + 1, coord_y, 0) : 0) >= gameController.getPhase () ){
						phy_up = true;
					}
				}
			}
		}
		// if stone is not fixed well enough drop it
		if ( !(phy_left || phy_down || phy_right || phy_up) ) {
			// Set grid entry empty
			gameController.getGrid()[coord_x, coord_y,0] = 0;
			
			// adjust rotation
			float temp_angle= (float)((int)stones_group.transform.rotation.eulerAngles.z)%90;
			float correct_angle = temp_angle > 45.0f ? temp_angle-90.0f: temp_angle; 
			stone.transform.Rotate(new Vector3(0.0f,0.0f, correct_angle));
			
			Vector2 movement;
			
			// Adjust drop vector
			int grav_x = 0;
			int grav_y = 0;
			
			switch(gravity){
				
			case 0:
				grav_x = 1;
				grav_y = 0;
				break;
			case 1:
				grav_x = 0;
				grav_y = 1;
				break;
			case 2:
				grav_x = 1;
				grav_y = 0;
				break;
			case 3:
				grav_x = 0;
				grav_y = 1;
				break;
			}
			// adjust drop vector
			if(stone.transform.position.x > 1.0f && (gravity == 0 || gravity == 2) || stone.transform.position.y >1.0f && (gravity == 1 || gravity == 3)){
				movement = new Vector2 (grav_x*0.1f, grav_y*0.1f);
			}
			else
			{
				if(stone.transform.position.x < 0.0f && (gravity == 0 || gravity == 2) || stone.transform.position.y < 0.0f && (gravity == 1 || gravity == 3)){
					movement = new Vector2 (grav_x*(-0.1f), grav_y*(-0.1f));
				}
				else{
					movement = new Vector2 (0.0f, 0.0f);
				}
			}
			// drop it
			stone.GetComponent<Rigidbody2D>().isKinematic = false;
			stone.GetComponent<Rigidbody2D>().AddForce(movement);
			stone.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
			stone.GetComponent<Rigidbody2D>().mass = 0.0f;
			
			if(stone.tag == "InputStones"){
				// substract score
				gameController.AddScore(-1*stone.GetComponent<StoneScript> ().getScore());
				// change stone tag to exclude void stones
				stone.tag = "DeleteStones";
			}
			
			Destroy (stone, 5.0f);
			return false;
		}
		return true;
	}
}
