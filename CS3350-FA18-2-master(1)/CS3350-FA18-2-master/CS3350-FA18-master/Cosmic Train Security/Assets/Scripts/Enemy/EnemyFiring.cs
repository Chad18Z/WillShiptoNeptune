using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFiring : MonoBehaviour {

    #region Fields
    public GameObject prefabBullet;             // Bullet that is fired from Enemy gun
    float shootCoolDown = 0.33f;                  // Cooldown timer for shooting 
    bool canFire = true;
    public const float MAX_WEAPON_FIRING_RATE = 10f;
    #endregion

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void tryShooting()
    {
        // if the enemy is alive and can shoot
        if (canFire)
        {
            // stop the enemy from shooting 
            // and set a cool down timer
            //shootCoolDown = (int)MAX_WEAPON_FIRING_RATE;

            // shoot at the player
            GameObject bulletInstance = Instantiate(prefabBullet, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            bulletInstance.GetComponent<EnemyBulletScript>().BulletSpeed = 3f;
            AudioManager.Instance.Play(AudioClipName.Fire);
            canFire = false;
        }
        else
        {
            shootCoolDown -= Time.deltaTime;
            Debug.Log(shootCoolDown);

            if (shootCoolDown <= 0)
            {
                canFire = true;
                shootCoolDown = 0.33f;
            }
        }
    }
}
