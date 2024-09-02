using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using CustomInterface;
using TMPro;

public class PlayerController : MonoBehaviour, IShooter, IInventory, IEntity
{
    public float HpMax = 20f;

    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _speedShift = 10f;

    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _hands;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _UIAmmo;
    [SerializeField] private GameObject _spawnPoint;

    private HandController _handsController;
    private Vector2 moveDirection;

    public float _hp;

    private void Start()
    {
        _handsController = _hands.GetComponent<HandController>();
        _hp = HpMax;
        SetHandsOwner(_handsController, this.gameObject);
        SetInventoryPlayer();
    }

    private void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (IsEntityDead()) Die();
        Healing();

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _handsController.HandWeapon.GetComponent<BaseWeapon>().RemoveAmmo();
        }

        _UIAmmo.GetComponent<TextMeshProUGUI>().text = $"{_handsController.HandWeapon.GetComponent<BaseWeapon>().Ammo} / {_handsController.HandWeapon.GetComponent<BaseWeapon>().MaxAmmo}";
    }

    private void FixedUpdate()
    {
        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();

        float speed = _speed;
        speed = Input.GetKey(KeyCode.LeftShift) ? _speedShift : speed;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    private void Healing()
    {
        if (_hp < HpMax) _hp += Time.deltaTime * 1.3f;
    }

    /* ---------------- Realization Interface ---------------- */

    public void Shoot()
    {
        _handsController.HandWeapon.Shoot();
    }

    public void Die()
    {
        transform.position = _spawnPoint.transform.position;
        _hp = HpMax;
    }

    public void SetInventoryPlayer()
    {
        _inventory.GetComponent<PlayerInventory>().Player = this.gameObject;
    }

    public void SetHandsOwner(HandController handsController, GameObject shooter)
    {
        handsController.Owner = shooter;
    }

    public void HitEntity(float damage)
    {
        _hp -= damage;
    }

    public bool IsEntityDead()
    {
        return _hp <= 0;
    }

}
