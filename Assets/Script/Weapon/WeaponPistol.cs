using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPistol : BaseWeapon
{
    private void Update()
    {
        base.ShootCoolDown();
    }

}
