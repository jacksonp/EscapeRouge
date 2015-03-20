using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool simulateAccel;

	public float minVerticalSpeed;
  
  public float accelMultiplier;

	public float accelOffset = 0.5f;

	public GUIText debugText;
	
	void Start () {
		GetComponent<Rigidbody2D>().velocity = new Vector2 (0, minVerticalSpeed);
		//debugText.text = Input.acceleration.z + "";
		//accelOffset = Input.acceleration.z;
	}

	void FixedUpdate () {

		Vector2 dir = Vector2.zero;
		if (simulateAccel) {
      dir.x = Input.GetAxis ("Horizontal") * 1.5f;
      dir.y = Mathf.Max (minVerticalSpeed + Input.GetAxis ("Vertical") * 1.5f, minVerticalSpeed);
		} else {
			dir.x = Input.acceleration.x * accelMultiplier;
      dir.y = Mathf.Max (minVerticalSpeed, Mathf.Clamp(Input.acceleration.z + accelOffset, -0.5f, 0.0f) * accelMultiplier * -2);
		}

    transform.Translate (dir.x * Time.deltaTime, dir.y * Time.deltaTime, 0);

	}
}
