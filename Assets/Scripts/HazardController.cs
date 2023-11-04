using UnityEngine;
using System.Collections;

public class HazardController : MonoBehaviour
{

    public float maxSpawnSpeed;

    public float minScale;

    public float maxScale;

    void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-maxSpawnSpeed, maxSpawnSpeed), Random.Range(-maxSpawnSpeed, maxSpawnSpeed));
        float scale = Random.Range(minScale, maxScale);
        GetComponent<Rigidbody2D>().mass *= 10 * scale;
        transform.localScale = new Vector3(scale, scale, 1);
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0, 120);
    }

}