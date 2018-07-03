using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public ScoreManager scoreMgr;

    #region Arrow
    public GameObject arrow;
    public BigScale guideLineScale;
    public Vector3 arrowDirection;
    #endregion
    public enum GAME_STATUS
    {
        IDLE,
        SHOOTING,
        BLOCK_DOWN
    }

    GameObject ballPrefab;
    move moveScript;

    GameObject boxPrefab;


    public GAME_STATUS gameStatus;
    #region val
    public Vector2 nowPos, prePos;
    [SerializeField]
    public List<GameObject> shapeList;

    #region ball Value
    public short ballCount;
    //첫 낙하지점.
    public Vector3 firstFalledBallPos;
    //볼 스피드
    public float ballSpeed;
    public GameObject firstBall;
    public bool firstBallIsSetted;
    float shotBallDeleyTime;

    #endregion 
    #region constant val
    //시작지점변수.
    const float START_X = -2.4f;
    const float START_Y = 2.49f;

    //블록의 offset
    const float OFFSET = 0.805f;
    //최대 행
    const int MAX_ROW = 9;
    //최대 열
    const int MAX_COL = 7;

    const float StartBallPosX = 0;
    const float StartBallPosY = -4;
    const float FinalLine = -3.145f;
    #endregion
    #endregion

    #region singletonPatton
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if (_Instance == null)
                {
                    print("no GameManager Class");
                }
            }
            return _Instance;
        }

    }
    #endregion

    #region UnityEngine Overload Method
    void Start()
    {
        scoreMgr.iScore = 0;
        //         print("start");
        boxPrefab = Resources.Load("Prefabs/box") as GameObject;
        ballPrefab = Resources.Load("Prefabs/Ball") as GameObject;
        arrow.SetActive(false);
        ballCount = 5;
        firstFalledBallPos = new Vector3(StartBallPosX, StartBallPosY+ballPrefab.GetComponent<CircleCollider2D>().radius);
        ballPrefab.transform.position = firstFalledBallPos;
        firstBall = Instantiate(ballPrefab);
        moveScript = ballPrefab.GetComponent<move>();

        guideLineScale = arrow.GetComponentInChildren<BigScale>();
        shotBallDeleyTime = 0.2f;

        InvokeRepeating("NextRound", 0, 10);
    }
    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            prePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            prePos = Camera.main.ScreenToWorldPoint(prePos);
            arrow.transform.position = firstFalledBallPos;
            arrow.SetActive(true);
        }
       else if(Input.GetMouseButton(0))
        {
            nowPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            nowPos = Camera.main.ScreenToWorldPoint(nowPos);

            float zRotateAngle = GetAngle(nowPos, prePos);
            if(zRotateAngle > 30 && zRotateAngle < 150)
                arrow.transform.rotation = Quaternion.Euler(0, 0, zRotateAngle);

            float scaleFactor = Vector3.Magnitude( prePos - nowPos);
            guideLineScale.ScaleBig(scaleFactor);
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ShotBall();
        }
    }
    #endregion

    #region Status_Method
    void DecideStatus()
    {
        //블록이 내려오지 않고있고 , 공이 멈춰있으면 IDLE Status

        //공이 움직이고 있으면 Shoting Status.

        //블록이 내려오고 있으면 BlockDown Status.
    }
    #endregion
    #region BlockControllMethod
    void NextRound()
    {
        //print("next");
        scoreMgr.score.text = (++scoreMgr.iScore).ToString();
        CreateRowBlock();
        BlockMoveDown();
    }
    void CreateRowBlock()
    {
        boxPrefab.GetComponent<Block>().hp = (short)scoreMgr.iScore;
        for (int i = 0; i < MAX_COL; ++i)
        {
            boxPrefab.transform.position = new Vector3(START_X + (OFFSET * i), START_Y);
            shapeList.Add(MonoBehaviour.Instantiate(boxPrefab) as GameObject);
        }
    }
    void BlockMoveDown()
    {
        print("down");
        Block tmpScript;

        for (int i = 0; i < shapeList.Count; ++i)
        {
            tmpScript = shapeList[i].GetComponent<Block>();
            StartCoroutine(tmpScript.MoveDown(tmpScript.transform, OFFSET, 1));
        }

    }
    #endregion

    #region GameFunction
    void ShotBall()
    {
        arrowDirection = (prePos - nowPos).normalized;
        firstBallIsSetted = false;

        move scripts = firstBall.GetComponent<move>();
        scripts.dir = arrowDirection;
        scripts.Shot();

        for(int i = 1;  i< ballCount; ++i)
        {
            StartCoroutine(MakeBallAndShoot(i * shotBallDeleyTime));
        }
        arrow.SetActive(false);
       
    }
    IEnumerator MakeBallAndShoot(float delay)
    {
        print("makeBallAndShoot");
        GameObject tmpBall;
        ballPrefab.transform.position = firstBall.transform.position;
        moveScript.dir = arrowDirection;

        tmpBall = MonoBehaviour.Instantiate(ballPrefab);
       
        yield return new WaitForSeconds(delay);
        tmpBall.GetComponent<move>().Shot();

    }
    public float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
    #endregion
}
