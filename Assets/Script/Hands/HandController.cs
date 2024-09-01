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

    public void SwitchCurrentWeapon(GameObject slot)
    {
        GameObject WeaponSlot = slot.transform.childCount == 1 ? slot.transform.GetChild(0).gameObject : null;
        
        if (WeaponSlot != null)
        {

            WeaponSlot.transform.SetParent(transform, false);
            WeaponSlot.transform.position = transform.position;
            WeaponSlot.transform.rotation = transform.rotation;

            if (HandWeapon != null)
            {
                HandWeapon.gameObject.transform.SetParent(slot.transform, false); // Оружие в слот
                HandWeapon.gameObject.transform.position = slot.transform.position;
                HandWeapon.gameObject.transform.rotation = slot.transform.rotation;
            }

            HandWeapon = WeaponSlot.GetComponent<BaseWeapon>();
        }
    }
}
