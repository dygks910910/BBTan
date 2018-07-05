using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addBallItem : Item {

    public override void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.color = Color.yellow;
        gameObject.AddComponent<CircleCollider2D>().isTrigger = true; 
        hit = gameObject.AddComponent<AddBallHit>();
    }
}
