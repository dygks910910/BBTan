using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    public float speed;
    public Vector3 dir;
    private Rigidbody2D rigidbd;
    private Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = transform.position;
        rigidbd = GetComponent<Rigidbody2D>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision");
        Vector3 n = collision.contacts[0].normal;
        dir = dir + (2 * n * (Vector3.Dot(-dir, n)));
        dir.Normalize();
    }
   
    // Update is called once per frame
    void Update () {
        
	}
    private void FixedUpdate()
    {
        //transform.Translate(dir * speed);
        rigidbd.MovePosition(transform.position + (dir * speed));
    }
}
