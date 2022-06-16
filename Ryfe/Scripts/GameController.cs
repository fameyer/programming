using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	private PhysicsController physicsController;

	// Position vector to save position of last fixed stone to check winning condition
	private Vector3 stone_position;

	// Matrix to compute physical conditions
	private const int dim = 17;
	
	// grid initialized with 0 values - not set
	private int[,,] grid = new int[dim,dim,5];  

	// save last updated grid coordinates
	int[] last_coord = new int[2];
	
	public GameObject stone1, stone2, stone3, stone4, stone5, stone6, stone7;
	private GameObject[] stones = new GameObject[7];

	// array to store stones: 1 the player controls, 2 to come
	private int[] preview_stones = new int[3];

	// preview stone objects
	public GameObject previewStone1;
	public GameObject previewStone2;

	// bool for end of level in case of victory (restart) 
	private bool restart = false;
	private bool do_again = true;
	private bool start_fire = false;
	
	// bool for indication of full phase bar - sign for defeat
	private bool bar_full = false;

	// phase bar object
	public GameObject phase_bar;

	// score
	private int score;

	// temporary div
	private int div_temp = 0;

	// phase and duration of a phase 
	private int phase;
	public float phase_time;

	// group of fixed rotating stones
	GameObject stones_group; 

	// track winning condition
	GameObject win_stone;
	bool winning_condition = false;
	Vector3 win_position;
	Vector3 win_position_fix;

	// Victory/Defeat Screens
	public GameObject VictoryScreenNext;
	public GameObject VictoryScreen;
	public GameObject DefeatScreen;

	// texts
	public Text scoreText;
	public Text levelText;

	// time tracking
	private float start_time = 0;
	private float end_time = 0;

	// sound controller
	public GameObject SoundController;
	
	// Use this for initialization
	void Start () {
		// get physics controller
		GameObject physicsControllerObject = GameObject.FindWithTag("PhysicsController");
		if (physicsControllerObject != null) {
			physicsController = physicsControllerObject.GetComponent<PhysicsController> ();
		} 
		else {
			Debug.Log("Cannot find 'PhysicsController' script");
		}

		phase_time = Data.getPhaseTime ();

		// Modify level text
		levelText.text = "Level: "+(Data.getCounter()+1);

		// instantiate phase bar
		Instantiate (phase_bar);

		score = 0;
		stone_position = new Vector3 (0.0f, 0.0f, 0.0f);

		stones_group = GameObject.FindGameObjectWithTag ("Stones");

		// group stones in array
		stones [0] = stone1;
		stones [1] = stone2;
		stones [2] = stone3;
		stones [3] = stone4;
		stones [4] = stone5;
		stones [5] = stone6;
		stones [6] = stone7;

		// generate initial stone selection
		for (int i = 0; i <= 2; ++i) {
			preview_stones [i] = Random.Range (0, 7);
		}

		// initial phase
		phase = 0;

		// Gravity Changing and Timer
		StartCoroutine( PhaseChanger());

		// save start time
		start_time = Time.time;

		new_stone ();
	}	

	public int get_div_temp(){
		return div_temp;
	}
	public void set_div_temp(int value){
		div_temp = value;
	}

	// update score
	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore()
	{
		scoreText.text = "Score: " + score;
	}

	public int getGridDimension(){
		return dim;
	}

	public int[,,] getGrid(){
		return grid;
	}

	public int Grid(int i, int j, int k){
		return grid [i, j, k];
	}

	// Update counter
	public void incrCounter(){
		Data.incrCounter ();
	}

	public GameObject getStone(int i){
		return stones [i];
	}

	// set entry in grid
	public void setGridEntry(int i, int j, int left, int down, int right, int up){
		// check for invalid coordinates
		if (i > dim - 1 || j > dim - 1)
			throw new System.IndexOutOfRangeException();
		grid [i, j, 0] = 1;
		grid [i, j, 1] = left;
		grid [i, j, 2] = down;
		grid [i, j, 3] = right;
		grid [i, j, 4] = up;
		last_coord [0] = i;
		last_coord [1] = j;
	}
	// check if grid entry occupied, returns true if not so
	public bool checkGridEntry(int i, int j){
		if (grid [i, j, 0] == 0) {
			return true;
		}
		else{
			return false;
		}
	}

	// save position of last saved stone to check winning condition
	public void setStonePosition(Vector3 a){
		stone_position = a;
	}

	// In case set winning condition to true after some time
	IEnumerator WaitForWinning(){
		yield return new WaitForSeconds (2.0f);
		winning_condition = true;
	}

	// stones preview and instantiation
	public void new_stone(GameObject stone = null){
		// Check if winning distance is reached
		if (Vector3.Distance (stone_position, new Vector3 (0.5f, 0.5f, 0.0f)) >= 7.8f && !winning_condition ) {
			win_stone = stone;
			// enable winning_condition after some time
			winning_condition = false;
			// start fire effects
			start_fire = true;
			StartCoroutine(WaitForWinning());
			win_position_fix = stone.GetComponent<Transform>().position;
		}
		
		Quaternion spawnRotation = Quaternion.identity;

		// Instantiate and preview stones
		previewStone2.GetComponent<SpriteRenderer> ().sprite = stones [preview_stones [2]].GetComponent<SpriteRenderer> ().sprite;
		previewStone1.GetComponent<SpriteRenderer> ().sprite = stones [preview_stones [1]].GetComponent<SpriteRenderer> ().sprite;
		Instantiate (stones[preview_stones[0]], new Vector3 (-10.5f, 0.5f, 0.0f), spawnRotation);

		// New stone is chosen by random and preview_stones is updated
		preview_stones [0] = preview_stones [1];
		preview_stones [1] = preview_stones [2];
		preview_stones [2] = Random.Range (0, 7);
	}

	IEnumerator PhaseChanger(){
		while (true) {
			if(restart){
				break;
			}
			//phaseText.text = (phase + 1).ToString ();
			yield return new WaitForSeconds (phase_time);
			// change phase, if not final phase reached
			phase = phase < 2 ? phase + 1 : phase;
		}
	}

	public int getPhase(){
		return phase;
	}

	public float getPhaseTime(){
		return phase_time;
	}

	public void setBarFull(){
		bar_full = true;
	}

	public bool getBarFull(){
		return bar_full;
	}

	public bool getWinningCondition(){
		return winning_condition;
	}
	public bool getFireStart(){
		return start_fire;
	}

	// get win_stone (for maineffects.cs)
	public GameObject getWinStone(){
		if (win_stone != null) {
			return win_stone;
		} else {
			return null;
		}
	}

	public bool getRestart(){
		return restart;
	}


	public bool checkColors(colors stone_colors, int coord_x, int coord_y){
		
		// get recent rotation to adjust color fitting accordingly
		float rotation_angle = stones_group.transform.rotation.eulerAngles.z;
		int div = (int)System.Math.Round(rotation_angle / 90.0f,0);
		
		// test sides of fixed stone
		if( coord_x-1 >= 0){
			if(grid[coord_x-1, coord_y, 0] == 1 && grid[coord_x-1, coord_y, 3] != stone_colors.color[(0+div)%4])
				return false;
		}
		// test up and down
		if(coord_y-1 >= 0){
			if(grid[coord_x, coord_y-1, 0] == 1 && grid[coord_x, coord_y-1, 4] != stone_colors.color[(1+div)%4])
				return false;
		}
		if(coord_y+1 < dim){
			if(grid[coord_x, coord_y+1, 0] == 1 && grid[coord_x, coord_y+1, 2] != stone_colors.color[(3+div)%4])
				return false;
		}
		// test right
		if( coord_x+1 < dim){
			if(grid[coord_x+1, coord_y, 0] == 1 && grid[coord_x+1, coord_y, 1] != stone_colors.color[(2+div)%4])
				return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () {
		// rotate all game objects
		stones_group.GetComponent<Rigidbody2D> ().rotation += physicsController.getRotationSpeed ();

		float rotation_angle = stones_group.transform.rotation.eulerAngles.z;
		
		// shift angle about 45 degree for physics check
		//float shift_angle =  rotation_angle-35.0f < 360.0f ? rotation_angle - 35.0f : -35.0f-(360.0f-rotation_angle);	// to investigate
		float shift_angle = rotation_angle+45.0f < 360.0f ? rotation_angle+45.0f  : (rotation_angle+45.0f-360.0f);
		int div_shift = (int)System.Math.Floor(shift_angle  / 90.0f);
		div_shift %= 4; // sometimes div_shift turns 4

		// Have to change div_shift due to changing gravity in effect
		div_shift -= physicsController.getGravity();
		div_shift += 4;
		div_shift %= 4;

		// check physics of all existing stones
		GameObject[] stone_objects = GameObject.FindGameObjectsWithTag ("InputStones");
		if (!restart && !bar_full ) {
			foreach (GameObject single_stone in stone_objects) {
				physicsController.checkPhysics (single_stone, single_stone.GetComponent<gridCoord> ().grid_coord [0], single_stone.GetComponent<gridCoord> ().grid_coord [1], div_shift);	
			}
		}

		// check winning condition (needed for correct rotation state)
		if(win_stone != null && !restart){
			if (winning_condition && win_stone.tag != "DeleteStones") {
				print ("win?");
				win_position = win_stone.GetComponent<Transform> ().position;
				if (Vector3.Distance(win_position, win_position_fix) < 0.05f) {
					restart = true;
					end_time = Time.time;
				}
			}
			else{
				winning_condition = false;
				if(win_stone.tag == "DeleteStones"){
					start_fire = false;
				}
			}
		}

		// End of level screens
		if (bar_full && do_again) {
			// end time measurement
			end_time = Time.time;
			// Activate DefeatScreen
			DefeatScreen.SetActive(true);
			DefeatScreen.GetComponent<ScreenDisplay> ().setLevelText("Level: \t"+(Data.getCounter()+1));
			DefeatScreen.GetComponent<ScreenDisplay> ().setScoreText("Score: \t"+score.ToString());
			DefeatScreen.GetComponent<ScreenDisplay> ().setTimeText("Time:  \t"+System.Math.Round(end_time-start_time,2).ToString());

			do_again = false;

		} else {
			// Player decides at the end of level what to do
			if (restart && do_again) {
				// Activate VictoryScreen
				if(Data.getMode() && Data.getCounter () < Data.getFinalLevel ()){
					VictoryScreenNext.SetActive(true);
					VictoryScreenNext.GetComponent<ScreenDisplay> ().setLevelText("Level: \t"+(Data.getCounter()+1));
					VictoryScreenNext.GetComponent<ScreenDisplay> ().setScoreText("Score: \t"+score.ToString());
					VictoryScreenNext.GetComponent<ScreenDisplay> ().setTimeText("Time:  \t"+System.Math.Round(end_time-start_time,2).ToString());
				}
				else{
					VictoryScreen.SetActive(true);
					VictoryScreen.GetComponent<ScreenDisplay> ().setLevelText("Level: \t"+(Data.getCounter()+1));
					VictoryScreen.GetComponent<ScreenDisplay> ().setScoreText("Score: \t"+score.ToString());
					VictoryScreen.GetComponent<ScreenDisplay> ().setTimeText("Time:  \t"+System.Math.Round(end_time-start_time,2).ToString());
				}
				// save levelcounter and related score
				SaveLoad.Save (Data.getCounter (), score, System.Math.Round(end_time-start_time,2), Data.getName());

				do_again = false;
			}
		}
		// restart and quit keys
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (Application.loadedLevel);
		}
		// Quit
		if (Input.GetKeyDown (KeyCode.Q) || Input.GetKeyDown (KeyCode.Escape)) {
			// Restart data object and reload menu
			Data.restart();
			Application.LoadLevel (1);
		}
		if (restart) {
			if (Input.GetKeyDown (KeyCode.N) && Data.getMode () && Data.getCounter () < Data.getFinalLevel ()) {
				Data.incrCounter ();
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}