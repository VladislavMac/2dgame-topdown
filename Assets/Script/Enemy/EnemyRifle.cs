using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifle : BaseEnemy
{
    private void FixedUpdate()
    {
        UpdateDistanceFromPlayer();
        AILogic();
    }
}
