using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public ScoreManager scoreMgr;
    
    
    #region prefabs
    #endregion

    [SerializeField]
    List<GameObject> shapeList;
    List<GameObject> ballList;
    #region val
    public float ballSpeed;
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
    //볼 스피드
    #endregion
    // Use this for initialization
    #region UnityEngine Overload Method
    void Start () {
        scoreMgr.iScore = 5;
        //         print("start");
        //         InvokeRepeating("NextRound", 0, 2);
        NextRound();
    }
    // Update is called once per frame
    void Update () {
		
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
        GameObject prefab = Resources.Load("Prefabs/box") as GameObject;
        prefab.GetComponent<Block>().hp = (short)scoreMgr.iScore;
        for (int i = 0; i < MAX_COL; ++i)
        {
            prefab.transform.position = new Vector3(START_X + (OFFSET * i), START_Y);
            shapeList.Add(MonoBehaviour.Instantiate(prefab) as GameObject);
        }
    }
    void BlockMoveDown()
    {
        print("down");
        Block tmpScript;

        for (int i = 0; i < shapeList.Count; ++i)
        {
            if (shapeList[i])
            {
                tmpScript = shapeList[i].GetComponent<Block>();
                StartCoroutine(tmpScript.MoveDown(tmpScript.transform, OFFSET, 1));
            }
            else
            {
                shapeList.RemoveAt(i);
            }
        }
    }
    #endregion
}
