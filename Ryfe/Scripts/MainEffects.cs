using UnityEngine;
using System.Collections;

public class MainEffects : MonoBehaviour
{
	private GameController gameController;
	private SoundController soundController;
	private PhysicsController physicsController;

	// fire effect
	public GameObject firewall;

	// indicate on going animation
	private bool go_on = true;
	private bool fire_sound = true;

	// Use this for initialization
	void Start ()
	{
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
		firewall.GetComponent<Transform> ().localScale = new Vector3 (0.0f, 0.0f, 1.0f);
		firewall.GetComponent<ParticleSystem> ().gravityModifier = -0.5f;
		firewall.GetComponent<ParticleSystem> ().loop = true;
	}

	IEnumerator AnimateFireRing(){
		while (true) {
			// just loop as long winning condition fulfilled
			if(!gameController.getFireStart() || gameController.getRestart() || gameController.getWinStone ().tag == "DeleteStones"){
				break;
			}
			yield return new WaitForSeconds (1.0f/(physicsController.getRotationSpeed()*10.0f));
			// spawn new firewall
			Quaternion spawnRotation = Quaternion.identity;
			if(gameController.getWinStone() != null){
				// spawn position has to be more exact
				Instantiate (firewall, gameController.getWinStone().GetComponent<Transform>().position + new Vector3(0.0f,0.0f,-0.2f), spawnRotation);

				// Start fire Sound if not already done
				if(fire_sound){
					soundController.FireSoundOn();
					fire_sound = false;
				}
			}
		}
		go_on = true;
		fire_sound = true;
	}

	// Update is called once per frame
	void Update ()
	{
		// check for winning condition
		if (gameController.getFireStart () && go_on) {
			go_on = false;
			// Start fire effect
			StartCoroutine(AnimateFireRing());
		}
		// in case of bad win attempt, end fires
		if (gameController.getWinStone () != null) {
			if (gameController.getWinStone ().tag == "DeleteStones") {
				GameObject[] fire_objects = GameObject.FindGameObjectsWithTag ("Fire");
				// Deactivate fire
				soundController.FireSoundOff();
				foreach (GameObject fires in fire_objects) {
					fires.GetComponent<ParticleSystem> ().loop = false;
					Destroy (fires,15.0f);
				}
			}
		}
	}

	// Instantiate Fireobjects in an enumeration

}

