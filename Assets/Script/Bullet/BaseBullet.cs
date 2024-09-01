using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;

public abstract class BaseBullet : MonoBehaviour
{
    [HideInInspector] public GameObject Shooter;
    [HideInInspector] public abstract float Speed { get; }

    [SerializeField] protected LayerMask _layersWhichRaycastSee;
    [SerializeField] protected GameObject _bulletDirect;

    private float _lengthRaycast = 0.5f;

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<Collider2D>() && !collision.isTrigger)
     //   {
     //       HitSomeone(collision);
     //   }
    //}

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
        RaycastHit2D rayCastBullet;
        rayCastBullet = Physics2D.Raycast(_bulletDirect.transform.position, _bulletDirect.transform.position - transform.position, _lengthRaycast);

        Debug.DrawRay(_bulletDirect.transform.position, _bulletDirect.transform.position - transform.position, Color.red, _lengthRaycast);

        if (rayCastBullet.collider != null)
        {
            if (rayCastBullet.collider.gameObject.GetComponent<Collider2D>() && !rayCastBullet.collider.isTrigger)
            {
                HitSomeone(rayCastBullet.collider);
            }
        }

        transform.Translate(Vector2.up * Speed * Time.deltaTime);

        StartCoroutine(DeleteBullet());
    }

    protected IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }
}
