using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro damageText;
    private float maxLifeTime = 1f;
    private float currentLifeTime;
    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }
    public void AssignDamage(int damage)
    {
        damageText.text = damage.ToString();
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        float moveYSpeed = 1f;
        float partOfMaxLifeTime = maxLifeTime / 15;
        transform.position += new Vector3(0, moveYSpeed)*Time.deltaTime;
        if(currentLifeTime > partOfMaxLifeTime)
        {
            damageText.alpha -= Time.deltaTime*2;
        }
        if(currentLifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
    public static void Create(Transform parent, int damage)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.damagePopupTransform, parent.position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopupTransform.position += new Vector3(0, .15f);
        damagePopup.AssignDamage(damage);
    }
}
