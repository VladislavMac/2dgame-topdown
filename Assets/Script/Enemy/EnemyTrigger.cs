using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject EnemyGameObject;

    private List<GameObject> _enemiesAround = new List<GameObject>();
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
                try
                {
                    _enemyBase.LastPlayerPosition = other.GetComponent<BaseBullet>().Shooter.transform.position;
                    Warning(other.GetComponent<BaseBullet>().Shooter.transform.position);
                }
                catch { }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>())
        {
            if (other.gameObject.GetComponent<BaseEnemy>())
            {
                _enemiesAround.Add(other.gameObject);
            }
        }
    }

    public void Warning(Vector3 warningPosition)
    {
        foreach (GameObject enemy in _enemiesAround)
        {
            try
            {
                enemy.GetComponent<BaseEnemy>().Status = EnemyStatus.Search;
                enemy.GetComponent<BaseEnemy>().LastPlayerPosition = warningPosition;
            }
            catch { }
        }
    }
}
