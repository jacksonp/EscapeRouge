using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour
{

    public float explodeWait;

    public GameObject explosion;

    public GameObject hazard;

    void Start()
    {
        StartCoroutine(ExplodeMe());
    }

    IEnumerator ExplodeMe()
    {
        yield return new WaitForSeconds(explodeWait);
        Vector3 ePosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.2f, 10));
        GameObject h = (GameObject)Instantiate(hazard, ePosition, transform.rotation);
        Instantiate(explosion, ePosition, transform.rotation);
        Destroy(gameObject);
        h.SetActive(true);
        h.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.2f);
    }

}
