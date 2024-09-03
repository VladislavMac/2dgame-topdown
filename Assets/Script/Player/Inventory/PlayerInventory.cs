using System.Collections.Generic;
using UnityEngine;

using Weapon;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public GameObject[] Slots;
        [SerializeField] private GameObject _hands;
        [HideInInspector] public GameObject Player;

        private Dictionary<BaseWeapon, int> WeaponSlots = new Dictionary<BaseWeapon, int>();

        private void Start()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].transform.GetChild(0) != null)
                    WeaponSlots[Slots[i].transform.GetChild(0).GetComponent<BaseWeapon>()] = i;
            }

            _hands.GetComponent<HandController>().RemoveCurrentWeapon();
            _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[0], Slots, WeaponSlots);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[0], Slots, WeaponSlots);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[1], Slots, WeaponSlots);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                _hands.GetComponent<HandController>().SwitchCurrentWeapon(Slots[2], Slots, WeaponSlots);
        }
    }
}