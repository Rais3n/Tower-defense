using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event EventHandler<OnEnemyDeathEventArgs> OnEnemyDeath;
    public class OnEnemyDeathEventArgs : EventArgs
    {
        public int money;
        public int damage;
    }

    public event EventHandler<OnEnemyShotEventArgs> OnEnemyShot;
    public class OnEnemyShotEventArgs : EventArgs
    {
        public int currentHP;
    }

    [SerializeField] private float speed;

    private int HP = 100;
    private int waypointIndex;
    private bool isAlive;
    public static List<Enemy> enemyList = new List<Enemy>();
    private void Awake()
    {
        enemyList.Add(this);
        gameObject.SetActive(false);
        isAlive = true;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Waypoints.Instance.GetWaypoint(waypointIndex).position, speed*Time.deltaTime);
        OnWaypointReached();
    }

    private void OnWaypointReached()
    {
        float minDistanceToWaypoint = .1f;
        if(Vector2.Distance(transform.position, Waypoints.Instance.GetWaypoint(waypointIndex).position) < minDistanceToWaypoint)
        {
            waypointIndex++;
        }

        if (waypointIndex == Waypoints.Instance.GetSizeWaypointArray())
        {
            gameObject.SetActive(false);
            OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs
            {
                money = 0,
                damage = 1
            });
            return;
        }

    }

    public Vector2 GetPosition()
    {
        return this.transform.position;
    }

    public void Damage(int damage)
    {
        if (isAlive) //case when more than 1 projectile goes towards ther same enemy
        {
            int currentHP = HP;
            HP -= damage;
            Debug.Log(HP);
            int minHP = 1;
            if (HP < minHP)
            {
                OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs
                {
                    money = 1,
                    damage = 0
                });
                isAlive = false;
                gameObject.SetActive(false);
                damage = currentHP;
            }
            DamagePopup.Create(transform, damage);
            OnEnemyShot?.Invoke(this, new OnEnemyShotEventArgs
            {
                currentHP = HP,
            });
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
