using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    // Use this for initialization
    private static PlayerInfo _Instance = null;
    private uint m_Score = 0;
    private uint m_HighScore = 0;



    public static PlayerInfo Instance
    {
        get
        {
            if (_Instance == null)
            {
                print("getInstance");
                _Instance = FindObjectOfType(typeof(PlayerInfo)) as PlayerInfo;
                if (_Instance == null)
                {
                    print("Instance is null");
                }
            }
            return _Instance;
        }
    }
    
	
}
