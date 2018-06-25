using UnityEngine;

public class move : MonoBehaviour {
    public float speed;
    public Vector3 dir;
    private Vector3 pos;
    private Vector3 lastPosition;

    private LayerMask collisionLayer;
	// Use this for initialization
	void Start () {
        collisionLayer = LayerMask.GetMask("RayColliderObj");
    }
    // Update is called once per frame
    void Update () {
      
    }
    private void FixedUpdate()
    {

        Vector3 translateVector = (this.dir * speed) * Time.deltaTime;
        transform.position += translateVector;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector3.Magnitude(translateVector), collisionLayer);
        if (hit)
        {
            
            CollisionShapeInteraction tmp = hit.collider.GetComponent<CollisionShapeInteraction>();
            if(tmp)
            {
                tmp.Hit();
            }
            Vector3 n = hit.normal;
            //반사벡터.
            dir = dir + (2 * n * (Vector2.Dot(-dir, n)));
            dir.Normalize();
        }

        Debug.DrawRay(transform.position, dir * speed * Time.deltaTime, Color.red);
        //transform.Translate(dir * speed);
        //rigidbd.MovePosition(transform.position + (dir * speed));

    }
}
