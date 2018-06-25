using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShapeInteraction : MonoBehaviour {
    public short hp;

    float hpWeight;
    private SpriteRenderer spRenderer;
	// Use this for initialization
	void Start () {
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.color = Color.red;
        hpWeight = 0.92f / hp;
	}
    public void Hit()
    {
        hp -= 1;
        if(hp <= 0)
        {
            DestroyEffect();
            Destroy(gameObject);
        }
        else
        {
            print(spRenderer.color);
            spRenderer.color += new Color(0.0f, hpWeight, 0,0);
        }
    }
    private void DestroyEffect()
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
