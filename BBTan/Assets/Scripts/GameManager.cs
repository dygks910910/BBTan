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
        BLOCK_DOWN,
        GAMEOVER
    }

    GameObject ballPrefab;
    move moveScript;

    GameObject boxPrefab;

    //게임 상태.
    public GAME_STATUS gameStatus;
    //터치 컨트롤.
    public Vector2 nowPos, prePos;

    //블록 리스트.
    [SerializeField]
    public List<GameObject> shapeList;


    #region ball Value
    public bool AllBlockDownSucess;

    public GameObject firstBall;
    public short ballCount;
    public short ballComeBackCount;
    //첫 낙하지점.
    public Vector3 firstFalledBallPos;
    //볼 스피드
    public float ballSpeed;
    public bool firstBallIsSetted;
    const float shotBallDeleyTime =  0.2f;
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
        //         print("start");
        //프리팹 Read.
        boxPrefab = Resources.Load("Prefabs/box") as GameObject;
        ballPrefab = Resources.Load("Prefabs/Ball") as GameObject;

        //초기 ball세팅값.
        ballCount = 1;
        firstFalledBallPos = new Vector3(StartBallPosX, StartBallPosY + ballPrefab.GetComponent<CircleCollider2D>().radius);
        ballPrefab.transform.position = firstFalledBallPos;
        moveScript = ballPrefab.GetComponent<move>();
        firstBall = Instantiate(ballPrefab);
        //기타 세팅값.
        arrow.SetActive(false);
        guideLineScale = arrow.GetComponentInChildren<BigScale>();
        gameStatus = GAME_STATUS.BLOCK_DOWN;
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameStatus)
        {
            case GAME_STATUS.IDLE:
                {
                    if(AllBlockDownSucess)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            prePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                            prePos = Camera.main.ScreenToWorldPoint(prePos);
                            arrow.transform.position = firstFalledBallPos;
                            arrow.SetActive(true);
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            nowPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                            nowPos = Camera.main.ScreenToWorldPoint(nowPos);

                            float zRotateAngle = GetAngle(nowPos, prePos);
                            if (zRotateAngle > 15 && zRotateAngle < 165)
                            {
                                arrow.transform.rotation = Quaternion.Euler(0, 0, zRotateAngle);
                                float scaleFactor = Vector3.Magnitude(prePos - nowPos);
                                guideLineScale.ScaleBig(scaleFactor);
                                arrowDirection = (prePos - nowPos).normalized;
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            gameStatus = GAME_STATUS.SHOOTING;
                            arrow.SetActive(false);
                            ShotBall();
                        }
                    }
                    break;
                }
            case GAME_STATUS.BLOCK_DOWN:
                {
                    print("nextRound IN Update");
                    NextRound();
                    gameStatus = GAME_STATUS.IDLE;
                    AllBlockDownSucess = false;
                    break;
                }
            case GAME_STATUS.SHOOTING:
                {
                    if (ballCount == ballComeBackCount)
                    {
                        gameStatus = GAME_STATUS.BLOCK_DOWN;
                    }
                    break;
                }
            case GAME_STATUS.GAMEOVER:
                {
                    for(int i = 0; i < shapeList.Count; ++i)
                    {
                        shapeList[i].AddComponent<Rigidbody2D>();
                    }

                    break;
                }
        }
    }

    private void LateUpdate()
    {
        if(AllBlockDownSucess)
        {
            for(int i = 0; i < shapeList.Count; ++i)
            {
                if(shapeList[i].transform.position.y <= FinalLine)
                {
                    gameStatus = GAME_STATUS.GAMEOVER;
                }
            }
        }
    }
    #endregion

    #region BlockControllMethod
    void NextRound()
    {
        //print("next");
        scoreMgr.NextScore();
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
         firstBallIsSetted = false;
        ballComeBackCount = 0;
        Destroy(firstBall);
        for (int i = 0;  i< ballCount; ++i)
        {
            StartCoroutine(MakeBallAndShoot(i * shotBallDeleyTime));
        }
    }
    IEnumerator MakeBallAndShoot(float delay)
    {
        print("makeBallAndShoot");
        GameObject tmpBall;
        ballPrefab.transform.position = firstFalledBallPos;
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
