using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPistol : BaseWeapon
{
    [SerializeField] private GameObject _weaponBullet;
    [SerializeField] private Transform _weaponBarrel;
    [SerializeField] private float _weaponShootCooldown;

    private void Start()
    {
        base.Initialize(_weaponBullet, _weaponBarrel, _weaponShootCooldown);
    }

    private void Update()
    {
        base.ShootCoolDown();
    }

}
