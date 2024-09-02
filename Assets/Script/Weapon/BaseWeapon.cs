using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class BaseWeapon : MonoBehaviour
{
    public int MaxAmmo;
    public float RechargeAmmo;
    public int Ammo;

    [HideInInspector] public GameObject Owner { get; set; }

    [SerializeField] protected private GameObject _bullet;
    [SerializeField] protected AudioClip _shootsSound;
    [SerializeField] protected AudioClip _soundRechargeWeapon;
    [SerializeField] protected Transform _barrel;
    [SerializeField] protected float _shootCooldown;

    protected AudioSource _shootAudioSource => GetComponent<AudioSource>();

    protected float _cooldown;
    protected bool _canShoot = true;

    protected float _rechargeCooldown;

    private void Start()
    {
        this._shootCooldown = this._shootCooldown * 0.1f;
        this._cooldown = this._shootCooldown;
        this._rechargeCooldown = RechargeAmmo;
        this.Ammo = MaxAmmo;
    }

    private void Update()
    {
        RechargeWeapon();
        if (Ammo > 0) ShootCoolDown();
    }

    public virtual void Shoot()
    {
        if (_canShoot)
        {
            PlaySoundShoot();
            GameObject bullet = Instantiate(_bullet, _barrel.position, _barrel.rotation);
            bullet.GetComponent<BaseBullet>().Shooter = Owner;
            RemoveBulletInAmmo();
        }

        _canShoot = false;
    }

    public void PlaySoundShoot()
    {
        _shootAudioSource.PlayOneShot(_shootsSound);
    }

    public void PlaySoundRecharge()
    {
        _shootAudioSource.PlayOneShot(_soundRechargeWeapon, 1f);
    }

    public void RemoveBulletInAmmo()
    {
        Ammo--;
    }

    public void RemoveAmmo()
    {
        Ammo = 0;
    }

    protected void RechargeWeapon()
    {

        if (Ammo <= 0)
        {
            _canShoot = false;

            if (_rechargeCooldown <= 0)
            {
                PlaySoundRecharge();
                _canShoot = true;
                Ammo = MaxAmmo;
                _rechargeCooldown = RechargeAmmo;
            }
            else
            {
                _rechargeCooldown -= Time.deltaTime;
            }
        }
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
