using UnityEngine;
using System.Collections;

public class PlayerCollides : MonoBehaviour {

	public GameObject explosion;

	private GameController gameController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject !=  null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null) {
			Debug.Log("Cannot find GameController script.");
		}
	}	

	void OnCollisionEnter2D(Collision2D collision) {
		//Handheld.Vibrate ();
		Instantiate (explosion, transform.position, transform.rotation);
		gameController.GameOver ();
		Destroy (gameObject);
	}
}
