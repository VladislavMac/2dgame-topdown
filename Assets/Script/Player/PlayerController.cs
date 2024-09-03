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
    [SerializeField] private GameObject _UIDeath;

    [SerializeField] private GameObject _UIWinPlane;
    [SerializeField] private GameObject _UICountKills;
    [SerializeField] private GameObject _UICountDeath;
    [SerializeField] private GameObject _UIOut;

    [SerializeField] private GameObject[] _UIPanelHp;
    [SerializeField] private GameObject[] _UIReactKills;

    private HandController _handsController;
    private Vector2 moveDirection;
    public int _countKills;
    private int _death;
    private float _hp;

    private void Start()
    {
        _handsController = _hands.GetComponent<HandController>();
        _hp = HpMax;
        _UIKills.GetComponent<TextMeshProUGUI>().text = $"{Kills}";
        _UIWinPlane.SetActive(false);

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
        SetUIDeath();

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _handsController.HandWeapon.GetComponent<BaseWeapon>().RemoveAmmo();
        }

        if (Input.GetKey("escape"))  
        {
            Application.Quit();  
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

    public void SetWinPlane()
    {
        TextMeshProUGUI outText = _UIOut.GetComponent<TextMeshProUGUI>();
        string result = "";

        _UIWinPlane.SetActive(true);
        _UICountKills.GetComponent<TextMeshProUGUI>().text = $"{_countKills}";
        _UICountDeath.GetComponent<TextMeshProUGUI>().text = $"{_death}";

        if (_countKills > 60 && _death == 0)
            result = "ТЫ БОГ";
        else if (_countKills > 0 && _countKills < 20 && _death <= 5)
            result = "Неплох";
        else if (_countKills >= 20 && _countKills < 30 && _death <= 5)
            result = "Это хорошо";
        else if (_countKills >= 30 && _countKills < 40 && _death <= 5)
            result = "Это очень даже";
        else if (_countKills >= 40 && _countKills < 50 && _death <= 5)
            result = "Ой ой... Это жостко";
        else if (_countKills >= 50 && _countKills < 60 && _death <= 5)
            result = "Уффф Это невероятно";
        else if (_countKills >= 60 && _countKills < 80 && _death <= 5)
            result = "Джон Уик это ты?";
        else 
            result = "Без комментариев";

        outText.text = result;
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

    private void SetUIDeath()
    {
        _UIDeath.GetComponent<TextMeshProUGUI>().text = $"{_death}";
    }

    private void AddDeath()
    {
        _death++;
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
        AddDeath();
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
        _countKills++;
    }

}
