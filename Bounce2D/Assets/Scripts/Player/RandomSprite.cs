 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    public Sprite[] Sprites;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int randomIndex = Random.Range(0, Sprites.Length);
        SpriteRenderer.sprite = Sprites[randomIndex];
    }
}
