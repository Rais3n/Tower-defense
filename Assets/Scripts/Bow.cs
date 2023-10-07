using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : BaseWeapon
{
    private void Awake()
    {
        projectileSpawnTimer = .4f;
        maxSpawnTimer = .4f;
        rotationSpeed = 14f;
        projectile = "arrow";
    }

    private void Update()
    {
        HandleEnemies();
    }
}
