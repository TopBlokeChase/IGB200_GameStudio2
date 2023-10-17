using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] bool infiniteHorizontal;
    [SerializeField] bool infiniteVertical;
    private Transform camTransform;
    private Vector3 lastCamPos;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPos = camTransform.position;

        if (infiniteHorizontal)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(camTransform.position.x + offsetPositionX, transform.position.y);
            }
        }
        
        if (infiniteVertical)
        {
            if (Mathf.Abs(camTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (camTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(camTransform.position.x + offsetPositionY, transform.position.y);
            }
        }     
    }
}
