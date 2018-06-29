using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HitInterface : MonoBehaviour
{
    //shapeHit전용.
    public abstract void Hit(ref short hp, ref float hpWeight, GameObject obj,SpriteRenderer renderer, System.Action destroyEffect);
}
public class ShapeHit : HitInterface
{
    public override void Hit(ref short hp,ref float hpWeight,GameObject obj,SpriteRenderer renderer,System.Action destroyEffect)
    {
        hp -= 1;
        if (hp <= 0)
        {
            print("destroy");
            destroyEffect();
            Destroy(obj);
        }
        else
        {
            print(renderer.color);
            renderer.color += new Color(0.0f, hpWeight, 0, 0);
        }
    }

}


public class Block : MonoBehaviour {


    protected SpriteRenderer spRenderer;
    //hp에따라 색을 바꿔주기 위한 가중치
    protected float hpWeight;
    public short hp;
    protected  HitInterface hit;


    public void CreateDestroyEffect()
    {
        GameObject prefab = Resources.Load("Prefabs/DestroyShapeEffect") as GameObject;
        prefab.gameObject.transform.position = transform.position;

        GameObject particle = MonoBehaviour.Instantiate(prefab) as GameObject;
        particle.name = "ParticleEffect";
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

    public virtual void Hit()
    {
        hit.Hit(ref hp, ref hpWeight, gameObject,spRenderer, CreateDestroyEffect);
    }
   

    public virtual void Start()
    {
    }
}

