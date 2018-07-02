using UnityEngine;

public class move : MonoBehaviour {

    public bool IsMoving;
    public float speed;
    public Vector3 dir;

    private Vector3 pos;
    private Vector3 lastPosition;
    private LayerMask collisionLayer;
    private CircleCollider2D myCollider;
	// Use this for initialization
	void Start () {

        collisionLayer = LayerMask.GetMask("RayColliderObj");
        myCollider = GetComponent<CircleCollider2D>();
    }
    // Update is called once per frame
    void Update () {
      
    }
    private void FixedUpdate()
    {
        Vector3 translateVector = (this.dir * speed) * Time.deltaTime;
        if(IsMoving)
            transform.position += translateVector;
  

        RaycastHit2D hit = Physics2D.Raycast(transform.position , dir, Vector3.Magnitude(translateVector) , collisionLayer);
        if (hit)
        {
            //FinishLine일 경우.
            if (hit.collider.name == "FinishLine" && IsMoving)
            {
                print("FinishLine");
                transform.position = new Vector3( hit.point.x,hit.point.y + myCollider.radius);
                IsMoving = false;
            }
            //FinishLine이 아닐경우.
            else
            {
                Block tmp = hit.collider.GetComponent<Block>();
                if (tmp)
                {
                    tmp.Hit();

                }
                Vector3 n = hit.normal;
                //반사벡터 공식.
                dir = dir + (2 * n * (Vector2.Dot(-dir, n)));
                dir.Normalize();
            }
        }
        Debug.DrawRay(transform.position, dir * speed * Time.deltaTime, Color.red);
        //transform.Translate(dir * speed);
        //rigidbd.MovePosition(transform.position + (dir * speed));

    }
}
