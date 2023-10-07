using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarUI;
    private Enemy enemy;
    private GameObject canvas;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        canvas = transform.Find("Canvas").gameObject;
    }
    private void Start()
    {
        canvas.SetActive(false);
        enemy.OnEnemyShot += Enemy_OnEnemyShot;
    }

    private void Enemy_OnEnemyShot(object sender, Enemy.OnEnemyShotEventArgs e)
    {
        canvas.SetActive(true);
        healthBarUI.fillAmount = (float)e.currentHP / 100f;
    }
}
