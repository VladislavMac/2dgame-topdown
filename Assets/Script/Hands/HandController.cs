using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [HideInInspector] public GameObject Owner;
    public BaseWeapon HandWeapon;

    private void Update()
    {
        if (HandWeapon != null)
        {
            HandWeapon.Owner = Owner;
        }
        else
        {
            if (transform.GetChild(0) != null)
                HandWeapon = transform.GetChild(0).gameObject.GetComponent<BaseWeapon>();
        }
    }

    public void RemoveCurrentWeapon()
    {
        if (transform.childCount == 1)
        {
            Destroy(transform.GetChild(0).gameObject);
            HandWeapon = null;
        }
    }

    public void AddCurrentWeapon(BaseWeapon weapon)
    {
        if (HandWeapon == null)
        {
            GameObject toggleWeapon = Instantiate(weapon.gameObject);
            toggleWeapon.transform.SetParent(transform);
            toggleWeapon.transform.position = transform.position;
        }
    }

    public void SwitchCurrentWeapon(GameObject slot, GameObject[] slots, Dictionary<BaseWeapon, int> weaponSlots)
    {
        GameObject WeaponInSlot = slot.transform.childCount == 1 ? slot.transform.GetChild(0).gameObject : null;
        
        if (WeaponInSlot != null)
        {

            WeaponInSlot.transform.SetParent(transform, false);
            WeaponInSlot.transform.position = transform.position;
            WeaponInSlot.transform.rotation = transform.rotation;

            if (HandWeapon != null)
            {
                if (weaponSlots.TryGetValue(HandWeapon.GetComponent<BaseWeapon>(), out int indexSlot))
                {
                    HandWeapon.gameObject.transform.SetParent(slots[indexSlot].transform, false);
                    HandWeapon.gameObject.transform.position = slots[indexSlot].transform.position;
                    HandWeapon.gameObject.transform.rotation = slots[indexSlot].transform.rotation;
                    HandWeapon.RemoveAmmo();
                }
            }

            HandWeapon = WeaponInSlot.GetComponent<BaseWeapon>();

        }
    }
}
