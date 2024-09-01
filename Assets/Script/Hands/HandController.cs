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
}
