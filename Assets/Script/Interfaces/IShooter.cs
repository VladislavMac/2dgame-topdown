using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    public void SetHandsOwner(HandController handsController, GameObject shooter);
    public void Shoot();
}
