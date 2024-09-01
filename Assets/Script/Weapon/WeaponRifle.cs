using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRifle : BaseWeapon
{
    private void Update()
    {
        base.ShootCoolDown();
    }
}
