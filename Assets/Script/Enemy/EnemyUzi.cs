using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUzi : BaseEnemy
{
    private void Update()
    {
        UpdateDistanceFromPlayer();
        AILogic();

    }
}
