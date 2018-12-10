using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    #region Fields
    string parentPrefabName = "";
    bool itemSpawned = false;

    [SerializeField]
    GameObject statisGrenadePrefab;
    [SerializeField]
    GameObject tripwirePrefab;
    [SerializeField]
    GameObject pingDevicePrefab;
    #endregion

    #region Methods
    /// <summary>
    /// Instantiates an object in the position of the empty itemDrop prefab.
    /// Spawns according to rules.
    /// </summary>
    void InstantiateObject () {
        // Act 2-1: narrow hallway
        // Act 2-2: Space pirates with ambush position over player
        // Act 2-3: firefight in open area
        // Act 2-4: Ourterwall Explosion
        // Act 3-1: outerwall explosion
        // Act 3-2: Foritfied no mans
        // Act 3-3: ambush position over player
        // Act 3-4: Fortified no mans
        // Act 4-1: fire fight in open area
        // Act 4-2: clumped explosions
        // Act 4-3: narrow hallay
        // Act 4-4: explsion cluster

        // Get name of current parent prefab
        itemSpawned = false;
        parentPrefabName = transform.parent.name;
        
        // if the act 1 prefab exists, make a ping device spawn in the location of the object.
        if (parentPrefabName == "act1(Clone)" || parentPrefabName == "act5(Clone)" && !itemSpawned)
        {
            //if(GameObject.Find("act2-2(Clone)") != null || GameObject.Find("act3-3(Clone") && !itemSpawned)
            //{
            //    Instantiate(pingDevicePrefab, transform.position, Quaternion.identity);
            //    itemSpawned = true;
            //}

            int rand = (int)Random.Range(0f, 3f);

            if (rand == 0)
                Instantiate(pingDevicePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            if (rand == 1)
                Instantiate(tripwirePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            if (rand == 2)
                Instantiate(statisGrenadePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            itemSpawned = true;
        }


        
        if (parentPrefabName == "act2-1(Clone)" || parentPrefabName == "act2-2(Clone)" || parentPrefabName == "act2-3(Clone)"
            || parentPrefabName == "act2-4(Clone)")
        {
            if (GameObject.Find("act4-1(Clone)") != null && !itemSpawned && GameObject.Find("Stasis Grenade Material(Clone)") == null)
            {
                Instantiate(statisGrenadePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                itemSpawned = true;
                //Debug.Log("item spawned");

            }
            if (GameObject.Find("act4-2(Clone)") != null || GameObject.Find("act4-4") != null && !itemSpawned)
            {

                if (GameObject.Find("Ping Materials(Clone)") == null)
                {
                    //Debug.Log("item spawned (ping didn't exist)");
                    Instantiate(pingDevicePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    itemSpawned = true;
                }
                else if (GameObject.Find("TripWireMaterials(Clone)") == null)
                {
                    //Debug.Log("item spawned (tripwire didn't exist)");
                    Instantiate(tripwirePrefab, new Vector2(transform.position.x , transform.position.y), Quaternion.identity);
                    itemSpawned = true;
                }

            }
            else
            {
                int rand = (int)Random.Range(0f, 3f);

                if (rand == 0)
                    Instantiate(pingDevicePrefab, new Vector2(transform.position.x + 1, transform.position.y + 1), Quaternion.identity);
                if (rand == 1)
                    Instantiate(tripwirePrefab, new Vector2(transform.position.x + 1, transform.position.y + 1), Quaternion.identity);
                if (rand == 2)
                    Instantiate(statisGrenadePrefab, new Vector2(transform.position.x + 1, transform.position.y + 1), Quaternion.identity);


                //Debug.Log("item spawned random" + rand);

                itemSpawned = true;
            }

        }
        
    }

    // Update is called once per frame
    void Update () {
        InstantiateObject();

        // If the item is spawned, delete the empty game object.
        if (itemSpawned)
            Destroy(gameObject);
	}

    #endregion
}
