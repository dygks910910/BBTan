using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {

    public Text score;
    private int iScore;

//     private GameObject[,] shapeArr;
[SerializeField]
    List<GameObject> shapeList;

    const float startX = -2.4f;
    const float startY = 2.49f;

    const float offset = 0.805f;
    const int MAX_ROW = 9;
    const int MAX_COL = 7;
    // Use this for initialization
    void Start () {

        score = GameObject.Find("Score").GetComponent<Text>();
        iScore = 1;
        score.text = iScore.ToString() ;

        InvokeRepeating("NextRound", 2, 3);
	}


    void NextRound()
    {
        CreateRowBlock();
        BlockMoveDown();
    }

    void CreateRowBlock()
    {
        GameObject prefab = Resources.Load("Prefabs/box") as GameObject;
        for(int i = 0; i < MAX_COL; ++i)
        {
             prefab.transform.position =new Vector3(startX + (offset*i),startY);
             shapeList.Add(MonoBehaviour.Instantiate(prefab) as GameObject);
        }
    }
    void BlockMoveDown()
    {
        print("down");
        ShapeInteraction tmpScript;

        for(int i = 0; i < shapeList.Count; ++i)
        {
            if(shapeList[i])
            {
                tmpScript = shapeList[i].GetComponent<ShapeInteraction>();
                StartCoroutine(tmpScript.MoveDown(tmpScript.transform, offset, 1));
            }
            else
            {
                shapeList.RemoveAt(i);
            }
        }
    }

    
	// Update is called once per frame
	void Update () {
       
	}
}
