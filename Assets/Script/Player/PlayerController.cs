using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using CustomInterface;
using TMPro;

public class PlayerController : MonoBehaviour, IShooter, IInventory, IEntity
{
    public float HpMax = 20f;
    public int Kills = 0;

    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _speedShift = 10f;

    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _hands;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private GameObject _UIAmmo;
    [SerializeField] private GameObject _UIKills;
    [SerializeField] private GameObject[] _UIPanelHp;
    [SerializeField] private GameObject[] _UIReactKills;

    private HandController _handsController;
    private Vector2 moveDirection;

    private float _hp;

    private void Start()
    {
        _handsController = _hands.GetComponent<HandController>();
        _hp = HpMax;
        _UIKills.GetComponent<TextMeshProUGUI>().text = $"{Kills}";

        SetHandsOwner(_handsController, this.gameObject);
        SetInventoryPlayer();
    }

    private void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        if (IsEntityDead()) Die();

        Healing();
        SetPanelHp();
        SetReactKills();

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

    private void SetPanelHp()
    {
        _UIKills.GetComponent<TextMeshProUGUI>().text = $"{Kills}";

        for (int i = 0; i < _UIPanelHp.Length; i++)
        {
            _UIPanelHp[i].SetActive(false);
        }

        if (_hp >= 20)
            _UIPanelHp[0].SetActive(true);
        else if (_hp >= 10)
            _UIPanelHp[1].SetActive(true);
        else if (_hp > 0)
            _UIPanelHp[2].SetActive(true);
    }

    private void SetReactKills()
    {
        for (int i = 0; i < _UIReactKills.Length; i++)
        {
            _UIReactKills[i].SetActive(false);
        }

        if (Kills >= 0 && Kills < 5)
            _UIReactKills[0].SetActive(true);
        else if (Kills >= 5 && Kills < 10)
            _UIReactKills[1].SetActive(true);
        else if (Kills >= 10 && Kills < 15)
            _UIReactKills[2].SetActive(true);
        else if (Kills >= 15 && Kills < 20)
            _UIReactKills[3].SetActive(true);
        else if (Kills >= 20 && Kills < 25)
            _UIReactKills[4].SetActive(true);
        else if (Kills >= 25 && Kills < 30)
            _UIReactKills[5].SetActive(true);
        else if (Kills >= 30 && Kills < 35)
            _UIReactKills[6].SetActive(true);
        else if (Kills >= 35 && Kills < 40)
            _UIReactKills[7].SetActive(true);
        else if (Kills >= 40)
            _UIReactKills[8].SetActive(true);
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
        if (Kills != 0) Kills = 0;
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

    public void AddKill()
    {
        Kills++;
    }

}
