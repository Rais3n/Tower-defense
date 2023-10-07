using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    private Enemy enemy;
    private float speed = 10f;
    private int minDmg;
    private int maxDmg;
    public static void Create(Vector3 spawnPosition, Enemy enemy, string projectile, Quaternion currentRotation)
    {
        Quaternion rotationToAdd = Quaternion.Euler(0, 0, 90);
        Quaternion spawnRotation = currentRotation * rotationToAdd;
        Transform projectileTransform = Instantiate(GameAssets.i.GetProjectile(projectile), spawnPosition, spawnRotation);
        Projectile createdProjectile = projectileTransform.GetComponent<Projectile>();
        createdProjectile.Setup(enemy);
        createdProjectile.SetSizeOfDmg(projectile);
    }
    private void Setup(Enemy target)
    {
        enemy = target;
    }
    private void Update()
    {
        if (enemy != null)
        {
            Vector2 projectilePos = transform.position;
            Vector2 moveDir = (enemy.GetPosition() - projectilePos).normalized;

            ChangeAngle(moveDir);
            MoveObject(projectilePos, moveDir);
            OnTargetReached();
        }
    }

    private void MoveObject(Vector2 position, Vector2 dir)
    {
        position += speed * Time.deltaTime * dir;
        transform.position = position;
    }

    private void ChangeAngle(Vector2 dir)
    {
        float rotationSpeed = 60f;
        float angle = GetAngleFromVector(dir);
        //

        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
        
        //transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void OnTargetReached()
    {
        float minDistance = .2f;
        if (Vector2.Distance(transform.position, enemy.GetPosition()) < minDistance)
        {
            Destroy(gameObject);
            int damage = Random.Range(minDmg, maxDmg + 1);
            enemy.Damage(damage);
        }
    }

    private float GetAngleFromVector(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private void SetSizeOfDmg(string projectile)
    {
        switch (projectile)
        {
            case "arrow":
                minDmg = 30;
                maxDmg = 50;
                break;
            case "cannonball":
                minDmg = 40;
                maxDmg = 60;
                break;
            case "rocket":
                minDmg = 90;
                maxDmg = 120;
                break;
            default: break;

        }
    }

}
