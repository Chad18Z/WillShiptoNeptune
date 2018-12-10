using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    BoxCollider2D bc2d;


    [SerializeField]
    Sprite brokenSprite;
    [SerializeField]
    public ParticleSystem destroy;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        EventManager.AddCanisterProjectileCollisionListener(HandleExplosions);
    }
	
    /// <summary>
    /// Ons the collision enter2 d.
    /// </summary>
    /// <param name="collision">Collision.</param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EnemyBullet")
        {
            //creates the oarticle system "destroy"
            ParticleSystem destroyParticleSystem = Instantiate(destroy, transform.position, transform.rotation);
            //changes sprite to destroyed sprite
            spriteRenderer.sprite = brokenSprite;
            StartCoroutine(FadeSprite(true));
            bc2d.enabled = false;
        }
    }

    /// <summary>
    /// Fades the sprite.
    /// </summary>
    /// <returns>The sprite.</returns>
    /// <param name="fadeAway">If set to <c>true</c> fade away.</param>
    IEnumerator FadeSprite(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                spriteRenderer.color = new Color(1, 1, 1, i);

                // Destroy the object after fade ends
                if (i <= .01)
                    Destroy(gameObject);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime) 
            {
                spriteRenderer.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }


    }

    /// <summary>
    /// Handles the explosions.
    /// </summary>
    /// <param name="location">Location.</param>
    void HandleExplosions(Vector2 location)
    {

        //determan distance from the explosion using pythagream formula 
        float distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(transform.position.x - location.x, 2) + Mathf.Pow(transform.position.y - location.y, 2)));
        //check distance and apply damage to every thing inside of that distance.
        if (distance < 3)
        {
            spriteRenderer.sprite = brokenSprite;
            StartCoroutine(FadeSprite(true));
            bc2d.enabled = false;
        }
    }
}
