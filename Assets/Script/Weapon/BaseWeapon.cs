using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class BaseWeapon : MonoBehaviour
{
    [HideInInspector] public GameObject Owner { get; set; }

    protected private GameObject _bullet;
    protected Transform _barrel;
    protected float _shootCooldown;

    protected float _cooldown;
    protected bool _canShoot = true;

    protected void Initialize(GameObject bullet, Transform barrel, float shootCooldown)
    {
        this._bullet = bullet;
        this._barrel = barrel;
        this._shootCooldown = shootCooldown;

        this._shootCooldown = this._shootCooldown * 0.01f;
        this._cooldown = this._shootCooldown;
    }

    public virtual void Shoot()
    {
        if (_canShoot)
        {
            GameObject bullet = Instantiate(_bullet, _barrel.position, _barrel.rotation);
            bullet.GetComponent<BaseBullet>().Shooter = Owner;
        }

        _canShoot = false;
    }

    protected void ShootCoolDown()
    {
        if (!_canShoot)
        {
            if (_cooldown <= 0)
            {
                _canShoot = true;
                _cooldown = _shootCooldown;
            }
            else
            {
                _canShoot = false;
                _cooldown -= Time.deltaTime;
            }
        }
    }
}
