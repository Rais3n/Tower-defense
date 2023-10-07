using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketWeapon : BaseWeapon
{
    [SerializeField] GameObject leftRocket;
    [SerializeField] GameObject rightRocket;

    private int availableProjectiles;
    private float cdAfterAttack;
    private float shootTimer;

    private void Awake()
    {
        projectileSpawnTimer = .8f;
        maxSpawnTimer = .8f;
        rotationSpeed = 8f;
        projectile = "rocket";
        availableProjectiles = 2;
        cdAfterAttack = 0.35f;
        shootTimer = cdAfterAttack;
    }

    private void Update()
    {
        PrepareProjectile();
        HandleEnemies();
    }

    protected override void HandleEnemies()
    {
        shootTimer += Time.deltaTime;
        if(target != null)
        {
            TargetOutOfRange();
        }
        if (target == null)
        {
            RaiseOnStopAttackEvent();
            GetClosestEnemy();
        }
        if (target != null)
        {
            if (target.IsAlive())
            {
                if (shootTimer > cdAfterAttack)
                {
                    if(availableProjectiles > 0) {
                        
                        RaycastHit2D hit = Physics2D.Raycast((Vector2)weaponCenter.transform.position, transform.up, attackRange);

                        if (hit == target)
                        {
                            Projectile.Create(spawnProjectilePoint.position, target, projectile, transform.rotation);
                            RaiseOnAttackEvent();
                            RestartTimers();
                            ChangeSpawnProjectilePoint();
                        }
                    }
                }
            }
            else target = null;
        }
    }

    private void RestartTimers()
    {
        shootTimer = 0f;
        if (availableProjectiles == 2) projectileSpawnTimer = 0f;
    }
    private void ChangeSpawnProjectilePoint()
    {
        spawnProjectilePoint.gameObject.SetActive(false);
        switch(availableProjectiles)
        {
            case 1:
                spawnProjectilePoint = null;
                availableProjectiles--;
                break;
            case 2:
                spawnProjectilePoint = leftRocket.transform;
                availableProjectiles--;
                break;
        }
    }

    private void PrepareProjectile()
    {
        projectileSpawnTimer += Time.deltaTime;
        if (projectileSpawnTimer > maxSpawnTimer)
        {
            switch (availableProjectiles)
            {
                case 0:
                    leftRocket.SetActive(true);
                    availableProjectiles++;
                    projectileSpawnTimer = 0f;
                    spawnProjectilePoint = leftRocket.transform;
                    break;
                case 1:
                    rightRocket.SetActive(true);
                    availableProjectiles++;
                    projectileSpawnTimer = 0f;
                    spawnProjectilePoint = rightRocket.transform;
                    break;
                case 2:
                    break;
            }
        }
    }
}
