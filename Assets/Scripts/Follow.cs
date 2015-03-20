using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;
	public float distance;

	void Update () {
		if (target != null) {
			transform.position = new Vector3 (target.position.x, target.position.y + distance, transform.position.z);
		}
	}
}
