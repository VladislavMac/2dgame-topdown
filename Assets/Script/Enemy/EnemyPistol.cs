using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPistol : BaseEnemy
{

    private void FixedUpdate()
    {
        UpdateDistanceFromPlayer();
        AILogic();
    }
}
