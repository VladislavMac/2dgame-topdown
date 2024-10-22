using System.Collections;
using UnityEngine;

using Enemy;
using Player;

namespace Bullet
{
    public abstract class BaseBullet : MonoBehaviour
    {
        [HideInInspector] public GameObject Shooter;
        [HideInInspector] public abstract float Speed { get; }
        [HideInInspector] public abstract float Damage { get; }

        [SerializeField] protected LayerMask _layersWhichRaycastSee;
        [SerializeField] protected GameObject _bulletDirect;

        private float _lengthRaycast = 0.5f;

        protected void HitSomeone(Collider2D collision) 
        {
            Destroy(transform.gameObject);

            if (collision.gameObject.GetComponent<BaseEnemy>())
            {
                collision.GetComponent<BaseEnemy>().HitEntity(Damage);
            }
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().HitEntity(Damage);
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
                    try
                    {
                        HitSomeone(rayCastBullet.collider);
                    }
                    catch { }
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

}

