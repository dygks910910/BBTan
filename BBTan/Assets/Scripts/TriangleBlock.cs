﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBlock : Shape {

        // Use this for initialization
        public override void Start()
        {
            spRenderer = GetComponent<SpriteRenderer>();
            spRenderer.color = Color.red;
            hpWeight = 0.92f / hp;
            gameObject.AddComponent<PolygonCollider2D>();
            hit = gameObject.AddComponent<ShapeHit>();
            GetComponentInChildren<TextMesh>().text = hp.ToString();
        }
}
