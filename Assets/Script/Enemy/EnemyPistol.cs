using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPistol : BaseEnemy
{
    private void Update()
    {
        UpdateDistanceFromPlayer();
        AILogic();
    }
}
