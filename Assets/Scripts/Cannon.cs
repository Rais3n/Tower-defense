using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : BaseWeapon
{
    private void Awake()
    {
        projectileSpawnTimer = .5f;
        maxSpawnTimer = .55f;
        rotationSpeed = 10f;
        projectile = "cannonball";
    }

    private void Update()
    {
        HandleEnemies();
    }


}
