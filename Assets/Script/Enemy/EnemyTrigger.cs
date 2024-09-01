using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject EnemyGameObject;

    private BaseEnemy _enemyBase; 

    private void Start()
    {
        _enemyBase = EnemyGameObject.GetComponent<BaseEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>())
        {
            if (other.gameObject.GetComponent<BaseBullet>())
            {
                _enemyBase.Status = EnemyStatus.Search;
                _enemyBase.LastPlayerPosition = other.GetComponent<BaseBullet>().Shooter.transform.position;
            }
        }
    }
}
