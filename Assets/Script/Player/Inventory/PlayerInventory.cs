using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject[] Slots;
    [SerializeField] private GameObject _hands; 
    [HideInInspector] public GameObject Player;

    private Dictionary<GameObject, BaseWeapon> WeaponSlots = new Dictionary<GameObject, BaseWeapon>();

    private void Start()
    {
        foreach (GameObject slot in Slots)
        {
            if (transform.childCount == 1)
                WeaponSlots.Add(slot, slot.transform.GetChild(0).GetComponent<BaseWeapon>());
            else
                WeaponSlots.Add(slot, null);
        }

        _hands.GetComponent<HandController>().RemoveCurrentWeapon();
        _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[1]);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[2]);
    }

}
