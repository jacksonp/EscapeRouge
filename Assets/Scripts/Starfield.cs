using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {

	private Transform tx;
	private ParticleSystem.Particle[] points;
	
	public int starsMax;
	public float starSize;
	
  public float xOffset;
  public float yOffset;
	
	void Start () {
    tx = transform;
    points = new ParticleSystem.Particle[starsMax];
    
    for (int i = 0; i < starsMax; i++) {
      points[i].position = new Vector3(Random.Range(-xOffset, xOffset), Random.Range(-yOffset, yOffset), 0) + tx.position;
      points[i].color = new Color(1, 1, 1, Random.Range(0.1f, 1.0f));
      points[i].size = Random.Range(starSize / 2, starSize);
    }
	}
	
	private void CreateStars() {
	}
	
	void Update () {
		
		for (int i = 0; i < starsMax; i++) {
			
      if ((tx.position.y - points[i].position.y > yOffset) || Mathf.Abs(tx.position.x - points[i].position.x) > xOffset) {
        points[i].position = new Vector3(Random.Range(-xOffset, xOffset), yOffset, 0) + tx.position;
        //points[i].position.Set(tx.position.x + Random.Range(-xOffset, xOffset), tx.position.y + yOffset, tx.position.z);
        //points[i].position.x = tx.position.x + Random.Range(-xOffset, xOffset);
        //points[i].position.y = tx.position.y + yOffset;
			}
			/*
			if ((points[i].position - tx.position).sqrMagnitude <= starClipDistanceSqr) {
				float percent = (points[i].position - tx.position).sqrMagnitude / starClipDistanceSqr;
				points[i].color = new Color(128,128,128, percent);
				points[i].size = percent * starSize;
			}
			*/
			
		}

		GetComponent<ParticleSystem>().SetParticles ( points, points.Length );
		
	}
}