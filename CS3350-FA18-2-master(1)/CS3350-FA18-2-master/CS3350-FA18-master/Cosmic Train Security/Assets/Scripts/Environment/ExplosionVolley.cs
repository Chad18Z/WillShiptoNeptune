using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVolley : MonoBehaviour
{
    #region Fields
    public float radius = 3;    // radius for explosion animation spawning
    public float lifeTimeLimit = 3;  // life time of explosion volley
    public float spawnRate = 0.5f;   // rate at which explosions spawn
    public float scaleMin = 0.35f;  // minimum scale value of explosion

    private float lifeTime;     // time that the volley has been active
    private float spawnTime;    // time of next explosion spawn
    private List<GameObject> explosions;    // list holding explosions created

    [SerializeField]
    GameObject explosionAnimation;     // explosion animation prefab

    #endregion

    #region Methods
    // Use this for initialization
    void Start ()
    {
        lifeTime = 0;
        spawnTime = spawnRate;
        explosions = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        lifeTime += Time.deltaTime;

        if (lifeTime >= spawnTime)
        {
            spawnTime += spawnRate;
            SpawnExplosive();
        }

        if (lifeTime > lifeTimeLimit)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnExplosive()
    {
        float randomScale = Random.Range(scaleMin, 1);
        Vector3 spawnLocation = new Vector3(gameObject.transform.position.x + Random.Range(-1 * radius, radius),
                gameObject.transform.position.y + Random.Range(-1 * radius, radius), 
                gameObject.transform.position.z);

        explosions.Add(Instantiate(explosionAnimation, spawnLocation, Quaternion.identity));
        explosions[explosions.Count - 1].transform.localScale = new Vector3(randomScale,
                randomScale, gameObject.transform.localScale.z);
        explosions[explosions.Count - 1].GetComponent<Animator>().SetBool("Exploded", true);
        Destroy(explosions[explosions.Count - 1], 1);
    }
    #endregion
}
