﻿using System.Collections;
using UnityEngine;

public class move : MonoBehaviour {

    public bool IsMoving;
    public float speed;
    public Vector3 dir;

    private static LayerMask collisionLayer;
    private CircleCollider2D myCollider;

	// Use this for initialization
	void Start () {
        collisionLayer = LayerMask.GetMask("RayColliderObj");
        myCollider = GetComponent<CircleCollider2D>();
        speed = 10;
    }
    // Update is called once per frame
    void Update () {
      
    }
    public void Shot()
    {
        
        IsMoving = true;
    }
    public IEnumerator MoveToPointAndDestroy(Vector3 firstBallPoint, float speed)
    {
        Vector3 thisPos = transform.position;

        Vector3 endPos = firstBallPoint;

        float startPos = transform.position.y;
        float rate = 1.0f / Mathf.Abs(thisPos.magnitude - endPos.magnitude) * speed;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            
            transform.position = Vector3.Lerp(thisPos, endPos, t); 
            yield return 0;
        }
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        Vector3 translateVector = (this.dir * speed) * Time.deltaTime;
        if(IsMoving)
            transform.position += translateVector;

        try
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, myCollider.radius, dir, myCollider.radius, collisionLayer);
            if (hit)
            {
                //FinishLine일 경우.
                if (hit.collider.name == "FinishLine" && IsMoving)
                {
                    if (!GameManager.Instance.firstBallIsSetted)
                    {
                        transform.position = new Vector3(hit.point.x, hit.point.y + myCollider.radius);
                        GameManager.Instance.firstFalledBallPos = transform.position;
                        GameManager.Instance.firstBallIsSetted = true;
                        IsMoving = false;
                        GameManager.Instance.firstBall = gameObject;
                    }
                    else
                    {
                        Destroy(myCollider);
                        StartCoroutine(MoveToPointAndDestroy(GameManager.Instance.firstFalledBallPos, 2));
                    }
                    GameManager.Instance.ballComeBackCount++;
                }
                //FinishLine이 아닐경우.
                else
                {
                    Block tmp = hit.collider.GetComponent<Block>();
                    if (tmp)
                    {
                        tmp.Hit();
                    }
                    if (hit.collider.isTrigger == false)
                    {
                        Vector3 n = hit.normal;
                        //반사벡터 공식.
                        dir = dir + (2 * n * (Vector2.Dot(-dir, n)));
                        dir.Normalize();
                    }


                }
            }
        }
        catch
        {

        }
       
        Debug.DrawRay(transform.position, dir * speed * Time.deltaTime, Color.red);
        //transform.Translate(dir * speed);
        //rigidbd.MovePosition(transform.position + (dir * speed));

    }

   
}
