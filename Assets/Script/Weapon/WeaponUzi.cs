using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUzi : BaseWeapon
{
    private void Update()
    {
        base.ShootCoolDown();
    }
}
