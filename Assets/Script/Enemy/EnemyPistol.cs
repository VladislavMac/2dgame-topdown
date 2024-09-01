using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyPistol : BaseEnemy
{

    private void FixedUpdate()
    {
        UpdateDistanceFromPlayer();
        AILogic();
    }
}
