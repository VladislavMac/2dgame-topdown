using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class BaseWeapon : MonoBehaviour
{
    [HideInInspector] public GameObject Owner { get; set; }

    [SerializeField] protected private GameObject _bullet;
    [SerializeField] protected Transform _barrel;
    [SerializeField] protected float _shootCooldown;

    protected float _cooldown;
    protected bool _canShoot = true;

    private void Start()
    {
        this._shootCooldown = this._shootCooldown * 0.1f;
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
