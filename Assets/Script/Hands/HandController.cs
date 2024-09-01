using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject Owner;
    public BaseWeapon HandWeapon;

    private void Update()
    {
        if (HandWeapon != null)
        {
            HandWeapon.Owner = Owner;
        }
    }
}
