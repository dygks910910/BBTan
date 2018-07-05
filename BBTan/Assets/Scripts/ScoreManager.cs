using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {

    public Text score;
    public short iScore;

    #region singletonPatton
    private static ScoreManager _Instance; 
    public static ScoreManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
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
