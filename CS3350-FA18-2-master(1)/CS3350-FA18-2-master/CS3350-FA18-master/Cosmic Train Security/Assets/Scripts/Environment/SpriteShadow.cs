using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {

    #region Fields
    [SerializeField]
    Vector2 offset = new Vector2(-3, -3);

    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;

    private Transform transCaster;
    private Transform transShadow;

    [SerializeField]
    Material shadowMaterial;
    [SerializeField]
    Color shadowColor;

    #endregion

    #region Methods
    // Use this for initialization
    void Start () {
        // Create new Game Object and assign the same sprite as parent.
        transCaster = transform;
        transShadow = new GameObject().transform;
        transShadow.parent = transCaster;
        transShadow.gameObject.name = "shadow";
        transShadow.localRotation = Quaternion.identity;

        // Give new game object a sprite renderer
        sprRndCaster = GetComponent<SpriteRenderer>();
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

        // Set up shadow material and layer sorting (so it appears under the object)
        sprRndShadow.material = shadowMaterial;
        sprRndShadow.color = shadowColor;
        shadowMaterial.color = shadowColor;
        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;
	}

    /// <summary>
    /// Used to set up the new GameObject's position
    /// </summary>
    void LateUpdate()
    {
        // Set up the new gameobject's position.
        transShadow.position = new Vector3(transCaster.position.x + offset.x, transCaster.position.y + offset.y, -10);
        sprRndShadow.sprite = sprRndCaster.sprite;
    }

    #endregion
}
