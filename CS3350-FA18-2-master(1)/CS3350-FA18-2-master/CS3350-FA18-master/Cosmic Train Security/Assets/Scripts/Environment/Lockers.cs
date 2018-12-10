using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockers : MonoBehaviour
{
    GameObject ItemDrop;

    [SerializeField]
    GameObject Pistol;
    [SerializeField]
    GameObject Grenade;
    [SerializeField]
    GameObject Tripwire;
    [SerializeField]
    GameObject Ping;

    bool canDrop = true;

    // Update is called once per frame
    void Update()
    {
        int droppedItem = Random.Range(0, 5);

        switch (droppedItem)
        {
            case 1:
                ItemDrop = Pistol;
                break;
            case 2:
                ItemDrop = Grenade;
                break;
            case 3:
                ItemDrop = Tripwire;
                break;
            case 4:
                ItemDrop = Ping;
                break;
        }
    }

    /// <summary>
    /// This method handles collisions with objects
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D trig)
    {
        //Play wall impact sound on collission with wall.
        if (trig.gameObject.tag == "Player" && canDrop == true)
        {
            GameObject itemDropInstance = Instantiate(ItemDrop, transform.position + new Vector3(-.20f, 0, 0), transform.rotation);
            AudioManager.Instance.Play(AudioClipName.gun_Drop);
            canDrop = false;
        }
    }
}
