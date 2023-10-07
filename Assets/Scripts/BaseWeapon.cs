using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected Transform spawnProjectilePoint;
    [SerializeField] protected Transform weaponCenter;
    [SerializeField] protected float attackRange;

    public event EventHandler OnAttack;
    public event EventHandler OnStopAttack;
    protected Enemy target = null;
    protected float projectileSpawnTimer;
    protected float maxSpawnTimer;
    protected float rotationSpeed;
    protected string projectile;

    public bool HasTarget()
    {
        return target != null;
    }
    protected virtual void HandleEnemies()
    {
        projectileSpawnTimer += Time.deltaTime;

        if(target != null)
        {
            TargetOutOfRange();
        }
        if (target == null)
        {
            OnStopAttack?.Invoke(this, EventArgs.Empty);
            GetClosestEnemy();
        }
        if (target != null)
        {
            if (target.IsAlive())
            {
                if (projectileSpawnTimer > maxSpawnTimer)
                {
                    RaycastHit2D hit = Physics2D.Raycast((Vector2)weaponCenter.transform.position, transform.up, attackRange, LayerMask.GetMask("Enemies"));
                    if (hit.transform == target.transform)
                    {
                        Projectile.Create(spawnProjectilePoint.transform.position, target, projectile, transform.rotation);
                        OnAttack?.Invoke(this, EventArgs.Empty);
                        projectileSpawnTimer = 0f;
                    }
                }
            }
            else target = null;
        }
    }

    protected void TargetOutOfRange()
    {
        if (Vector2.Distance(target.GetPosition(), weaponCenter.position) >= attackRange)
        {
            target = null;
        }
    }
    public void SetDirection()
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        transform.up = Vector3.Slerp(transform.up, targetDirection, Time.deltaTime * rotationSpeed);
    }

    public void GetClosestEnemy()
    {
        float distance = attackRange;
        foreach (Enemy enemy in Enemy.enemyList)
        {
            if (enemy.IsAlive())
            {
                if (Vector2.Distance(enemy.GetPosition(), weaponCenter.position) < distance)
                {
                    SetTarget(enemy);
                    distance = Vector2.Distance(enemy.GetPosition(), GetComponent<Transform>().position);
                }
            }
        }

    }

    public void SetTarget(Enemy target)
    {
        this.target = target;
    }

    protected void RaiseOnAttackEvent()
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    protected void RaiseOnStopAttackEvent()
    {
        OnStopAttack?.Invoke(this, EventArgs.Empty);
    }

    public Enemy GetTarget()
    {
        return target;
    }
}
