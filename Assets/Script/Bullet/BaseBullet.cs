using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [HideInInspector] public GameObject Shooter;

    [HideInInspector] public abstract float Speed { get; }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Collider2D>() && !collision.isTrigger)
        {
            HitSomeone(collision);
        }
    }

    protected void HitSomeone(Collider2D collision) 
    {
        Destroy(transform.gameObject);

        if (collision.gameObject.GetComponent<BaseEnemy>())
        {
            collision.GetComponent<BaseEnemy>().Die();
        }
    }

    protected void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);

        StartCoroutine(DeleteBullet());
    }

    protected IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }
}
