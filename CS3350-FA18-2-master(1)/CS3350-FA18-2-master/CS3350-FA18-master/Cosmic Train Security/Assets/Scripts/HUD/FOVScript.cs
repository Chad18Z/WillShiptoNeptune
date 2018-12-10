using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FOVScript : MonoBehaviour {

	#region Fields
	[SerializeField]
	public float viewRadius = 5; // View Radius
	[SerializeField]
	[Range(0,360)] // Clamp the angle to 360
	public float viewAngle = 60;
	Collider2D[] playerInRadius; // player in the radius of the fov
	public List<Transform> visiblePlayer = new List<Transform>();
	[SerializeField]
	public LayerMask obstacleMask, playerMask; // Get the obstacle layers
	FOVMesh mesh; // Mesh object
    //if the enemy is attacking the player
    public bool attack = false;
    bool playerInVision = false;
    EnemyFoundEvent enemyFoundEvent;
    #endregion

    #region Properties
    public bool PlayerInVision
    {
        get { return playerInVision; }
    }
    #endregion


    #region Methods


    /// <summary>
    /// Get the mesh in the parent.
    /// </summary>
    void Awake()
	{
		mesh = GetComponentInParent<FOVMesh>();
        enemyFoundEvent = new EnemyFoundEvent();
    }

    /// <summary>
    /// Add a listener to the EnemyPursueEvent invoked by this script
    /// </summary>
    /// <param name="call"></param>
    public void AddEnemyFoundEventListener(UnityAction<Vector3> call)
    {
        enemyFoundEvent.AddListener(call);
    }

    //if player is in vision, stop movement
	void FixedUpdate()
	{
        if (gameObject.tag == "Enemy" && IsPlayerInFOV())
        {
            enemyFoundEvent.Invoke(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }

    /// <summary>
    /// Finds the visible player.
    /// </summary>
	bool IsPlayerInFOV()
	{
		playerInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

		for (int i = 0; i < playerInRadius.Length; i++)
		{
            // Get the transform and direction of the player
			Transform player = playerInRadius[i].transform;
			Vector2 dirPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);

			if(Vector2.Angle(dirPlayer, transform.right) < viewAngle / 2)
			{
				float distancePlayer = Vector2.Distance(transform.position, player.position);

                // If the player is within the raycast, then add it to the list of players
				if (!Physics2D.Raycast(transform.position, dirPlayer, distancePlayer, obstacleMask) && player.tag == "Player")
				{
					visiblePlayer.Add(player);
					return true;
				}
			}
		}
		return false;
	}

    /// <summary>
    /// Returns correct angle for FOV
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="angleIsGlobal"></param>
    /// <returns></returns>
	public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.z;
		}
		return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
	}

    #endregion
}
