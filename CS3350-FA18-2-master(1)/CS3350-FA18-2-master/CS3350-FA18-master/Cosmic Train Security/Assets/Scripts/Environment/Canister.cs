using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Canister : MonoBehaviour
{

    #region Fields

    public float explosionRadius = 5;
    private bool bExploding;    // Boolean to check if the canister is exploding
    Animator ani;// get the animator 
    bool explosionFinished=false;//a bool to check if the explosion is finished
    float ExplosionTime = .1f;
    CanisterProjectileCollisionEvent collisionEvent;
    DestroyWallEvent wallDestructionEvent;
    [SerializeField]
    GameObject prefabCanisterExplosion;
    CircleCollider2D explosionCollider;    // Collider for the explosion
    Collider2D[] canisterInRadius;
    CanisterExplosionEvent canisterExplosionEvent;

    float rayLength = 1f; // length of raycast when checking for walls
    float WallDistance = 5f; // this is max distance of a wall that's affected by exploding canister

    float gridLeftEdge = -38f;
    float gridRightEdge = 40f;
    float gridTopEdge = 20f;
    float gridBottomEdge = -20f;

    #endregion

    #region methods
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
        //check if the animation is finished 
        ani = GetComponent<Animator>();

        // Add a listener to explode the canister   
        collisionEvent = new CanisterProjectileCollisionEvent();
        EventManager.AddCanisterProjectileCollisionInvoker(this);

        // Add an invoker to trigger the wall destruction event
        wallDestructionEvent = new DestroyWallEvent();
        EventManager.AddWallDestructionInvokers(this);

        // Add Canister Explosion event
        canisterExplosionEvent = new CanisterExplosionEvent();
        EventManager.AddCanisterExplosionInvoker(this);
        EventManager.AddCanisterExplosionListener(SecondaryExplosion);

    }

    /// <summary>
    /// Add a listener to the canisterExplosionEvent invoked by this script
    /// </summary>
    /// <param name="call"></param>
    public void AddCanisterExplosionListener(UnityAction<Vector3> call)
    {
        canisterExplosionEvent.AddListener(call);
    }
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update ()
    {
        //destroy this object after the animation finishes 
        if (bExploding)
        {
            ExplosionTime -=Time.deltaTime;
        }
        if (ExplosionTime < 0) //destroy the game object after one second
        {
            explosionFinished = true;
            Destroy(gameObject);
        }
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Bullet") || (collision.gameObject.tag == "EnemyBullet"))
        {
            collisionEvent.Invoke(transform.position);
            Explode();
        }
    }

    /// <summary>
    /// Function to explode the canister
    /// </summary>
    private void Explode()
    {
        // Get wall location
        FindNearestWallTile();

        //trigger the explosion animation
        GameObject.Instantiate(prefabCanisterExplosion, transform.position, Quaternion.identity);
        AudioManager.Instance.Play(AudioClipName.canister_Explosion);//play the audio
        bExploding = true; // Set exploding status to true
        canisterExplosionEvent.Invoke(transform.position);
    }


  
    /// <summary>
    /// Adds the given listener for the CanisterProjectileCollisionEvent
    /// </summary>
    /// <param name="listener"></param>
    public void AddCanisterProjectileCollisionListener(UnityAction<Vector2> listener)
    {
        collisionEvent.AddListener(listener);
    }

    public void AddWallDestructionListener(UnityAction<Vector3> listener)
    {
        wallDestructionEvent.AddListener(listener);
    }


    /// <summary>
    /// This method will destroy the nearest wall tile by casting rays in the 4 cardinal directions (total of 4 rays).
    /// </summary>
    /// <returns></returns>
    void FindNearestWallTile()
    {
        bool isDamagingWall = false; // let's only allow one wall to be damaged per canister explosion

        // shoot a ray to the left of this canister
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("ActSkeleton"));
        if (hit)
        {
            isDamagingWall = true;
            Vector2 newPoint = hit.point;
            newPoint.x -= 1; // offset to match the grid
            wallDestructionEvent.Invoke(newPoint);

        }

        // If this canister is not yet destroying a wall, then shoot a ray to the right of this canister
        if (!isDamagingWall)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right, rayLength, LayerMask.GetMask("ActSkeleton"));
            if (hit)
            {
                isDamagingWall = true;
                Vector2 newPoint = hit.point;
                newPoint.x += 1; // offset to match the grid
                wallDestructionEvent.Invoke(newPoint);
            }
        }

        // If this canister is not yet destroying a wall, then shoot a ray upward from this canister
        if (!isDamagingWall)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, rayLength, LayerMask.GetMask("ActSkeleton"));
            if (hit)
            {
                isDamagingWall = true;
                Vector2 newPoint = hit.point;
                newPoint.y += 1; // offset to match the grid
                wallDestructionEvent.Invoke(newPoint);
            }
        }

        // If this canister is not yet destroying a wall, then shoot a ray downward from this canister
        if (!isDamagingWall)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("ActSkeleton"));
            if (hit)
            {
                Vector2 newPoint = hit.point;
                newPoint.y -= 1; // offset to match the grid
                wallDestructionEvent.Invoke(newPoint);
            }
        }
    }

    void SecondaryExplosion(Vector3 explosionCenter)
    {
        // If a canister that isn't exploding is within range of an explosion...
        if (!bExploding && (Vector3.Distance(explosionCenter, transform.position) <= explosionRadius))
        {
            Explode();
        }
    }
    #endregion
}
