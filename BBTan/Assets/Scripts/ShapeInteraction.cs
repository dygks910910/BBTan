using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeInteraction : MonoBehaviour {
    public short hp;

    float hpWeight;
    private SpriteRenderer spRenderer;
	// Use this for initialization
	void Start () {
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.color = Color.red;
        hpWeight = 0.92f / hp;
        gameObject.AddComponent<BoxCollider2D>();

	}
    public void Hit()
    {
        hp -= 1;
        if(hp <= 0)
        {
            CreateDestroyEffect();
            Destroy(gameObject);
        }
        else
        {
            print(spRenderer.color);
            spRenderer.color += new Color(0.0f, hpWeight, 0,0);
        }
    }

    public IEnumerator MoveDown(Transform thisTransform, float distance, float speed)
    {
        float startPos = thisTransform.position.y;
        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(startPos - endPos) * speed;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            Vector3 pos = thisTransform.position;
            pos.y = Mathf.Lerp(startPos, endPos, t);
            thisTransform.position = pos;
            yield return 0;
        }
    }

    private void CreateDestroyEffect()
    {
        GameObject prefab = Resources.Load("Prefabs/DestroyShapeEffect") as GameObject;
        prefab.gameObject.transform.position = transform.position;

        GameObject particle = MonoBehaviour.Instantiate(prefab) as GameObject;
        particle.name = "ParticleEffect";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
