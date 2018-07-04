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
            GameManager.Instance.shapeList.Remove(obj);
            destroyEffect();
            Destroy(obj);
        }
        else
        {
            renderer.color += new Color(0.0f, hpWeight, 0, 0);
            obj.GetComponentInChildren<TextMesh>().text = hp.ToString();
        }
    }
}

public class Block : MonoBehaviour {
    protected SpriteRenderer spRenderer;
    //hp에따라 색을 바꿔주기 위한 가중치
    //블럭이 내려오는중인지?
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
        GameManager.Instance.AllBlockDownSucess |= true;
    }

    public virtual void Hit()
    {
        hit.Hit(ref hp, ref hpWeight, gameObject,spRenderer, CreateDestroyEffect);
    }
   

    public virtual void Start()
    {
    }
}

