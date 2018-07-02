using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public ScoreManager scoreMgr;

    public enum GAME_STATUS
    {
        IDLE,
        SHOOTING,
        BLOCK_DOWN
    }

    GameObject ballPrefab;
    GameObject boxPrefab;


    public GAME_STATUS gameStatus;
    #region val

    [SerializeField]
    public List<GameObject> shapeList;
    public List<GameObject> ballList;
    //첫 낙하지점.
    Vector3 firstFalledBallPos;
    //볼 스피드
    public float ballSpeed;
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
    // Use this for initialization
    #region UnityEngine Overload Method
    void Start()
    {
        scoreMgr.iScore = 0;
        //         print("start");
        InvokeRepeating("NextRound", 0, 10);
        boxPrefab = Resources.Load("Prefabs/box") as GameObject;
        ballPrefab = Resources.Load("Prefabs/Ball") as GameObject;
        //NextRound();
    }
    // Update is called once per frame
    void Update()
    {

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
    void AddBall()
    {
        ballList.Add(Instantiate(ballPrefab));
    }
    #endregion
}
