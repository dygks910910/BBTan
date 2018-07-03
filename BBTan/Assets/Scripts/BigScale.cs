using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScale : MonoBehaviour {

    Vector3 originScale;
    private void Start()
    {
        originScale = transform.localScale;
    }
    public void ScaleBig(float scaleFactor)
    {
        transform.localScale = originScale * scaleFactor;

    }
}
