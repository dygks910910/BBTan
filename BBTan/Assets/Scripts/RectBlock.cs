using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectBlock : Shape
{

    // Use this for initialization
    public override void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.color = Color.red;
        hpWeight = 0.92f / hp;
        gameObject.AddComponent<BoxCollider2D>();
        hit = new ShapeHit();
    }
    
}
