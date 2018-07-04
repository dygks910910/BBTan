using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {

    public Text score;
    public short iScore;

    // Use this for initialization
    public void NextScore()
    {
        iScore++;
        score.text = iScore.ToString();
    }
    void Start () {
        score = GameObject.Find("Score").GetComponent<Text>();
        iScore = 0;
        score.text = iScore.ToString() ;
	}
}
