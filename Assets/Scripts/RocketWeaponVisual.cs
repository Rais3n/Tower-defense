using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketWeaponVisual : MonoBehaviour
{
    private RocketWeapon rocketWeapon;

    private void Awake()
    {
        rocketWeapon = GetComponentInParent<RocketWeapon>();
    }

    private void Update()
    {
        if (rocketWeapon.HasTarget())
        {
            rocketWeapon.SetDirection();
        }
    }
}
